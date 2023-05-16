using System;
using FentrisDesktop.Board;
using FentrisDesktop.Gamemode;
using FontStashSharp;
using Microsoft.Xna.Framework;

namespace FentrisDesktop;

public static class FentrisHelper
{
    public static int Mod(int x, int m)
    {
        return (x % m + m) % m;
    }

    public static Color Color(this BlockKind bk)
    {
        return bk switch
        {
            BlockKind.Blue => new Microsoft.Xna.Framework.Color(32, 32, 255, 255),
            BlockKind.Bone => Microsoft.Xna.Framework.Color.Gray,
            BlockKind.Clear => Microsoft.Xna.Framework.Color.Transparent,
            BlockKind.Cyan => Microsoft.Xna.Framework.Color.Cyan,
            BlockKind.Garbage => Microsoft.Xna.Framework.Color.DarkGray,
            BlockKind.Green => new Microsoft.Xna.Framework.Color(32, 255, 32, 255),
            BlockKind.Magenta => Microsoft.Xna.Framework.Color.Magenta,
            BlockKind.Orange => Microsoft.Xna.Framework.Color.Orange,
            BlockKind.Red => Microsoft.Xna.Framework.Color.Red,
            BlockKind.Yellow => Microsoft.Xna.Framework.Color.Yellow,
            BlockKind.OutOfBounds => Microsoft.Xna.Framework.Color.Transparent,
            _ => Microsoft.Xna.Framework.Color.Transparent
        };
    }

    public static bool IsFinished(this GamemodeState state)
    {
        return state is GamemodeState.Clear or GamemodeState.Gameover;
    }

    public static (Vector2, Vector2) GetScoringLayout(Rectangle boardRect, float position,
        int marginX, int marginY,
        string infoText, DynamicSpriteFont infoFont,
        string valueText, DynamicSpriteFont valueFont
    )
    {
        var infoSize = infoFont.MeasureString(infoText);
        var valueSize = valueFont.MeasureString(valueText);
        
        var infoPos = new Vector2(boardRect.Right + marginX, boardRect.Top + boardRect.Height * position - infoSize.Y / 2f);
        var valuePos = infoPos + new Vector2(0, infoSize.Y / 2f + marginY + valueSize.Y / 2f);

        return (infoPos, valuePos);
    }
    
    public static (int m, int s, int cs) FramesToTime(int frames)
    {
        (var minutes, var rest) = Math.DivRem(frames, 60 * 60);
        (var seconds, var restFrames) = Math.DivRem(rest, 60);
        return (minutes, seconds, (int)Math.Round(restFrames / 60d * 100d));
    }
    
    public static string FormatTime((int m, int s, int cs) timeInfo)
    {
        return $"{timeInfo.m:D2}:{timeInfo.s:D2}.{timeInfo.cs:D2}";
    }
}