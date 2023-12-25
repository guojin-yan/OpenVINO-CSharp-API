using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.Extensions.utility
{
    public static class Slog
    {
        public static void INFO(string msg) 
        {
            Console.WriteLine("[ INFO ] " + msg);
        }
        public static void WARN(string msg)
        {
            Console.WriteLine("[ WARN ] " + msg);
        }
        public static void ERROR(string msg)
        {
            Console.WriteLine("[ ERROR ] " + msg);
        }
    }
}
