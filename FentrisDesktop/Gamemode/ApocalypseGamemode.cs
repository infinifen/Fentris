using System;

namespace FentrisDesktop.Gamemode;

public class ApocalypseGamemode : Gamemode
{
    public override string Id => "apocalypse";
    public double SpeedLv { get; private set; } = 0;
    public override int Gravity => 5120;

    public override int Das => Math.Clamp(LockDelay - 7, 5, 12);

    public override int Arr => 1;
    public override int Are => (int)Math.Clamp(LockDelay * 1.4, 5, 20);
    public override int LineAre => Are - 4;
    public override int LineClearDelay => 5;
    public override int LockDelay => (int)SpeedLv switch
    {
        >= 1000 => 3,
        >= 900 => 5,
        >= 850 => 6,
        >= 800 => 7,
        >= 777 => 8,
        >= 700 => 9,
        >= 650 => 10,
        >= 625 => 11,
        >= 600 => 12,
        >= 590 => 13,
        >= 570 => 14,
        >= 550 => 15,
        >= 500 => 18,
        >= 400 => 21,
        >= 300 => 25,
        >= 250 => 27,
        >= 200 => 30,
        >= 150 => 35,
        >= 100 => 40,
        >= 75 => 45,
        >= 50 => 50,
        _ => 60,
    };

    protected override void LockPiece()
    {
        base.LockPiece();
        SpeedLv += 4;
        SpeedLv *= CurrentFullRows.Count switch
        {
            0 => 1,
            1 => 1,
            2 => 0.98,
            3 => 0.91,
            _ => 0.8
        };
        Console.WriteLine($"speed: {SpeedLv}");
    }

    public override void SaveRecord(FentrisHighscores highscores)
    {
        if (Level > highscores.Apocalypse.Level)
        {
            highscores.Apocalypse = new Result
            {
                Frames = LastStateChangeFrame,
                Level = Level,
                Lines = LinesCleared
            };
        }
    }

    public class Result
    {
        public int Level { get; set; }
        public int Frames { get; set; }
        public int Lines { get; set; }
    }
}