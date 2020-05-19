using Sales.Model;

namespace Sales.Services
{
    public class CustomersService : Service
    {
        public CustomersService(DbContext db) : base(db) { }

        public int Count => db.Customers.Count;
    }
}
