using System;
using System.Collections.Generic;
using System.Linq;
using FentrisDesktop.Board;

namespace FentrisDesktop.Gamemode;

public class Gamemode
{
    public Board.Board Board;
    public Piece ActivePiece;
    public Queue<PieceShape> Next;
    public readonly int NextAmount;
    public int Gravity => 4; // gravity ticks per frame
    public int Das => 10;
    public int Arr => 2;
    public int Are => 20;
    public int LineAre => 16;
    public int LineClearDelay => 20;
    public int LockDelay => 30;
    public float LockDelayRatio => LockDelayLeft / (float) LockDelay;
    public int LockDelayLeft;
    public int HighestYSeen;

    public int DasCharge = 0;
    public int ArrCharge = 0;

    protected GamemodeState _state = GamemodeState.Placement;

    public GamemodeState State
    {
        get => _state;
        set
        {
            _state = value;
            LastStateChangeFrame = FrameCount;
        }
    }

    protected int LastStateChangeFrame;
    public int SinceLastStateChange => FrameCount - LastStateChangeFrame;
    public bool GhostPieceEnabled => true;

    public IRandomizer Randomizer;

    public int FrameCount;
    // rotation system goes here later

    public Gamemode()
    {
        NextAmount = 3; // to be specified by each subclass ig
        Board = new Board.Board();
        Randomizer = new History6RollRandomizer();
        Next = new(Enumerable.Range(0, NextAmount).Select(_ => Randomizer.GenerateNext()));
        OnNewPiece();
    }

    public void OnNewPiece()
    {
        var nextShape = Next.Dequeue();
        ActivePiece = new Piece(nextShape, 0, 3, 0, GetPieceKindForShape(nextShape));
        LockDelayLeft = LockDelay;
        HighestYSeen = -8;
        Next.Enqueue(Randomizer.GenerateNext());
    }

    public BlockKind GetPieceKindForShape(PieceShape shape)
    {
        if (shape.Equals(Tetrominoes.I))
        {
            return BlockKind.Red;
        }

        if (shape.Equals(Tetrominoes.O))
        {
            return BlockKind.Yellow;
        }

        if (shape.Equals(Tetrominoes.T))
        {
            return BlockKind.Cyan;
        }

        if (shape.Equals(Tetrominoes.L))
        {
            return BlockKind.Orange;
        }

        if (shape.Equals(Tetrominoes.J))
        {
            return BlockKind.Blue;
        }

        if (shape.Equals(Tetrominoes.S))
        {
            return BlockKind.Magenta;
        }

        if (shape.Equals(Tetrominoes.Z))
        {
            return BlockKind.Green;
        }

        return BlockKind.Bone;
    }

    public bool ActivePieceTouchingStack()
    {
        return Board.CollidePiece(ActivePiece, ActivePiece.X, ActivePiece.Y + 1);
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
        var newSubY = ActivePiece.SubY + gravity;

        while (ActivePiece.SubY < newSubY)
        {
            if (!Board.CollidePiece(ActivePiece, ActivePiece.X, ActivePiece.Y + 1))
            {
                ActivePiece.SubY += 256;
            }
            else
            {
                break;
            }
        }

        if (ActivePiece.SubY > newSubY)
        {
            ActivePiece.SubY = newSubY;
        }
    }

