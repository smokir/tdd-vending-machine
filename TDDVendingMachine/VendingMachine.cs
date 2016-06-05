using System;
using System.Collections.Generic;

namespace TDDVendingMachine
{
    public class VendingMachine : IVendingMachine
    {
        private static readonly List<int> AcceptableEuros = new List<int>(new[] { 1, 2 });
        private static readonly List<int> AcceptableCents = new List<int>(new[] { 5, 10, 20, 50 });

        private string _manufacturer;

        public string Manufacturer
        {
            get { return _manufacturer; }
            private set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new NullOrEmptyParameterException("Vending machine manufacturer cannot be empty.");
                }

                _manufacturer = value;
            }
        }

        public Money Amount { get; private set; }

        public Money Remainder { get; private set; }

        public Product[] Products { get; set; }

        public VendingMachine(string manufacturer)
        {
            Manufacturer = manufacturer;
            Amount = new Money { Euros = 0, Cents = 0 };
            Remainder = new Money { Euros = 0, Cents = 0 };
            Products = new Product[0];
        }

        public Money InsertCoin(Money amount)
        {
            if (AcceptableEuros.Contains(amount.Euros) && amount.Cents == 0
                || amount.Euros == 0 && AcceptableCents.Contains(amount.Cents))
            {
                var cents = Amount.Cents + amount.Cents;

                Amount = new Money
                {
                    Euros = Amount.Euros + amount.Euros + (cents / 100),
                    Cents = cents % 100
                };
            }
            else
            {
                throw new UnknowCoinException(String.Format("You can insert only one type of acceptable coin at once: {0} euro {1} cent.",
                    amount.Euros, amount.Cents));
            }

            return Amount;
        }

        public Money ReturnMoney()
        {
            var _return = Amount;
            Amount = new Money { Euros = 0, Cents = 0 };
            return _return;
        }

        public Product Buy(int productNumber)
        {
            Remainder = new Money { Euros = 0, Cents = 0 };

            if (productNumber < 0
                || productNumber >= Products.Length
                || Products[productNumber].Available <= 0)
            {
                throw new OutOfRangeOrUnavailableException("Wrong product number.");
            }

            var product = Products[productNumber];
            var productPrice = product.Price.Euros * 100 + product.Price.Cents;
            var insertedMoney = Amount.Euros * 100 + Amount.Cents;

            if (productPrice > insertedMoney)
            {
                throw new NotEnoughException("Not enough money.");
            }

            Products[productNumber].Available--;

            Remainder = new Money
            {
                Euros = (insertedMoney - productPrice) / 100,
                Cents = (insertedMoney - productPrice) % 100
            };

            Amount = new Money { Euros = 0, Cents = 0 };

            return product;
        }
    }
}