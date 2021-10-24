#nullable enable
namespace Assignment3
{
    public class Request
    {
        public string? Method { get; set; }
        public string? Path { get; set; }
        public string? Date { get; set; }
        public string? Body  { get; set; }
    }
    
    public class Response
    {
        public string Status { get; set; }
        public string? Body { get; set; }
    }

    
    
    class Program
    {
        private static void Main(string[] args)
        {
            var server = new Server(5000);
        }
    }
}
