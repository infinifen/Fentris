namespace FentrisDesktop.Gamemode;

public class BeginnerMarathonGamemode : Gamemode
{
    public override string Id => "beginnerm";
    public override int Das => 10;
    public override int Arr => 2;

    public override int Gravity => LinesCleared switch
    {
        >= 90 => 96,
        >= 80 => 64,
        >= 70 => 48,
        >= 60 => 36,
        >= 50 => 24,
        >= 40 => 18,
        >= 30 => 12,
        >= 20 => 6,
        >= 10 => 3,
        _ => 1
    };
}