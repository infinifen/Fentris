﻿using FentrisDesktop.Board;
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
            BlockKind.Blue => Microsoft.Xna.Framework.Color.Blue,
            BlockKind.Bone => Microsoft.Xna.Framework.Color.Gray,
            BlockKind.Clear => Microsoft.Xna.Framework.Color.Transparent,
            BlockKind.Cyan => Microsoft.Xna.Framework.Color.Cyan,
            BlockKind.Garbage => Microsoft.Xna.Framework.Color.DarkGray,
            BlockKind.Green => Microsoft.Xna.Framework.Color.Green,
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
}