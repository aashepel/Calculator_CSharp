using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Exceptions
{
    class CalculatorNegativeRootException : CalculatorBaseException
    {
        public override string Description
        {
            get { return "Невозможно взять корень от отрицательного числа"; }
        }
    }
}
