using Chess.Core;
using System.Resources;

namespace ChessClient;

internal struct BoardPoint
{
    public int row;
    public int col;
}
internal static class ChessUtils
{
    static ResourceManager RM = new ResourceManager("ChessClient.Resources", typeof(Resources).Assembly);

    internal static void ShowMoves(Button[,] buttonArray, Board board, Tile tile)
    {
        if (tile.Piece == null) return;

        var moves = tile.Piece.GetValidMoves(board);

        foreach (var move in moves)
        {
            if (move != null)
            {
                try
                {
                    buttonArray[move.Row, move.Column].Image = (Image)RM.GetObject("ValidSpace");
                }
                catch { continue; }
            }
        }
    }
    internal static void HideMoves(Button[,] buttonArray)
    {
        foreach (var btn in buttonArray)
        {
            btn.Image = null;
        }
    }
    internal static void DrawSymbols(Button[,] buttonArray, Board board)
    {
        for (int row = 0; row < board.Size; row++)
        {
            for (int col = 0; col < board.Size; col++)
            {
                if (board.Tiles[row, col].Piece == null)
                {
                    buttonArray[row, col].BackgroundImage = null;
                    continue;
                }

                switch (board.Tiles[row, col].Piece.Symbol)
                {
                    case 'P':
                        buttonArray[row, col].BackgroundImage = (Image)RM.GetObject("BlackPawn");
                        break;
                    case 'R':
                        buttonArray[row, col].BackgroundImage = (Image)RM.GetObject("BlackRook");
                        break;
                    case 'N':
                        buttonArray[row, col].BackgroundImage = (Image)RM.GetObject("BlackKnight");
                        break;
                    case 'B':
                        buttonArray[row, col].BackgroundImage = (Image)RM.GetObject("BlackBishop");
                        break;
                    case 'Q':
                        buttonArray[row, col].BackgroundImage = (Image)RM.GetObject("BlackQueen");
                        break;
                    case 'K':
                        buttonArray[row, col].BackgroundImage = (Image)RM.GetObject("BlackKing");
                        break;
                    case 'p':
                        buttonArray[row, col].BackgroundImage = (Image)RM.GetObject("WhitePawn");
                        break;
                    case 'r':
                        buttonArray[row, col].BackgroundImage = (Image)RM.GetObject("WhiteRook");
                        break;
                    case 'n':
                        buttonArray[row, col].BackgroundImage = (Image)RM.GetObject("WhiteKnight");
                        break;
                    case 'b':
                        buttonArray[row, col].BackgroundImage = (Image)RM.GetObject("WhiteBishop");
                        break;
                    case 'q':
                        buttonArray[row, col].BackgroundImage = (Image)RM.GetObject("WhiteQueen");
                        break;
                    case 'k':
                        buttonArray[row, col].BackgroundImage = (Image)RM.GetObject("WhiteKing");
                        break;
                    default:
                        break;
                }
            }
        }
    }

    internal static void CreateTiles(Control control, Button[,] buttonArray, Board board, int tileSize, Color color1, Color color2, bool reverse, EventHandler? tileClickEvent)
    {
        int x = 0;
        for (int row = 0; row < board.Size; row++)
        {
            for (int col = 0; col < board.Size; col++)
            {
                Button btn = new Button();

                btn.Size = new Size(tileSize, tileSize);
                btn.Location = new Point(col * tileSize, row * tileSize);
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = x % 2 == 0 ? color1 : color2;
                btn.BackgroundImageLayout = ImageLayout.Stretch;
                btn.ImageAlign = ContentAlignment.MiddleCenter;

                if (tileClickEvent != null)
                    btn.Click += tileClickEvent;

                btn.Tag = new BoardLocation()
                {
                    Row = row,
                    Column = col,
                };

                buttonArray[row, col] = btn;
                control.Controls.Add(btn);
                x++;
            }
            x++;
        }
    }
           
}
