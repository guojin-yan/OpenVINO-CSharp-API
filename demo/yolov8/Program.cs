using OpenCvSharp;

namespace yolov8
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //Console.Title = "������ʾ";
            ApplicationConfiguration.Initialize();
            Application.Run(new FormModelDeployPlat());
            //Application.Run(new FormTestTime());
        }
    }
}