using System;
using System.Collections.Generic;
using System.Linq;
using FentrisDesktop.Sound;

namespace FentrisDesktop.Gamemode;

public class BeginnerMarathonGamemode : Gamemode
{
    public override string Id => "beginnerm";
    public override int Das => 10;
    public override int Arr => 2;
    public override int LineClearDelay => _lcd;
    private int _lcd = 20;

    public bool IsInRoll = false;
    public int RollFramesRemaining = 30 * 60;
    public int Score = 0;
    public const int RollLines = 100;

    public new int Level => 1 + LinesCleared / 10;
    
    public override int Gravity => IsInRoll ? 6588 : LinesCleared switch
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

    public override void Frame(GamemodeInputs input)
    {
        base.Frame(input);

        if (LinesCleared >= RollLines)
        {
            LinesCleared = RollLines;
            IsInRoll = true;
        }

        if (IsInRoll && !State.IsFinished())
        {
            RollFramesRemaining--;
        }

        if (RollFramesRemaining == 0)
        {
            LockPiece();
            if (State != GamemodeState.Clear)
            {
                Sfx.PlaySoundOnce(SoundEffects.Finish);
            }
            State = GamemodeState.Clear;
        }
    }
    
    protected override void OnLineClear(List<int> full)
    {
        var baseScore = full.Count switch
        {
            1 => 100,
            2 => 300,
            3 => 700,
            4 => 1400,
        };

        var speedBonus = Math.Max(120 - SinceLastStateChange, 0);

        Score += (baseScore + speedBonus) * Level * (IsInRoll ? 0 : 1);

        if (LinesCleared + full.Count >= RollLines && !IsInRoll)
        {
            _lcd = 180;
        }
    }

    protected override void OnNoLineClear()
    {
        if (IsInRoll) return;
        
        var speedBonus = Math.Max((120 - SinceLastStateChange) / 4, 0);
        Score += speedBonus;
    }

    protected override void OnPieceLock() { }

    protected override void LockPiece()
    {
        Board.PlacePiece(ActivePiece, FrameCount);
        PiecesPlaced++;
        OnPieceLock();
        Sfx.PlaySound(SoundEffects.Drop);
        var full = Board.FullRows().ToList();
        if (full.Any())
        {
            OnLineClear(full);
            State = GamemodeState.LineClear;
            CurrentFullRows = full;
            LinesCleared += CurrentFullRows.Count;
        }
        else
        {
            OnNoLineClear();
            State = GamemodeState.Are;
        }
    }

    protected override void OnPieceEnter(ref GamemodeInputs input)
    {
        base.OnPieceEnter(ref input);
        _lcd = 20;
    }
    
    public override void SaveRecord(FentrisHighscores highscores)
    {
        Console.WriteLine(highscores);
        Console.WriteLine(Score);
        if (Score > highscores.Beginner.Score)
        {
            highscores.Beginner = new Result
            {
                Frames = GameplayFrames,
                Level = Level,
                Lines = LinesCleared,
                Score = Score
            };
        }
    }
    
    public class Result
    {
        public int Level { get; set; }
        public int Score { get; set; }
        public int Lines { get; set; }
        public int Frames { get; set; }
    }

    public BeginnerMarathonGamemode(ISoundEffectManager sfxManager) : base(sfxManager)
    {
    }
}