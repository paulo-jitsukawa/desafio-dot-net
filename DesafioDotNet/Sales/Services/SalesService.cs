using Sales.Model;
using System.Linq;

namespace Sales.Services
{
    public class SalesService : Service
    {
        public SalesService(DbContext db) : base(db) { }

        public decimal Total => db.Sales.Sum(s => s.Total);

        public int GetBestSaleId()
        {
            Sale sale = db.Sales.FirstOrDefault();
            if (sale == null)
            {
                return -1;
            }

            db.Sales.ForEach(s => sale = sale.Total > s.Total ? sale : s);
            return sale.Id;
        }
    }
}
