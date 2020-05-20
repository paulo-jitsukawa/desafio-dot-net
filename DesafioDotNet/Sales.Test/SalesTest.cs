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
        [DataRow("001Á1234567891234ÁPedroÁ50000\n001Á3245678865434ÁPauloÁ40000.99\n002Á2345675434544345ÁJose da SilvaÁRural\n002Á2345675433444345ÁEduardo PereiraÁRural\n003Á10Á[1-10-100,2-30-2.50,3-40-3.10]ÁPedro\n003Á08Á[1-34-10,2-33-1.50,3-40-0.10]ÁPaulo", 2, 2, 10, "Paulo")]
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
        [DataRow("001Á1234567891234ÁPedroÁ50000\n001Á3245678865434ÁPauloÁ40000.99\n002Á2345675434544345ÁJose da SilvaÁRural\n002Á2345675433444345ÁEduardo PereiraÁRural\n003Á10Á[1-10-100,2-30-2.50,3-40-3.10]ÁPedro\n003Á08Á[1-34-10,2-33-1.50,3-40-0.10]ÁPaulo", "1592.50")]
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
