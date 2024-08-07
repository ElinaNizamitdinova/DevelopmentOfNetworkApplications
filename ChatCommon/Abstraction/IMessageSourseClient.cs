using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using ChatCommon.Models;


namespace ChatCommon
{
    public interface IMessageSourseClient<T>
    {

        Task SendAsync(NetMessage message, T? ep);
        NetMessage Receive( ref T ep);
        T GetServer();
        T CreateEndPoint();

    }
}
