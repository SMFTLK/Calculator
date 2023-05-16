using System;
using System.Drawing;
using System.Windows.Forms;

namespace Calc
{
    public partial class Form : System.Windows.Forms.Form
    {
        #region Переменные

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

        /// <summary>
        /// Тема калькулятора (светлая или тёмная)
        /// </summary>
        private bool isDarkTheme;


        private const int dimmingAmount = 30;


        /// <summary>
        /// Переменная, содержащая в себе изначальные размеры кнопки
        /// </summary>
        private Size originalButtonSize;

        /// <summary>
        /// Переменная, содержащая в себе изначальные размеры формы
        /// </summary>
        private Size originalFormSize;

        /// <summary>
        /// Переменная, содержащая в себе изначальные размеры текста
        /// </summary>
        private Size originalTextBoxSize;

        /// <summary>
        /// Переменная, содержащая в себе изначальные размеры текста
        /// </summary>
        private Size originalLabelSize;

        #endregion

        public Form()
        {
            InitializeComponent();

            isNewNumber = true;
            isReadingFirstNumber = true;
            isOperationGoingOn = false;
            isDarkTheme = true;

            operation = "";

            leftValue = "0";
            rightValue = "0";

            uI_Label.Text = "";
            uI_TextBox.Text = "0";
        }

        private void Form_Load(object sender, EventArgs e)
        {
            isDarkTheme = true;

            originalFormSize = new Size(Size.Width, Size.Height);
            originalButtonSize = new Size(UI_Button_Number.buttonWidth, UI_Button_Number.buttonHeight);
            originalTextBoxSize = new Size(UI_TextBox.textBoxWidth, UI_TextBox.textBoxHeight);
            originalLabelSize = new Size(UI_Label.labelWidth, UI_Label.labelHeight);
        }

        #region События

        #region Кнопки (клик)

