namespace FentrisDesktop.Board;

public class Tetrominoes
{
    // ARS I piece for now
    public static readonly PieceShape I = new(new[]
    {
        new[] { (0, 1), (1, 1), (2, 1), (3, 1) },
        new[] { (2, 0), (2, 1), (2, 2), (2, 3) },
        new[] { (0, 1), (1, 1), (2, 1), (3, 1) },
        new[] { (2, 0), (2, 1), (2, 2), (2, 3) }
    });

    public static readonly PieceShape O = new(new[]
    {
        new[] { (1, 1), (1, 2), (2, 1), (2, 2) },
        new[] { (1, 1), (1, 2), (2, 1), (2, 2) },
        new[] { (1, 1), (1, 2), (2, 1), (2, 2) },
        new[] { (1, 1), (1, 2), (2, 1), (2, 2) }
    });

    public static readonly PieceShape T = new(new[]
    {
        new[] { (0, 1), (1, 1), (2, 1), (1, 2) },
        new[] { (1, 0), (1, 1), (1, 2), (0, 1) },
        new[] { (0, 2), (1, 2), (2, 2), (1, 1) },
        new[] { (1, 0), (1, 1), (1, 2), (2, 1) },
    });

    public static readonly PieceShape L = new(new[]
    {
        new[] { (0, 1), (1, 1), (2, 1), (0, 2) },
        new[] { (0, 0), (1, 0), (1, 1), (1, 2) },
        new[] { (0, 2), (1, 2), (2, 2), (2, 1) },
        new[] { (1, 0), (1, 1), (1, 2), (2, 2) },
    });

    public static readonly PieceShape J = new(new[]
    {
        new[] { (0, 1), (1, 1), (2, 1), (2, 2) },
        new[] { (0, 2), (1, 0), (1, 1), (1, 2) },
        new[] { (0, 2), (1, 2), (2, 2), (0, 1) },
        new[] { (1, 0), (1, 1), (1, 2), (2, 0) },
    });

    public static readonly PieceShape S = new(new[]
    {
        new[] { (0, 2), (1, 2), (1, 1), (2, 1) },
        new[] { (0, 0), (0, 1), (1, 1), (1, 2) },
        new[] { (0, 2), (1, 2), (1, 1), (2, 1) },
        new[] { (0, 0), (0, 1), (1, 1), (1, 2) },
    });

    public static readonly PieceShape Z = new(new[]
    {
        new[] { (0, 1), (1, 1), (1, 2), (2, 2) },
        new[] { (2, 0), (2, 1), (1, 1), (1, 2) },
        new[] { (0, 1), (1, 1), (1, 2), (2, 2) },
        new[] { (2, 0), (2, 1), (1, 1), (1, 2) },
    });

    public static readonly PieceShape[] All = { I, O, T, L, J, S, Z };
}