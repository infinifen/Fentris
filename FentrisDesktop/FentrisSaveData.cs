using System;
using FentrisDesktop.Config;
using FentrisDesktop.Gamemode;

namespace FentrisDesktop;

public class FentrisSaveData
{
    public KeyConfig Keybinds { get; set; }
    public long Mastery { get; set; }
    public FentrisHighscores Highscores { get; set; }

    public static FentrisSaveData Default()
    {
        return new FentrisSaveData
        {
            Highscores = new FentrisHighscores
            {
                Beginner = new BeginnerMarathonGamemode.Result
                {
                    Frames = 0,
                    Level = 0,
                    Lines = 0,
                    Score = 0,
                },
                Normal = new NormalGamemode.Result
                {
                    Frames = 0,
                    Level = 0,
                    Lines = 0,
                    Score = 0,
                },
                Apocalypse = new ApocalypseGamemode.Result
                {
                    Frames = 0,
                    Level = 0,
                    Lines = 0,
                },
                Ferocity = new FerocityGamemode.Result
                {
                    Frames = 0,
                    Level = 0,
                    Lines = 0,
                }
            },
            Keybinds = KeyConfig.Default(),
            Mastery = 0,
        };
    }

    public override string ToString()
    {
        return $"Highscores: {Highscores} is null {Highscores==null}, Keybinds: {Keybinds}, Mastery: {Mastery}";
    }
}

public class FentrisHighscores
{
    public BeginnerMarathonGamemode.Result Beginner { get; set; }
    public NormalGamemode.Result Normal { get; set; }
    public ApocalypseGamemode.Result Apocalypse { get; set; }
    public FerocityGamemode.Result Ferocity { get; set; }

    public override string ToString()
    {
        return $"BeginnerScore: {Beginner.Score}, ScoreAttackScore: {Normal.Score}, ApocalypseLevel: {Apocalypse.Level}";
    }
}