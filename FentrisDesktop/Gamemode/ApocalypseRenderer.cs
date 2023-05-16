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
    }

    public override Vector4 Tint1 => new Vector4(0.4f, 0.2f, 0.2f, 1f);
    public override Vector4 Tint2 => new Vector4(0.9f, 0.04f, 0.1f, 1f);
    public override float DistScale => 12 + (float)(Mode.SpeedLv / 200f);

    protected override void DrawScoring()
    {
        base.DrawScoring();
        SpriteBatch.Begin();
        var boardRect = CalculateBoardRect();
        var speedInfo = "Speed";
        var speedStr = $"{Mode.SpeedLv:F0}";

        (var speedPos, var speedInfoPos) = FentrisHelper.GetScoringLayout(
            boardRect, 0.6f, 10, 10, speedInfo, Game.MediumFont, speedStr, Game.LargeFont
        );

        SpriteBatch.DrawString(Game.LargeFont, speedStr, speedPos, Color.Red);
        SpriteBatch.DrawString(Game.MediumFont, speedInfo, speedInfoPos, Color.White);
        SpriteBatch.End();
    }
}