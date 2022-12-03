using System;
using FentrisDesktop.Board;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Screens;

namespace FentrisDesktop.Gamemode;

public class GamemodeRenderer : GameScreen
{
    private new FentrisGame Game => (FentrisGame)base.Game;

    protected RenderTarget2D BoardRenderTarget;
    protected SpriteBatch SpriteBatch;
    protected int BoardBorderThickness = 5;
    protected FentrisDesktop.Gamemode.Gamemode Mode;
    protected InputHandler InputHandler;
    protected SpriteFont Font;

    public GamemodeRenderer(FentrisGame game, FentrisDesktop.Gamemode.Gamemode mode) : base(game)
    {
        Mode = mode;
        BoardRenderTarget =
            new RenderTarget2D(game.GraphicsDevice, 640 + BoardBorderThickness * 2, 1280 + BoardBorderThickness * 2,
                false, SurfaceFormat.Alpha8, DepthFormat.None);
        InputHandler = new InputHandler();
    }

    public override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
        Font = Content.Load<SpriteFont>("default");
    }

    public override void Update(GameTime gameTime)
    {
        var inputs = InputHandler.GetInputs();

        Mode.Frame(inputs);
        InputHandler.CycleInputStates();
    }

    public override void Draw(GameTime gameTime)
    {
        DrawBoard();

        SpriteBatch.Begin();
        SpriteBatch.DrawString(Font, $"{Mode.DasCharge} {Mode.ArrCharge} {Mode.ActivePiece.Y} {Mode.ActivePiece.SubY}", Vector2.Zero, Color.White);
        SpriteBatch.DrawString(Font, Mode.State.ToString(), new Vector2(0, 30), Color.White);
        SpriteBatch.DrawString(Font, Mode.ActivePieceTouchingStack().ToString(), new Vector2(0, 60), Color.White);
        SpriteBatch.End();
    }

    protected void DrawActivePiece()
    {
        var p = Mode.ActivePiece;
        foreach (var (bx, by) in p.GetBlockOffsets())
        {
            DrawBoardBlock(p.Kind, p.X + bx, p.Y + by);
        }
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
        for (int y = 1; y < Mode.Board.board.GetLength(1); y++) // start from 1 because of vanish row
        {
            for (int x = 0; x < Mode.Board.board.GetLength(0); x++)
            {
                var block = Mode.Board[x, y];
                if (block.Kind != BlockKind.Clear)
                {
                    DrawBoardBlock(block.Kind, x, y);
                }
            }
        }

        if (Mode.State == GamemodeState.Placement)
        {
            DrawActivePiece();
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

    private void DrawBoardBlock(BlockKind kind, int x, int y)
    {
        // board render target minus border is 640x1280, so a single mino is 64x64
        // y - 1 is there because of the vanish row, the first actual row that should be visible is row y=1
        SpriteBatch.FillRectangle(new Vector2(x * 64 + BoardBorderThickness, (y - 1) * 64 + BoardBorderThickness),
            new Size2(64, 64),
            kind.Color());
    }

    public override void UnloadContent()
    {
        BoardRenderTarget.Dispose();
        base.UnloadContent();
    }
}