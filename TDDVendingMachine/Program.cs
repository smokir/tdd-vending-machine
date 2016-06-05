using System;
using System.ComponentModel;
using System.Linq;

namespace TDDVendingMachine
{
    class Program
    {
        public static T Parse<T>(string input)
        {
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(input);
        }

        static void Main(string[] args)
        {
            var vendingMachine = new VendingMachine("VM Inc.");

            while (true)
            {
                Console.WriteLine(vendingMachine.Manufacturer);
                Console.WriteLine();

                var productList = vendingMachine.Products.Where(x => x.Available > 0).ToList();
                var isAnyProducts = productList.Count > 0;

                if (isAnyProducts)
                {
                    productList = vendingMachine.Products.Where(x => x.Available > 0).ToList();

                    productList.ForEach(x => Console.WriteLine("{0}: {1} for {2},{3} EUR",
                        productList.IndexOf(x), x.Name, x.Price.Euros, x.Price.Cents));
                }
                else
                {
                    Console.WriteLine("No products availabe.");
                }
                Console.WriteLine();

                if (vendingMachine.Amount.Euros > 0 || vendingMachine.Amount.Cents > 0)
                {
                    Console.WriteLine("Inserted: {0} euro and {1} cent", vendingMachine.Amount.Euros, vendingMachine.Amount.Cents);
                    Console.WriteLine();
                }

                Console.WriteLine("Available commands:");
                Console.WriteLine("  \"{0}\" to update product list.", Command.Update);
                Console.WriteLine("  \"{0}\" to insert coins.", Command.Insert);
                Console.WriteLine("  \"{0}\" to buy product.", Command.Buy);
                Console.WriteLine("  \"{0}\" to return your coins.", Command.Return);
                Console.WriteLine("  \"{0}\" to exit.", Command.Exit);
                Console.WriteLine();
                Console.Write("Please enter command: ");

                try
                {
                    var input = Console.ReadLine();
                    switch (input)
                    {
                        case Command.Update:
                            Console.WriteLine("Use following format to add product to product list:");
                            Console.WriteLine("  [available] [price in cents] [name].");
                            Console.WriteLine("Example: 8 \"Coke\" each for 150 cents => 8 150 Coke");
                            Console.Write("Please enter new product info: ");

                            var updateInput = Console.ReadLine();
                            var updateParts = updateInput.Split(' ');

                            if (updateParts.Length < 3)
                            {
                                throw new WrongCommandFormatException("Wrong product info format.");
                            }

                            var available = Parse<int>(updateParts[0]);
                            var price = Parse<int>(updateParts[1]);
                            var priceEuros = price / 100;
                            var priceCents = price % 100;
                            var name = Parse<string>(updateParts[2]);

                            var product = new Product
                            {
                                Available = available,
                                Price = new Money
                                {
                                    Euros = priceEuros,
                                    Cents = priceCents
                                },
                                Name = name
                            };

                            vendingMachine.Products = vendingMachine.Products.Concat(new[] { product }).ToArray();

                            Console.Write("{0} \"{1}\" each for {2} euro {3} cent added.",
                                product.Available, product.Name, product.Price.Euros, product.Price.Cents);

                            break;
                        case Command.Insert:
                            Console.WriteLine("Machine accepts following coins:");
                            Console.WriteLine("  5 cent, 10 cent, 20 cent, 50 cent");
                            Console.WriteLine("  and 1 euro, 2 euro.");
                            Console.WriteLine("You can insert only one coin at once.");
                            Console.WriteLine("Use following format to insert coin: [euro] [cent].");
                            Console.WriteLine("Example: 1 euro => 1 0, 5 cent => 0 5");
                            Console.Write("Please enter coin: ");

                            var insertInput = Console.ReadLine();
                            var insertParts = insertInput.Split(' ');

                            if (insertParts.Length < 2)
                            {
                                throw new WrongCommandFormatException("Wrong format.");
                            }

                            var euroCoin = Parse<int>(insertParts[0]);
                            var centCoin = Parse<int>(insertParts[1]);

                            vendingMachine.InsertCoin(new Money { Euros = euroCoin, Cents = centCoin });

                            Console.Write("{0} euro {1} cent inserted.", euroCoin, centCoin);

                            break;
                        case Command.Buy:
                            Console.Write("Enter product number: ");

                            var buyInput = Console.ReadLine();
                            var number = Parse<int>(buyInput);

                            var purchase = vendingMachine.Buy(number);
                            Console.WriteLine("Please take your {0}.", purchase.Name);

                            Console.Write("Please take your change: {0} euro {1} cent.",
                                vendingMachine.Remainder.Euros, vendingMachine.Remainder.Cents);

                            break;
                        case Command.Return:
                            var _return = vendingMachine.ReturnMoney();
                            Console.WriteLine("Returned {0} euro {1} cent", _return.Euros, _return.Cents);
                            Console.Write("Please collect your money.");
                            break;
                        case Command.Exit:
                            return;
                        default:
                            throw new UnknownCommandException("Unknow command.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Write("Command failed: {0}", ex.Message);
                }
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}
