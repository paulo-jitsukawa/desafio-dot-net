using Sales.Controllers;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace Sales
{
    public class Program
    {
        private static string InputDir { get; set; }
        private static string OutputDir { get; set; }

        private static SalesController Controller { get; set; } = new SalesController();

        public static void Main(string[] args)
        {
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            try
            {
                var settings = ConfigurationManager.AppSettings;

                InputDir = settings["InputDir"];
                OutputDir = settings["OutputDir"];

                if (InputDir == null || OutputDir == null)
                {
                    Console.WriteLine("O arquivo App.config deve conter os parâmetros InputDir e OutputDir.");
                    return;
                }
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Erro ao ler o arquivo App.config.");
                return;
            }

            Console.WriteLine("Sistema iniciado. (Pressione qualquer tecla para sair.){0}", Environment.NewLine);

            Directory.CreateDirectory(InputDir);
            Directory.CreateDirectory(OutputDir);

            var files = new DirectoryInfo(InputDir).GetFiles().Select(f => f.FullName).ToArray();
            Controller.Compile(files, OutputDir);

            var fsw = new FileSystemWatcher();
            fsw.Created += FileCreated;
            fsw.Path = InputDir;
            fsw.EnableRaisingEvents = true;

            Console.ReadLine();
            Console.WriteLine("Sistema encerrado.");
        }

        private static void FileCreated(object sender, FileSystemEventArgs e)
        {
            Controller.Compile(new string[] { e.FullPath }, OutputDir);
        }
    }
}
