using Sales.Exceptions;
using Sales.Model;
using System;
using System.Globalization;

namespace Sales.Services
{

    public class FilesService : Service
    {
        public FilesService(DbContext db) : base(db) { }

        public void Fill(string name, string[] rows)
        {
            var line = 0;

            foreach (var row in rows)
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
                            db.Salesmans.Add(salesman);
                            break;

                        case "002":
                            var customer = new Customer
                            {
                                CNPJ = cols[1],
                                Name = cols[2],
                                BusinessArea = cols[3]
                            };
                            db.Customers.Add(customer);
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
                                    Price = decimal.Parse(itemParameter[2], NumberStyles.Currency)
                                };
                                sale.Items.Add(item);
                            }

                            db.Sales.Add(sale);
                            break;

                        default:
                            throw new FileInvalidFormatException($"Identificador não reconhecido na linha {line} do arquivo {name}.", line, name);
                    }
                }
                catch (Exception e)
                {
                    throw new FileInvalidFormatException($"Formato de registro inválido na linha {line} do arquivo {name}.", line, name, e);
                }
            }
        }
    }
}
