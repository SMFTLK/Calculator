using System;

namespace Calc
{
    public partial class Form : System.Windows.Forms.Form
    {
        /// <summary>
        /// Операция над числами
        /// </summary>
        private string operation;
        /// <summary>
        /// Первое число
        /// </summary>
        private string leftValue;
        /// <summary>
        /// Второе число 
        /// </summary>
        private string rightValue;

        /// <summary>
        /// Переменная для исключения деления на ноль
        /// </summary>
        private const string warnAboutDivisionByZero = "Деление на ноль невозможно.";

        /// <summary>
        /// Очищение строк и переменных, если вводится новое число после операции
        /// </summary>
        private bool isNewNumber;
        /// <summary>
        /// Состояние, обозначающее следующее: записывается первое или второе число в данный момент
        /// </summary>
        private bool isReadingFirstNumber;
        /// <summary>
        /// Состояние, обозначающее завершённость операции над числами
        /// </summary>
        private bool isOperationGoingOn;

        public Form()
        {
            InitializeComponent();

            isNewNumber = true;
            isReadingFirstNumber = true;
            isOperationGoingOn = false;

            operation = "";

            leftValue = "0";
            rightValue = "0";

            uI_Label.Text = "";
            uI_TextBox.Text = "0";
        }

        /// <summary>
        /// Кнопка с цифрой для ввода числа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Numbers_Click(object sender, EventArgs e)
        {
            // Отправитель (кнопка)
            UI_Button_Numbers B = (UI_Button_Numbers)sender;

            // Если пользователь вводит число в первый раз (или после результата)
            if (!isNewNumber && !isOperationGoingOn)
            {
                isNewNumber = true;

                leftValue = "0";
                rightValue = "0";

                uI_Label.Text = "";
                uI_TextBox.Text = "0";
                operation = "";
            }

            // Если произошло исключения деления на 0
            if (uI_TextBox.Text == warnAboutDivisionByZero)
            {
                uI_TextBox.Text = B.Text;
                uI_TextBox.Refresh();
            }
            else
            {
                if (uI_TextBox.Text.Length < 17)
                {
                    if (uI_TextBox.Text != "0")
                        uI_TextBox.Text += B.Text;

                    else
                        uI_TextBox.Text = B.Text;

                    uI_TextBox.Refresh();
                }
            }
        }

        /// <summary>
        /// Кнопка арифметической операции над числами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Operations_Click(object sender, EventArgs e)
        {
            // Отправитель (кнопка)
            UI_Button_Operations B = (UI_Button_Operations)sender;
            
            // При вызове события нажатия кнопки операции в переменную operation записывается арифм. операция кнопки
            operation = B.Text;

            if (isOperationGoingOn && rightValue != "0")
            {

            }

            isOperationGoingOn = true;

            // Если считывается число В ПЕРВЫЙ РАЗ
            if (isReadingFirstNumber)
            {
                leftValue = uI_TextBox.Text;

                // Это нужно для смены символа (с короткого тире на дефис)
                if (operation == "−")
                    operation = "-";

                isReadingFirstNumber = false;
            }
            else
            {
                switch (operation)
                {
                    case "+":

                        leftValue = (Convert.ToDouble(leftValue) + Convert.ToDouble(uI_TextBox.Text)).ToString(); break;

                    case "−":
                    {
                        if (leftValue == "0")
                            leftValue = (Convert.ToDouble(uI_TextBox.Text)).ToString();
                        else
                            leftValue = (Convert.ToDouble(leftValue) - Convert.ToDouble(uI_TextBox.Text)).ToString();

                        operation = "-";
                        break;
                    }
                    case "×":
                    {
                        if (leftValue == "0")
                            leftValue = (Convert.ToDouble(uI_TextBox.Text)).ToString();
                        else
                            leftValue = (Convert.ToDouble(leftValue) * Convert.ToDouble(uI_TextBox.Text)).ToString();

                        break;
                    }
                    case "÷":
                    {
                        if (leftValue == "0")
                            leftValue = (Convert.ToDouble(uI_TextBox.Text)).ToString();
                        else
                            leftValue = (Convert.ToDouble(leftValue) / Convert.ToDouble(uI_TextBox.Text)).ToString();

                        break;
                    }
                    case "%":
                    {
                        if (leftValue == "0")
                            leftValue = (Convert.ToDouble(uI_TextBox.Text)).ToString();
                        else
                            leftValue = (Convert.ToDouble(leftValue) * Convert.ToDouble(uI_TextBox.Text) / 100).ToString();
                        
                        break;
                    }
                    default:

                        leftValue = (Convert.ToDouble(uI_TextBox.Text)).ToString(); break;
                }
            }

            uI_Label.Text = leftValue + " " + operation;
            uI_Label.Refresh();

            uI_TextBox.Text = "0";
        }