        /// <summary>
        /// Кнопка с цифрой для ввода числа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Number_Click(object sender, EventArgs e)
        {
            // Отправитель (кнопка)
            UI_Button_Number B = (UI_Button_Number)sender;

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
        private void UI_Button_Operation_Click(object sender, EventArgs e)
        {
            // Отправитель (кнопка)
            UI_Button_Operation B = (UI_Button_Operation)sender;

            // Если считывается число В ПЕРВЫЙ РАЗ
            if (isReadingFirstNumber)
            {
                leftValue = uI_TextBox.Text;

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

            // При вызове события нажатия кнопки операции в переменную operation записывается арифм. операция кнопки
            operation = B.Text;

            isOperationGoingOn = true;

            uI_Label.Text = leftValue + " " + operation;
            uI_Label.Refresh();

            uI_TextBox.Text = "0";
        }

        /// <summary>
        /// Равно
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Equals_Click(object sender, EventArgs e)
        {
            double DRValue, DLValue, result;

            // Если нет операции
            if (!isOperationGoingOn)
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

            // Если первое число - ноль, и операция идёт
            if (isOperationGoingOn && leftValue == "0" && rightValue != "0")
            {
                DRValue = Convert.ToDouble(rightValue);
                result = GetResult(operation, 0, DRValue);

                uI_Label.Text = leftValue + " " + operation + " " + rightValue + " =";

                leftValue = result.ToString();
            }
            // Если второе число - ноль
            else if (isOperationGoingOn && leftValue != "0" && rightValue == "0")
            {
                rightValue = leftValue;

                DLValue = Convert.ToDouble(leftValue);
                DRValue = Convert.ToDouble(rightValue);
                result = GetResult(operation, DLValue, DRValue);

                uI_Label.Text = leftValue + " " + operation + " " + rightValue + " =";

                leftValue = result.ToString();
            }
            // Если оба числа не равны нулю
            else if (isOperationGoingOn && rightValue != "0" && leftValue != "0")
            {
                DLValue = Convert.ToDouble(leftValue);
                DRValue = Convert.ToDouble(rightValue);
                result = GetResult(operation, DLValue, DRValue);

                uI_Label.Text += " " + uI_TextBox.Text + " =";

                leftValue = result.ToString();
            }
            // Если цифра введена, но не выбрана операция и произошло нажатие кнопки "равняется"
            else
            {
                result = Convert.ToDouble(uI_TextBox.Text);

                uI_Label.Text = uI_TextBox.Text + " =";
            }

            #endregion

            // Обнуление переменных
            isNewNumber = false;
            isReadingFirstNumber = true;
            isOperationGoingOn = false;

            uI_TextBox.Text = result.ToString();

            uI_TextBox.Refresh();
            uI_Label.Refresh();
        }

        /// <summary>
        /// Запятая
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Number_Comma_Click(object sender, EventArgs e)
        {
            UI_Button_Number B = (UI_Button_Number)sender;

            if (!uI_TextBox.Text.Contains(","))
                uI_TextBox.Text += B.Text;

            uI_TextBox.Refresh();
        }

        /// <summary>
        /// С
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Operation_C_Click(object sender, EventArgs e)
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
        /// СЕ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Operation_CE_Click(object sender, EventArgs e)
        {
            uI_TextBox.Text = "0";

            uI_TextBox.Refresh();
        }

        /// <summary>
        /// Возвести в квадрат
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Operation_Square_Click(object sender, EventArgs e)
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
        /// Поменять знак
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Number_ChangeSign_Click(object sender, EventArgs e)
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
        /// Возвести в степень -1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Operation_Hyperbole_Click(object sender, EventArgs e)
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
        /// Корень второй степени
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Operation_SquareRoot_Click(object sender, EventArgs e)
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
        /// Delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Operation_Delete_Click(object sender, EventArgs e)
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

        /// <summary>
        /// Sin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Operation_Sin_Click(object sender, EventArgs e)
        {
            double result;
            result = Math.Sin(Convert.ToDouble(uI_TextBox.Text));

            uI_Label.Text = "sin(" + uI_TextBox.Text + ") =";
            uI_TextBox.Text = result.ToString();

            uI_TextBox.Refresh();
            uI_Label.Refresh();

            isNewNumber = false;
        }

        /// <summary>
        /// Cos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Operation_Cos_Click(object sender, EventArgs e)
        {
            double result;
            result = Math.Cos(Convert.ToDouble(uI_TextBox.Text));

            uI_Label.Text = "cos(" + uI_TextBox.Text + ") =";
            uI_TextBox.Text = result.ToString();

            uI_TextBox.Refresh();
            uI_Label.Refresh();

            isNewNumber = false;
        }

        /// <summary>
        /// Tang
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Operation_Tg_Click(object sender, EventArgs e)
        {
            double result;
            result = Math.Tan(Convert.ToDouble(uI_TextBox.Text));

            uI_Label.Text = "tg(" + uI_TextBox.Text + ") =";
            uI_TextBox.Text = result.ToString();

            uI_TextBox.Refresh();
            uI_Label.Refresh();

            isNewNumber = false;
        }

        /// <summary>
        /// Abs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Operation_Abs_Click(object sender, EventArgs e)
        {
            double result;
            result = Math.Abs(Convert.ToDouble(uI_TextBox.Text));

            uI_Label.Text = "|" + uI_TextBox.Text + "| =";
            uI_TextBox.Text = result.ToString();

            uI_TextBox.Refresh();
            uI_Label.Refresh();

            isNewNumber = false;
        }

        /// <summary>
        /// Cotang
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Operation_Ctg_Click(object sender, EventArgs e)
        {
            double result;
            result = 1 / Math.Tan(Convert.ToDouble(uI_TextBox.Text));

            uI_Label.Text = "ctg(" + uI_TextBox.Text + ") =";
            uI_TextBox.Text = result.ToString();

            uI_TextBox.Refresh();
            uI_Label.Refresh();

            isNewNumber = false;
        }

        /// <summary>
        /// Factorial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Operation_Fact_Click(object sender, EventArgs e)
        {
            double result = 1.0;

            for (int i = Convert.ToInt32(uI_TextBox.Text); i > 1; i--)
            {
                result *= i;
            }

            uI_Label.Text = uI_TextBox.Text + "! =";
            uI_TextBox.Text = result.ToString();

            uI_TextBox.Refresh();
            uI_Label.Refresh();

            isNewNumber = false;
        }

        #endregion

        #region Изменение размера шрифта компонентов

        /// <summary>
        /// Событие, при котором изменяется размер шрифта у кнопок UI_Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Number_SizeChanged(object sender, EventArgs e)
        {

            #region Кнопка запятой и эквивалентности

            ResizeNumberFont(uI_Button_Number_Comma);
            ResizeNumberFont(uI_Button_Equals);

            #endregion

            #region Цифры

            ResizeNumberFont(uI_Button_Number0);
            ResizeNumberFont(uI_Button_Number1);
            ResizeNumberFont(uI_Button_Number2);
            ResizeNumberFont(uI_Button_Number3);
            ResizeNumberFont(uI_Button_Number4);
            ResizeNumberFont(uI_Button_Number5);
            ResizeNumberFont(uI_Button_Number6);
            ResizeNumberFont(uI_Button_Number7);
            ResizeNumberFont(uI_Button_Number8);
            ResizeNumberFont(uI_Button_Number9);

            #endregion
        }

        /// <summary>
        /// Событие, при котором изменяется размер шрифта у кнопок Operations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Operation_SizeChanged(object sender, EventArgs e)
        {
            #region Арифметические операции

            // Основные арифметические операции
            ResizeNumberFont(uI_Button_Operation_Addition);
            ResizeNumberFont(uI_Button_Operation_Subtraction);
            ResizeNumberFont(uI_Button_Operation_Multiplication);
            ResizeNumberFont(uI_Button_Operation_Division);

            // Другие арифметические операции
            ResizeNumberFont(uI_Button_Operation_Hyperbole);
            ResizeNumberFont(uI_Button_Operation_Modulo);
            ResizeNumberFont(uI_Button_Operation_Square);
            ResizeNumberFont(uI_Button_Operation_SquareRoot);
            ResizeNumberFont(uI_Button_Number_ChangeSign);

            #endregion

            #region Тригонометрические функции

            ResizeNumberFont(uI_Button_Operation_Sin);
            ResizeNumberFont(uI_Button_Operation_Cos);
            ResizeNumberFont(uI_Button_Operation_Tg);
            ResizeNumberFont(uI_Button_Operation_Ctg);

            #endregion

            #region Кнопки для стирания

            ResizeNumberFont(uI_Button_Operation_C);
            ResizeNumberFont(uI_Button_Operation_CE);
            ResizeNumberFont(uI_Button_Operation_Delete);

            #endregion

            ResizeNumberFont(uI_Button_Operation_Abs);
            ResizeNumberFont(uI_Button_Operation_Fact);
        }

        /// <summary>
        /// Событие, при котором изменяется размер шрифта у текста
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Texts_SizeChanged(object sender, EventArgs e)
        {
            ResizeTextBoxFont(uI_TextBox);
            ResizeLabelFont(uI_Label);
        }

        #endregion

        /// <summary>
        /// Событие, при котором изменяются состояния кнопок в зависимости от содержимого в uI_TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_TextBox_TextChanged(object sender, EventArgs e)
        {
            UI_TextBox T = (UI_TextBox)sender;

            if (T.Text == warnAboutDivisionByZero)
            {
                uI_Button_Number_ChangeSign.Enabled         = false;
                uI_Button_Number_Comma.Enabled              = false;

                uI_Button_Operation_Abs.Enabled             = false;
                uI_Button_Operation_Fact.Enabled            = false;
                uI_Button_Operation_Sin.Enabled             = false;
                uI_Button_Operation_Cos.Enabled             = false;
                uI_Button_Operation_Tg.Enabled              = false;
                uI_Button_Operation_Ctg.Enabled             = false;

                uI_Button_Operation_Addition.Enabled        = false;
                uI_Button_Operation_Division.Enabled        = false;
                uI_Button_Operation_Multiplication.Enabled  = false;
                uI_Button_Operation_Subtraction.Enabled     = false;

                uI_Button_Operation_Hyperbole.Enabled       = false;
                uI_Button_Operation_Modulo.Enabled          = false;
                uI_Button_Operation_Square.Enabled          = false;
                uI_Button_Operation_SquareRoot.Enabled      = false;

                uI_Button_Equals.Enabled                    = false;
            }
            else
            {
                uI_Button_Number_ChangeSign.Enabled         = true;
                uI_Button_Number_Comma.Enabled              = true;

                uI_Button_Operation_Abs.Enabled             = true;
                uI_Button_Operation_Fact.Enabled            = true;
                uI_Button_Operation_Sin.Enabled             = true;
                uI_Button_Operation_Cos.Enabled             = true;
                uI_Button_Operation_Tg.Enabled              = true;
                uI_Button_Operation_Ctg.Enabled             = true;

                uI_Button_Operation_Addition.Enabled        = true;
                uI_Button_Operation_Division.Enabled        = true;
                uI_Button_Operation_Multiplication.Enabled  = true;
                uI_Button_Operation_Subtraction.Enabled     = true;

                uI_Button_Operation_Hyperbole.Enabled       = true;
                uI_Button_Operation_Modulo.Enabled          = true;
                uI_Button_Operation_Square.Enabled          = true;
                uI_Button_Operation_SquareRoot.Enabled      = true;

                uI_Button_Equals.Enabled                    = true;
            }
        }
        
        private void Form_Resize(object sender, EventArgs e)
        {
            #region Арифметические операции

            // Основные арифметические операции
            ResizeControl(originalButtonSize, uI_Button_Operation_Division, uI_Button_Operation_Division.originalLocation);
            ResizeControl(originalButtonSize, uI_Button_Operation_Multiplication, uI_Button_Operation_Multiplication.originalLocation);
            ResizeControl(originalButtonSize, uI_Button_Operation_Subtraction, uI_Button_Operation_Subtraction.originalLocation);
            ResizeControl(originalButtonSize, uI_Button_Operation_Addition, uI_Button_Operation_Addition.originalLocation);

            // Другие арифметические операции
            ResizeControl(originalButtonSize, uI_Button_Operation_Hyperbole, uI_Button_Operation_Hyperbole.originalLocation);
            ResizeControl(originalButtonSize, uI_Button_Operation_Modulo, uI_Button_Operation_Modulo.originalLocation);
            ResizeControl(originalButtonSize, uI_Button_Operation_Square, uI_Button_Operation_Square.originalLocation);
            ResizeControl(originalButtonSize, uI_Button_Operation_SquareRoot, uI_Button_Operation_SquareRoot.originalLocation);
            ResizeControl(originalButtonSize, uI_Button_Number_ChangeSign, uI_Button_Number_ChangeSign.originalLocation);

            #endregion

            #region Тригонометрические функции

            ResizeControl(originalButtonSize, uI_Button_Operation_Sin, uI_Button_Operation_Sin.originalLocation);
            ResizeControl(originalButtonSize, uI_Button_Operation_Cos, uI_Button_Operation_Cos.originalLocation);
            ResizeControl(originalButtonSize, uI_Button_Operation_Tg, uI_Button_Operation_Tg.originalLocation);
            ResizeControl(originalButtonSize, uI_Button_Operation_Ctg, uI_Button_Operation_Ctg.originalLocation);

            #endregion

            ResizeControl(originalButtonSize, uI_Button_Operation_Abs, uI_Button_Operation_Abs.originalLocation);
            ResizeControl(originalButtonSize, uI_Button_Operation_Fact, uI_Button_Operation_Fact.originalLocation);


            #region Кнопки для стирания и эквивалентности 

            ResizeControl(originalButtonSize, uI_Button_Operation_C, uI_Button_Operation_C.originalLocation);
            ResizeControl(originalButtonSize, uI_Button_Operation_CE, uI_Button_Operation_CE.originalLocation);
            ResizeControl(originalButtonSize, uI_Button_Operation_Delete, uI_Button_Operation_Delete.originalLocation);
            ResizeControl(originalButtonSize, uI_Button_Number_Comma, uI_Button_Number_Comma.originalLocation);

            ResizeControl(originalButtonSize, uI_Button_Equals, uI_Button_Equals.originalLocation);

            #endregion

            #region Цифры

            ResizeControl(originalButtonSize, uI_Button_Number0, uI_Button_Number0.originalLocation);
            ResizeControl(originalButtonSize, uI_Button_Number1, uI_Button_Number1.originalLocation);
            ResizeControl(originalButtonSize, uI_Button_Number2, uI_Button_Number2.originalLocation);
            ResizeControl(originalButtonSize, uI_Button_Number3, uI_Button_Number3.originalLocation);
            ResizeControl(originalButtonSize, uI_Button_Number4, uI_Button_Number4.originalLocation);
            ResizeControl(originalButtonSize, uI_Button_Number5, uI_Button_Number5.originalLocation);
            ResizeControl(originalButtonSize, uI_Button_Number6, uI_Button_Number6.originalLocation);
            ResizeControl(originalButtonSize, uI_Button_Number7, uI_Button_Number7.originalLocation);
            ResizeControl(originalButtonSize, uI_Button_Number8, uI_Button_Number8.originalLocation);
            ResizeControl(originalButtonSize, uI_Button_Number9, uI_Button_Number9.originalLocation);

            #endregion

            #region Текст

            // Текст и сноска
            ResizeControl(originalTextBoxSize, uI_TextBox, uI_TextBox.originalLocation);
            ResizeControl(originalLabelSize, uI_Label, uI_Label.originalLocation);

            #endregion
        }

        #endregion

        #region Методы формы

        private double GetResult(string oper, double leftVar, double rightVar)
        {
            double result;

            switch (oper)
            {
                case "+":
                    result = leftVar + rightVar; break;

                case "−":
                    result = leftVar - rightVar; break;

                case "×":
                    result = leftVar * rightVar; break;

                case "÷":
                    result = leftVar / rightVar; break;

                case "%":
                    result = leftVar * rightVar / 100; break;

                default:
                    result = Convert.ToDouble(uI_TextBox.Text); break;
            }

            return result;
        }

        private void ResizeControl(in Size size, Control ctrl, in Point origLocation)
        {
            // Отношение нынешних ширин и высот формы к изначальным размерам формы
            float xRatio = (float)Width / (float)originalFormSize.Width;
            float yRatio = (float)Height / (float)originalFormSize.Height;

            // Позиция компонента
            int newPosX = (int)(origLocation.X * xRatio);
            int newPosY = (int)(origLocation.Y * yRatio);

            // Размер компонента
            int newButtonWidth = (int)(size.Width * xRatio);
            int newButtonHeight = (int)(size.Height * yRatio);

            ctrl.Location = new Point(newPosX, newPosY);
            ctrl.Size = new Size(newButtonWidth, newButtonHeight);
        }

        private void ResizeNumberFont(Control button)
        {
            if (WindowState == FormWindowState.Maximized)
                button.Font = new Font(button.Font.FontFamily, 30F);

            else if (Width <= 1280 || Height <= 720)
                button.Font = new Font(button.Font.FontFamily, 15F);

            else if ((Width <= 1600 && Width > 1280) || (Height <= 900 && Height > 720))
                button.Font = new Font(button.Font.FontFamily, 20F);

            else if ((Width <= 1920 && Width > 1600) || (Height <= 1080 && Height > 900))
                button.Font = new Font(button.Font.FontFamily, 25F);

            else if ((Width <= 2560 && Width > 1920) || (Height <= 1440 && Height > 1080))
                button.Font = new Font(button.Font.FontFamily, 30F);
        }

        private void ResizeTextBoxFont(Control text)
        {
            if (WindowState == FormWindowState.Maximized)
                text.Font = new Font(text.Font.FontFamily, 60F);

            else if (Width <= 1280 || Height <= 720)
                text.Font = new Font(text.Font.FontFamily, 30F);

            else if ((Width <= 1600 && Width > 1280) || (Height <= 900 && Height > 720))
                text.Font = new Font(text.Font.FontFamily, 45F);

            else if ((Width <= 1920 && Width > 1600) || (Height <= 1080 && Height > 900))
                text.Font = new Font(text.Font.FontFamily, 50F);

            else if ((Width <= 2560 && Width > 1920) || (Height <= 1440 && Height > 1080))
                text.Font = new Font(text.Font.FontFamily, 60F);
        }

        private void ResizeLabelFont(Control label)
        {
            if (WindowState == FormWindowState.Maximized)
                label.Font = new Font(label.Font.FontFamily, 30F);

            else if ((Width <= 1600 && Width > 1280) || (Height <= 900 && Height > 720))
                label.Font = new Font(label.Font.FontFamily, 15F);

            else if ((Width <= 2560 && Width > 1920) || (Height <= 1440 && Height > 1080))
                label.Font = new Font(label.Font.FontFamily, 30F);
        }

        private void ChangeButtonColor(Color clr)
        {
            if (isDarkTheme)
            {
                UI_Button_Number.BoxColor = Color.FromArgb(65 + dimmingAmount > 255 ? 255 : 65 + dimmingAmount, clr);
                UI_Button_Operation.BoxColor = Color.FromArgb(65 - dimmingAmount < 0 ? 0 : 65 - dimmingAmount, clr);
            }
            else
            {
                UI_Button_Number.BoxColor = Color.FromArgb(185 + dimmingAmount > 255 ? 255 : 185 + dimmingAmount, clr);
                UI_Button_Operation.BoxColor = Color.FromArgb(185 - dimmingAmount < 0 ? 0 : 185 - dimmingAmount, clr);
            }
        }

        private void ChangeFormColor(Color clr)
        {
            if (isDarkTheme)
            {
                BackColor = Color.FromArgb(clr.R - 50 < 0 ? 0 : clr.R - 50, clr.G - 50 < 0 ? 0 : clr.G - 50, clr.B - 50 < 0 ? 0 : clr.B - 50);
            }
            else
            {
                BackColor = Color.FromArgb(clr.R + 50 > 255 ? 255 : clr.R + 50, clr.G + 50 > 255 ? 255 : clr.G + 50, clr.B + 50 > 255 ? 255 : clr.B + 50);
            }
        }

        private void changeTextsColor()
        {
            UI_TextBox.BoxColor = BackColor;
            UI_Label.BoxColor = BackColor;
        }

        #endregion

        #region Режимы

        private void ИнженерныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            originalButtonSize = new Size(187, 80);
            Width = 1280;
            Height = 960;

            uI_Button_Operation_Cos.Visible     = true;
            uI_Button_Operation_Sin.Visible     = true;
            uI_Button_Operation_Tg.Visible      = true;
            uI_Button_Operation_Ctg.Visible     = true;
            uI_Button_Operation_Fact.Visible    = true;
            uI_Button_Operation_Abs.Visible     = true;

            #region Тригонометрические функции + факториал и модуль

            uI_Button_Operation_Fact.Location = uI_Button_Operation_Fact.originalLocation = new Point(4, 427);
            ResizeControl(originalButtonSize, uI_Button_Operation_Fact, uI_Button_Operation_Fact.originalLocation);

            uI_Button_Operation_Abs.Location = uI_Button_Operation_Abs.originalLocation = new Point(4, 509);
            ResizeControl(originalButtonSize, uI_Button_Operation_Abs, uI_Button_Operation_Abs.originalLocation);

            uI_Button_Operation_Ctg.Location = uI_Button_Operation_Ctg.originalLocation = new Point(4, 591);
            ResizeControl(originalButtonSize, uI_Button_Operation_Ctg, uI_Button_Operation_Ctg.originalLocation);

            uI_Button_Operation_Tg.Location = uI_Button_Operation_Tg.originalLocation = new Point(4, 673);
            ResizeControl(originalButtonSize, uI_Button_Operation_Tg, uI_Button_Operation_Tg.originalLocation);

            uI_Button_Operation_Cos.Location = uI_Button_Operation_Cos.originalLocation = new Point(4, 755);
            ResizeControl(originalButtonSize, uI_Button_Operation_Cos, uI_Button_Operation_Cos.originalLocation);

            uI_Button_Operation_Sin.Location = uI_Button_Operation_Sin.originalLocation = new Point(4, 837);
            ResizeControl(originalButtonSize, uI_Button_Operation_Sin, uI_Button_Operation_Sin.originalLocation);

            #endregion

            #region Остаток, -1 степень, 1 4 7, +-

            uI_Button_Operation_Modulo.Location = uI_Button_Operation_Modulo.originalLocation = new Point(193, 427);
            ResizeControl(originalButtonSize, uI_Button_Operation_Modulo, uI_Button_Operation_Modulo.originalLocation);

            uI_Button_Operation_Hyperbole.Location = uI_Button_Operation_Hyperbole.originalLocation = new Point(193, 509);
            ResizeControl(originalButtonSize, uI_Button_Operation_Hyperbole, uI_Button_Operation_Hyperbole.originalLocation);

            uI_Button_Number7.Location = uI_Button_Number7.originalLocation = new Point(193, 591);
            ResizeControl(originalButtonSize, uI_Button_Number7, uI_Button_Number7.originalLocation);

            uI_Button_Number4.Location = uI_Button_Number4.originalLocation = new Point(193, 673);
            ResizeControl(originalButtonSize, uI_Button_Number4, uI_Button_Number4.originalLocation);

            uI_Button_Number1.Location = uI_Button_Number1.originalLocation = new Point(193, 755);
            ResizeControl(originalButtonSize, uI_Button_Number1, uI_Button_Number1.originalLocation);

            uI_Button_Number_ChangeSign.Location = uI_Button_Number_ChangeSign.originalLocation = new Point(193, 837);
            ResizeControl(originalButtonSize, uI_Button_Number_ChangeSign, uI_Button_Number_ChangeSign.originalLocation);

            #endregion

            #region C, квадрат, 2 5 8 0

            uI_Button_Operation_C.Location = uI_Button_Operation_C.originalLocation = new Point(382, 427);
            ResizeControl(originalButtonSize, uI_Button_Operation_C, uI_Button_Operation_C.originalLocation);

            uI_Button_Operation_Square.Location = uI_Button_Operation_Square.originalLocation = new Point(382, 509);
            ResizeControl(originalButtonSize, uI_Button_Operation_Square, uI_Button_Operation_Square.originalLocation);

            uI_Button_Number8.Location = uI_Button_Number8.originalLocation = new Point(382, 591);
            ResizeControl(originalButtonSize, uI_Button_Number8, uI_Button_Number8.originalLocation);

            uI_Button_Number5.Location = uI_Button_Number5.originalLocation = new Point(382, 673);
            ResizeControl(originalButtonSize, uI_Button_Number5, uI_Button_Number5.originalLocation);

            uI_Button_Number2.Location = uI_Button_Number2.originalLocation = new Point(382, 755);
            ResizeControl(originalButtonSize, uI_Button_Number2, uI_Button_Number2.originalLocation);

            uI_Button_Number0.Location = uI_Button_Number0.originalLocation = new Point(382, 837);
            ResizeControl(originalButtonSize, uI_Button_Number0, uI_Button_Number0.originalLocation);

            #endregion

            #region СЕ, корень, 3 6 9, запятая

            uI_Button_Operation_CE.Location = uI_Button_Operation_CE.originalLocation = new Point(571, 427);
            ResizeControl(originalButtonSize, uI_Button_Operation_CE, uI_Button_Operation_CE.originalLocation);

            uI_Button_Operation_SquareRoot.Location = uI_Button_Operation_SquareRoot.originalLocation = new Point(571, 509);
            ResizeControl(originalButtonSize, uI_Button_Operation_SquareRoot, uI_Button_Operation_SquareRoot.originalLocation);

            uI_Button_Number9.Location = uI_Button_Number9.originalLocation = new Point(571, 591);
            ResizeControl(originalButtonSize, uI_Button_Number9, uI_Button_Number9.originalLocation);

            uI_Button_Number6.Location = uI_Button_Number6.originalLocation = new Point(571, 673);
            ResizeControl(originalButtonSize, uI_Button_Number6, uI_Button_Number6.originalLocation);

            uI_Button_Number3.Location = uI_Button_Number3.originalLocation = new Point(571, 755);
            ResizeControl(originalButtonSize, uI_Button_Number3, uI_Button_Number3.originalLocation);

            uI_Button_Number_Comma.Location = uI_Button_Number_Comma.originalLocation = new Point(571, 837);
            ResizeControl(originalButtonSize, uI_Button_Number_Comma, uI_Button_Number_Comma.originalLocation);

            #endregion

            #region Арифметические операции, равно

            uI_Button_Operation_Delete.Location = uI_Button_Operation_Delete.originalLocation = new Point(760, 427);
            ResizeControl(originalButtonSize, uI_Button_Operation_Delete, uI_Button_Operation_Delete.originalLocation);

            uI_Button_Operation_Division.Location = uI_Button_Operation_Division.originalLocation = new Point(760, 509);
            ResizeControl(originalButtonSize, uI_Button_Operation_Division, uI_Button_Operation_Division.originalLocation);

            uI_Button_Operation_Multiplication.Location = uI_Button_Operation_Multiplication.originalLocation   = new Point(760, 591);
            ResizeControl(originalButtonSize, uI_Button_Operation_Multiplication, uI_Button_Operation_Multiplication.originalLocation);

            uI_Button_Operation_Subtraction.Location = uI_Button_Operation_Subtraction.originalLocation = new Point(760, 673);
            ResizeControl(originalButtonSize, uI_Button_Operation_Subtraction, uI_Button_Operation_Subtraction.originalLocation);

            uI_Button_Operation_Addition.Location = uI_Button_Operation_Addition.originalLocation = new Point(760, 755);
            ResizeControl(originalButtonSize, uI_Button_Operation_Addition, uI_Button_Operation_Addition.originalLocation);

            uI_Button_Equals.Location = uI_Button_Equals.originalLocation = new Point(760, 837);
            ResizeControl(originalButtonSize, uI_Button_Equals, uI_Button_Equals.originalLocation);

            #endregion
        }

        private void ОбычныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            originalButtonSize = new Size(240, 100);
            Width = 1280;
            Height = 960;

            uI_Button_Operation_Cos.Visible     = false;
            uI_Button_Operation_Sin.Visible     = false;
            uI_Button_Operation_Tg.Visible      = false;
            uI_Button_Operation_Ctg.Visible     = false;
            uI_Button_Operation_Fact.Visible    = false;
            uI_Button_Operation_Abs.Visible     = false;

            #region Остаток, -1 степень, 1 4 7, +-

            uI_Button_Operation_Modulo.Location         = uI_Button_Operation_Modulo.originalLocation           = new Point(4, 307);
            ResizeControl(originalButtonSize, uI_Button_Operation_Modulo, uI_Button_Operation_Modulo.originalLocation);

            uI_Button_Operation_Hyperbole.Location      = uI_Button_Operation_Hyperbole.originalLocation        = new Point(4, 409);
            ResizeControl(originalButtonSize, uI_Button_Operation_Hyperbole, uI_Button_Operation_Hyperbole.originalLocation);

            uI_Button_Number7.Location                  = uI_Button_Number7.originalLocation                    = new Point(4, 511);
            ResizeControl(originalButtonSize, uI_Button_Number7, uI_Button_Number7.originalLocation);

            uI_Button_Number4.Location                  = uI_Button_Number4.originalLocation                    = new Point(4, 613);
            ResizeControl(originalButtonSize, uI_Button_Number4, uI_Button_Number4.originalLocation);

            uI_Button_Number1.Location                  = uI_Button_Number1.originalLocation                    = new Point(4, 715);
            ResizeControl(originalButtonSize, uI_Button_Number1, uI_Button_Number1.originalLocation);

            uI_Button_Number_ChangeSign.Location        = uI_Button_Number_ChangeSign.originalLocation          = new Point(4, 817);
            ResizeControl(originalButtonSize, uI_Button_Number_ChangeSign, uI_Button_Number_ChangeSign.originalLocation);

            #endregion

            #region C, квадрат, 2 5 8 0

            uI_Button_Operation_C.Location              = uI_Button_Operation_C.originalLocation                = new Point(246, 307);
            ResizeControl(originalButtonSize, uI_Button_Operation_C, uI_Button_Operation_C.originalLocation);

            uI_Button_Operation_Square.Location         = uI_Button_Operation_Square.originalLocation           = new Point(246, 409);
            ResizeControl(originalButtonSize, uI_Button_Operation_Square, uI_Button_Operation_Square.originalLocation);

            uI_Button_Number8.Location                  = uI_Button_Number8.originalLocation                    = new Point(246, 511);
            ResizeControl(originalButtonSize, uI_Button_Number8, uI_Button_Number8.originalLocation);

            uI_Button_Number5.Location                  = uI_Button_Number5.originalLocation                    = new Point(246, 613);
            ResizeControl(originalButtonSize, uI_Button_Number5, uI_Button_Number5.originalLocation);

            uI_Button_Number2.Location                  = uI_Button_Number2.originalLocation                    = new Point(246, 715);
            ResizeControl(originalButtonSize, uI_Button_Number2, uI_Button_Number2.originalLocation);

            uI_Button_Number0.Location                  = uI_Button_Number0.originalLocation                    = new Point(246, 817);
            ResizeControl(originalButtonSize, uI_Button_Number0, uI_Button_Number0.originalLocation);

            #endregion

            #region СЕ, корень, 3 6 9, запятая

            uI_Button_Operation_CE.Location             = uI_Button_Operation_CE.originalLocation               = new Point(488, 307);
            ResizeControl(originalButtonSize, uI_Button_Operation_CE, uI_Button_Operation_CE.originalLocation);

            uI_Button_Operation_SquareRoot.Location     = uI_Button_Operation_SquareRoot.originalLocation       = new Point(488, 409);
            ResizeControl(originalButtonSize, uI_Button_Operation_SquareRoot, uI_Button_Operation_SquareRoot.originalLocation);

            uI_Button_Number9.Location                  = uI_Button_Number9.originalLocation                    = new Point(488, 511);
            ResizeControl(originalButtonSize, uI_Button_Number9, uI_Button_Number9.originalLocation);

            uI_Button_Number6.Location                  = uI_Button_Number6.originalLocation                    = new Point(488, 613);
            ResizeControl(originalButtonSize, uI_Button_Number6, uI_Button_Number6.originalLocation);

            uI_Button_Number3.Location                  = uI_Button_Number3.originalLocation                    = new Point(488, 715);
            ResizeControl(originalButtonSize, uI_Button_Number3, uI_Button_Number3.originalLocation);

            uI_Button_Number_Comma.Location             = uI_Button_Number_Comma.originalLocation               = new Point(488, 817);
            ResizeControl(originalButtonSize, uI_Button_Number_Comma, uI_Button_Number_Comma.originalLocation);

            #endregion

            #region Арифметические операции, равно

            uI_Button_Operation_Delete.Location         = uI_Button_Operation_Delete.originalLocation           = new Point(730, 307);
            ResizeControl(originalButtonSize, uI_Button_Operation_Delete, uI_Button_Operation_Delete.originalLocation);

            uI_Button_Operation_Division.Location       = uI_Button_Operation_Division.originalLocation         = new Point(730, 409);
            ResizeControl(originalButtonSize, uI_Button_Operation_Division, uI_Button_Operation_Division.originalLocation);

            uI_Button_Operation_Multiplication.Location = uI_Button_Operation_Multiplication.originalLocation   = new Point(730, 511);
            ResizeControl(originalButtonSize, uI_Button_Operation_Multiplication, uI_Button_Operation_Multiplication.originalLocation);

            uI_Button_Operation_Subtraction.Location    = uI_Button_Operation_Subtraction.originalLocation      = new Point(730, 613);
            ResizeControl(originalButtonSize, uI_Button_Operation_Subtraction, uI_Button_Operation_Subtraction.originalLocation);

            uI_Button_Operation_Addition.Location       = uI_Button_Operation_Addition.originalLocation         = new Point(730, 715);
            ResizeControl(originalButtonSize, uI_Button_Operation_Addition, uI_Button_Operation_Addition.originalLocation);

            uI_Button_Equals.Location                   = uI_Button_Equals.originalLocation                     = new Point(730, 817);
            ResizeControl(originalButtonSize, uI_Button_Equals, uI_Button_Equals.originalLocation);

            #endregion
        }

