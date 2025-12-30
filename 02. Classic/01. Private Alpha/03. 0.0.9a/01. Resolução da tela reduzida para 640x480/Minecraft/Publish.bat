@echo off
echo ========================================
echo PUBLICANDO PARA MULTIPLAS PLATAFORMAS
echo ========================================
echo.

REM 1. Limpa as pastas de publicação anteriores
if exist pub rmdir /s /q pub

REM 2. Publicar
echo.
echo [1/3] Publicando para Windows x64...
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -o "pub/win-x64"
if %errorlevel% neq 0 (
    echo ERRO: Falha ao publicar para Windows x64
    pause
    exit /b 1
)

REM Cria README para Windows
(
echo Para executar no Windows:
echo 1. Execute RubyDung.exe
) > "pub\win-x64\README.txt"

echo.
echo [2/3] Publicando para Linux x64...
dotnet publish -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true -o "pub/linux-x64"
if %errorlevel% neq 0 (
    echo ERRO: Falha ao publicar para Linux x64
    pause
    exit /b 1
)

REM Cria README para Linux
(
echo Para executar no Linux:
echo 1. Torne o executavel permitido: chmod +x RubyDung
echo 2. Execute: ./RubyDung
) > "pub\win-x64\README.txt"

echo.
echo [3/3] Publicando para macOS x64...
dotnet publish -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true -o "pub/osx-x64"
if %errorlevel% neq 0 (
    echo ERRO: Falha ao publicar para macOS x64
    pause
    exit /b 1
)

REM Cria README para macOS
(
echo Para executar no macOS:
echo 1. Torne o executavel permitido: chmod +x RubyDung
echo 2. Execute: ./RubyDung
) > "pub\osx-x64\README.txt"

echo.
echo ========================================
echo TODAS AS PUBLICACOES CONCLUIDAS COM SUCESSO!
echo ========================================
pause
