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

    protected bool Equals(PieceShape other)
    {
        return Equals(_BlockOffsets, other._BlockOffsets);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((PieceShape) obj);
    }

    public override int GetHashCode()
    {
        return (_BlockOffsets != null ? _BlockOffsets.GetHashCode() : 0);
    }
}