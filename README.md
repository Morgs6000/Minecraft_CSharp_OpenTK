# Minecraft_CSharp_OpenTK

Não, não sei usar GitHub, e não, não vou aprender, não faço questão de aprender, ninguem ta me ajudando com esta m@#$%, povo fala que vai ajudar e some. Aprendi a editar o README.md pra deixar o mais mastigadinho possivel.

Então para de reclamar que tem um monte de pasta, procura a p@#$% da pasta que você quer, copia a p@#$% do URL, e cola nessa p@#$% desse link: 

  * <img src="https://github.com/user-attachments/assets/ed00c22a-2aa4-4854-be83-83e270da6d27" width="30" height="30"> [Download GitHub Directory](https://download-directory.github.io/)

## 📖 Sobre

Este projeto tem como objetivo recriar as primeiras versões do Minecraft, sem engine, usando a linguagem C# e a blioteca OpenTK.

Baixe os executaveis do projeto neste link para testar, e se possivel nos mande um feedback:

* 🎮 [Versões](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/releases/tag/Teste)

Quem quiser ajudar com o projeto seja bem-vindo a se juntar a nós no Discord:

  * <img src="https://github.com/user-attachments/assets/fe0fedfb-5be2-4ccd-bf54-f61446301e84" width="15" height="15"> [Discord](https://discord.gg/2NHGc8PHmq)

O progresso deste trabalho esta sendo registrado em video no YouTube:

 * <img src="https://github.com/user-attachments/assets/abc360e9-233c-49e2-891f-3511c6c4ae55" width="15" height="15"> [Stradivarius Dev](https://www.youtube.com/@StradivariusDev/)

E o instagram, só pra ter...

 * <img src="https://github.com/user-attachments/assets/c30107c0-a584-4a00-80c1-960ec2075591" width="15" height="15"> [stradivarius_dev](https://www.instagram.com/stradivarius_dev/)

## 🛠️ Ferramentas e Tecnologias

 * <img src="https://github.com/user-attachments/assets/3717a45d-d2d6-4d70-8cbe-aab0816e3c6c" width="30" height="30"> VS Code

 * <img src="https://github.com/user-attachments/assets/73828f67-0382-482c-b566-62e24cb93d55" width="30" height="30"> C#

 * <img src="https://github.com/user-attachments/assets/dded9073-78aa-478e-ab2b-f5a42d21d6a2" width="30" height="30"> OpenGL

 * <img src="https://github.com/user-attachments/assets/ef9fb0e1-d0fd-4632-bbf5-e1623cd32a6a" width="30" height="30"> OpenTK

 * <img src="https://github.com/user-attachments/assets/e59048c2-1bb9-4b35-93e4-f9769cfffc68" width="30" height="30"> StbImageSharp

## 🐞 Bugs

**rd-131655 (Cave game tech test)**
* O mouse do jogador é exibido movimento antes da tela carregar totalmente.
* O jogador nasce abaixo do mundo por causa da gravidade.
* Foi adicionado um delay de 5 segundo antes de dar os Updates da camera.
* * para não ter que esperar pelo delay, se a tela ja apareceu para o jogador, basta dar um click com o mouse.
* Jogador dando umas enroscadas no chão enquanto anda.
* O jogador quando esta encostado em uma parede com mais de 2 blocos de altura e tenta pular, ele escala a parede.

**rd-132211**
* O icone de janela não aceita arquivo '.ico', e o icone do '.exe' não aceita arquivo '.png'.

**rd-132328**
* Não sei gerar os Steve(s) doidos.

**rd-160052**
* O arquivo 'level.dat' comprimido apresenta problemas para salvar e carregar o jogo.
  * O arquivo comprimido pesa cerca de 30 KB, enquanto o arquivo não comprido pesa cerca de 4.000 KB, isso falando de um mundo de 256x64x256 blcoos (16x4x16 chunks).
* não sei gerar o bloco da GUI e a mira na mesma malha, se é que isso é possivel.

## 📝 Correções

* Altura da camera corrigina.
* Escalada aranha corrigida, mas se o jogador ficar pressionando Espaço contra uma parede, ainda consegue escalar.
* Despausar nao faz mais com que o jogador caia para fora do mundo.
* Despausar não faz mais a camera se mover instantaneamente em uma direção "aleatória".
* Corrigito crosshair. Um testador relatou que crosshair não acompanhava o redimensionamento de tela.

## 💾 Versões

Todas as versões podem ser consultadas na Wiki do Minecraft. Abaixam estão listadas apenas versões que adicionam ou mudam alguma coisa no jogo, não se assuste, algumas versões adicionam apenas um bloco ou item, e obviamente não faremos tudo, uma das coisas que pensamos em descartar são coisas originais do Minecraft, como Creepers e Redstone. Nas primeiras versões o mundo era limitado a 256x64x256 blocos. O objetivo até o momento é chegar até a versão Infdev, onde o mundo se torna infinito.

<img src="https://github.com/user-attachments/assets/0313e113-9c0b-43ec-93c1-cc7dff0a99c7" width="90" height="90"> [Java Edition version history](https://minecraft.wiki/w/Java_Edition_version_history)

Abaixo, links de acesso rapido as pastas que estão em progresso ou ja foram concluidas:

**Legenda:**
* 🟩 Concluido
* 🟨 Iniciado e/ou Precisa de correções
* 🟥 Não iniciado
* ⁉️ Alguma versão que talvez não altere nada de util e eu vou remvoer da lista depois...

### Pré-Classico

* 🟨 [rd-131655 (Cave game tech test)](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/01.%20Pre-Classic/01.%20rd-131655%20(Cave%20game%20tech%20test)) >> [wiki](https://minecraft.wiki/w/Cave_game_tech_test)
* 🟨 [rd-132211](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/01.%20Pre-Classic/02.%20rd-132211) >> [wiki](https://minecraft.wiki/w/Java_Edition_pre-Classic_rd-132211)
* 🟥 [rd-132328](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/01.%20Pre-Classic/03.%20rd-132328) >> [wiki](https://minecraft.wiki/w/Java_Edition_pre-Classic_rd-132328)
* 🟨 [rd-160052](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/01.%20Pre-Classic/04.%20rd-160052) >> [wiki¹](https://minecraft.wiki/w/Java_Edition_pre-Classic_rd-20090515) - [wiki²](https://minecraft.wiki/w/Java_Edition_pre-Classic_rd-160052)
* 🟨 [rd-161348](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/01.%20Pre-Classic/05.%20rd-160052) >> [wiki](https://minecraft.wiki/w/Java_Edition_pre-Classic_rd-161348)

### Clássico

**Classic | Private Alpha**
* 🟨 [0.0.2a](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/02.%20Classic/01.%20Private%20Alpha/01.%200.0.2a) >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.2a)
* 🟨 [0.0.3a](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/02.%20Classic/01.%20Private%20Alpha/02.%200.0.3a) >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.3a)
* 🟨 [0.0.9a](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/02.%20Classic/01.%20Private%20Alpha/03.%200.0.9a) >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.9a)
* 🟨 [0.0.10a](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/02.%20Classic/01.%20Private%20Alpha/04.%200.0.10a) >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.10a)

**Classic | Early Classic**
* 🟥 [0.0.12a](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/02.%20Classic/02.%20Early%20Classic/01.%200.0.12a) >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.12a)
* ⁉️ 0.0.12a_01 >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.12a_01)
* 🟥 [0.0.13a](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/02.%20Classic/02.%20Early%20Classic/02.%200.0.13a) >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.13a)
* 🟥 [0.0.13a_03](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/02.%20Classic/02.%20Early%20Classic/03.%200.0.13a_03) >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.13a_03)
* 🟥 [0.0.14a](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/02.%20Classic/02.%20Early%20Classic/04.%200.0.14a) >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.14a)
* 🟥 [0.0.14a_04](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/02.%20Classic/02.%20Early%20Classic/05.%200.0.14a_04) >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.14a_04)
* ⁉️ 0.0.14a_08 >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.14a_08)

