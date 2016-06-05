using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TDDVendingMachine.Test
{
    [TestClass]
    public class VendingMachineTest
    {
        private VendingMachine _vendingMachine;

        [TestInitialize]
        public void SetupTest()
        {
            _vendingMachine = new VendingMachine("VM Inc.");
        }

        [TestMethod]
        public void InsertCoinAreEqualAmountEuroValue()
        {
            var forInsert = new Money { Euros = 1, Cents = 0 };

            _vendingMachine.InsertCoin(forInsert);

            Assert.AreEqual(_vendingMachine.Amount.Euros, 1);
            Assert.AreEqual(_vendingMachine.Amount.Cents, 0);
        }

        [TestMethod]
        public void InsertCoinAreEqualAmountValue()
        {
            var forInsert = new Money { Euros = 0, Cents = 50 };

            _vendingMachine.InsertCoin(forInsert);

            Assert.AreEqual(_vendingMachine.Amount, forInsert);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknowCoinException))]
        public void InsertCoinUnknownCoinEuroAndCents()
        {
            var forInsert = new Money { Euros = 1, Cents = 10 };

            _vendingMachine.InsertCoin(forInsert);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknowCoinException))]
        public void InsertCoinUnknownCoinZeros()
        {
            var forInsert = new Money { Euros = 0, Cents = 0 };

            _vendingMachine.InsertCoin(forInsert);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknowCoinException))]
        public void InsertCoinUnknownCoinEuro()
        {
            var forInsert = new Money { Euros = 4, Cents = 0 };

            _vendingMachine.InsertCoin(forInsert);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknowCoinException))]
        public void InsertCoinUnknownCoinCents()
        {
            var forInsert = new Money { Euros = 0, Cents = 8 };

            _vendingMachine.InsertCoin(forInsert);
        }

        [TestMethod]
        public void InsertCoinCentsOverflow()
        {
            var forInsert = new Money { Euros = 0, Cents = 50 };

            _vendingMachine.InsertCoin(forInsert);
            _vendingMachine.InsertCoin(forInsert);

            Assert.AreEqual(_vendingMachine.Amount.Euros, 1);
            Assert.AreEqual(_vendingMachine.Amount.Cents, 0);
        }

        [TestMethod]
        public void ReturnMoneyNoInsert()
        {
            var returned = _vendingMachine.ReturnMoney();

            Assert.AreEqual(returned.Euros, 0);
            Assert.AreEqual(returned.Cents, 0);
        }

        [TestMethod]
        public void ReturnMoneyWhenInserted()
        {
            var forInsert = new Money { Euros = 0, Cents = 50 };

            _vendingMachine.InsertCoin(forInsert);

            var returned = _vendingMachine.ReturnMoney();

            Assert.AreEqual(returned.Euros, 0);
            Assert.AreEqual(returned.Cents, 50);
        }

        [TestMethod]
        public void ReturnMoneyWhenInsertedWithOverflow()
        {
            var forInsert = new Money { Euros = 0, Cents = 50 };

            _vendingMachine.InsertCoin(forInsert);
            _vendingMachine.InsertCoin(forInsert);

            var returned = _vendingMachine.ReturnMoney();

            Assert.AreEqual(returned.Euros, 1);
            Assert.AreEqual(returned.Cents, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(NegativeParameterException))]
        public void UpdateProductsWithNegativeAvailable()
        {
            _vendingMachine.Products = new[]{
                new Product
                {
                    Available = -1,
                    Price = new Money
                    {
                        Euros = 2,
                        Cents = 10
                    },
                    Name = "Qwe"
                }
            };
        }

        [TestMethod]
        [ExpectedException(typeof(NullOrEmptyParameterException))]
        public void UpdateProductsWithEmptyName()
        {
            _vendingMachine.Products = new[]{
                new Product
                {
                    Available = 1,
                    Price = new Money
                    {
                        Euros = 2,
                        Cents = 10
                    },
                    Name = ""
                }
            };
        }

        [TestMethod]
        [ExpectedException(typeof(OutOfRangeOrUnavailableException))]
        public void BuyOutOfRange()
        {
            _vendingMachine.Products = new[]{
                new Product
                {
                    Available = 1,
                    Price = new Money
                    {
                        Euros = 2,
                        Cents = 10
                    },
                    Name = "Qwe"
                }
            };

            _vendingMachine.Buy(1);
        }

        [TestMethod]
        [ExpectedException(typeof(OutOfRangeOrUnavailableException))]
        public void BuyUnavailable()
        {
            _vendingMachine.Products = new[]{
                new Product
                {
                    Available = 0,
                    Price = new Money
                    {
                        Euros = 2,
                        Cents = 10
                    },
                    Name = "Qwe"
                }
            };

            _vendingMachine.Buy(0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotEnoughException))]
        public void BuyWithNoMoney()
        {
            _vendingMachine.Products = new[]{
                new Product
                {
                    Available = 1,
                    Price = new Money
                    {
                        Euros = 2,
                        Cents = 10
                    },
                    Name = "Qwe"
                }
            };

            _vendingMachine.Buy(0);
        }

        [TestMethod]
        public void BuyNoRemainder()
        {
            var product = new Product
            {
                Available = 1,
                Price = new Money
                {
                    Euros = 2,
                    Cents = 0
                },
                Name = "Qwe"
            };

            _vendingMachine.Products = new[] { product };

            var forInsert = new Money { Euros = 1, Cents = 0 };

            _vendingMachine.InsertCoin(forInsert);
            _vendingMachine.InsertCoin(forInsert);

            var purchase = _vendingMachine.Buy(0);

            Assert.AreEqual(_vendingMachine.Products[0].Available, 0);
            Assert.AreEqual(_vendingMachine.Remainder.Euros, 0);
            Assert.AreEqual(_vendingMachine.Remainder.Cents, 0);
            Assert.AreEqual(product, purchase);
        }

        [TestMethod]
        public void BuyWithRemainder()
        {
            var product = new Product
            {
                Available = 1,
                Price = new Money
                {
                    Euros = 1,
                    Cents = 90
                },
                Name = "Qwe"
            };

            _vendingMachine.Products = new[] { product };

            var forInsert = new Money { Euros = 1, Cents = 0 };

            _vendingMachine.InsertCoin(forInsert);
            _vendingMachine.InsertCoin(forInsert);

            var purchase = _vendingMachine.Buy(0);

            Assert.AreEqual(_vendingMachine.Products[0].Available, 0);
            Assert.AreEqual(_vendingMachine.Remainder.Euros, 0);
            Assert.AreEqual(_vendingMachine.Remainder.Cents, 10);
            Assert.AreEqual(product, purchase);
        }
    }
}
