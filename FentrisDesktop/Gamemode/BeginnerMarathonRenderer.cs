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
        var borderColor = Mode.IsInRoll ? Color.Gold : Color.Green;
        SpriteBatch.DrawRectangle(0, Layout.BoardStartY, BoardRenderTarget.Width,
            BoardRenderTarget.Height - Layout.BoardStartY, borderColor, Layout.BoardBorderThickness);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        SpriteBatch.Begin();
        SpriteBatch.DrawString(Game.MediumFont, Mode.LinesCleared.ToString(), new Vector2(0, 320), Color.Red);
        SpriteBatch.DrawString(Game.LargeFont, Mode.Score.ToString(), Vector2.Zero, Color.White);
        SpriteBatch.End();
    }
}