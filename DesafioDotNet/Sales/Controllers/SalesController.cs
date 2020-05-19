using Sales.Exceptions;
using Sales.Model;
using Sales.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sales.Controllers
{
    /// <summary>
    /// Controla o sistema de vendas.
    /// </summary>
    public class SalesController
    {
        private string path;
        private DirectoryInfo dir;
        private DbContext db;

        private CustomersService customersService;
        private FilesService filesService;
        private ReportService reportsService;
        private SalesmansService salesmansService;
        private SalesService salesService;

        public SalesController(string root)
        {
            path = $"{root.Replace('\\', '/').TrimEnd('/')}/Data";
            dir = new DirectoryInfo($"{path}/in");
            db = new DbContext();

            customersService = new CustomersService(db);
            filesService = new FilesService(db);
            reportsService = new ReportService(db);
            salesmansService = new SalesmansService(db);
            salesService = new SalesService(db);
        }

        public async Task Run(int sleep = 500)
        {
            Console.WriteLine("Sistema iniciado.");

            while (true)
            {
                var newFile = false;

                Directory.CreateDirectory($"{path}/in");
                Directory.CreateDirectory($"{path}/out");

                foreach (var file in dir.GetFiles()?.Where(d => !d.Name.Contains(".processado")))
                {
                    newFile = true;

                    try
                    {
                        Console.Write($"Processando {file.Name}... ");

                        var lines = await File.ReadAllLinesAsync(file.FullName);
                        filesService.Fill(file.Name, lines);

                        File.Move(file.FullName, $"{file.FullName}.processado");

                        Console.WriteLine(" OK!");
                    }
                    catch (FileInvalidFormatException e)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"ERRO: {e.Message}" + e.InnerException == null ? "" : $" ({e.InnerException.Message}).");
                        Console.WriteLine("Pressione qualquer tecla para continuar...");
                        Console.ReadLine();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"ERRO INESPERADO: {e.Message}");
                        Console.WriteLine("Pressione qualquer tecla para continuar...");
                        Console.ReadLine();
                    }
                }

                if (newFile)
                {
                    var report = new Report
                    {
                        CustomersCount = customersService.Count,
                        SalesmansCount = salesmansService.Count,
                        BestSaleId = salesService.GetBestSaleId(),
                        WorstSalesman = salesmansService.GetWorstSalesmanName()
                    };

                    File.WriteAllText
                    (
                        $"{path}/out/Report.txt",
                        reportsService.GetReport(report)
                    );
                }

                Thread.Sleep(sleep);
            }
        }
    }
}
