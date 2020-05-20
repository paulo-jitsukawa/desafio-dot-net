using Sales.Exceptions;
using Sales.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sales.Services
{
    public class FilesService : Service
    {
        public FilesService(DbContext db) : base(db) { }

        public void Fill(string path)
        {
            var line = 0;
            var customers = new List<Customer>();
            var salesmans = new List<Salesman>();
            var sales = new List<Sale>();

            using (var stream = new StreamReader(path))
            {
                var row = string.Empty;

                while ((row = stream.ReadLine()) != null)
                {
                    line++;
                    var cols = row.Split('ç');

                    try
                    {
                        switch (cols[0])
                        {
                            case "001":
                                var salesman = new Salesman
                                {
                                    CPF = cols[1],
                                    Name = cols[2],
                                    Salary = decimal.Parse(cols[3])
                                };
                                salesmans.Add(salesman);
                                break;

                            case "002":
                                var customer = new Customer
                                {
                                    CNPJ = cols[1],
                                    Name = cols[2],
                                    BusinessArea = cols[3]
                                };
                                customers.Add(customer);
                                break;

                            case "003":
                                var sale = new Sale
                                {
                                    Id = int.Parse(cols[1]),
                                    SalesmanName = cols[3]
                                };

                                foreach (var itemRegister in cols[2].TrimStart('[').TrimEnd(']').Split(','))
                                {
                                    var itemParameter = itemRegister.Split('-');
                                    var item = new SaleItem
                                    {
                                        Id = int.Parse(itemParameter[0]),
                                        Quantity = int.Parse(itemParameter[1]),
                                        Price = decimal.Parse(itemParameter[2])
                                    };
                                    sale.Items.Add(item);
                                }

                                sales.Add(sale);
                                break;

                            default:
                                var name = Path.GetFileName(path);
                                throw new FileInvalidFormatException($"Os dados do arquivo {name} não foram inseridos porque o identificador da linha {line} está incorreto.", line, name);
                        }
                    }
                    catch (Exception e)
                    {
                        var name = Path.GetFileName(path);
                        throw new FileInvalidFormatException($"Os dados do arquivo {name} não foram inseridos porque a linha {line} está com formato inválido.", line, name, e);
                    }
                }
            }

            db.Customers.AddRange(customers);
            db.Salesmans.AddRange(salesmans);
            db.Sales.AddRange(sales);
        }
    }
}
