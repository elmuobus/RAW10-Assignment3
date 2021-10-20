// RAW10 - DELEBECQUE Alexis, DUMONT-ROTY Loïc, SHOWIKI Ali Isac
using System;

namespace Assignment3
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Server server = new Server();
                server.startServer();
            }
            catch (Exception e) 
            {
                Console.WriteLine(e);
            }

        }
    }
}