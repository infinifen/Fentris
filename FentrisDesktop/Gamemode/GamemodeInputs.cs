namespace FentrisDesktop.Gamemode;

public struct GamemodeInputs
{
    // the bools might be replaced by some other enum if i want to do DTET-style rotation attempts or anything like that
    public bool RotateCw;
    public bool RotateCcw;
    public DirectionInput Direction;
    public DirectionInput PreviousDirection;
    public bool SonicDrop;
    public bool SoftDrop;
    public bool IrsCw;
    public bool IrsCcw;

    public override string ToString()
    {
        return
            $"cw={RotateCw} ccw={RotateCcw}, dir={Direction}, pdir={PreviousDirection}, sonic={SonicDrop}, soft={SoftDrop}, icw={IrsCw}, iccw={IrsCcw}";
    }
}

public enum DirectionInput
{
    Left,
    Neutral,
    Right
}

public static class DirectionInputExt
{
    public static int ToInt(this DirectionInput di)
    {
        return di switch
        {
            DirectionInput.Left => -1,
            DirectionInput.Neutral => 0,
            DirectionInput.Right => 1
        };
    }
}