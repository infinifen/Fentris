using FontStashSharp;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace FentrisDesktop.Gamemode;

public class NormalRenderer : GamemodeRenderer
{
    private new NormalGamemode Mode;
    public NormalRenderer(FentrisGame game, Gamemode mode) : base(game, mode)
    {
        Mode = (NormalGamemode)mode;
    }
    
    protected override void DrawBorder()
    {
        SpriteBatch.DrawRectangle(0, Layout.BoardStartY, BoardRenderTarget.Width,
            BoardRenderTarget.Height - Layout.BoardStartY, Color.White, Layout.BoardBorderThickness);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        SpriteBatch.Begin();
        SpriteBatch.DrawString(DebugFont, Mode.Combo.ToString(), new Vector2(0, 300), Color.Aqua);
        SpriteBatch.DrawString(((FentrisGame)Game).LargeFont, Mode.Score.ToString(), new Vector2(0, 320), Color.Red);
        SpriteBatch.End();
    }
}