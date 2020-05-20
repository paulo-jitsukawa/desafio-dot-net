using Sales.Exceptions;
using Sales.Extensions;
using Sales.Model;
using Sales.Services;
using System;
using System.IO;

namespace Sales.Controllers
{
    /// <summary>
    /// Controla o sistema de vendas.
    /// </summary>
    public class SalesController
    {
        private DbContext db;
        private CustomersService customersService;
        private FilesService filesService;
        private ReportService reportsService;
        private SalesmansService salesmansService;
        private SalesService salesService;

        public SalesController()
        {
            db = new DbContext();
            customersService = new CustomersService(db);
            filesService = new FilesService(db);
            reportsService = new ReportService(db);
            salesmansService = new SalesmansService(db);
            salesService = new SalesService(db);
        }

        public void Compile(string[] inputs, string output)
        {
            if (inputs.Length == 0 || string.IsNullOrWhiteSpace(output))
            {
                return;
            }

            try
            {
                foreach (var input in inputs)
                {
                    var path = input.ToNormalizedPath();

                    Console.Write($"Processando {Path.GetFileName(path)}... ");

                    filesService.Fill(path);

                    Console.WriteLine("OK!");
                }
            }
            catch (FileInvalidFormatException e)
            {
                Console.WriteLine("ERRO!");
                Console.WriteLine(e.InnerException == null ? e.Message : $"{e.Message} ({e.InnerException.Message}).");
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRO!");
                Console.WriteLine($"ERRO INESPERADO: {e.Message}");
            }

            var report = new Report
            {
                CustomersCount = customersService.Count,
                SalesmansCount = salesmansService.Count,
                BestSaleId = salesService.GetBestSaleId(),
                WorstSalesman = salesmansService.GetWorstSalesmanName()
            };

            try
            {
                Console.Write($"Gerando Report.txt... ");

                File.WriteAllText
                (
                    $"{output}/Report.txt".ToNormalizedPath(),
                    reportsService.GetReport(report)
                );

                Console.WriteLine("OK!");
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRO!");
                Console.WriteLine(e.Message);
            }
        }
    }
}