**Classic | Multiplayer Test**
* 🟥 [0.0.15a (Multiplayer Teste 1)](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/02.%20Classic/03.%20Multiplayer%20Test/01.%200.0.15a%20(Multiplayer%20Test%201)) >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.15a_(Multiplayer_Test_1))
* ⁉️ 0.0.15a (Multiplayer Teste 2) >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.15a_(Multiplayer_Test_2))
* 🟥 [0.0.15a (Multiplayer Teste 4)](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/02.%20Classic/03.%20Multiplayer%20Test/02.%200.0.15a%20(Multiplayer%20Test%204)) >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.15a_(Multiplayer_Test_4))
* 🟥 [0.0.15a (Multiplayer Teste 5)](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/02.%20Classic/03.%20Multiplayer%20Test/03.%200.0.15a%20(Multiplayer%20Test%205)) >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.15a_(Multiplayer_Test_5))
* 🟥 [0.0.16a](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/02.%20Classic/03.%20Multiplayer%20Test/04.%200.0.16a) >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.16a)
* ⁉️ 0.0.16a_01 >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.16a_01)
* ⁉️ 0.0.16a_02 >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.16a_02)
* 🟥 [0.0.17a](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/02.%20Classic/03.%20Multiplayer%20Test/05.%200.0.17a) >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.17a)
* 🟥 [0.0.18a](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/02.%20Classic/03.%20Multiplayer%20Test/05.%200.0.17a) >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.18a)
* ⁉️ 0.0.18a_02 >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.18a_02)
* 🟥 [0.0.19a](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/02.%20Classic/03.%20Multiplayer%20Test/05.%200.0.17a) >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.19a)
* ⁉️ 0.0.19a_01 >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.19a_01)
* ⁉️ 0.0.19a_02 >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.19a_02)
* 🟥 [0.0.20a](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/02.%20Classic/03.%20Multiplayer%20Test/08.%200.0.20a) >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.20a)
* ⁉️ 0.0.20a_01 >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.20a_01)
* ⁉️ 0.0.20a_02 >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.20a_02)
* ⁉️ 0.0.21a >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.21a)
* 🟥 [0.0.22a](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/02.%20Classic/03.%20Multiplayer%20Test/09.%200.0.22a) >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.22a)
* ⁉️ 0.0.22a_01 >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.22a_01)
* ⁉️ 0.0.22a_05 >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.22a_05)
* 🟥 [0.0.23a](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/02.%20Classic/03.%20Multiplayer%20Test/10.%200.0.23a) >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.23a)
* ⁉️ 0.0.23a_01 >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.0.23a_01)

