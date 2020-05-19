using System.Collections.Generic;
using System.Linq;

namespace Sales.Model
{
    public class Sale
    {
        public int Id { get; set; }

        public string SalesmanName { get; set; }

        public List<SaleItem> Items { get; set; } = new List<SaleItem>();

        public decimal Total => Items.Sum(i => i.Quantity * i.Price);
    }
}
