using ChatCommon;
using ChatCommon.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace ChatApp
{
    public class UdpMessageSourseClient : IMessageSourseClient<IPEndPoint>
    {
        private readonly UdpClient _udpClient;
        private readonly IPEndPoint _udpEndPoint;
        public UdpMessageSourseClient(string Ip, int port)
        {
            _udpClient = new UdpClient();
            _udpEndPoint = new IPEndPoint(IPAddress.Parse(Ip), port);
        }

        public IPEndPoint CreateEndPoint()
        {
           return new IPEndPoint(IPAddress.Any, 0);
        }

        public IPEndPoint GetServer()
        {
          return _udpEndPoint;
        }

        public NetMessage Receive( ref IPEndPoint ep)
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
