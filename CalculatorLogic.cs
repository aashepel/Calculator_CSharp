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
        Squaring = '^'
    }
    internal class CalculatorLogic
    {
        public CalculatorLogic()
        {
        }
        private const byte _maxLengthCurrentNumber = 49;
        private bool _displayCurrentNumber_readOnly = false;
        private bool _abilityChangeOperand = false;
        private string _currentNumber = "0";
        private string _secondNumber = "0";
        private string _result = null;
        private bool _firstNullSym = false;
        private string _memory;
        private bool _memoryIsSet = false;
        private bool _currentNumberIsSet = false;
        private Operands _currentOperand;

        public bool DisplayCurrentNumber_readOnly
        {
            get { return _displayCurrentNumber_readOnly; }
            set { _displayCurrentNumber_readOnly = value; }
        }
        public bool AbilityChangeOperand
        {
            get { return _abilityChangeOperand; }
            set { _abilityChangeOperand = value; }
        }
        public bool CurrentNumberIsSet
        {
            get { return _currentNumberIsSet; }
            set { _currentNumberIsSet = value; }
        }
        public string Memory
        {
            get {  return _memory; }
            set {  _memory = value; }
        }
        public bool MemoryIsSet
        {
            get {  return _memoryIsSet; }
            set {  _memoryIsSet = value; }
        }
        public Operands CurrentOperand
        {
            get {  return _currentOperand; }
            set {  _currentOperand = value; }
        }
        public bool OperandPerformed { get; set; }
        public string Result 
        {
            get { return _result; }
            set { _result = value; }
        }
        public bool FirstNullSym
        {
            get {  return _firstNullSym; }
            set {  _firstNullSym = value; }
        }
        public string CurrentNumber 
        {
            get {  return _currentNumber; }
            set
            {
                if (value.Length > _maxLengthCurrentNumber) return;
                if (value == "")
                {
                    _currentNumber = "0";
                }
                else if (IsValidNumber(value))
                {
                    _currentNumber = value;
                }
            }
        }
        public string SecondNumber
        {
            get {  return _secondNumber; }
            set
            {
                _secondNumber = value;
            }
        }
        public string Del_CurrentNumber()
        {
            if (_currentNumber.Length == 1)
            {
                return "0";
            }
            else if (_currentNumber.Length > 1)
            {
                 _currentNumber = _currentNumber.Remove(_currentNumber.Length - 1, 1);
            }
            return _currentNumber;
        }
        public static bool IsValidNumber(string value)
        {
            return (String.IsNullOrEmpty(value) || (value.FirstOrDefault() == '-' && value.Length == 1) || Double.TryParse(value, out _));
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
            ResultString += '=';
            return ResultString;
        }
        public static string NormalizeNumber(string value)
        {
            if (value.Last() == ',')
            {
                value = value.Remove(value.Length - 1, 1);
            }
            if (value.First() == '-')
            {
                bool compilanceCondition = false;
                for (int i = 1; i < value.Length; i++)
                {
                    if (value[i] == '0' || value[i] == ',')
                    {

                    }
                    else
                    {
                        compilanceCondition = true;
                        break;
                    }
                }
                if (compilanceCondition == false)
                {
                    value = "0";
                }
            }
            return value;
        }
        private void CheckCalculateBinaryOperandException(double value)
        {
            if (Double.IsInfinity(value))
            {
                CurrentNumber = "";
                throw new CalculatorDoubleInfinityException();
            }
        }
        public string CalculateBinaryOperand()
        {
            if (!OperandPerformed || SecondNumber.Length <= 0)
            {
                return null;
            }
            if (String.IsNullOrEmpty(CurrentNumber))
            {
                CurrentNumber = SecondNumber;
            }
            double DSecondNumber = 0;
            double DCurrentNumber = 0;

            try
            {
                DSecondNumber = DoubleParse(SecondNumber);
                DCurrentNumber = DoubleParse(CurrentNumber);
            }
            catch
            {
                throw new CalculatorDoubleParseException();
            }
            double resultOpertion = 0;
            switch (CurrentOperand)
            {
                case Operands.Addition:
                    resultOpertion = DSecondNumber + DCurrentNumber;
                    CheckCalculateBinaryOperandException(resultOpertion);
                    break;
                case Operands.Substract:
                    resultOpertion = DSecondNumber - DCurrentNumber;
                    CheckCalculateBinaryOperandException(resultOpertion);
                    break;
                case Operands.Div:
                    if (DCurrentNumber == 0)
                    {
                        CurrentNumber = "";
                        throw new CalculatorZeroDivideException();
                    }
                    resultOpertion = DSecondNumber / DCurrentNumber;
                    CheckCalculateBinaryOperandException(resultOpertion);
                    break;
                case Operands.Multiply:
                    resultOpertion = DSecondNumber * DCurrentNumber;
                    CheckCalculateBinaryOperandException(resultOpertion);
                    break;
                default:
                    return null;
            }
            Result = DoubleToString(resultOpertion);
            OperandPerformed = false;
            return Result;
        }
        private void CheckCalculateUnaryOperandException(double value)
        {
            if (Double.IsInfinity(value))
            {
                throw new CalculatorDoubleInfinityException();
            }
        }
        public string CalculateUnaryOperand()
        {
            if (CurrentNumber.Length == 0) return null;
            double resultOperation;
            double DCurrentNumber;
            try
            {
                DCurrentNumber = Double.Parse(CurrentNumber);
            }
            catch
            {
                throw new CalculatorDoubleParseException();
            }
            switch (CurrentOperand)
            {           
                case Operands.Sqrt:
                    resultOperation = Math.Sqrt(DCurrentNumber);
                    CheckCalculateUnaryOperandException(resultOperation);
                    if (DCurrentNumber.CompareTo(0) < 0)
                    {
                        throw new CalculatorNegativeRootException();
                    }
                    break;
                case Operands.Squaring:
                    resultOperation = Math.Pow(DCurrentNumber, 2);
                    CheckCalculateUnaryOperandException(resultOperation);
                    break;
                default:
                    return null;
            }
            OperandPerformed = false;
            Result = DoubleToString(resultOperation);
            return Result;
        }
    }
}
