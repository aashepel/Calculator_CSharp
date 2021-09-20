using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Exceptions
{
    class CalculatorBaseException : Exception
    {
        public CalculatorBaseException()
        {

        }

        public CalculatorBaseException(string message) : base(message)
        {

        }

        public CalculatorBaseException(string message, Exception inner) : base(message, inner)
        {

        }

        public virtual string Description
        {
            get
            {
                return "";
            }
        }
    }
}
