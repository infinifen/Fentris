using FontStashSharp;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace FentrisDesktop.Gamemode;

public class ApocalypseRenderer : GamemodeRenderer
{
    private new ApocalypseGamemode Mode;
    public ApocalypseRenderer(FentrisGame game, Gamemode mode) : base(game, mode)
    {
        Mode = (ApocalypseGamemode)mode;
    }
    
    protected override void DrawBorder()
    {
        SpriteBatch.DrawRectangle(0, Layout.BoardStartY, BoardRenderTarget.Width,
            BoardRenderTarget.Height - Layout.BoardStartY, Color.Red, Layout.BoardBorderThickness);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        SpriteBatch.Begin();
        SpriteBatch.DrawString(((FentrisGame)Game).LargeFont, $"SPEED: {Mode.SpeedLv:.0F}", new Vector2(0, 320), Color.Red);
        SpriteBatch.End();
    }
}