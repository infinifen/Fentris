using System;
using System.Collections.Generic;
using FentrisDesktop.Sound;

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
        Combo *= 0.99;
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
        var baseIncrement = n + 0.5 * Math.Pow(n, 1 + n * 0.2);

        Combo += n * (n * 0.5 * (1 + Level / 200f));
        Score += (long) (148 * baseIncrement * Combo);
    }

    protected override void LockPiece()
    {
        Combo = Math.Max(1, Combo);
        base.LockPiece();
    }

    public override void Frame(GamemodeInputs input)
    {
        Combo *= 0.9996;
        base.Frame(input);
        if (Level >= 400 && State != GamemodeState.LineClear)
        {
            State = GamemodeState.Clear;
        }
    }
    
    public override void SaveRecord(FentrisHighscores highscores)
    {
        var ferocity = highscores.FerocityUnlocked;
        
        if (Score > highscores.Normal.Score)
        {
            highscores.Normal = new Result
            {
                Frames = GameplayFrames,
                Level = Level,
                Lines = LinesCleared,
                Score = Score
            };
        }

        if (!ferocity && highscores.FerocityUnlocked)
        {
            Sfx.PlaySound(SoundEffects.Unlock);
        }
    }
    
    public class Result
    {
        public int Level { get; set; }
        public long Score { get; set; }
        public int Lines { get; set; }
        public int Frames { get; set; }
    }

    public NormalGamemode(ISoundEffectManager sfxManager) : base(sfxManager)
    {
    }
}