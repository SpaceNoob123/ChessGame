using Chess.Core.Pieces;
using Newtonsoft.Json;

namespace Chess.Core
{
    public enum GameOverType
    {
        Checkmate,
        Stalemate
    }

    public class Board
    {

        public delegate void KingChecked(King? kingThatIsChecked);
        public event KingChecked? OnKingChecked;

        public delegate void GameOver(char? winner, GameOverType type);
        public event GameOver OnGameOver;

        const int DEFAULT_SIZE = 8;

        private Tile[,] _tiles;
        public Stack<Tuple<BoardLocation, BoardLocation, IPiece?>> MoveStack = new Stack<Tuple<BoardLocation, BoardLocation, IPiece?>>();

        private BoardLocation _whiteKingLocation;
        private BoardLocation _blackKingLocation;
        private char? _kingInCheck;
        private bool _gameOver = false;
        private char? _winner = null;

        public Tile[,] Tiles { get { return _tiles; } set { _tiles = value; } }
        public int Size { get; set; }
        public char? KingInCheck { get => _kingInCheck; set => _kingInCheck = value; }
        public bool IsGameOver { get => _gameOver; set => _gameOver = value; }
        public char? Winner { get => _winner; set => _winner = value; }
        public BoardLocation WhiteKingLocation { get => _whiteKingLocation; set => _whiteKingLocation = value; }
        public BoardLocation BlackKingLocation { get => _blackKingLocation; set => _blackKingLocation = value; }
        public Board(int size, bool addDefaultPieces)
        {
            _tiles = new Tile[size, size];
            Size = size;
            CreateTiles(size, size);

            if (addDefaultPieces)
            {
                AddDefaultPieces();
                _blackKingLocation = new BoardLocation(0, 4);
                _whiteKingLocation = new BoardLocation(7, 4);
            }
        }
        public Board(Tile[,] tiles)
        {
            _tiles = tiles;
            Size = tiles.GetLength(0);
        }

        [JsonConstructor]
        public Board() { }

        ~Board() => System.Diagnostics.Debug.WriteLine($"Chessboard was disposed");

