namespace FentrisDesktop.Board;

public struct Block
{
    public BlockKind kind = BlockKind.Clear;
    public float age = 1;
    
    public Block() : this(BlockKind.Clear, 1f) {}

    public Block(BlockKind kind = BlockKind.Clear, float age = 1)
    {
        this.kind = kind;
        this.age = age;
    }

    public override string ToString()
    {
        return $"{(char) kind}";
    }
}