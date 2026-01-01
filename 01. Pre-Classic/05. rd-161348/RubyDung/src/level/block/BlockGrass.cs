namespace RubyDung;

public class BlockGrass : Block
{
    public BlockGrass(int id) : base(id)
    {
        tex = 3;
    }

    protected override int GetTexture(int face)
    {
        base.GetTexture(face);

        if (face == 3)
        {
            return 0;
        }
        else
        {
            return face == 2 ? 2 : 3;
        }
    }
}
