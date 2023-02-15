using FontStashSharp;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace FentrisDesktop.Gamemode;

public class BeginnerMarathonRenderer : GamemodeRenderer
{
    private new BeginnerMarathonGamemode Mode;
    public BeginnerMarathonRenderer(FentrisGame game, Gamemode mode) : base(game, mode)
    {
        Mode = (BeginnerMarathonGamemode)mode;
    }
    
    protected override void DrawBorder()
    {
        SpriteBatch.DrawRectangle(0, Layout.BoardStartY, BoardRenderTarget.Width,
            BoardRenderTarget.Height - Layout.BoardStartY, Color.Green, Layout.BoardBorderThickness);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        SpriteBatch.Begin();
        SpriteBatch.DrawString(Game.LargeFont, Mode.LinesCleared.ToString(), new Vector2(0, 320), Color.Red);
        SpriteBatch.End();
    }
}