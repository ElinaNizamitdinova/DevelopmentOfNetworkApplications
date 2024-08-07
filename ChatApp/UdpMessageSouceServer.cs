
using ChatCommon;
using ChatCommon.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatApp
{
    internal class UdpMessageSouceServer : IMessageSourseServer<IPEndPoint>
    {
        private readonly UdpClient _udpClient;
        public UdpMessageSouceServer()
        {
            _udpClient = new UdpClient();
        }

        public IPEndPoint copyEndPoint(IPEndPoint t)
        {
            return new IPEndPoint(t.Address, t.Port);
        }

        public IPEndPoint CreateEndPoint()
        {
           return new IPEndPoint(IPAddress.Any, 0);
        }


        public NetMessage Receive(ref IPEndPoint ep)
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
