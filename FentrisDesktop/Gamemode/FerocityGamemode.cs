using FentrisDesktop.Board;

namespace FentrisDesktop.Gamemode;

public class FerocityGamemode : Gamemode
{
    public override int Gravity => Level switch
    {
        >= 4 => 5120,
        3 => 2048,
        2 => 1024,
        1 => 512,
        0 => 256,
    };

    public override int Are => Level switch
    {
        >= 950 => 4,
        >= 900 => 6,
        >= 500 => 9,
        >= 200 => 12,
        _ => 15,
    };

    public override int LineAre => Are + Level switch
    {
        >= 500 => 0,
        >= 300 => 1,
        _ => 2,
    };

    public override int LineClearDelay => Level switch
    {
        >= 950 => 2,
        >= 900 => 4,
        >= 700 => 6,
        >= 500 => 8,
        >= 300 => 10,
        >= 100 => 12,
        _ => 15,
    };

    public override int LockDelay => Level switch
    {
        >= 950 => 7,
        >= 900 => 9,
        >= 800 => 10,
        >= 700 => 13,
        >= 600 => 13,
        >= 500 => 15,
        >= 400 => 15,
        >= 300 => 18,
        >= 200 => 21,
        >= 100 => 24,
        _ => 27,
    };
    public override int Arr => 1;

    public override int Das => Level switch
    {
        >= 700 => 5,
        >= 400 => 6,
        >= 300 => 7,
        _ => 8,
    };

    protected override void OnStart()
    {
        base.OnStart();
        // Level = 690;
    }

    public override void Frame(GamemodeInputs input)
    {
        base.Frame(input);
        if (Level >= 999 && State != GamemodeState.LineClear)
        {
            Level = 999;
            State = GamemodeState.Clear;
        }
    }
    
    protected override void OnPieceLock()
    {
        if (Level % SectionLength != SectionLength - 1 && Level != 998)
        {
            // is not levelstopped
            Level++;
        }
    }

    public override BlockKind GetPieceKindForShape(PieceShape shape)
    {
        if (Level >= 700 && Level < 950)
        {
            return BlockKind.Bone;
        }
        return base.GetPieceKindForShape(shape);
    }

    public class Result
    {
        public int Level { get; set; }
        public int Frames { get; set; }
        public int Lines { get; set; }
    }
    
    public override void SaveRecord(FentrisHighscores highscores)
    {
        if (Level > highscores.Ferocity.Level)
        {
            highscores.Ferocity = new Result
            {
                Frames = GameplayFrames,
                Level = Level,
                Lines = LinesCleared
            };
        }
    }
}