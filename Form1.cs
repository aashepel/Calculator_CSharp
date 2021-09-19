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

namespace Calculator
{
    public partial class Calculator : Form
    {
        private CalculatorLogic _calculatorLogic = new CalculatorLogic();
        private ToolTip _toolTip_DisplaySecondNumber = new ToolTip();
        private ToolTip _toolTip_DisplayFirstNumber = new ToolTip();
        private bool _displayCurrentNumber_readOnly = false;
        private const byte _maxLengthCurrentNumber = 49;
        public Calculator()
        {
            InitializeComponent();
        }

        private void NumberClickEvent(char num)
        {
            if (display_currentNumber.Text.Length >= _maxLengthCurrentNumber)
            {
                return;
            }
            if (_displayCurrentNumber_readOnly)
            {
                button_c_Click(null, null);
                _displayCurrentNumber_readOnly = false;
            }
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
            else if (_calculatorLogic.CurrentNumber.Length == 2)
            {
                if (_calculatorLogic.CurrentNumber.First() == '-')
                {
                    if (_calculatorLogic.CurrentNumber.ElementAt(1) == '0')
                    {
                        CurrentNumberChange(_calculatorLogic.CurrentNumber.Replace('0', num));
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
            else
            {
                CurrentNumberChange(_calculatorLogic.CurrentNumber + num);
            }
        }

        private void OperandClickEvent(Operands operand)
        {
            try
            {
                if (_displayCurrentNumber_readOnly) _displayCurrentNumber_readOnly = false;
                if (_calculatorLogic.OperandPerformed)
                {
                    if (_calculatorLogic.CurrentNumber.Length == 0)
                    {
                        _calculatorLogic.CurrentOperand = operand;
                        display_secondNumber.Text = _calculatorLogic.SecondNumber + (char)_calculatorLogic.CurrentOperand;
                    }
                    else
                    {
                        _calculatorLogic.Calculate();
                        _calculatorLogic.CurrentOperand = operand;
                        _calculatorLogic.SecondNumber = _calculatorLogic.Result;
                        display_secondNumber.Text = _calculatorLogic.Result + (char)operand;
                    }
                }
                else
                {
                    if (_calculatorLogic.CurrentNumber.Length == 0) return;
                    _calculatorLogic.OperandPerformed = true;
                    _calculatorLogic.CurrentOperand = operand;
                    _calculatorLogic.SecondNumber = _calculatorLogic.CurrentNumber;
                    display_secondNumber.Text = _calculatorLogic.CurrentNumber + (char)operand;
                }
                CurrentNumberChange("");
            }
            catch (Exceptions.CalculatorZeroDivideException)
            {
                MessageBox.Show("Деление на ноль невозможно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exceptions.CalculatorDoubleInfinityException)
            {
                MessageBox.Show("Невозможно выполнить операцию из-за переполнения", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CurrentNumberChange(string value)
        {
            _calculatorLogic.CurrentNumber = value;
            display_currentNumber.Text = _calculatorLogic.CurrentNumber;
            _toolTip_DisplayFirstNumber.SetToolTip(display_currentNumber, value);
        }
        private void SecondNumberChange(string value)
        {
            display_secondNumber.Text = value;
            _calculatorLogic.SecondNumber = value;
            _toolTip_DisplaySecondNumber.SetToolTip(display_secondNumber, value);
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
            CurrentNumberChange("");
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
            OperandClickEvent(Operands.Addition);
        }

        private void button_substraction_Click(object sender, EventArgs e)
        {
            OperandClickEvent(Operands.Substract);
        }

        private void button_mult_Click(object sender, EventArgs e)
        {
            OperandClickEvent(Operands.Multiply);
        }

        private void button_div_Click(object sender, EventArgs e)
        {
            OperandClickEvent(Operands.Div);
        }

        private void button_ce_Click(object sender, EventArgs e)
        {
            CurrentNumberChange("");
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
            string result = "";
            try
            {
                 result = _calculatorLogic.Calculate();
            }
            catch (Exceptions.CalculatorZeroDivideException)
            {
                MessageBox.Show("Деление на ноль невозможно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exceptions.CalculatorDoubleInfinityException)
            {
                MessageBox.Show("Невозможно выполнить операцию из-за переполнения", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (result != "")
            {
                display_secondNumber.Text = _calculatorLogic.CompilationResutString();
                _toolTip_DisplaySecondNumber.SetToolTip(display_secondNumber, display_secondNumber.Text);
                CurrentNumberChange(result);
                _calculatorLogic.OperandPerformed = false;
                _displayCurrentNumber_readOnly = true;
            }
        }

        private void button_negative_Click(object sender, EventArgs e)
        {
            if (_displayCurrentNumber_readOnly)
            {
                return;
            }
            if (_calculatorLogic.CurrentNumber.Length == 0)
            {
                CurrentNumberChange("-");
                return;
            }
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
            try
            {
                if (_calculatorLogic.CurrentNumber.Length <= 0)
                {
                    return;
                }
                _calculatorLogic.OperandPerformed = true;
                _calculatorLogic.CurrentOperand = Operands.Sqrt;
                display_secondNumber.Text = (char)Operands.Sqrt + $"({_calculatorLogic.CurrentNumber}) = {_calculatorLogic.Calculate()}";
                CurrentNumberChange(_calculatorLogic.Result);
                _displayCurrentNumber_readOnly = true;
            }
            catch (Exceptions.CalculatorNegativeRootException)
            {
                MessageBox.Show("Невозможно взять корень от отрицательного числа", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exceptions.CalculatorDoubleInfinityException)
            {
                MessageBox.Show("Невозможно взять корень из-за переполнения", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            }
        }
    }
}
