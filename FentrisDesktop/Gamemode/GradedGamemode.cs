using System;
using System.Collections.Generic;
using FentrisDesktop.Sound;

namespace FentrisDesktop.Gamemode;

public class GradedGamemode : Gamemode
{
    public override string Id => "graded";

    public int SniNumerator = 100;
    public int SniDenominator = 100;
    public double Sni => (double)SniNumerator / SniDenominator;

    public double NeatnessGrades;
    public double AuxiliaryGrades;
    public double SpeedGrades;
    public double AuxGradeMultiplier = 1;

    public double Grade => NeatnessGrades + AuxiliaryGrades + SpeedGrades;
    public int GradeInt => (int)Math.Floor(Grade);
    public int Section => Level / 100;

    public override int Gravity => Level switch
    {
        >= 500 => 20 * 256,
        >= 400 => 928,
        >= 300 => 528 + (Level - 300) * 4,
        >= 200 => 128 + (Level - 200) * 4,
        >= 170 => 128,
        >= 150 => 112,
        >= 100 => 96,
        >= 000 => Level / 2 + 16, // 75 at 99
        _ => throw new ArgumentOutOfRangeException()
    };

    public override int Arr => 1;
    public override int Das => Section switch
    {
        >= 3 => 6,
        2 => 7,
        1 => 8,
        _ => 10,
    };

    public override int Are => Section switch
    {
        12 => 1,
        11 => 6,
        10 => 8,
        9 => 8,
        8 => 12,
        7 => 12,
        6 => 15,
        5 => 15,
        4 => 16,
        3 => 16,
        2 => 18,
        1 => 18,
        _ => 23,
    };

    public override int LockDelay => Section switch
    {
        >= 11 => 13,
        10 => 15,
        9 => 18,
        8 => 22,
        7 => 27,
        6 => 30,
        _ => 40
    };

    public override int LineAre => Section switch
    {
        >= 9 => 10,
        >= 5 => 15,
        4 => 18,
        _ => 24,
    };

    public override int LineClearDelay => LineAre / 2;

    public double SniRequirement => Section switch {
        0 => 0.5,
        1 => 0.55,
        2 => 0.6,
        3 => 0.65,
        4 => 0.7,
        5 => 0.5,
        6 => 0.6,
        7 => 0.7,
        8 => 0.8,
        9 => 0.82,
        10 => 0.86,
        11 => 0.9,
        12 => 1,
        _ => throw new ArgumentOutOfRangeException()
    };
    
    public GradedGamemode(ISoundEffectManager gameSfxManager) : base(gameSfxManager)
    {
        // Level = 1090;
    }

    protected override void OnPieceLock()
    {
        if (Level % SectionLength != SectionLength - 1)
        {
            // is not levelstopped
            ProcessSniGrading(1);
            Level++;
        }
    }

    private void ProcessSniGrading(int levelsProgressed)
    {
        if (Sni > SniRequirement)
        {
            NeatnessGrades += 0.01 * levelsProgressed;
        }
    }


    protected override void OnLineClear(List<int> full)
    {
        // sni grades are processed using new sni, but old level
        
        var levelIncrement = CurrentFullRows.Count switch
        {
            1 => 1,
            2 => 2,
            3 => 3,
            4 => 5,
            _ => 5
        };

        SniNumerator += full.Count - 1;
        SniDenominator += 3;
        Console.WriteLine($"sni = {Sni}");
        
        ProcessSniGrading(levelIncrement);

        Level += levelIncrement;
    }
}