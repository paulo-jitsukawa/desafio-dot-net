using Sales.Controllers;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Sales
{
    public class Program
    {
        public const string HOMEPATH = "../../../../..";

        public static async Task Main(string[] args)
        {
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            await new SalesController(HOMEPATH).Run();
        }
    }
}
