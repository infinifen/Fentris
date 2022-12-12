using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FentrisDesktop.Board;

public class Board
{
    internal Block[,] board;

    public Board()
    {
        board = new Block[10, 21];
        board.Initialize();
    }

    public void Clear()
    {
        board.Initialize();
    }

    public Block this[int x, int y]
    {
        get
        {
            if (x < 0 || y < 0 || x >= board.GetLength(0) || y >= board.GetLength(1))
            {
                return new Block(BlockKind.OutOfBounds);
            }

            return board[x, y];
        }
        set
        {
            try
            {
                board[x, y] = value;
            }
            catch
            {
            }
        }
    }

    public IEnumerable<Block> Row(int y)
    {
        for (int x = 0; x < board.GetLength(0); ++x)
        {
            yield return this[x, y];
        }
    }

    public IEnumerable<Block> Column(int x)
    {
        for (int y = 0; y < board.GetLength(1); ++y)
        {
            yield return this[x, y];
        }
    }

    public bool IsRowFull(int row)
    {
        return Row(row).All(b => b.Kind != BlockKind.Clear);
    }

    public IEnumerable<int> FullRows() => Enumerable.Range(0, board.GetLength(1)).Where(IsRowFull);

    public void ClearRow(int rowToClear)
    {
        for (int row = rowToClear; row >= 0; row--)
        {
            for (int col = 0; col < board.GetLength(0); col++)
            {
                this[col, row] = this[col, row - 1];
                if (this[col, row].Kind == BlockKind.OutOfBounds) // row 0 will try to get from -1
                {
                    this[col, row] = new Block();
                }
            }
        }
    }
    
    public bool CollidePiece(Piece p, int x, int y)
    {
        foreach (var (bx, by) in p.GetBlockOffsets())
        {
            if (this[x + bx, y + by].Kind != BlockKind.Clear)
            {
                return true;
            }
        }

        return false;
    }

    public bool CollidePiece(Piece p) => CollidePiece(p, p.X, p.Y);

    public void PlacePiece(Piece p, int x, int y, int frame = 0)
    {
        foreach (var (bx, by) in p.GetBlockOffsets())
        {
            this[x + bx, y + by] = new Block(p.Kind, frame);
        }
    }

    public void PlacePiece(Piece p, int frame = 0) => PlacePiece(p, p.X, p.Y, frame);

    public int GetGhostY(Piece p, int x, int y)
    {
        while (!this.CollidePiece(p, x, y++))
        {
        }

        return y - 2;
    }

    public int GetGhostY(Piece p) => GetGhostY(p, p.X, p.Y);

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (int row = 0; row < board.GetLength(1); row++)
        {
            foreach (var c in Row(row).Select(b => (char) b.Kind))
            {
                sb.Append(c);
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}