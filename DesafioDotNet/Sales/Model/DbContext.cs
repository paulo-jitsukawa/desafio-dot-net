using System.Collections.Generic;

namespace Sales.Model
{
    /// <summary>
    /// Fornece acesso às coleções de dados de maneira semelhante ao DbContext do EntityFramework Core.
    /// </summary>
    public class DbContext
    {
        public List<Customer> Customers { get; private set; } = new List<Customer>();

        public List<Salesman> Salesmans { get; private set; } = new List<Salesman>();

        public List<Sale> Sales { get; private set; } = new List<Sale>();

        public Report Report { get; set; }
    }
}
