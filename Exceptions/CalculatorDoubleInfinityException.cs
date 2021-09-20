using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Exceptions
{
    class CalculatorDoubleInfinityException : CalculatorBaseException
    {
        public override string Description
        {
            get { return "Невозможно выполнить операцию из-за переполнения"; }
        }
    }
}
