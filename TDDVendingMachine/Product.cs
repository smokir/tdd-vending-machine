using System;

namespace TDDVendingMachine
{
    public struct Product
    {
        private int _available;
        private string _name;

        /// <summary>Gets or sets the available amount of product.</summary>
        public int Available
        {
            get { return _available; }
            set
            {
                if (value < 0)
                {
                    throw new NegativeParameterException("Available amount of product cannot be less than 0.");
                }

                _available = value;
            }
        }

        /// <summary>Gets or sets the product price.</summary>
        public Money Price { get; set; }

        /// <summary>Gets or sets the product name.</summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new NullOrEmptyParameterException("Product name cannot be empty.");
                }

                _name = value;
            }
        }
    }
}
