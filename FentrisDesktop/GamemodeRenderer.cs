using FentrisDesktop.Board;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;

namespace FentrisDesktop;

public class GamemodeRenderer : GameScreen
{
    private new FentrisGame Game => (FentrisGame)base.Game;
    
    protected RenderTarget2D BoardRenderTarget;
    protected SpriteBatch SpriteBatch;
    protected int BoardBorderThickness = 5;
    protected Gamemode Mode;
    protected InputHandler InputHandler;

    public GamemodeRenderer(FentrisGame game, Gamemode mode) : base(game)
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
        for (int y = 0; y < Mode.Board.board.GetLength(1); y++)
        {
            for (int x = 0; x < Mode.Board.board.GetLength(0); x++)
            {
                var block = Mode.Board[x, y];
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