        /// <summary>
        /// Кнопка "равняется" для завершения арифметической операции
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Equals_Click(object sender, EventArgs e)
        {
            double DRValue, DLValue, result;

            // Если нет операции
            if (!isOperationGoingOn && leftValue != "0" && rightValue != "0")
            {
                leftValue = uI_TextBox.Text;

                DLValue = Convert.ToDouble(leftValue);
                DRValue = Convert.ToDouble(rightValue);
                result = GetResult(operation, DLValue, DRValue);

                uI_Label.Text = leftValue + " " + operation + " " + rightValue + " =";
                uI_TextBox.Text = result.ToString();

                uI_TextBox.Refresh();
                uI_Label.Refresh();

                return;
            }

            rightValue = uI_TextBox.Text;

            #region Если есть операция

            // Если первое число - ноль
            if (isOperationGoingOn && leftValue == "0" && rightValue != "0")
            {
                DRValue = Convert.ToDouble(rightValue);
                result = GetResult(operation, DRValue);

                uI_Label.Text = leftValue + " " + operation + " " + rightValue + " =";
            }
            // Если второе число - ноль
            else if (isOperationGoingOn && leftValue != "0" && rightValue == "0")
            {
                rightValue = leftValue;

                DLValue = Convert.ToDouble(leftValue);
                DRValue = Convert.ToDouble(rightValue);
                result = GetResult(operation, DLValue, DRValue);

                uI_Label.Text = leftValue + " " + operation + " " + rightValue + " =";
            }
            // Если оба числа не равны нулю
            else if (isOperationGoingOn && rightValue != "0" && leftValue != "0")
            {
                DLValue = Convert.ToDouble(leftValue);
                DRValue = Convert.ToDouble(rightValue);
                result = GetResult(operation, DLValue, DRValue);

                leftValue = result.ToString();
                uI_Label.Text += " " + uI_TextBox.Text + " =";
            }
            // Если цифра введена, но не выбрана операция и произошло нажатие кнопки "равняется"
            else
            {
                result = Convert.ToDouble(uI_TextBox.Text);
                uI_Label.Text = uI_TextBox.Text + " =";
            }

            #endregion

            isNewNumber = false;
            isReadingFirstNumber = true;
            isOperationGoingOn = false;

            uI_TextBox.Text = result.ToString();

            uI_TextBox.Refresh();
            uI_Label.Refresh();
        }

        /// <summary>
        /// Кнопка "запятая"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Numbers_Comma_Click(object sender, EventArgs e)
        {
            UI_Button_Numbers B = (UI_Button_Numbers)sender;

            if (!uI_TextBox.Text.Contains(","))
                uI_TextBox.Text += B.Text;

            uI_TextBox.Refresh();
        }

        /// <summary>
        /// Кнопка "С"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Operations_C_Click(object sender, EventArgs e)
        {
            uI_TextBox.Text = "0";
            leftValue = "0";
            rightValue = "0";

            isNewNumber = true;
            isReadingFirstNumber = true;
            isOperationGoingOn = false;

            uI_Label.Text = string.Empty;
            operation = string.Empty;

            uI_TextBox.Refresh();
            uI_Label.Refresh();
        }

        /// <summary>
        /// Кнопка "СЕ"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Operations_CE_Click(object sender, EventArgs e)
        {
            uI_TextBox.Text = "0";

            uI_TextBox.Refresh();
        }

        /// <summary>
        /// Кнопка "Возвести в квадрат"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Operations_Square_Click(object sender, EventArgs e)
        {
            double result;
            result = Convert.ToDouble(uI_TextBox.Text) * Convert.ToDouble(uI_TextBox.Text);

            uI_Label.Text = uI_TextBox.Text + "² =";
            uI_TextBox.Text = result.ToString();

            uI_TextBox.Refresh();
            uI_Label.Refresh();

            isNewNumber = false;
        }

        /// <summary>
        /// Кнопка "Поменять знак"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Numbers_ChangeSign_Click(object sender, EventArgs e)
        {
            double result;
            result = Convert.ToDouble(uI_TextBox.Text);
            
            if (leftValue == "0")
                uI_Label.Text = "-(" + uI_TextBox.Text + ") =";

            uI_TextBox.Text = (-result).ToString();

            uI_TextBox.Refresh();
            uI_Label.Refresh();
        }

