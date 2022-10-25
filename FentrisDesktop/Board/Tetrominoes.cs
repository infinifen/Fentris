namespace FentrisDesktop.Board;

public class Tetrominoes
{
    // ARS I piece for now
    public static readonly PieceShape I = new (new []
    {
        new [] {(0, 1), (1, 1), (2, 1), (3, 1)},
        new [] {(2, 0), (2, 1), (2, 2), (2, 3)},
        new [] {(0, 1), (1, 1), (2, 1), (3, 1)},
        new [] {(2, 0), (2, 1), (2, 2), (2, 3)}
    });

    public static readonly PieceShape[] All = {I};
}