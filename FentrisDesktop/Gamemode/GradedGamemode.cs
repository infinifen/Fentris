using System;
using System.Collections.Generic;
using System.Linq;
using FentrisDesktop.Sound;
using static FentrisDesktop.FentrisHelper;

namespace FentrisDesktop.Gamemode;

public class GradedGamemode : Gamemode
{
    public override string Id => "graded";
    
    public List<int> LineClears = new(500);
    public double Sni = 1;
    public readonly double SniPowerWeight = 0.8;

    public decimal NeatnessGrades;
    public decimal AuxiliaryGrades;
    public decimal SpeedGrades;
    public decimal AuxGradeMultiplier = 1;

    public decimal Grade => NeatnessGrades + AuxiliaryGrades + SpeedGrades;
    public int GradeInt => (int)Math.Floor(Grade);
    public int Section => Level / 100;
    public int[] SummaricSectionTimes = new int[12];
    public int[] SectionTimes = new int[12];

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
        >= 4 => 6,
        3 => 7,
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
        12 => 1.591, // no sni grades in credits
        _ => throw new ArgumentOutOfRangeException()
    };

    public (int, int) SpeedRequirements => Section switch
    {
        0 => (TimeToFrames(1, 20, 00), TimeToFrames(0, 50, 00)),
        1 => (TimeToFrames(1, 00, 00), TimeToFrames(0, 45, 00)),
        2 => (TimeToFrames(0, 55, 00), TimeToFrames(0, 42, 00)),
        3 => (TimeToFrames(0, 50, 00), TimeToFrames(0, 38, 00)),
        4 => (TimeToFrames(0, 50, 00), TimeToFrames(0, 36, 00)),
        5 => (TimeToFrames(0, 50, 00), TimeToFrames(0, 34, 00)),
        _ => (2, 1)
    };
    
    public GradedGamemode(ISoundEffectManager gameSfxManager) : base(gameSfxManager)
    {
        // Level = 1090;
        LineClears.AddRange(Enumerable.Repeat(4, 15));
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
            NeatnessGrades += 0.01m * levelsProgressed;
        }
    }


    protected override void OnLineClear(List<int> full)
    {
        // sni grades are processed using new sni, but old level
        var oldLevel = Level;
        
        var levelIncrement = CurrentFullRows.Count switch
        {
            1 => 1,
            2 => 2,
            3 => 3,
            4 => 5,
            _ => 5
        };
        
        LineClears.Add(full.Count);
        RecalculateSni();
        AddAuxiliaryGrades(full);
        
        Console.WriteLine($"sni = {Sni}");
        
        ProcessSniGrading(levelIncrement);

        Level += levelIncrement;
        if (Level / 100 > oldLevel / 100)
        {
            OnSectionTransition();
        }
    }

    protected virtual void OnSectionTransition()
    {
        var completedSection = Section - 1;
        SummaricSectionTimes[completedSection] = FrameCount;
        SectionTimes[completedSection] =
            completedSection == 0 ? FrameCount : FrameCount - SummaricSectionTimes[completedSection - 1];

        var (speedRequirementSlow, speedRequirementFast) = SpeedRequirements;
        var clamped = Math.Clamp(SectionTimes[completedSection], speedRequirementFast, speedRequirementSlow);
        var speedGradeIncrement = (speedRequirementSlow - clamped) / new decimal(speedRequirementSlow - speedRequirementFast);
        SpeedGrades += decimal.Round(speedGradeIncrement, 3, MidpointRounding.ToZero);
    }

    private void AddAuxiliaryGrades(List<int> full)
    {
        var baseScore = full.Count switch
        {
            1 => 0.001m,
            2 => 0.007m,
            3 => 0.02m,
            4 => 0.03m,
            _ => 0m,
        };

        AuxiliaryGrades += baseScore * (Section + 1) * AuxGradeMultiplier;
    }

    private void RecalculateSni()
    {
        var (num, den) = LineClears
            .Select((clear, idx) => (clear, LineClears.Count - idx - 1))
            .Select(tuple =>
            {
                var (clear, power) = tuple;
                var num = (clear - 1) * clear;
                var den = clear * 3;
                var weight = Math.Pow(SniPowerWeight, power);
                return (num * weight, den * weight);
            })
            .Aggregate((0d, 0d), (acc, val) => (acc.Item1 + val.Item1, acc.Item2 + val.Item2));

        Sni = num / den;
    }
    
    public override void Frame(GamemodeInputs input)
    {
        base.Frame(input);
        if (Level >= 1200 && State != GamemodeState.LineClear)
        {
            Level = 1200;
            if (State != GamemodeState.Clear)
            {
                Sfx.PlaySoundOnce(SoundEffects.Finish);
            }
            State = GamemodeState.Clear;
        }
    }
} 