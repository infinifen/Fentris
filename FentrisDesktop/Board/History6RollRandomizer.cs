using System;
using System.Collections.Generic;

namespace FentrisDesktop.Board;

public class History6RollRandomizer : IRandomizer
{
    protected Queue<PieceShape> History = new(4);
    protected const int Rerolls = 6;

    public History6RollRandomizer()
    {
        History.Enqueue(Tetrominoes.S);
        History.Enqueue(Tetrominoes.Z);
        History.Enqueue(Tetrominoes.S);
        History.Enqueue(Tetrominoes.Z);
    }

    public PieceShape GenerateNext()
    {
        for (int i = 0; i < Rerolls; i++)
        {
            var numPieces = Tetrominoes.All.Length;
            var roll = Tetrominoes.All[Random.Shared.Next(numPieces)];

            if (!History.Contains(roll))
            {
                CycleHistory(roll);
                return roll;
            }
        }
        
        var n = Tetrominoes.All.Length;
        return Tetrominoes.All[Random.Shared.Next(n)];
    }

    private void CycleHistory(PieceShape roll)
    {
        History.Dequeue();
        History.Enqueue(roll);
    }
}