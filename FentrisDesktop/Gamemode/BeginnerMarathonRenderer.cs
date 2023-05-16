using System;
using FentrisDesktop.Easing;
using FontStashSharp;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace FentrisDesktop.Gamemode;

public class BeginnerMarathonRenderer : GamemodeRenderer
{
    protected EasingCounter ScoreEasingCounter = new(x => 0.3 / (1 + Math.Pow(Math.E, -x)), timeScale: 8);
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
    
    protected override void AfterFrame(GameTime gameTime)
    {
        ScoreEasingCounter.Goal = Mode.Score;
        ScoreEasingCounter.Update(gameTime);
    }
    

    protected override void DrawScoring()
    {
        SpriteBatch.Begin();
        var boardRect = CalculateBoardRect();
        var lineCountInfo = !Mode.IsInRoll ? "Lines" : "20G Challenge";
        var rollTime = FentrisHelper.FramesToTime(Mode.RollFramesRemaining);
        var lineCountStr = !Mode.IsInRoll ? Mode.LinesCleared.ToString() : $"{rollTime.s:D2}:{rollTime.cs:D2}";

        var scoreInfo = "Score";
        var scoreStr = $"{ScoreEasingCounter.Value:F0}";

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