namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var dbContext = new ChatContext()) ;
        }
    }
}
