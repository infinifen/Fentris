namespace FentrisDesktop.Board;

public struct Block
{
    public BlockKind Kind = BlockKind.Clear;
    public float Age = 1;
    
    public Block() : this(BlockKind.Clear, 1f) {}

    public Block(BlockKind kind = BlockKind.Clear, float age = 1)
    {
        this.Kind = kind;
        this.Age = age;
    }

    public override string ToString()
    {
        return $"{(char) Kind}";
    }
}