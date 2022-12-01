using System.Collections.Generic;
using System.Linq;

namespace FentrisDesktop.Board;

public class Gamemode
{
    public Board Board;
    public Piece ActivePiece;
    public Queue<PieceShape> Next;
    public readonly int NextAmount;
    public int Gravity => 0; // gravity ticks per frame
    public int Das => 6;
    public int Arr => 1;

    protected int DasCharge = 0;
    protected int ArrCharge => 0;
    
    public IRandomizer Randomizer;

    public int frame = 0;
    // rotation system goes here later

    public Gamemode()
    {
        NextAmount = 3; // to be specified by each subclass ig
        Board = new Board();
        Randomizer = new TestRandomizer();
        Next = new(Enumerable.Range(0, NextAmount).Select(_ => Randomizer.GenerateNext()));
    }
    
    public void CycleNext()
    {
        var nextShape = Next.Dequeue();
        ActivePiece = new Piece(nextShape, 0, 3, 0, GetPieceColor(nextShape));
        Next.Enqueue(Randomizer.GenerateNext());
    }

    protected BlockKind GetPieceColor(PieceShape shape)
    {
        if (shape.Equals(Tetrominoes.I))
        {
            return BlockKind.Red;
        }

        return BlockKind.Bone;
    }

    public bool ActivePieceTouchingStack()
    {
        return Board.CollidePiece(ActivePiece, ActivePiece.X, ActivePiece.Y - 1);
    }

    public bool HorizontalMove(int direction)
    {
        if (Board.CollidePiece(ActivePiece, ActivePiece.X + direction, ActivePiece.Y))
        {
            return false;
        }

        ActivePiece.X += direction;
        return true;
    }

    public void ApplyGravity(int gravity)
    {
        var newY = (ActivePiece.SubY + gravity) / 256;
        for (int row = ActivePiece.Y; row < newY; row++)
        {
            if (!Board.CollidePiece(ActivePiece, ActivePiece.X, row))
                continue; // no collision after going down a row, magnificent

            // the piece got shoved into the stack due to gravity, last good position was one above current
            ActivePiece.Y = row - 1;
            break;
        }
    }

    public void Rotate(int direction)
    {
        var oldRotation = ActivePiece.Rotation;
        // rotation system stuff will go here later
        ActivePiece.Rotation += direction;
        if (!Board.CollidePiece(ActivePiece)) return; // successful natural rotation

        // natural rotation blocked, for now just fail the rotation
        ActivePiece.Rotation = oldRotation;
        return;
    }

    public void Frame(GamemodeInputs input)
    {
        TestPattern();
    }

    private void TestPattern()
    {
        Board[0, 0] = new Block(BlockKind.Red);
        Board[1, 1] = new Block(BlockKind.Orange);
        Board[2, 2] = new Block(BlockKind.Yellow);
        Board[3, 3] = new Block(BlockKind.Green);
        Board[4, 4] = new Block(BlockKind.Cyan);
        Board[5, 5] = new Block(BlockKind.Blue);
        Board[6, 6] = new Block(BlockKind.Magenta);
        Board[7, 7] = new Block(BlockKind.Bone);
        Board[8, 8] = new Block(BlockKind.Magenta);
        Board[9, 9] = new Block(BlockKind.Blue);
        Board[8, 10] = new Block(BlockKind.Cyan);
        Board[7, 11] = new Block(BlockKind.Green);
        Board[6, 12] = new Block(BlockKind.Yellow);
        Board[5, 13] = new Block(BlockKind.Orange);
        Board[4, 14] = new Block(BlockKind.Red);
        Board[3, 15] = new Block(BlockKind.Garbage);
        Board[2, 16] = new Block(BlockKind.Garbage);
        Board[1, 17] = new Block(BlockKind.Garbage);
        Board[0, 18] = new Block(BlockKind.Garbage);
        Board[1, 19] = new Block(BlockKind.Garbage);
    }
}