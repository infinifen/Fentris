using System.Collections.Generic;

namespace FentrisDesktop.Board;

public class Piece
{
    public PieceShape Shape;
    public int Rotation;
    public int X;
    public int Y;

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