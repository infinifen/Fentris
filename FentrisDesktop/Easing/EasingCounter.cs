using System;
using Microsoft.Xna.Framework;

namespace FentrisDesktop.Easing;

public class EasingCounter
{
    public static double LerpPrecise(double value1, double value2, double amount) => ((1.0 - amount) * value1 + value2 * amount);

    public double Goal
    {
        get => _goal;
        set
        {
            if (value != _goal)
            {
                Console.WriteLine("goal set true");
                goalSet = true;
                _goal = value;
            }
        }
    }
    private double _goal;
    private bool goalSet = false;
    private TimeSpan _prevGoalTime;
    public double Value { get; protected set; }
    public double TimeScale { get; }

    public Func<double, double> Easing;

    public EasingCounter(Func<double, double> easing, double goal = 0, double value = 0, double timeScale = 1)
    {
        Easing = easing;
        Goal = goal;
        Value = value;
        TimeScale = timeScale;
    }

    public void Update(GameTime gt)
    {
        if (goalSet)
        {
            _prevGoalTime = gt.TotalGameTime;
            goalSet = false;
        }

        var sinceSet = gt.TotalGameTime - _prevGoalTime;
        var x = sinceSet.TotalSeconds / TimeScale;
        Console.WriteLine(x);
        // var x = Math.Clamp(sinceSet.TotalSeconds / TimeScale, 0, 1);

        Value = LerpPrecise(Value, _goal, Easing(x));
    }

}