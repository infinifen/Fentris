using FontStashSharp;
using Microsoft.Xna.Framework;

namespace FentrisDesktop.Gamemode;

public class GradedRenderer : GamemodeRenderer
{
    private new GradedGamemode Mode;
    public GradedRenderer(FentrisGame game, Gamemode mode) : base(game, mode)
    {
        Mode = (GradedGamemode)mode;
    }

    protected override void DrawScoring()
    {
        base.DrawScoring();
        SpriteBatch.Begin();
        var boardRect = CalculateBoardRect();
        var gradeInfo = "Grade";
        var grade = Mode.Grade.ToString();

        (var gradePos, var gradeInfoPos) = FentrisHelper.GetScoringLayout(
            boardRect, 0.3f, 10, 10, gradeInfo, Game.MediumFont, grade, Game.LargeFont
        );

        SpriteBatch.DrawString(Game.LargeFont, grade, gradePos, Color.White);
        SpriteBatch.DrawString(Game.MediumFont, gradeInfo, gradeInfoPos, Color.White);
        
        var sniInfo = "SNI";
        var sni = Mode.Sni.ToString();

        (var sniPos, var sniInfoPos) = FentrisHelper.GetScoringLayout(
            boardRect, 0.5f, 10, 10, sniInfo, Game.MediumFont, sni, Game.LargeFont
        );

        SpriteBatch.DrawString(Game.LargeFont, sni, sniPos, Color.White);
        SpriteBatch.DrawString(Game.MediumFont, sniInfo, sniInfoPos, Color.White);
        SpriteBatch.End();
    }
}