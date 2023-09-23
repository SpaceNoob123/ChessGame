using Chess.Core.UDP;
using Chess.Core;
using Newtonsoft.Json;
using System.Diagnostics;
namespace ChessClient;

public partial class FormUDPClient : Form
{
    string _username;
    private UdpClient _client;

    private Button[,] _buttons = new Button[8, 8];

    private Board _board = new Board(8, false);
    private Tile? _selectedTile = null;
    private Player _clientPlayer;
    private char _turn;
    private bool _boardIsFlipped = false;

    public FormUDPClient(string name, string ip, int port)
    {
        InitializeComponent();
        this.Icon = new Icon("icon.ico");
        this.MinimumSize = this.Size;
        this.StartPosition = FormStartPosition.CenterScreen;

        _username = name;
        this.Text = "Chess - " + _username;
        ChessUtils.CreateTiles(pnlBoard, _buttons, _board, pnlBoard.Width / 8, Color.Gainsboro, Color.Tan, false, OnTileClicked);

        _client = UdpClient.ConnectTo(_username, ip, port);
        _client.OnPacketReceived += Client_OnPacketReceived;
        _client.Listen();
    }
    private void SendUpdateGameRequest()
    {
        Packet p = new(_username, $"{_username} requested a game update", PacketType.GameUpdateRequest);
        _client.Send(p);
    }

    private void Client_OnPacketReceived(Packet packet)
    {
        HandlePacket(packet);
    }

    private void HandlePacket(Packet packet)
    {
        switch (packet.Type)
        {
            case PacketType.GameUpdateResponse:
                UpdateGame(packet);
                break;
            case PacketType.Disconnect:
                throw new NotImplementedException();
            case PacketType.Move:
                SendUpdateGameRequest();
                throw new NotImplementedException();
            case PacketType.GameStart:
                SendUpdateGameRequest();
                break;
            case PacketType.Error:
                this.Invoke(() =>
                {
                    MessageBox.Show(packet.SenderName + ": " + packet.Payload, "Chess", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                });
                break;
            default:
                break;
        }

    }

    private void UpdateGame(Packet packet)
    {
        var game = JsonConvert.DeserializeObject<Game>(packet.Payload, new JsonSerializerSettings
        {
            TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
            NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
        });


        this.Invoke(() =>
        {
            _board = game.Board;
            //_board = new Board(8, true);

            if (_clientPlayer == null)
                _clientPlayer = game.Player1.Name == _username ? game.Player1 : game.Player2;

            _turn = game.Turn;

            ChessUtils.DrawSymbols(_buttons, _board);

        });
    }

    private void OnTileClicked(object? sender, EventArgs e)
    {
        if (_clientPlayer == null) return;
        if (_turn != _clientPlayer.Symbol) return;

        Button btn = (Button)sender;
        BoardLocation boardPoint = (BoardLocation)btn.Tag;
        int row = boardPoint.Row;
        int col = boardPoint.Column;

        if (_selectedTile == null)
        {
            var piece = _board.GetPiece(row, col);

            if (piece != null)
            {
                Tile selectedTile = _board.GetTile(row, col);

                if (selectedTile.Piece.Color != _clientPlayer.Symbol) return;

                _selectedTile = selectedTile;
                ChessUtils.ShowMoves(_buttons, _board, _selectedTile);
            }
        }
        else if (_selectedTile == _board.GetTile(row, col))
        {
            ChessUtils.HideMoves(_buttons);
            _selectedTile = null;
        }
        else
        {
            Tile to = _board.GetTile(row, col);

            if (to.Piece != null)
            {
                if (_selectedTile.Piece.Color == to.Piece.Color)
                {
                    _selectedTile = to;
                    ChessUtils.HideMoves(_buttons);
                    ChessUtils.ShowMoves(_buttons, _board, _selectedTile);
                    return;
                }
            }

            var moveToSend = Tuple.Create(_selectedTile, to);

            var moveDataJson = JsonConvert.SerializeObject(moveToSend, new JsonSerializerSettings
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
            });

            Packet p = new(_username, moveDataJson, PacketType.Move);
            _client.Send(p);

            _selectedTile = null;
            ChessUtils.HideMoves(_buttons);

        }
    }

    private void FormUDPClient_FormClosing(object sender, FormClosingEventArgs e)
    {
        Packet p = new(_username, $"{_username} left", PacketType.Disconnect);
        _client.Send(p);
    }
}
