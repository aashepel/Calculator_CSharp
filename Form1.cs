using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using Calculator.Exceptions;

namespace Calculator
{
    public partial class Calculator : Form
    {
        private CalculatorLogic _calculatorLogic = new CalculatorLogic();
        private bool _displayCurrentNumber_readOnly = false;
        private bool _abilityChangeOperand = false;
        private const byte _maxLengthCurrentNumber = 49;
        public Calculator()
        {
            InitializeComponent();
            CurrentNumberChange("0");
        }

        private void NumberClickEvent(char num)
        {
            if (display_currentNumber.Text.Length >= _maxLengthCurrentNumber)
            {
                return;
            }
            if (_displayCurrentNumber_readOnly)
            {
                CurrentNumberChange("0");
                if (!_calculatorLogic.OperandPerformed)
                {
                    SecondNumberChange("");
                }
                _displayCurrentNumber_readOnly = false;
            }
            _abilityChangeOperand = true;
            _calculatorLogic.CurrentNumberIsSet = true;
            if (_calculatorLogic.CurrentNumber.Length == 1)
            {
                if (_calculatorLogic.CurrentNumber.First() == '0')
                {
                    CurrentNumberChange(num.ToString());
                }
                else
                {
                    CurrentNumberChange(_calculatorLogic.CurrentNumber + num);
                }
            }
            else
            {
                CurrentNumberChange(_calculatorLogic.CurrentNumber + num);
            }
        }

