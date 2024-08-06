using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class UDPServer {
        public async Task ServerListenAsync()
        {
            UdpClient udpClient = new UdpClient(45512);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);

            Console.WriteLine("Сервер ждет сообщений");
            CancellationTokenSource cts = new CancellationTokenSource();
            bool canWork = true;
            while (!cts.IsCancellationRequested)
            {
                byte[] buffer = udpClient.Receive(ref iPEndPoint);
                var messageText = Encoding.UTF8.GetString(buffer);
                Console.WriteLine($"Полученно {buffer.Length} байт");

                byte[] reply = Encoding.UTF8.GetBytes("Cообщение получено");

                int bytes = await udpClient.SendAsync(reply, iPEndPoint);
                Console.WriteLine($"отправлено {bytes} байт ");

                NetMessage? message = NetMessage.DeserializeMessageFronJSON(messageText);
                if (message.Text.ToLower().Equals("exit")) cts.Cancel();
                message.PrintGetMessageFrom();

            }

        }
    }
}
