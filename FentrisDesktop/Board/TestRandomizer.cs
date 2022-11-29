namespace FentrisDesktop.Board;

public class TestRandomizer : IRandomizer
{
    public PieceShape GenerateNext()
    {
        return Tetrominoes.I;
    }
}