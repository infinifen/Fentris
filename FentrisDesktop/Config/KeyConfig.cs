using Microsoft.Xna.Framework.Input;

namespace FentrisDesktop.Config;

public class KeyConfig
{
    public Keys Start { get; set; }
    public Keys Back { get; set; }
    public Keys MenuUp { get; set; }
    public Keys MenuDown { get; set; }
    public Keys Left { get; set; }
    public Keys Right { get; set; }
    public Keys Sonic { get; set; }
    public Keys Soft { get; set; }
    public Keys RotateCw { get; set; }
    public Keys SecondaryRotateCw { get; set; }
    public Keys RotateCcw { get; set; }
    public Keys SecondaryRotateCcw { get; set; }
    public const int KeyCount = 12;

    public KeyConfig(Keys start, Keys back, Keys menuUp, Keys menuDown, Keys left, Keys right, Keys sonic, Keys soft, Keys rotateCw, Keys secondaryRotateCw, Keys rotateCcw, Keys secondaryRotateCcw)
    {
        Start = start;
        Back = back;
        MenuUp = menuUp;
        MenuDown = menuDown;
        Left = left;
        Right = right;
        Sonic = sonic;
        Soft = soft;
        RotateCw = rotateCw;
        SecondaryRotateCw = secondaryRotateCw;
        RotateCcw = rotateCcw;
        SecondaryRotateCcw = secondaryRotateCcw;
    }


    public override string ToString()
    {
        return $"Left: {Left}, Right: {Right}, Sonic: {Sonic}, Soft: {Soft}, RotateCw: {RotateCw}, SecondaryRotateCw: {SecondaryRotateCw}, RotateCcw: {RotateCcw}, SecondaryRotateCcw: {SecondaryRotateCcw}, Start: {Start}, Back: {Back}";
    }

    public static KeyConfig Default()
    {
        return new KeyConfig(Keys.Enter, Keys.Escape, Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Space, Keys.Down, Keys.Up, Keys.X, Keys.Z, Keys.C);
    }
}