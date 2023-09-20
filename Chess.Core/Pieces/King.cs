using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core.Pieces
{
    public class King : Piece
    {
        public King() : base()
        {
            _symbol = 'k';
        }

        public King(char player, int row, int col) : base(player, row, col)
        {
            _symbol = 'k';
        }

        private static readonly int[][] MoveTemplates = new int[][]
        {
            new [] { 1, -1 },
            new [] { 1, 0 },
            new [] { 1, 1 },
            new [] { 0, -1 },
            new [] { 0, 1 },
            new [] { -1, -1 },
            new [] { -1, 0 },
            new [] { -1, 1 },
        };

        public override IList<Tile> GetValidMoves(Board board)
        {
            var moves = Movement.GetMoves(board, this, 1, MoveTemplates);

            return moves;
        }

    }
}