**Classic | Survival Test**
* 🟥 [0.24_SURVIVAL_TEST](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/02.%20Classic/03.%20Multiplayer%20Test/10.%200.0.23a) >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.24_SURVIVAL_TEST)
* ⁉️ 0.25 SURVIVAL TEST >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.25_SURVIVAL_TEST)
* ⁉️ 0.25 SURVIVAL TEST 2 >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.25_SURVIVAL_TEST_2)
* ⁉️ 0.25 SURVIVAL TEST 3 >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.25_SURVIVAL_TEST_3)
* ⁉️ 0.25 SURVIVAL TEST 4 >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.25_SURVIVAL_TEST_4)
* ⁉️ 0.25_05 SURVIVAL TEST >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.25_05_SURVIVAL_TEST)
* 🟥 [0.26 SURVIVAL TEST](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/02.%20Classic/03.%20Multiplayer%20Test/10.%200.0.23a) >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.26_SURVIVAL_TEST)
* ⁉️ 0.27 SURVIVAL TEST >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.27_SURVIVAL_TEST)

**Classic | Late Classic**
* 🟥 [0.28](https://github.com/Morgs6000/Minecraft_CSharp_OpenTK/tree/main/02.%20Classic/05.%20Late%20Classic/01.%200.28) >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.28)
* ⁉️ 0.29 >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.29)
* ⁉️ 0.29_01 >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.29_01)
* ⁉️ 0.30 >> [wiki](https://minecraft.wiki/w/Java_Edition_Classic_0.30)

### Indev
* Indev 0.31 20091223-0040
* Indev 0.31 20091223-1457
* Indev 0.31 20091231-1856
* Indev 0.31 20091231-2147
* Indev 0.31 20091231-2255
* Indev 0.31 20100104-2154
* Indev 0.31 20100106-2158
* Indev 0.31 20100107-1851
* Indev 0.31 20100107-1947
* Indev 0.31 20100109-1939
* Indev 0.31 20100109-2000
* Indev 0.31 20100110
* Indev 0.31 20100111-2207
* Indev 0.31 20100113-2015
* Indev 0.31 20100113-2244
* Indev 0.31 20100114
* Indev 0.31 20100122-1708
* Indev 0.31 20100122-2251
* Indev 0.31 20100124-2119
* Indev 0.31 20100124-2310
* Indev 0.31 20100125
* Indev 0.31 20100128-2200
* Indev 0.31 20100128-2304
* Indev 0.31 20100129-1447
* Indev 0.31 20100129-2332
* Indev 0.31 20100130
* Indev 0.31 20100131-2156
* Indev 0.31 20100131-2244
* Indev 0.31 20100201-0025
* Indev 0.31 20100201-0038
* Indev 0.31 20100201-2227
* Indev 0.31 20100202
* Indev 0.31 20100204-2027
* Indev 0.31 20100205-1558
* Indev 20100206-2034
* Indev 20100206-2103
* Indev 20100207-1057
* Indev 20100207-1647
* Indev 20100207-1703
* Indev 20100211-2327
* Indev 20100212-1210
* Indev 20100212-1622
* Indev 20100213
* Indev 20100214
* Indev 20100218-0011
* Indev 20100218-0016
* Indev 20100219
* Indev 20100223

### Infdev
* Infdev 20100227-1414
* Infdev 20100227-1433
* Infdev 20100313
* Infdev 20100316
* Infdev 20100316
* Infdev 20100325-1545
* Infdev 20100327
* Infdev 20100330-1203
* Infdev 20100413-1951
* Infdev 20100415
* Infdev 20100420
* Infdev 20100607
* Infdev 20100608
* Infdev 20100611
* Infdev 20100615
* Infdev 20100616-1808
* Infdev 20100616-2210
* Infdev 20100617-1531
* Infdev 20100618
* Infdev 20100624
* Infdev 20100625-1917
* Infdev 20100627
* Infdev 20100629
* Infdev 20100630-1340
* Infdev 20100630-1835
