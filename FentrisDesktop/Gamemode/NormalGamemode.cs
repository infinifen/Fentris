using System;
using System.Collections.Generic;

namespace FentrisDesktop.Gamemode;

public class NormalGamemode : Gamemode
{
    public override int Arr => 1;
    public override int Das => 8;

    public override int Gravity => Level switch
    {
        > 400 => 9999,
        > 350 => 320,
        > 320 => 200,
        > 300 => 160,
        > 250 => 120,
        > 200 => 80,
        > 140 => 64,
        > 100 => 48,
        > 80 => 32,
        > 50 => 24,
        > 40 => 20,
        > 30 => 16,
        > 20 => 12,
        > 10 => 8,
        _ => 4,
    };
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

        Score += (long) (148 * baseIncrement * Combo * Level / 40f);
    }

    protected override void LockPiece()
    {
        Combo = Math.Max(1, Combo);
        base.LockPiece();
    }

    public override void Frame(GamemodeInputs input)
    {
        Combo *= 0.999;
        if (Level > 400 && State == GamemodeState.Are)
        {
            State = GamemodeState.Clear;
        }
        base.Frame(input);
    }
}