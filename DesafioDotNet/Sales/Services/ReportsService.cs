using Sales.Model;
using System;

namespace Sales.Services
{
    public class ReportService : Service
    {
        public ReportService(DbContext db) : base(db) { }

        public string GetReport(Report r)
        {
            var text = string.Concat(
                "{0} Quantidade de clientes no arquivo de entrada: ", r.CustomersCount,
                "{0} Quantidade de vendedores no arquivo de entrada: ", r.SalesmansCount,
                "{0} ID da venda mais cara: ", r.BestSaleId,
                "{0} O pior vendedor: ", r.WorstSalesman
            );

            return string.Format(text, Environment.NewLine);
        }
    }
}
