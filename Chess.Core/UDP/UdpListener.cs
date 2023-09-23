using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Chess.Core.UDP
{
    public class UdpListener : UdpBase
    {

        public int UsersConnected
        {
            get => _userConnections.Count;
        }

        public string IpAddress 
        { 
            get => _listenOn.Address.ToString();
        }

        private int _maxClientCount;

        private IPEndPoint _listenOn;

        private Dictionary<IPEndPoint, string> _userConnections = new Dictionary<IPEndPoint, string>();
        private List<Packet> _packetHistory = new List<Packet>();

        public UdpListener(IPEndPoint endpoint, int maxClientCount)
        {
            _maxClientCount = maxClientCount;
            _listenOn = endpoint;
            Client = new System.Net.Sockets.UdpClient(_listenOn);
        }


        public void StartListening()
        {
            Task.Factory.StartNew(async () => {
                while (true)
                {
                    var packet = await this.Receive();
                    HandleClientPacket(packet);
                }
            });
        }

        public void Reply(Packet packet, IPEndPoint endpoint)
        {
            string json = Packet.Serialize(packet);
            byte[] datagram = Encoding.ASCII.GetBytes(json);
            Client.Send(datagram, datagram.Length, endpoint);
        }

        public void ReplyAll(Packet packet)
        {
            string json = Packet.Serialize(packet);
            byte[] datagram = Encoding.ASCII.GetBytes(json);

            foreach (var endpoint in _userConnections.Keys)
                Client.Send(datagram, datagram.Length, endpoint);
        }
        private bool TryStoreUserConnection(string name, IPEndPoint clientEnpoint)
        {
            return _userConnections.TryAdd(clientEnpoint, name);
        }

        private void DisconnectUser(IPEndPoint clientEndpoint)
        {
            _userConnections.Remove(clientEndpoint, out string? user);
            Console.WriteLine($"{user ?? "A user"} was disconnected from the server.");

            Reply(new Packet("SERVER", "Game is full", PacketType.Error), clientEndpoint);

            if (UsersConnected == 0)
                Console.WriteLine("No users are connected to the server.");
            else
                Console.WriteLine($"Current number of users connected: {UsersConnected}");
        }

        private void HandleClientPacket(Packet packet)
        {

            switch (packet.Type)
            {
                case PacketType.Connect:
                    if (_userConnections.Values.Contains(packet.SenderName))
                    {
                        Reply(new Packet("SERVER", $"Username '{packet.SenderName} already exists! Try joining with another username.", PacketType.Error), packet.SenderEndpointParsed);
                        ReplyAll(new Packet("SERVER", $"Disconnecting user '{packet.SenderName}', (username already exists in the server)", PacketType.Message));
                        DisconnectUser(packet.SenderEndpointParsed);
                        return;
                    }

                    bool isNewConnection = TryStoreUserConnection(packet.SenderName, packet.SenderEndpointParsed);

                    if (isNewConnection && UsersConnected <= 2)
                    {
                        
                        foreach (var p in _packetHistory)
                            Reply(p, packet.SenderEndpointParsed);

                        ReplyAll(packet);
                        Reply(new Packet("SERVER", $"waiting for players to join...", PacketType.Message), packet.SenderEndpointParsed);

                        if (UsersConnected == _maxClientCount)
                        {
                            Packet gameStartPacket = new("SERVER", "none", PacketType.GameStart);
                            ReplyAll(gameStartPacket);
                        }
                    }
                    else
                    {
                        DisconnectUser(packet.SenderEndpointParsed);
                    }

                    break;

                case PacketType.Disconnect:
                    DisconnectUser(packet.SenderEndpointParsed);
                    ReplyAll(new Packet("SERVER" , packet.Payload, PacketType.Message));
                    break;

                case PacketType.Message:
                    ReplyAll(packet);
                    break;
                default:
                    break;
            }

            _packetHistory.Add(packet);
        }
    }
}
