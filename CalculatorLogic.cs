using Calculator.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Calculator
{
    enum Operands
    {
        Addition = '+',
        Percent = '%',
        Multiply = '*',
        Div = '/',
        Substract = '-',
        Sqrt = '√',
        Square = 'p'
    }
    internal class CalculatorLogic
    {
        public CalculatorLogic()
        {
            MemoryIsSet = false;
            Memory = "";
        }
        private string _currentNumber = "0";
        private string _secondNumber = "0";
        private string _result = "0";
        private bool _firstNullSym = false;
        public string Memory { get; set; }
        public bool MemoryIsSet { get; set; }
        public Operands CurrentOperand { get; set; }
        public bool OperandPerformed { get; set; }
        public string Result 
        {
            get 
            {
                return _result;
            }
            set
            {
                _result = value;
            }
        }
        public bool FirstNullSym
        {
            get
            {
                return _firstNullSym;
            }
            set
            {
                _firstNullSym = value;
            }
        }
        public string CurrentNumber 
        {
            get 
            {
                return _currentNumber;
            }
            set
            {
                if (IsValidNumber(value))
                {
                    _currentNumber = value;
                }
            }
        }
        public string SecondNumber
        {
            get
            {
                return _secondNumber;
            }
            set
            {
                if (IsValidNumber(value))
                {
                    _secondNumber = value;
                }
            }
        }
        public string Del_CurrentNumber()
        {
            if (CurrentNumber.Length > 0)
            {
                CurrentNumber.Remove(CurrentNumber.Length - 1, 1);
            }
            return CurrentNumber;
        }
        public static bool IsValidNumber(string value)
        {
            return (value.Length == 0 || value.FirstOrDefault() == '-' || Double.TryParse(value, out _));
        }
        private static double DoubleParse(string value)
        {   
            return Double.Parse(value);
        }
        private static string DoubleToString(double value)
        {
            return value.ToString();
        }
        public string CompilationResutString()
        {
            string curNum = CurrentNumber;
            string secNum = SecondNumber;
            string res = Result;
            Operands operand = CurrentOperand;
            string ResultString = secNum + ' ' + (char)operand + ' ';
            if (curNum.Length > 0 && curNum.First() == '-')
            {
                ResultString += $"({curNum})";
            }
            else
            {
                ResultString += curNum;
            }
            ResultString += '=' + res;
            return ResultString;
        }
        public string Calculate()
        {
            if ((OperandPerformed && SecondNumber.Length > 0) || CurrentOperand == Operands.Sqrt)
            {
                if (CurrentNumber.Length == 0)
                {
                    CurrentNumber = SecondNumber;
                }
                double DSecondNumber = 0;
                double DCurrentNumber = 0;

                try
                {
                    if (CurrentOperand != Operands.Sqrt)
                    {
                        DSecondNumber = DoubleParse(SecondNumber);
                    }
                    DCurrentNumber = DoubleParse(CurrentNumber);
                }
                catch (FormatException ex)
                {
                    throw;
                }
                double resultOpertion = 0;
                switch (CurrentOperand)
                {
                    case Operands.Addition:                        
                        resultOpertion = DSecondNumber + DCurrentNumber;
                        if (Double.IsInfinity(resultOpertion))
                        {
                            CurrentNumber = "";
                            throw new CalculatorDoubleInfinityException();
                        }
                        Result = DoubleToString(resultOpertion);
                        break;
                    case Operands.Substract:
                        resultOpertion = DSecondNumber - DCurrentNumber;
                        if (Double.IsInfinity(resultOpertion))
                        {
                            CurrentNumber = "";
                            throw new CalculatorDoubleInfinityException();
                        }
                        Result = DoubleToString(resultOpertion);
                        break;
                    case Operands.Div:
                        if (DCurrentNumber == 0)
                        {
                            CurrentNumber = "";
                            throw new CalculatorZeroDivideException();
                        }
                        resultOpertion = DSecondNumber / DCurrentNumber;
                        if (Double.IsInfinity(resultOpertion))
                        {
                            CurrentNumber = "";
                            throw new CalculatorDoubleInfinityException();
                        }
                        Result = DoubleToString(resultOpertion);
                        break;
                    case Operands.Multiply:
                        resultOpertion = DSecondNumber * DCurrentNumber;
                        if (Double.IsInfinity(resultOpertion))
                        {
                            CurrentNumber = "";
                            throw new CalculatorDoubleInfinityException();
                        }
                        Result = DoubleToString(resultOpertion);
                        break;
                    case Operands.Sqrt:
                        resultOpertion = Math.Sqrt(DCurrentNumber);
                        if (Double.IsInfinity(resultOpertion))
                        {
                            CurrentNumber = "";
                            throw new CalculatorDoubleInfinityException();
                        }
                        if (DCurrentNumber < 0)
                        {
                            CurrentNumber = "";
                            throw new CalculatorNegativeRootException();
                        }
                        Result = DoubleToString(resultOpertion);
                        break;
                    default:
                        return "";
                }
                return Result;
            }
            return "";
        }
    }
}
