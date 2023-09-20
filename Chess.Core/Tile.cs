using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class Tile
    {
        public IPiece? Piece { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsEmptySpace { get { return Piece == null; } }


        public Tile(int row, int col)
        {
            Row = row;
            Column = col;
            Piece = null;
        }


        public Tile(int row, int col, IPiece? piece)
        {
            Row = row;
            Column = col;
            Piece = piece;
        }

        [JsonConstructor]
        public Tile() { }



        public string GetDisplayCoordinates()
        {
            char rowCoordinate = Convert.ToChar(Row + 65 + 32);

            return rowCoordinate + Column.ToString();
        }

        public override string ToString()
        {
            if (Piece != null)
                return Piece.ToString();
            else
                return $"Empty tile at {Row}, {Column}";
        }

    }
}
