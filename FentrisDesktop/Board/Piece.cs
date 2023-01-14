using System.Collections.Generic;

namespace FentrisDesktop.Board;

public class Piece
{
    public PieceShape Shape;
    int _rot;

    private static int Mod(int x, int m)
    {
        return (x % m + m) % m;
    }

    public int Rotation
    {
        get => Mod(_rot, Shape.BlockOffsets.Length);
        set => _rot = value;
    }

    public BlockKind Kind { get; set; }

    public int LockDelayCounter { get; set; }
    public int X { get; set; }

    public int Y
    {
        get => SubY / 256;
        set => SubY = value * 256 + SubY % 256;
    }

    public int SubY { get; set; }


    public Piece(PieceShape shape, int rotation, int x, int y, BlockKind blockKind = BlockKind.Bone)
    {
        Shape = shape;
        Rotation = rotation;
        X = x;
        Y = y;
        Kind = blockKind;
    }

    public IEnumerable<(int, int)> GetBlockOffsets()
    {
        return Shape.BlockOffsets[Rotation];
    }
}