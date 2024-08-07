using ChatCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChatCommon
{
    public interface IMessageSourseServer<T>
    {
        Task SendAsync (NetMessage message, T? ep);
        NetMessage Receive(ref T ep);
        T CreateEndPoint();
        T copyEndPoint(IPEndPoint t);

    }
}
