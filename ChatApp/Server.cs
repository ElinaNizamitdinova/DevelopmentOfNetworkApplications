
using ChatCommon;
using ChatCommon.Models;
using ChatDB;
using System.Net;

namespace ChatApp
{
    //Предполагаемое разделение библиотек:
    //ChatCommon - интерфейсы и сообщения
    //ChatDb - база данных
    //ChatNetwork - сетевое взаимодействие
    //ChatApp - приложение где будет клиент и сервер(в рамках 1 семинара мы не успеем разделить клиент и сервер на два независимых приложения).
    //В первой работе мы создаем библиотеки и наполняем код ChatCommon и ChatDb

    public class Server<T>
    {
        Dictionary<string, T> clients = new Dictionary<string, T>();
        private readonly IMessageSourseServer<T> _messageSouce;
        T ep;
        public Server(IMessageSourseServer<T> server)
        {
            _messageSouce = server;
            ep = _messageSouce.CreateEndPoint();
        }
        bool work = true;
        public void Stop()
        {
            work = false;
        }

        private async Task Register(NetMessage message)
        {
            Console.WriteLine($"Message register name = {message.NickNameFrom}");
            if (clients.TryAdd(message.NickNameFrom, _messageSouce.copyEndPoint(message.EndPoint))) ;
            {
                using (ChatContext context = new ChatContext())
                {
                    context.Users.Add(new User() { FullName = message.NickNameFrom });
                    await context.SaveChangesAsync();
                }
            }

            async Task RelyMessage(NetMessage message)
            {
                if (clients.TryGetValue(message.NickNameTo, out T iPEnd))
                {
                    int id = 0;
                    using (var ctx = new ChatContext())
                    {
                        var fromUser = ctx.Users.First(x => x.FullName == message.NickNameFrom);
                        var toUser = ctx.Users.First(x => x.FullName == message.NickNameTo);
                        var msg = new Message { UserFrom = fromUser, UserTo = toUser, IsSend = false, Text = message.Text };
                        ctx.Messages.Add(msg);
                        ctx.SaveChanges();
                        id = msg.MessageId;
                    }
                    message.id = id;
                    await _messageSouce.SendAsync(message, ep);
                    var forwardMessageJSON = message.SerialazeMessageFromJSON();
                    Console.WriteLine($"Message from = {message.NickNameFrom} to = {message.NickNameTo}");
                }
                else
                {
                    Console.WriteLine("ПОсльзователь не найден");
                }
            }
            //async Task RelyMessage(NetMessage message)
            //{
            //    if (clients.TryGetValue(message.NickNameTo, out IPEndPoint iPEnd))
            //    {
            //        int id = 0;
            //        using (var ctx = new ChatContext())
            //        {
            //            var fromUser = ctx.Users.First(x => x.FullName == message.NickNameFrom);
            //            var toUser = ctx.Users.First(x => x.FullName == message.NickNameTo);
            //            var msg = new Message { UserFrom = fromUser, UserTo = toUser, IsSend = false, Text = message.Text };
            //            ctx.Messages.Add(msg);
            //            ctx.SaveChanges();
            //            id = msg.MessageId;
            //        }
            //        message.id = id;
            //        await _messageSouce.SendAsync(message, ep);
            //        var forwardMessageJSON = message.SerialazeMessageFromJSON();
            //        Console.WriteLine($"Message from = {message.NickNameFrom} to = {message.NickNameTo}");
            //    }
            //    else
            //    {
            //        Console.WriteLine("Посльзователь не найден");
            //    }
            //}

            //async Task GetUnreadMessages(string userNickName)
            //{
            //    using (var ctx = new ChatContext())
            //    {
            //        var messages = ctx.Messages.Where(m => m.UserTo.FullName == userNickName && !m.IsSend).ToList();
            //            foreach (var message in messages)
            //        {
            //            if (clients.TryGetValue(message.UserFrom.FullName, out IPEndPoint iPEnd))
            //            {
            //                var netMessage = new NetMessage { Text = message.Text, DateTime = DateTime.Now, NickNameFrom = message.UserFrom.FullName
            //                    , NickNameTo = message.UserTo.FullName, EndPoint = clients.GetValueOrDefault(userNickName), Command = Command.Confirmation};
            //                await _messageSouce.SendAsync( netMessage, clients.GetValueOrDefault(userNickName));
            //            }
            //        }
            //    }
            //}

            async Task ConfirmMessageReceived(int? id)
            {
                Console.WriteLine("Message confiration id = ", id);
                using (var ctx = new ChatContext())
                {
                    var msg = ctx.Messages.FirstOrDefault(x => x.MessageId == id);
                    if (msg != null)
                    {
                        msg.IsSend = true;
                        await ctx.SaveChangesAsync();
                    }
                }
            }

            async Task ProcessMessage(NetMessage message)
            {
                switch (message.Command)
                {
                    case Command.Register:
                        await Register(message);
                        break;
                    case Command.Message:
                        await RelyMessage(message);
                        break;
                    case Command.Confirmation:
                        await ConfirmMessageReceived(message.id);
                        break;

                    default:
                        break;
                }
            }
            async Task Start()
            {

                //UdpClient udpClient = new UdpClient(12345);

                Console.WriteLine("Ожидание соообщения");
                while (work)
                {
                    try
                    {
                        var message = _messageSouce.Receive(ref ep);
                        Console.WriteLine(message.ToString());
                        await ProcessMessage(message);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }

            }

        }
    }
}

