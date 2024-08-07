using ChatApp;
using System.Net;

namespace StartSolutions
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if(args.Length == 0)
            {
                var s = new Server<IPEndPoint>(new UdpMessageSouceServer());
               await s.Start();
            }
            else
            if(args.Length == 1) 
            {
                var c= new Client<IPEndPoint>(args[0],new UdpMessageSourseClient(args[1],12345));
                await c.Start();
            }
           
        }
    }
}
