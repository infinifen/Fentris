﻿using System;
using FentrisDesktop.Board;
using FentrisDesktop.Easing;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Content;
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
    protected Effect SkyShader;
    protected Texture2D SkyTexture;
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
        SkyShader = Content.Load<Effect>("SkyBg");
        SkyTexture = Content.Load<Texture2D>("sky-dark-4k");
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

        BeforeFrame(gameTime);
        Mode.Frame(inputs);
        AfterFrame(gameTime);
        
        InputHandler.CycleInputStates();
    }

    protected virtual void BeforeFrame(GameTime gameTime)
    {
    }
    
    protected virtual void AfterFrame(GameTime gameTime)
    {
    }

    public override void Draw(GameTime gameTime)
    {
        DrawBoard();
        DrawBackground(gameTime);
        // draw board to screen
        SpriteBatch.Begin();
        SpriteBatch.Draw(BoardRenderTarget, CalculateBoardRect(), Color.White);
        SpriteBatch.End();
        DrawScoring();

        SpriteBatch.Begin();
        DrawTimer();

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

    protected void DrawBackground(GameTime gameTime)
    {
        SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        SkyShader.Parameters["tint1"].SetValue(Tint1);
        SkyShader.Parameters["tint2"].SetValue(Tint2);
        SkyShader.Parameters["distScale"].SetValue(DistScale);
        SkyShader.Parameters["timeScale"].SetValue(TimeScale);
        SkyShader.Parameters["time"].SetValue((float) gameTime.TotalGameTime.TotalSeconds);
        SkyShader.Techniques["SpriteDrawing"].Passes[0].Apply();
        var (bgPos, bgScale) = ComputeBackgroundScale(SkyTexture);
        SpriteBatch.Draw(SkyTexture, bgPos, scale: bgScale, sourceRectangle: null, color: Color.White,
            origin: Vector2.Zero, effects: SpriteEffects.None, rotation: 0f, layerDepth: 0f);
        SpriteBatch.End();
    }

    public virtual Vector4 Tint1 => new Vector4(0.6f, 0.0f, 0.8f, 1f);
    public virtual Vector4 Tint2 => new Vector4(0.8f, 0.0f, 0.6f, 1f);
    public virtual float TimeScale => 1.2f;
    public virtual float DistScale => 6f;

    private (Vector2, float) ComputeBackgroundScale(Texture2D tex)
    {
        var xB = tex.Width;
        var yB = tex.Height;

        var xR = Game.W / (float)xB;
        var yR = Game.H / (float)yB;
        var scale = Math.Max(xR, yR);

        var excessX = xB * scale - Game.W;
        var excessY = yB * scale - Game.H;

        return (new Vector2(-excessX / 2, -excessY / 2), scale);
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

    protected virtual Rectangle CalculateBoardRect()
    {
        var boardH = Game.H - Layout.Margin * 2;
        var boardW = (int)(boardH / ((float)BoardRenderTarget.Height / BoardRenderTarget.Width));

        var rx = Game.W / 2 - boardW / 2;
        var ry = Layout.Margin;
        return new Rectangle(rx, ry, boardW, boardH);
    }
    
    protected virtual void DrawBoard()
    {
        Game.GraphicsDevice.SetRenderTarget(BoardRenderTarget);
        Game.GraphicsDevice.Clear(new Color(0, 0, 0, 180));

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

        if (Mode.State is GamemodeState.Are or GamemodeState.LineClear && Mode.SinceLastStateChange == 1)
        {
            foreach ( var (bx, by) in Mode.ActivePiece.GetBlockOffsets())
            {
                LockFlashFill(Mode.ActivePiece.X + bx, Mode.ActivePiece.Y + by);
            }
        }

        DrawNextQueue();

        DrawBorder();

        SpriteBatch.End();
        Game.GraphicsDevice.SetRenderTarget(null);
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

    protected virtual void LockFlashFill(int x, int y)
    {
        var screenX = x * 64 + Layout.BoardBorderThickness;
        var screenY = (y - 1) * 64 + Layout.BoardStartY;
        var size = 64;
        SpriteBatch.FillRectangle(new Vector2(screenX, screenY), new Size2(size, size), Color.White);
    }

    protected virtual void DrawBlock(BlockKind kind, int screenX, int screenY, int size, float blackness = 0, float opacity = 1)
    {
        var blacked = Color.Multiply(kind.Color(), 1 - blackness);
        blacked.A = Byte.MaxValue; // set up for the lerp because multiplying the alpha was never actually wanted
        var alpha = Color.Lerp(Color.Transparent, blacked, opacity);
        // SpriteBatch.FillRectangle(new Vector2(screenX, screenY), new Size2(size, size), alpha);
        SpriteBatch.Draw(BlockTexture, new Rectangle(screenX + 64 - size, screenY + 64 - size,size, size), alpha);
    }

    protected virtual void DrawScoring()
    {
        SpriteBatch.Begin();
        var boardRect = CalculateBoardRect();
        var levelInfo = "Level";
        var LevelStr = Mode.Level.ToString();

        (var levelPos, var levelInfoPos) = FentrisHelper.GetScoringLayout(
            boardRect, 0.8f, 10, 10, levelInfo, Game.MediumFont, LevelStr, Game.LargeFont
        );

        SpriteBatch.DrawString(Game.LargeFont, LevelStr, levelPos, Color.White);
        SpriteBatch.DrawString(Game.MediumFont, levelInfo, levelInfoPos, Color.White);
        SpriteBatch.End();
    }

    protected virtual void DrawTimer()
    {
        var time = FentrisHelper.FramesToTime(Mode.GameplayFrames);
        var timeStr = $"{time.m:D2}:{time.s:D2}:{time.cs:D2}";
        var textSize = Game.LargeFont.MeasureString(timeStr);
        var posX = (Game.W - textSize.X) / 2f;
        var posY = (CalculateBoardRect().Y - textSize.Y);
        SpriteBatch.DrawString(Game.LargeFont, timeStr, new Vector2(posX, posY), Color.White);
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