    public void Rotate(int direction, bool kick = true)
    {
        var kicksCw = new List<(int, int)> { (0, 0), (1, 0), (-1, 0), (1, 1), (-1, 1), (1, 2), (-1, 2) };
        var kicksCcw = new List<(int, int)> { (0, 0), (-1, 0), (1, 0), (-1, 1), (1, 1), (-1, 2), (1, 2) };
        var kicksICw = new List<(int, int)> { (0, 0), (1, 0), (-1, 0), (0, 1) };
        var kicksICcw = new List<(int, int)> { (0, 0), (-1, 0), (1, 0), (0, 1) };

        var oldRotation = ActivePiece.Rotation;
        // rotation system stuff will go here later
        ActivePiece.Rotation += direction;
        if (kick)
        {
            var (cw, ccw) =
                ActivePiece.Shape.Equals(Tetrominoes.I) ? (kicksICw, kicksICcw) : (kicksCw, kicksCcw);
            var kicks = direction > 0 ? cw : ccw;
            foreach (var (dx, dy) in kicks)
            {
                if (!Board.CollidePiece(ActivePiece, ActivePiece.X + dx, ActivePiece.Y + dy))
                {
                    ActivePiece.X += dx;
                    ActivePiece.Y += dy;
                    return; // successful natural rotation
                }
            }
        }
        else
        {
            if (!Board.CollidePiece(ActivePiece)) return;
        }


        // natural rotation blocked, for now just fail the rotation
        ActivePiece.Rotation = oldRotation;
    }

    public void Frame(GamemodeInputs input)
    {
        if (State == GamemodeState.LineClear && SinceLastStateChange >= LineClearDelay)
        {
            OnLineClearEnd();
            State = GamemodeState.LineAre;
        }

        if (State == GamemodeState.Are && SinceLastStateChange >= Are)
        {
            State = GamemodeState.Placement;
            OnNewPiece();
            OnPieceEnter(ref input);
            Console.WriteLine($"afterIrs {input}");
        }

        if (State == GamemodeState.LineAre && SinceLastStateChange >= LineAre)
        {
            State = GamemodeState.Placement;
            OnNewPiece();
            OnPieceEnter(ref input);
            Console.WriteLine($"afterIrs {input}");
        }

        if (State == GamemodeState.Gameover)
        {
            return;
        }

        ChargeDas(input);

        switch (State)
        {
            case GamemodeState.Placement:
                HandleRotation();
                ApplyGravity(Gravity + (input.SoftDrop ? 256 : 0) + (input.SonicDrop ? 9999 : 0));
                HandleMovement();

                if (ActivePiece.Y > HighestYSeen)
                {
                    // step reset
                    HighestYSeen = ActivePiece.Y;
                    LockDelayLeft = LockDelay;
                }

                if (ActivePieceTouchingStack())
                {
                    LockDelayLeft--;

                    if (input.SoftDrop || LockDelayLeft == 0)
                    {
                        LockPiece();
                    }
                }

                break;
        }

        FrameCount++;

        void HandleRotation()
        {
            if (input.RotateCcw)
            {
                Rotate(-1);
            }

            if (input.RotateCw)
            {
                Rotate(1);
            }
        }

        void HandleMovement()
        {
            ArrCharge += DasCharge >= Das ? 1 : 0;

            if (input.Direction != DirectionInput.Neutral && input.PreviousDirection != input.Direction ||
                ArrCharge >= Arr)
            {
                // a movement button was just pressed or ARR went off
                HorizontalMove(input.Direction.ToInt());
                ArrCharge = 0;
            }
        }
    }

    protected void OnPieceEnter(ref GamemodeInputs input)
    {
        if (input.IrsCw)
        {
            Rotate(1, false);
            input.RotateCw =
                false; // block regular rotation on the frame IRS goes out so it doesn't double-rotate the piece
        }

        if (input.IrsCcw)
        {
            Rotate(-1, false);
            input.RotateCcw = false; // same as above
        }

        if (Board.CollidePiece(ActivePiece))
        {
            State = GamemodeState.Gameover;
        }
    }

    public void OnLineClearEnd()
    {
        foreach (var row in Board.FullRows())
        {
            Board.ClearRow(row);
        }
    }

    protected void LockPiece()
    {
        Board.PlacePiece(ActivePiece, FrameCount);
        State = Board.FullRows().Any() ? GamemodeState.LineClear : GamemodeState.Are;
    }

    private void ChargeDas(GamemodeInputs input)
    {
        if (input.Direction != DirectionInput.Neutral && input.Direction == input.PreviousDirection)
        {
            if (DasCharge < Das)
            {
                DasCharge++;
            }
        }
        else
        {
            DasCharge = 0;
            ArrCharge = 0;
        }
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