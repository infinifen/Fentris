using System;
using System.Collections.Generic;
using FentrisDesktop.Sound;

namespace FentrisDesktop.Gamemode;

public class GradedGamemode : Gamemode
{
    public int SniNumerator;
    public int SniDenominator;
    public double Sni => (double) SniNumerator / SniDenominator;

    public double NeatnessGrades;
    public double AuxiliaryGrades;
    public double SpeedGrades;
    public double AuxGradeMultiplier = 1;

    public double Grade => NeatnessGrades + AuxiliaryGrades + SpeedGrades;
    public int GradeInt => (int)Math.Floor(Grade);
    
    public GradedGamemode(ISoundEffectManager gameSfxManager) : base(gameSfxManager)
    {
        
    }

    protected override void OnLineClear(List<int> full)
    {
        base.OnLineClear(full);
        SniNumerator += full.Count - 1;
        SniDenominator += 3;
        Console.WriteLine($"sni = {Sni}");
    }
}