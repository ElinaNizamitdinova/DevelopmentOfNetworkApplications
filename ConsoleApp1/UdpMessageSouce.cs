using ConsoleApp1.Abstracts;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleApp1
{
    internal class UdpMessageSouce : IMessageSourse
    {
        private readonly UdpClient _udpClient;
        public UdpMessageSouce()
        {
            _udpClient = new UdpClient();
        }
        public NetMessage Recive(IPEndPoint ep)
        {
            byte[] data = _udpClient.Receive(ref ep);
            string str = Encoding.UTF8.GetString(data);
            return NetMessage.DeserializeMessageFronJSON(str) ?? new NetMessage();
        }

        public async Task SendAsync(NetMessage message, IPEndPoint ep)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message.SerialazeMessageFromJSON());
            _udpClient.SendAsync(buffer, buffer.Length, ep).Wait();

        }

    }
}
