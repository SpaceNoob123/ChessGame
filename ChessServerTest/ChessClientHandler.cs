using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ChessServerTest;
namespace ChessServerTest
{
    public class ChessClientHandler
    {
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        private ChessGame game; // Класс игры в шахматы
        private string playerName; // Имя игрока

        public ChessClientHandler(TcpClient client, ChessGame game)
        {
            this.client = client;
            this.game = game;

            NetworkStream networkStream = client.GetStream();
            reader = new StreamReader(networkStream);
            writer = new StreamWriter(networkStream);
            writer.AutoFlush = true;
        }

        public void HandleClient()
        {
            try
            {
                playerName = reader.ReadLine(); // Получить имя игрока от клиента
                Console.WriteLine($"Игрок {playerName} присоединился к игре.");

                while (true)
                {
                    string clientMessage = reader.ReadLine(); // Получить ход от клиента
                                                              // Обработать ход клиента и обновить состояние игры
                                                              // Например, game.MakeMove(clientMessage);

                    // Отправить обновленное состояние игры всем клиентам// Получить текущее состояние игры в виде строки
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Игрок {playerName} отключился: {ex.Message}");
                // Обработка отключения клиента
                // Например, удаление клиента из списка и завершение игры при необходимости
            }
            finally
            {
                client.Close();
            }
        }

        public void SendMessage(string message)
        {
            writer.WriteLine(message);
        }
    }
}
