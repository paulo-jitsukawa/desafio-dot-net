using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sales.Extensions;
using Sales.Model;
using Sales.Services;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Sales.Test
{
    [TestClass]
    public class SalesTest
    {
        [ClassInitialize]
        public static void Init(TestContext context)
        {
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }

        /// <summary>
        /// Testa resultado esperado para o programa reportar.
        /// </summary>
        [DataTestMethod]
        [DataRow("001�1234567891234�Pedro�50000\n001�3245678865434�Paulo�40000.99\n002�2345675434544345�Jose da Silva�Rural\n002�2345675433444345�Eduardo Pereira�Rural\n003�10�[1-10-100,2-30-2.50,3-40-3.10]�Pedro\n003�08�[1-34-10,2-33-1.50,3-40-0.10]�Paulo", 2, 2, 10, "Paulo")]
        public void ReportTest(string content, int customersCount, int salesmansCount, int bestSaleId, string worstSalesman)
        {
            var db = new DbContext();
            var ser = new FilesService(db);
            var path = $"{Directory.GetCurrentDirectory()}/SalesReportTest.txt".ToNormalizedPath();

            File.WriteAllText(path, content);
            ser.Fill(path);

            Assert.AreEqual(customersCount, new CustomersService(db).Count);
            Assert.AreEqual(salesmansCount, new SalesmansService(db).Count);
            Assert.AreEqual(bestSaleId, new SalesService(db).GetBestSaleId());
            Assert.AreEqual(worstSalesman, new SalesmansService(db).GetWorstSalesmanName());
        }

        /// <summary>
        /// Testa soma dos totais das vendas registradas.
        /// </summary>
        [DataTestMethod]
        [DataRow("001�1234567891234�Pedro�50000\n001�3245678865434�Paulo�40000.99\n002�2345675434544345�Jose da Silva�Rural\n002�2345675433444345�Eduardo Pereira�Rural\n003�10�[1-10-100,2-30-2.50,3-40-3.10]�Pedro\n003�08�[1-34-10,2-33-1.50,3-40-0.10]�Paulo", "1592.50")]
        public void SalesTotalTest(string content, string total)
        {
            var db = new DbContext();
            var ser = new FilesService(db);
            var path = $"{Directory.GetCurrentDirectory()}/SalesTotalTest.txt".ToNormalizedPath();

            File.WriteAllText(path, content);
            ser.Fill(path);

            Assert.AreEqual(decimal.Parse(total), new SalesService(db).Total);
        }
    }
}
