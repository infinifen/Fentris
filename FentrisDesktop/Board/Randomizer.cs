namespace FentrisDesktop.Board;

public abstract class Randomizer
{
    public PieceShape GenerateNext()
    {
        return Tetrominoes.I;
    }
}