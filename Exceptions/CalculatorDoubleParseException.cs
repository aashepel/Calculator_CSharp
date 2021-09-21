using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Exceptions
{
    class CalculatorDoubleParseException : CalculatorBaseException
    {
        public override string Description
        {
            get
            {
                return "Упс, что-то пошло не так";
            }
        }
    }
}
