using System.Collections.Generic;
using System.Linq;

namespace FentrisDesktop.Board;

public class Gamemode
{
    public Board Board;
    public Piece ActivePiece;
    public Queue<PieceShape> Next;
    public readonly int NextAmount;
    public int Gravity = 0; // gravity ticks per frame
    public Randomizer Randomizer;
    // rotation system goes here later

    public Gamemode()
    {
        NextAmount = 3; // to be specified by each subclass ig
        Board = new Board();
        Next = new(Enumerable.Range(0, NextAmount).Select(_ => Randomizer.GenerateNext()));
    }

    public void CycleNext()
    {
        ActivePiece = new Piece(Next.Dequeue(), 0, 3, 0);
        Next.Enqueue(Randomizer.GenerateNext());
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
            if (!Board.CollidePiece(ActivePiece, ActivePiece.X, row)) continue; // no collision after going down a row, magnificent
            
            // the piece got shoved into the stack due to gravity, last good position was one above current
            ActivePiece.Y = row - 1;
            break;
        }
    }

    public void Rotate(int direction)
    {
        var old_rotation = ActivePiece.Rotation;
        // rotation system stuff will go here later
        ActivePiece.Rotation += direction;
        if (!Board.CollidePiece(ActivePiece)) return; // successful natural rotation
        
        // natural rotation blocked, for now just fail the rotation
        ActivePiece.Rotation = old_rotation;
        return;
    }

    public void Update()
    {
        
    }
}