        private void OperandBinaryClickEvent(Operands operand)
        {
            if (!_abilityChangeOperand) return;
            try
            {
                if (_displayCurrentNumber_readOnly) _displayCurrentNumber_readOnly = false;
                if (_calculatorLogic.OperandPerformed)
                {
                    if (!_calculatorLogic.CurrentNumberIsSet)
                    {
                        _calculatorLogic.CurrentOperand = operand;
                        _calculatorLogic.OperandPerformed = true;
                        display_secondNumber.Text = _calculatorLogic.SecondNumber + (char)_calculatorLogic.CurrentOperand;
                    }
                    else
                    {
                        CurrentNumberChange(CalculatorLogic.NormalizeNumber(_calculatorLogic.CurrentNumber));
                        _calculatorLogic.CalculateBinaryOperand();
                        _calculatorLogic.CurrentOperand = operand;
                        _calculatorLogic.OperandPerformed = true;
                        _calculatorLogic.SecondNumber = _calculatorLogic.Result;
                        display_secondNumber.Text = _calculatorLogic.Result + (char)operand;
                        _calculatorLogic.CurrentNumberIsSet = false;
                    }
                }
                else
                {
                    CurrentNumberChange(CalculatorLogic.NormalizeNumber(_calculatorLogic.CurrentNumber));
                    if (String.IsNullOrEmpty(_calculatorLogic.CurrentNumber)) return;
                    _calculatorLogic.OperandPerformed = true;
                    _calculatorLogic.CurrentNumberIsSet = false;
                    _calculatorLogic.CurrentOperand = operand;
                    _calculatorLogic.SecondNumber = _calculatorLogic.CurrentNumber;
                    display_secondNumber.Text = _calculatorLogic.CurrentNumber + (char)operand;
                }
                CurrentNumberChange("0");
            }
            catch (CalculatorBaseException ex)
            {
                MessageBox.Show(ex.Description, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void OperandUnaryClickEvent(Operands operand)
        {
            try
            {
                _calculatorLogic.OperandPerformed = true;
                _calculatorLogic.CurrentOperand = operand;
                display_secondNumber.Text = (char)operand + $"({_calculatorLogic.CurrentNumber}) = {_calculatorLogic.CalculateUnaryOperand()}";
                CurrentNumberChange(_calculatorLogic.Result);
                _displayCurrentNumber_readOnly = true;
                _calculatorLogic.CurrentNumberIsSet = true;
            }
            catch (CalculatorBaseException ex)
            {
                MessageBox.Show(ex.Description, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CurrentNumberChange(string value)
        {
            _calculatorLogic.CurrentNumber = value;
            display_currentNumber.Text = _calculatorLogic.CurrentNumber;
        }
        private void SecondNumberChange(string value)
        {
            display_secondNumber.Text = value;
            _calculatorLogic.SecondNumber = value;
        }

        private void button_0_Click(object sender, EventArgs e)
        {
            NumberClickEvent('0');
        }

        private void button_1_Click(object sender, EventArgs e)
        {
            NumberClickEvent('1');
        }

        private void button_2_Click(object sender, EventArgs e)
        {
            NumberClickEvent('2');
        }

        private void button_3_Click(object sender, EventArgs e)
        {
            NumberClickEvent('3');
        }

        private void button_4_Click(object sender, EventArgs e)
        {
            NumberClickEvent('4');
        }

        private void button_5_Click(object sender, EventArgs e)
        {
            NumberClickEvent('5');
        }

        private void button_6_Click(object sender, EventArgs e)
        {
            NumberClickEvent('6');
        }

        private void button_7_Click(object sender, EventArgs e)
        {
            NumberClickEvent('7');
        }

        private void button_8_Click(object sender, EventArgs e)
        {
            NumberClickEvent('8');
        }

        private void button_9_Click(object sender, EventArgs e)
        {
            NumberClickEvent('9');
        }

        private void button_c_Click(object sender, EventArgs e)
        {
            _calculatorLogic.CurrentNumberIsSet = false;
            CurrentNumberChange("0");
            SecondNumberChange("");
            _calculatorLogic.OperandPerformed = false;
            _displayCurrentNumber_readOnly = false;
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (!_displayCurrentNumber_readOnly && _calculatorLogic.CurrentNumber.Length > 0)
            {
                CurrentNumberChange(_calculatorLogic.Del_CurrentNumber());
            }
        }

        private void button_plus_Click(object sender, EventArgs e)
        {
            OperandBinaryClickEvent(Operands.Addition);
        }

        private void button_substraction_Click(object sender, EventArgs e)
        {
            OperandBinaryClickEvent(Operands.Substract);
        }

        private void button_mult_Click(object sender, EventArgs e)
        {
            OperandBinaryClickEvent(Operands.Multiply);
        }

        private void button_div_Click(object sender, EventArgs e)
        {
            OperandBinaryClickEvent(Operands.Div);
        }

        private void button_ce_Click(object sender, EventArgs e)
        {
            _calculatorLogic.CurrentNumberIsSet = false;
            CurrentNumberChange("0");
            if (_displayCurrentNumber_readOnly)
            {
                SecondNumberChange("");
                _displayCurrentNumber_readOnly = false;
            }
        }

        private void button_comma_Click(object sender, EventArgs e)
        {
            if (!_displayCurrentNumber_readOnly)
            {
                CurrentNumberChange(_calculatorLogic.CurrentNumber + ',');
            }
        }

        private void button_eq_Click(object sender, EventArgs e)
        {
            if (!_calculatorLogic.OperandPerformed) return;
            string result;
            try
            {
                CurrentNumberChange(CalculatorLogic.NormalizeNumber(_calculatorLogic.CurrentNumber));
                result = _calculatorLogic.CalculateBinaryOperand();
                if (result != "")
                {
                    display_secondNumber.Text = _calculatorLogic.CompilationResutString();
                    CurrentNumberChange(result);
                    _calculatorLogic.OperandPerformed = false;
                    _displayCurrentNumber_readOnly = true;
                }
                _abilityChangeOperand = true;
            }
            catch (CalculatorBaseException ex)
            {
                MessageBox.Show(ex.Description, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_negative_Click(object sender, EventArgs e)
        {
            if (_calculatorLogic.CurrentNumber == "0" || _calculatorLogic.CurrentNumber.Length == 0) return;
            if (_calculatorLogic.CurrentNumber.First() == '-')
            {
                CurrentNumberChange(_calculatorLogic.CurrentNumber.Remove(0, 1));
            }
            else
            {
                CurrentNumberChange(_calculatorLogic.CurrentNumber.Insert(0, "-"));
            }
        }

        private void button_sqrt_Click(object sender, EventArgs e)
        {
            OperandUnaryClickEvent(Operands.Sqrt);
        }

        private void button_ms_Click(object sender, EventArgs e)
        {
            _calculatorLogic.Memory = _calculatorLogic.CurrentNumber;
            memory_label.Text = $"Memory: {_calculatorLogic.Memory}";
            _calculatorLogic.MemoryIsSet = true;
        }

        private void button_mc_Click(object sender, EventArgs e)
        {
            _calculatorLogic.Memory = "";
            memory_label.Text = "Memory No Set";
            _calculatorLogic.MemoryIsSet = false;
        }

        private void button_mr_Click(object sender, EventArgs e)
        {
            if (_calculatorLogic.MemoryIsSet)
            {
                CurrentNumberChange(_calculatorLogic.Memory);
                if (!_calculatorLogic.OperandPerformed)
                {
                    SecondNumberChange("");
                }
                _displayCurrentNumber_readOnly = true;
            }
        }

        private void button_power_Click(object sender, EventArgs e)
        {
            OperandUnaryClickEvent(Operands.Squaring);
        }
    }
}
