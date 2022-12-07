using System;
using MonoGame.Extended.Collections;

namespace FentrisDesktop.Board;

public class TestRandomizer : IRandomizer
{
    public PieceShape GenerateNext()
    {
        var numPieces = Tetrominoes.All.Length;
        return Tetrominoes.All[Random.Shared.Next(numPieces)];
    }
}