using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class User
    {
        public int id { get; set; }
        public string? FullName { get; set; }
        public virtual List<Message>? MessagesTo { get; set; } = new();
        public virtual List<Message>? MessagesFrom { get; set; } = new();
    }
}
