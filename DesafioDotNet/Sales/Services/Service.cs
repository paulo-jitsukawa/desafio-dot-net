using Sales.Model;

namespace Sales.Services
{
    /// <summary>
    /// Serviços sobre dados da camada Model.
    /// </summary>
    public abstract class Service
    {
        protected DbContext db;

        public Service(DbContext db)
        {
            this.db = db;
        }
    }
}
