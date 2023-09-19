using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessServerTest
{
    public partial class ChessClientForm : Form
    {
        private TcpListener listener;
        private List<ChessClientHandler> clients = new List<ChessClientHandler>();
        private ChessGame game; // Класс игры в шахматы

        public ChessClientForm()
        {
            listener = new TcpListener(IPAddress.Any, 12345); // Замените на необходимый порт
            listener.Start();
            Console.WriteLine("Сервер запущен и ожидает подключений...");

            game = new ChessGame(); // Инициализация игры

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                ChessClientHandler clientHandler = new ChessClientHandler(client, game);
                clients.Add(clientHandler);

                Thread clientThread = new Thread(clientHandler.HandleClient);
                clientThread.Start();
            }
        }

        public void Broadcast(string message)
        {
            foreach (var client in clients)
            {
                client.SendMessage(message);
            }
        }
    }
}
