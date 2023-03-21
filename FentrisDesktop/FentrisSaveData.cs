using FentrisDesktop.Config;

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
                ApocalypseLevel = -2,
                ScoreAttackScore = -2,
                BeginnerScore = -2
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
    public long BeginnerScore { get; set; }
    public long ScoreAttackScore { get; set; }
    public int ApocalypseLevel { get; set; }

    public override string ToString()
    {
        return $"BeginnerScore: {BeginnerScore}, ScoreAttackScore: {ScoreAttackScore}, ApocalypseLevel: {ApocalypseLevel}";
    }
}