﻿using Calculator.Exceptions;
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
        }
        private string _currentNumber = "0";
        private string _secondNumber = "0";
        private string _result = null;
        private bool _firstNullSym = false;
        private string _memory;
        private bool _memoryIsSet = false;
        private Operands _currentOperand;
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
                if (IsValidNumber(value))
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
            ResultString += '=' + res;
            return ResultString;
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
            catch (FormatException ex)
            {
                
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
                    break;
                case Operands.Substract:
                    resultOpertion = DSecondNumber - DCurrentNumber;
                    if (Double.IsInfinity(resultOpertion))
                    {
                        CurrentNumber = "";
                        throw new CalculatorDoubleInfinityException();
                    }
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
                    break;
                case Operands.Multiply:
                    resultOpertion = DSecondNumber * DCurrentNumber;
                    if (Double.IsInfinity(resultOpertion))
                    {
                        CurrentNumber = "";
                        throw new CalculatorDoubleInfinityException();
                    }
                    break;
                default:
                    return null;
            }
            Result = DoubleToString(resultOpertion);
            OperandPerformed = false;
            return Result;
        }
        public string CalculateUnaryOperand()
        {
            if (CurrentNumber.Length == 0) return null;
            double resultOperation;
            double DCurrentNumber = 0;
            try
            {
                DCurrentNumber = Double.Parse(CurrentNumber);
            }
            catch (FormatException ex)
            {

            }
            switch (CurrentOperand)
            {           
                case Operands.Sqrt:
                    resultOperation = Math.Sqrt(DCurrentNumber);
                    if (Double.IsInfinity(resultOperation))
                    {
                        throw new CalculatorDoubleInfinityException();
                    }
                    if (DCurrentNumber < 0)
                    {
                        throw new CalculatorNegativeRootException();
                    }
                    Result = DoubleToString(resultOperation);
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
