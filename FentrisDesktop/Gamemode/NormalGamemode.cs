using System;
using System.Collections.Generic;

namespace FentrisDesktop.Gamemode;

public class NormalGamemode : Gamemode
{
    public override string Id => "normal";
    
    public long Score = 0;
    private double _combo;

    public double Combo
    {
        get => _combo;
        set => _combo = Math.Max(1, value);
    }

    protected override void OnNoLineClear()
    {
        Combo *= 0.98;
    }

    protected override void OnLineClear(List<int> full)
    {
        Level += CurrentFullRows.Count switch
        {
            1 => 1,
            2 => 2,
            3 => 3,
            4 => 5,
            _ => 5
        };
        var n = CurrentFullRows.Count;
        var baseIncrement = Math.Pow(n, 1 + n * 0.2);
        Combo += n * (n * 0.5);

        Score += (long) (148 * baseIncrement * Combo * Level / 8f);
    }

    protected override void LockPiece()
    {
        Combo = Math.Max(1, Combo);
        base.LockPiece();
        Console.WriteLine($"combo ={Combo} score = ${Score}");
    }

    public override void Frame(GamemodeInputs input)
    {
        Combo *= 0.999;
        base.Frame(input);
    }
}