        #endregion

        #region Темы
        private void СветлаяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isDarkTheme)
            {
                isDarkTheme = false;

                UI_Button_Equals.Dimming = Color.FromArgb(30, Color.White);

                ChangeButtonColor(UI_Button_Operation.BoxColor);
                ChangeButtonColor(UI_Button_Number.BoxColor);
                ChangeFormColor(BackColor);
                changeTextsColor();

                Refresh();
            }
        }

        private void ТёмнаяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!isDarkTheme)
            {
                isDarkTheme = true;

                UI_Button_Equals.Dimming = Color.FromArgb(30, Color.Black);

                ChangeButtonColor(UI_Button_Operation.BoxColor);
                ChangeButtonColor(UI_Button_Number.BoxColor);
                ChangeFormColor(BackColor);
                changeTextsColor();

                Refresh();
            }
        }

        #endregion

        #region Цвета

        private void КрасныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeButtonColor(Color.Red);
            ChangeFormColor(Color.Red);
            changeTextsColor();

            Refresh();
        }

        private void ЗелёныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeButtonColor(Color.FromArgb(0, 128, 0));
            ChangeFormColor(Color.FromArgb(0, 128, 0));
            changeTextsColor();

            Refresh();
        }

        private void РозовыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeButtonColor(Color.FromArgb(255, 100, 128));
            ChangeFormColor(Color.FromArgb(255, 100, 128));
            changeTextsColor();

            Refresh();
        }

        private void СинийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeButtonColor(Color.Blue);
            ChangeFormColor(Color.Blue);
            changeTextsColor();

            Refresh();
        }

        private void КоричневыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeButtonColor(Color.FromArgb(150, 75, 0));
            ChangeFormColor(Color.FromArgb(150, 75, 0));
            changeTextsColor();

            Refresh();
        }

        #endregion
    }
}
