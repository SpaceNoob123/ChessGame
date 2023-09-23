using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core.UDP
{
    public enum PacketType : byte
    {
        Connect,
        Disconnect,
        Message,
        Move,
        GameUpdateRequest,
        GameUpdateResponse,
        GameStart,
        GameEnd,
        Error,
    }

    public class Packet
    {

        [JsonIgnore]
        public IPEndPoint SenderEndpointParsed
        {
            get
            {
                if (SenderEndpoint == null)
                    return null;
                var parts = SenderEndpoint.Split(':');
                return new IPEndPoint(IPAddress.Parse(parts[0]), int.Parse(parts[1]));
            }
        }


        public string Payload { get; set; }
        public string SenderName { get; set; }

        [JsonIgnore]
        public string? SenderEndpoint { get; set; }
        public PacketType Type { get; set; }


        public Packet(string senderName, string payload, PacketType type)
        {
            Payload = payload;
            SenderName = senderName;
            Type = type;
        }



        public static string Serialize(Packet packet)
        {
            return JsonConvert.SerializeObject(packet);
        }   

        public static Packet Deserialize(string packetJson)
        {
            return JsonConvert.DeserializeObject<Packet>(packetJson);
        }
    }
}
