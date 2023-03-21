using System;
using FentrisDesktop.Board;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;

namespace FentrisDesktop.Gamemode;

public class GamemodeRenderer : GameScreen
{
    protected new FentrisGame Game => (FentrisGame)base.Game;
    protected RenderTarget2D BoardRenderTarget;
    protected SpriteBatch SpriteBatch;
    protected FentrisDesktop.Gamemode.Gamemode Mode;
    protected InputHandler InputHandler;
    protected DynamicSpriteFont DebugFont => Game.SmallFont;
    protected Texture2D BlockTexture;
    protected LayoutInfo Layout = new();

    public GamemodeRenderer(FentrisGame game, FentrisDesktop.Gamemode.Gamemode mode) : base(game)
    {
        Mode = mode;
        BoardRenderTarget =
            new RenderTarget2D(game.GraphicsDevice, 640 + Layout.BoardBorderThickness * 2,
                1280 + Layout.BoardBorderThickness * 2 + Layout.PreviewHeight,
                false, SurfaceFormat.Alpha8, DepthFormat.None);
        InputHandler = new InputHandler(game.KeyBinds);
    }

    public override void LoadContent()
    {
        Console.WriteLine("loadContent");
        SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
        BlockTexture = Content.Load<Texture2D>("block");
    }

    public override void Update(GameTime gameTime)
    {
        var inputs = InputHandler.GetInputs();
        
        if ((Mode.State.IsFinished() && Keyboard.GetState().IsKeyDown(Game.KeyBinds.Start)) ||
            Keyboard.GetState().IsKeyDown(Game.KeyBinds.Back))
        {
            if (Mode.State.IsFinished())
            {
                Console.WriteLine(Game.SaveData.Highscores == null);
                Mode.SaveRecord(Game.SaveData.Highscores);
                Game.WriteSave();
            }
            Game.LoadMenu();
        }

        Mode.Frame(inputs);
        
        InputHandler.CycleInputStates();
    }

    public override void Draw(GameTime gameTime)
    {
        DrawBoard();

        SpriteBatch.Begin();
        // SpriteBatch.DrawString(DebugFont,
            // $"{Mode.DasCharge} {Mode.ArrCharge} {Mode.ActivePiece.Y} {Mode.ActivePiece.SubY} {Mode.LockDelayLeft}",
            // Vector2.Zero, Color.White);
        // SpriteBatch.DrawString(DebugFont, Mode.State.ToString(), new Vector2(0, 30), Color.White);
        // SpriteBatch.DrawString(DebugFont, Mode.ActivePieceTouchingStack().ToString(), new Vector2(0, 60), Color.White);
        // SpriteBatch.DrawString(DebugFont, InputHandler.GetInputs().ToString(), new Vector2(0, 100), Color.White);
        // SpriteBatch.DrawString(DebugFont, (1f - Mode.LockDelayRatio).ToString(), new Vector2(0, 140), Color.White);
        SpriteBatch.DrawString(DebugFont, $"lv{Mode.Level}", new Vector2(0, 400), Color.White);

        if (Mode.State == GamemodeState.Gameover)
        {
            SpriteBatch.DrawString(Game.MediumFont, "game over, press enter to go back to menu", new Vector2(0, Game.H - 40), Color.White);
        }
        
        if (Mode.State == GamemodeState.Clear)
        {
            SpriteBatch.DrawString(Game.MediumFont, "Congratulations! press enter to go back to menu", new Vector2(0, Game.H - 40), Color.White);
        }
        
        SpriteBatch.End();
    }

    protected virtual void DrawActivePiece()
    {
        var p = Mode.ActivePiece;
        var ghostY = Mode.Board.GetGhostY(Mode.ActivePiece);
        foreach (var (bx, by) in p.GetBlockOffsets())
        {
            DrawBoardBlock(p.Kind, p.X + bx, p.Y + by, 0.9f - 1.2f * Mode.LockDelayRatio, 1f);
            if (Mode.GhostPieceEnabled)
            {
                DrawBoardBlock(p.Kind, p.X + bx, ghostY + by, 0, 0.2f);
            }
        }
    }

