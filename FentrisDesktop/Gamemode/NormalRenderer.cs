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
    }

    protected override void DrawScoring()
    {
        base.DrawScoring();
        
        SpriteBatch.Begin();
        var boardRect = CalculateBoardRect();
        var multInfo = "";
        var multStr = $"x{Mode.Combo:F2}";
        var scoreInfo = "Score";
        var scoreStr = Mode.Score.ToString();

        (var multPos, var multInfoPos) = FentrisHelper.GetScoringLayout(
            boardRect, 0.55f, 10, 10, multInfo, Game.MediumFont, multStr, Game.MediumFont
        );
        
        (var scorePos, var scoreInfoPos) = FentrisHelper.GetScoringLayout(
            boardRect, 0.6f, 10, 10, scoreInfo, Game.MediumFont, scoreStr, Game.LargeFont
        );

        SpriteBatch.DrawString(Game.MediumFont, multStr, multPos, Color.Aqua);
        SpriteBatch.DrawString(Game.LargeFont, scoreStr, scorePos, Color.White);
        SpriteBatch.DrawString(Game.MediumFont, scoreInfo, scoreInfoPos, Color.White);
        SpriteBatch.DrawString(Game.MediumFont, multInfo, multInfoPos, Color.White);
        SpriteBatch.End();
    }
}