using System;

namespace TDDVendingMachine
{
    public class NullOrEmptyParameterException : ApplicationException
    {
        public NullOrEmptyParameterException(string message) : base(message) { }
    }

    public class UnknowCoinException : ApplicationException
    {
        public UnknowCoinException(string message) : base(message) { }
    }

    public class NegativeParameterException : ApplicationException
    {
        public NegativeParameterException(string message) : base(message) { }
    }

    public class OutOfRangeOrUnavailableException : ApplicationException
    {
        public OutOfRangeOrUnavailableException(string message) : base(message) { }
    }

    public class NotEnoughException : ApplicationException
    {
        public NotEnoughException(string message) : base(message) { }
    }

    public class WrongCommandFormatException : ApplicationException
    {
        public WrongCommandFormatException(string message) : base(message) { }
    }

    public class UnknownCommandException : ApplicationException
    {
        public UnknownCommandException(string message) : base(message) { }
    }
}
