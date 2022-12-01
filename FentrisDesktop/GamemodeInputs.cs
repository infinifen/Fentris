namespace FentrisDesktop;

public struct GamemodeInputs
{
    // the bools might be replaced by some other enum if i want to do DTET-style rotation attempts or anything like that
    public bool RotateCw;
    public bool RotateCcw;
    public DirectionInput Direction;
    public bool SonicDrop;
    public bool SoftDrop;
    public bool IrsCw;
    public bool IrsCcw;
}

public enum DirectionInput
{
    Left, 
    Neutral,
    Right
}