    protected virtual void DrawBoard()
    {
        var boardH = Game.H - Layout.Margin * 2;
        var boardW = (int)(boardH / ((float)BoardRenderTarget.Height / BoardRenderTarget.Width));

        var rx = Game.W / 2 - boardW / 2;
        var ry = Layout.Margin;

        Game.GraphicsDevice.SetRenderTarget(BoardRenderTarget);
        Game.GraphicsDevice.Clear(Color.Transparent);

        SpriteBatch.Begin();

        if (Mode.State == GamemodeState.ReadyGo)
        {
            var countdownFont = Game.DefaultFonts.GetFont(120);
            var secondsLeft = Mode.StartupLeft > 60 ? (Mode.StartupLeft / 60).ToString() : "Go!";
            var renderSize = countdownFont.MeasureString(secondsLeft);
            var renderPos = new Vector2(320, 640) - renderSize / 2;
            SpriteBatch.DrawString(countdownFont, secondsLeft, renderPos, Color.White);
        }

        for (int y = 1; y < Mode.Board.board.GetLength(1); y++) // start from 1 because of vanish row
        {
            for (int x = 0; x < Mode.Board.board.GetLength(0); x++)
            {
                var block = Mode.Board[x, y];
                if (block.Kind != BlockKind.Clear)
                {
                    var contains = Mode.CurrentFullRows.Contains(y);
                    float opacity = (float)(contains
                        ? 1f - Math.Pow(Mode.SinceLastStateChange / (float)Mode.LineClearDelay, 4)
                        : 1f);
                    DrawBoardBlock(block.Kind, x, y, 0, opacity);
                }
            }
        }

        if (Mode.State == GamemodeState.Placement)
        {
            DrawActivePiece();
        }

        DrawNextQueue();

        DrawBorder();

        SpriteBatch.End();
        Game.GraphicsDevice.SetRenderTarget(null);

        // draw board to screen
        SpriteBatch.Begin();
        SpriteBatch.Draw(BoardRenderTarget, new Rectangle(rx, ry, boardW, boardH), Color.White);
        SpriteBatch.End();
    }

    protected virtual void DrawBorder()
    {
        SpriteBatch.DrawRectangle(0, Layout.BoardStartY, BoardRenderTarget.Width,
            BoardRenderTarget.Height - Layout.BoardStartY, Color.White, Layout.BoardBorderThickness);
    }

    protected virtual void DrawNextQueue()
    {
        int i = Layout.BoardBorderThickness + 3*64;
        int initialI = i;
        foreach (var shape in Mode.Next)
        {
            var s = (int)(Layout.PreviewMinoSize * (i == initialI ? 1 : 1f / (Mode.NextAmount)));
            foreach (var (bx, by) in shape.BlockOffsets[0])
            {
                DrawBlock(Mode.GetPieceKindForShape(shape), i + bx * s,
                    Layout.PreviewMargin + by * s, s);
            }

            i += 4 * s;
        }
    }

    protected virtual void DrawBoardBlock(BlockKind kind, int x, int y, float blackness = 0, float opacity = 1)
    {
        // board render target minus border is 640x1280, so a single mino is 64x64
        // y - 1 is there because of the vanish row, the first actual row that should be visible is row y=1
        DrawBlock(kind, x * 64 + Layout.BoardBorderThickness, (y - 1) * 64 + Layout.BoardStartY, 64, blackness,
            opacity);
    }

    protected virtual void DrawBlock(BlockKind kind, int screenX, int screenY, int size, float blackness = 0, float opacity = 1)
    {
        var blacked = Color.Multiply(kind.Color(), 1 - blackness);
        blacked.A = Byte.MaxValue; // set up for the lerp because multiplying the alpha was never actually wanted
        var alpha = Color.Lerp(Color.Transparent, blacked, opacity);
        // SpriteBatch.FillRectangle(new Vector2(screenX, screenY), new Size2(size, size), alpha);
        SpriteBatch.Draw(BlockTexture, new Rectangle(screenX + 64 - size, screenY + 64 - size,size, size), alpha);
    }

    public override void UnloadContent()
    {
        Console.WriteLine("unloadContent");
        BoardRenderTarget.Dispose();
        // BlockTexture.Dispose();
        base.UnloadContent();
    }
}

public struct LayoutInfo
{
    public int Margin = 40;
    public int BoardBorderThickness = 5;
    public int PreviewMinoSize = 64;
    public int PreviewMargin = 10;
    public int PreviewHeight => 4 * PreviewMinoSize + 2 * PreviewMargin;
    public int BoardStartY => BoardBorderThickness + PreviewHeight;


    public LayoutInfo()
    {
    }
}