        private void CreateTiles(int rows, int columns)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    _tiles[i, j] = new Tile(i, j);
                }
            }
        }

        private void AddDefaultPieces()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i == 1)
                        _tiles[i, j].Piece = new Pawn('b', i, j); // adds 8 player black pawns to 2nd row
                    if (i == 6)
                        _tiles[i, j].Piece = new Pawn('w', i, j); // adds 8 white pawns to 7th row

                    // player 1's backrow
                    if (i == 7)
                    {
                        if (j == 0 || j == 7)
                            _tiles[i, j].Piece = new Rook('w', i, j); // adds both white rooks
                        if (j == 1 || j == 6)
                            _tiles[i, j].Piece = new Knight('w', i, j); // adds both white knights
                        if (j == 2 || j == 5)
                            _tiles[i, j].Piece = new Bishop('w', i, j); // adds both white bishops
                        if (j == 3)
                            _tiles[i, j].Piece = new Queen('w', i, j); // adds white queen
                        if (j == 4)
                            _tiles[i, j].Piece = new King('w', i, j); // adds white king
                    }

                    // player 2's backrow
                    if (i == 0)
                    {
                        if (j == 0 || j == 7)
                            _tiles[i, j].Piece = new Rook('b', i, j); // adds both black rooks
                        if (j == 1 || j == 6)
                            _tiles[i, j].Piece = new Knight('b', i, j); // adds both black knights
                        if (j == 2 || j == 5)
                            _tiles[i, j].Piece = new Bishop('b', i, j); // adds both black bishops
                        if (j == 3)
                            _tiles[i, j].Piece = new Queen('b', i, j); // adds black queen
                        if (j == 4)
                            _tiles[i, j].Piece = new King('b', i, j); // adds black king
                    }
                }
            }
        }

        private void UpdateKingPosition(char color, int row, int col)
        {
            if (color == 'w')
                _whiteKingLocation = new BoardLocation(row, col);
            else
                _blackKingLocation = new BoardLocation(row, col);
        }


        public bool TryMakeMove(Tile? from, Tile? to)
        {
            if (_gameOver) return false;
            if (from.Piece is null) return false;  
            

            if (!Movement.MoveIsValid(this, from, to))
                return false;

            if (Movement.MovePutsKingInCheck(this, from, to))
                return false;

            if (from.Piece.Color == to.Piece?.Color) return false; 
            if (to.Piece is King) return false; 

            MovePiece(from, to);

            if (IsKingInCheck(to.Piece.Color))
            {
                var kingLoc = to.Piece.Color == 'w' ? _blackKingLocation : _whiteKingLocation;
                King king = GetPiece(kingLoc.Row, kingLoc.Column) as King;

                _kingInCheck = king.Color;
                OnKingChecked?.Invoke(king);

                if (GenerateAllValidMoves(king.Color).Count == 0)
                {
                    _gameOver = true;
                    _winner = king.Color == 'w' ? 'b' : 'w';
                    OnGameOver?.Invoke(_winner, GameOverType.Checkmate);
                }
            }
            else
                _kingInCheck = null;

            return true;

        }

        public void UndoMove()
        {
            if (MoveStack.Count == 0) return;

            var lastMove = MoveStack.Pop();

            var from = GetTile(lastMove.Item1.Row, lastMove.Item1.Column);
            var to = GetTile(lastMove.Item2.Row, lastMove.Item2.Column);

            from.Piece = to.Piece;
            if (from.Piece != null)
                from.Piece.CurrentLocation = new BoardLocation(from.Row, from.Column);

            to.Piece = lastMove.Item3;            

            if (from.Piece is King)
                UpdateKingPosition(from.Piece.Color, from.Row, from.Column);

            if (_gameOver) _gameOver = false;
            if (_kingInCheck != null) _kingInCheck = null;

        }

        public IPiece AddPiece<T>(int row, int col, char color) where T : IPiece, new()
        {
            if (row >= Size || col >= Size)
                throw new IndexOutOfRangeException("Row or column is out of range.");

            T piece = new T();
            piece.CurrentLocation = new BoardLocation(row, col);
            piece.Color = color;
            _tiles[row, col].Piece = piece;

            if (piece is King && piece.Color == 'b')
                _blackKingLocation = new BoardLocation(row, col);
            else if (piece is King && piece.Color == 'w')
                _whiteKingLocation = new BoardLocation(row, col);

            return piece;
        }

        
        public IPiece AddPiece(int row, int col, IPiece piece)
        {
            if (row >= Size || col >= Size)
                throw new IndexOutOfRangeException("Row or column is out of range.");

            piece.CurrentLocation = new BoardLocation(row, col);
            _tiles[row, col].Piece = piece;

            if (piece is King && piece.Color == 'b')
                _blackKingLocation = new BoardLocation(row, col);
            else if (piece is King && piece.Color == 'w')
                _whiteKingLocation = new BoardLocation(row, col);

            return piece;
        }

        public IPiece? GetPiece(int row, int col)
        {
            return _tiles[row, col].Piece ?? null;
        }

        public IPiece? GetPiece(BoardLocation boardLocation)
        {
            return _tiles[boardLocation.Row, boardLocation.Column].Piece ?? null;
        }

        public Tile? GetTile(int row, int col)
        {
            try
            {
                return _tiles[row, col];
            }
            catch { return null; }
        }
        public Tile GetTile(BoardLocation boardLocation)
        {
            try
            {
                return _tiles[boardLocation.Row, boardLocation.Column];
            }
            catch { throw new NullReferenceException("tile doesn't exist."); }
        }
        public Tile? GetTileByPiece(IPiece piece)
        {
            foreach (Tile tile in _tiles)
            {
                if (tile.Row == piece.CurrentLocation.Row && tile.Column == piece.CurrentLocation.Column)
                    return tile;
            }
            return null;
        }
        public void RemovePiece(IPiece piece)
        {
            var tile = GetTileByPiece(piece);
            if (tile != null)
                tile.Piece = null;
        }
        public void RemovePiece(int row, int col)
        {
            var tile = GetTile(row, col);
            if (tile != null)
                tile.Piece = null;
        }

        internal void MovePiece(Tile from, Tile to)
        {
            var fromLoc = new BoardLocation(from.Row, from.Column);
            var toLoc = new BoardLocation(to.Row, to.Column);
            IPiece? capturedPiece = to.Piece;
            MoveStack.Push(Tuple.Create(fromLoc, toLoc, capturedPiece));

            to.Piece = from.Piece;
            to.Piece.CurrentLocation = new BoardLocation(to.Row, to.Column);
            from.Piece = null;

            if (to.Piece is King)
                UpdateKingPosition(to.Piece.Color, to.Row, to.Column);
        }

        internal void MovePiece(BoardLocation from, BoardLocation to)
        {
            var fromLoc = new BoardLocation(from.Row, from.Column);
            var toLoc = new BoardLocation(to.Row, to.Column);

            IPiece? capturedPiece = GetTile(to).Piece;
            MoveStack.Push(Tuple.Create(fromLoc, toLoc, capturedPiece));

            var toTile = GetTile(to);
            var fromTile = GetTile(from);

            toTile.Piece = fromTile.Piece;
            toTile.Piece.CurrentLocation = new BoardLocation(toTile.Row, toTile.Column);
            fromTile.Piece = null;

            if (toTile.Piece is King)
                UpdateKingPosition(toTile.Piece.Color, toTile.Row, toTile.Column);
        }

        internal bool IsKingInCheck(char attackerColor)
        {

            var kingPos = attackerColor == 'w' ? _blackKingLocation : _whiteKingLocation;
            foreach (var tile in _tiles)
            {
                if (tile.Piece == null) continue;
                if (tile.Piece is King) continue;

                if (tile.Piece.Color == attackerColor)
                {
                    var attackerMoves = GetPiece(tile.Row, tile.Column).GetValidMoves(this);
                    if (attackerMoves.Any(a => a.Row == kingPos.Row && a.Column == kingPos.Column))
                    {
                        var tmpKing = GetPiece(kingPos.Row, kingPos.Column) as King;
                        if (tmpKing.Color != attackerColor)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }


        internal List<Move> GenerateAllValidMoves(char? playerColor = null)
        {
            List<Move> moves = new List<Move>();

            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    var currentTile = GetTile(row, col);
                    if (currentTile.Piece == null) continue;

                    if (playerColor != null)
                        if (currentTile.Piece.Color != playerColor) continue;

                    foreach (var move in currentTile.Piece.GetValidMoves(this))
                        moves.Add(new Move(currentTile, move));
                }
            }

            return moves;
        }

        internal Board Copy()
        {
            return new Board()
            {
                Tiles = this.Tiles,
                Size = this.Size,
                KingInCheck = this.KingInCheck,
                IsGameOver = this.IsGameOver,
                Winner = this.Winner,
                _blackKingLocation = this._blackKingLocation,
                _whiteKingLocation = this._whiteKingLocation,
                MoveStack = this.MoveStack,
            };
        }
    }
}
