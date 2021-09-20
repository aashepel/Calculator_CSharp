using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Exceptions
{
    class CalculatorZeroDivideException : CalculatorBaseException
    {
        public CalculatorZeroDivideException()
        {

        }

        public CalculatorZeroDivideException(string message) : base(message)
        {

        }

        public CalculatorZeroDivideException(string message, Exception inner) : base(message, inner)
        {

        }

        public override string Description
        {
            get { return "Деление на ноль невозможно"; }
        }
    }
}
