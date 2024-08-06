using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Abstracts
{
    internal interface IMessageSourse
    {
        Task SendAsync (NetMessage message,IPEndPoint ep);
        NetMessage Recive( IPEndPoint ep);

    }
}
