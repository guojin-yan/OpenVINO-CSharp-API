using OpenVinoSharp;
using Version = OpenVinoSharp.Version;

namespace Yolo26Det_net10._0
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Core core = new Core();
            KeyValuePair<string, Version> version = core.get_versions("CPU");
            Console.WriteLine($"Device: {version.Key}, Version: {version.Value.to_string()}");
        }
    }
}