        /// <summary>
        /// Кнопка "Возвести в степень -1"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Operations_Hyperbole_Click(object sender, EventArgs e)
        {
            double result;

            if (Convert.ToDouble(uI_TextBox.Text) == 0)
                uI_TextBox.Text = warnAboutDivisionByZero;

            else
            {
                result = 1.0d / Convert.ToDouble(uI_TextBox.Text);

                uI_Label.Text = "1 / " + uI_TextBox.Text + " =";
                uI_TextBox.Text = result.ToString();
            }

            uI_TextBox.Refresh();
            uI_Label.Refresh();

            isNewNumber = false;
        }

        /// <summary>
        /// Событие, при котором изменяется текст в uI_Label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_TextBox_TextChanged(object sender, EventArgs e)
        {
            UI_TextBox T = (UI_TextBox)sender;

            if (T.Text == warnAboutDivisionByZero)
            {
                uI_Button_Numbers_ChangeSign.Enabled = false;
                uI_Button_Numbers_Comma.Enabled = false;

                uI_Button_Operations_Addition.Enabled = false;
                uI_Button_Operations_Division.Enabled = false;
                uI_Button_Operations_Multiplication.Enabled = false;
                uI_Button_Operations_Subtraction.Enabled = false;

                uI_Button_Operations_Hyperbole.Enabled = false;
                uI_Button_Operations_Modulo.Enabled = false;
                uI_Button_Operations_Square.Enabled = false;
                uI_Button_Operations_SquareRoot.Enabled = false;

                uI_Button_Equals.Enabled = false;
            }
            else
            {
                uI_Button_Numbers_ChangeSign.Enabled = true;
                uI_Button_Numbers_Comma.Enabled = true;

                uI_Button_Operations_Addition.Enabled = true;
                uI_Button_Operations_Division.Enabled = true;
                uI_Button_Operations_Multiplication.Enabled = true;
                uI_Button_Operations_Subtraction.Enabled = true;

                uI_Button_Operations_Hyperbole.Enabled = true;
                uI_Button_Operations_Modulo.Enabled = true;
                uI_Button_Operations_Square.Enabled = true;
                uI_Button_Operations_SquareRoot.Enabled = true;

                uI_Button_Equals.Enabled = true;
            }
        }

        /// <summary>
        /// Кнопка "Возвести в корень второй степени"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Operations_SquareRoot_Click(object sender, EventArgs e)
        {
            double result;
            result = Math.Sqrt(Convert.ToDouble(uI_TextBox.Text));

            uI_Label.Text = "²√(" + uI_TextBox.Text + ") =";
            uI_TextBox.Text = result.ToString();

            uI_TextBox.Refresh();
            uI_Label.Refresh();

            isNewNumber = false;
        }

        /// <summary>
        /// Кнопка "D"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Operations_Delete_Click(object sender, EventArgs e)
        {
            if (uI_TextBox.Text == warnAboutDivisionByZero)
                uI_TextBox.Text = "0";

            else
            {
                uI_TextBox.Text = uI_TextBox.Text.Substring(0, uI_TextBox.Text.Length - 1);

                if (uI_TextBox.Text == "") uI_TextBox.Text = "0";
            }

            uI_TextBox.Refresh();
        }

        private double GetResult(string oper, double rightVar, double leftVar)
        {
            double result;

            switch (oper)
            {
                case "+":
                    result = rightVar + leftVar; break;

                case "-":
                    result = rightVar - leftVar; break;

                case "×":
                    result = rightVar * leftVar; break;

                case "÷":
                    result = rightVar / leftVar; break;

                case "%":
                    result = rightVar * leftVar / 100; break;

                default:
                    result = Convert.ToDouble(uI_TextBox.Text); break;
            }

            return result;
        }

        private double GetResult(string oper, double var)
        {
            double result;

            switch (oper)
            {
                case "+":
                    result = var + var; break;

                case "-":
                    result = var - var; break;

                case "×":
                    result = var * var; break;

                case "÷":
                    result = var / var; break;

                case "%":
                    result = var * var / 100; break;

                default:
                    result = Convert.ToDouble(uI_TextBox.Text); break;
            }

            return result;
        }

        private void Form_SizeChanged(object sender, EventArgs e)
        {

        }
    }
}
