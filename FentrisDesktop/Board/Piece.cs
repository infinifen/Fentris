using System.Collections.Generic;

namespace FentrisDesktop.Board;

public class Piece
{
    public PieceShape Shape;
    int _rot;
    public int Rotation { get => _rot % 4; set => _rot = value; }

    public int LockDelayCounter { get; set; }
    public int X { get; set; }

    public int Y
    {
        get => SubY / 256;
        set => SubY = value * 256 + SubY % 256;
    }

    public int SubY { get; set; }


    public Piece(PieceShape shape, int rotation, int x, int y)
    {
        Shape = shape;
        Rotation = rotation;
        X = x;
        Y = y;
    }

    public IEnumerable<(int, int)> GetBlockOffsets()
    {
        return Shape.BlockOffsets[Rotation];
    }
    
    
}