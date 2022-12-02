using System;

namespace FentrisDesktop.Board;

public class TestRandomizer : IRandomizer
{
    public PieceShape GenerateNext()
    {
        return Random.Shared.NextDouble() > 0.5 ? Tetrominoes.I : Tetrominoes.O;
    }
}