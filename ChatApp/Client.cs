
using ChatApp;
using ChatCommon.Models;
using System.Net;
using System.Net.Sockets;
using ChatCommon;

namespace ChatApp
{
    public class Client<T>
    {
        private readonly string _name;
        string address;
        int port;
        public IMessageSourseClient<T> _messageSourse;
        public T remoteEndPoint;
        public Client(string name,IMessageSourseClient<T> messageSourseClient)
        {
            this._name = name;
            _messageSourse = messageSourseClient;
            remoteEndPoint = _messageSourse.CreateEndPoint();

        }
        UdpClient udpClient = new UdpClient();
        public async Task ClientListener()
        {
            
          while (true)
            {
                try
                {
                    var messageReceived = _messageSourse.Receive( ref remoteEndPoint);
                    Console.WriteLine($"Получено сообщение от {messageReceived.NickNameFrom}");
                    Console.WriteLine(messageReceived.Text);
                    await Confirm(messageReceived, remoteEndPoint);
                }
                catch(Exception ex) {
                    Console.WriteLine("Ошибка при получении сообщения" + ex.Message);
                }
            }

        }
        async Task Confirm(NetMessage message, T remoreEndPoint)
        {
            message.Command = Command.Confirmation;
            await _messageSourse.SendAsync(message, remoreEndPoint);    

        }
        async Task Register(T remoteEndPoint)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any,0);
            var message = new NetMessage() { NickNameFrom = _name, NickNameTo = null, Text = null, Command = Command.Register,EndPoint = ep  };
            _messageSourse.SendAsync(message, remoteEndPoint);
        }
        async Task ClientSender()
        {
            Register(remoteEndPoint);
            while (true)
            {
                try
                {
                    Console.WriteLine("Ожидание собщения клиентом :");
                    var nameTo = Console.ReadLine();
                    Console.WriteLine("Введите сообщение");
                    var messageText = Console.ReadLine();
                    var message = new NetMessage() { Command = Command.Message, NickNameFrom = _name, NickNameTo = nameTo, Text = messageText };
                    await _messageSourse.SendAsync(message, remoteEndPoint);
                    Console.WriteLine("Сообщение отправденно");
                }
                catch (Exception ex) { Console.WriteLine("Ошибка при обработке сообщения" + ex.ToString()); }
            }
        }
        public void Start()
        {
            /*UdpClient udpClient = new UdpClient()*/;
            ClientListener();
            ClientSender();
        }

        
    }
}
