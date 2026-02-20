using OpenVinoSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Version = OpenVinoSharp.Version;

namespace Yolo26Det_net4._8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Core core = new Core();
            KeyValuePair<string, Version> version = core.get_versions("CPU");
            Console.WriteLine($"Device: {version.Key}, Version: {version.Value.to_string()}");
        }
    }
}
