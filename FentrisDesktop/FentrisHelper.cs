using FentrisDesktop.Board;
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
}