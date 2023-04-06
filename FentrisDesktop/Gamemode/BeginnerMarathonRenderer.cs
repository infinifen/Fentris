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
    }
    

    protected override void DrawScoring()
    {
        SpriteBatch.Begin();
        var boardRect = CalculateBoardRect();
        var lineCountInfo = "Lines";
        var lineCountStr = Mode.LinesCleared.ToString();
        var scoreInfo = "Score";
        var scoreStr = Mode.Score.ToString();

        (var lineCountPos, var lineCountInfoPos) = FentrisHelper.GetScoringLayout(
            boardRect, 0.6f, 10, 10, lineCountInfo, Game.MediumFont, lineCountStr, Game.LargeFont
        );
        
        (var scorePos, var scoreInfoPos) = FentrisHelper.GetScoringLayout(
            boardRect, 0.8f, 10, 10, scoreInfo, Game.MediumFont, scoreStr, Game.LargeFont
        );

        SpriteBatch.DrawString(Game.LargeFont, lineCountStr, lineCountPos, Color.White);
        SpriteBatch.DrawString(Game.LargeFont, scoreStr, scorePos, Color.White);
        SpriteBatch.DrawString(Game.MediumFont, scoreInfo, scoreInfoPos, Color.White);
        SpriteBatch.DrawString(Game.MediumFont, lineCountInfo, lineCountInfoPos, Color.White);
        SpriteBatch.End();
    }
}