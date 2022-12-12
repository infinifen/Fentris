namespace FentrisDesktop.Board;

public struct Block
{
    public BlockKind Kind = BlockKind.Clear;
    public int FramePlaced = 0;
    
    public Block() : this(BlockKind.Clear, 1) {}

    public Block(BlockKind kind = BlockKind.Clear, int framePlaced = 0)
    {
        this.Kind = kind;
        this.FramePlaced = framePlaced;
    }

    public override string ToString()
    {
        return $"{(char) Kind}";
    }
}