namespace FentrisDesktop.Board;

public class PieceShape
{
    // (x, y)[orientation][block]
    protected readonly (int, int)[][] _BlockOffsets;

    public (int, int)[][] BlockOffsets => _BlockOffsets;

    public PieceShape((int, int)[][] blockOffsets)
    {
        _BlockOffsets = blockOffsets;
    }
}