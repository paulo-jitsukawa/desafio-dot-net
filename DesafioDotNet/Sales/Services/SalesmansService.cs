using Sales.Model;
using System.Linq;

namespace Sales.Services
{
    public class SalesmansService : Service
    {
        public SalesmansService(DbContext db) : base(db) { }

        public int Count => db.Salesmans.Count;

        public string GetWorstSalesmanName()
        {
            dynamic salesman = null;
            var price = decimal.MaxValue;

            db.Sales
                .GroupBy(s => s.SalesmanName)
                .Select(s => new { SalesmanName = s.Key, Total = s.Sum(t => t.Total) })
                .ToList()
                .ForEach(s => salesman = s.Total <= price ? s : salesman);

            return salesman.SalesmanName;
        }
    }
}
