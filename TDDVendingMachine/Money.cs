using System;

namespace TDDVendingMachine
{
    public struct Money
    {
        private int _cents;

        public int Euros { get; set; }

        public int Cents
        {
            get { return _cents; }
            set
            {
                if (value % 5 == 0)
                {
                    _cents = value;
                }
                else
                {
                    throw new UnknowCoinException(String.Format("Cents amount must be divisible by 5: {0}.", value));
                }
            }
        }
    }
}
