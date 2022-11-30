using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Screens;

namespace FentrisDesktop.Board;

public class Gamemode : GameScreen
{
    private new FentrisGame Game => (FentrisGame)base.Game;

    public Board Board;
    public Piece ActivePiece;
    public Queue<PieceShape> Next;
    public readonly int NextAmount;
    protected RenderTarget2D BoardRenderTarget;
    protected SpriteBatch SpriteBatch;
    protected int BoardBorderThickness = 5;
    public int Gravity => 0; // gravity ticks per frame
    public IRandomizer Randomizer;

    public int frame = 0;

    // rotation system goes here later

    public Gamemode(FentrisGame game) : base(game)
    {
        NextAmount = 3; // to be specified by each subclass ig
        Board = new Board();
        Randomizer = new TestRandomizer();
        Next = new(Enumerable.Range(0, NextAmount).Select(_ => Randomizer.GenerateNext()));
        BoardRenderTarget =
            new RenderTarget2D(game.GraphicsDevice, 640 + BoardBorderThickness * 2, 1280 + BoardBorderThickness * 2,
                false, SurfaceFormat.Alpha8, DepthFormat.None);
    }

    public override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
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

    public override void Update(GameTime gameTime)
    {
        Board[0, 0] = new Block(BlockKind.Bone);
        Board[1, 1] = new Block(BlockKind.Bone);
        Board[2, 2] = new Block(BlockKind.Bone);
        Board[3, 3] = new Block(BlockKind.Bone);
        Board[4, 4] = new Block(BlockKind.Bone);
        Board[5, 5] = new Block(BlockKind.Bone);
        Board[6, 6] = new Block(BlockKind.Bone);
        Board[7, 7] = new Block(BlockKind.Bone);
        Board[8, 8] = new Block(BlockKind.Bone);
        Board[9, 9] = new Block(BlockKind.Bone);
        Board[8, 10] = new Block(BlockKind.Bone);
        Board[7, 11] = new Block(BlockKind.Bone);
        Board[6, 12] = new Block(BlockKind.Bone);
        Board[5, 13] = new Block(BlockKind.Bone);
        Board[4, 14] = new Block(BlockKind.Bone);
        Board[3, 15] = new Block(BlockKind.Bone);
        Board[2, 16] = new Block(BlockKind.Bone);
        Board[1, 17] = new Block(BlockKind.Bone);
        Board[0, 18] = new Block(BlockKind.Bone);
        Board[1, 19] = new Block(BlockKind.Bone);
    }

    public override void Draw(GameTime gameTime)
    {
        DrawBoard();
    }

    private void DrawBoard()
    {
        var margin = 40;
        var boardH = Game.H - margin * 2;
        var boardW = boardH / 2;

        var rx = Game.W / 2 - boardW / 2;
        var ry = margin;

        Game.GraphicsDevice.SetRenderTarget(BoardRenderTarget);
        Game.GraphicsDevice.Clear(Color.Transparent);

        SpriteBatch.Begin();
        // board render target minus border is 640x1280, so a single mino is 64x64
        for (int y = 0; y < Board.board.GetLength(1); y++)
        {
            for (int x = 0; x < Board.board.GetLength(0); x++)
            {
                var block = Board[x, y];
                if (block.kind != BlockKind.Clear)
                {
                    SpriteBatch.FillRectangle(new Vector2(x * 64 + BoardBorderThickness, y * 64 + BoardBorderThickness),
                        new Size2(64, 64),
                        block.kind.Color());
                }
            }
        }
        
        SpriteBatch.DrawRectangle(0, 0, BoardRenderTarget.Width,
            BoardRenderTarget.Height, Color.White, BoardBorderThickness);

        SpriteBatch.End();
        Game.GraphicsDevice.SetRenderTarget(null);

        // draw board to screen
        SpriteBatch.Begin();
        SpriteBatch.Draw(BoardRenderTarget, new Rectangle(rx, ry, boardW, boardH), Color.White);
        SpriteBatch.End();
    }

    public override void UnloadContent()
    {
        BoardRenderTarget.Dispose();
        base.UnloadContent();
    }
}