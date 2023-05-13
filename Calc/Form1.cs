using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

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
        /// Переменная, хранящая в себе промежуток между кнопками
        /// </summary>
        private int controlsMargin = 2;

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

        private void Form_Load(object sender, EventArgs e)
        {
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

            // Обнуление переменных
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
        private void UI_Button_Number_Comma_Click(object sender, EventArgs e)
        {
            UI_Button_Number B = (UI_Button_Number)sender;

            if (!uI_TextBox.Text.Contains(","))
                uI_TextBox.Text += B.Text;

            uI_TextBox.Refresh();
        }

        /// <summary>
        /// Кнопка "С"
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
        /// Кнопка "СЕ"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_Button_Operation_CE_Click(object sender, EventArgs e)
        {
            uI_TextBox.Text = "0";

            uI_TextBox.Refresh();
        }

        /// <summary>
        /// Кнопка "Возвести в квадрат"
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
        /// Кнопка "Поменять знак"
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
        /// Кнопка "Возвести в степень -1"
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
        /// Кнопка "Возвести в корень второй степени"
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
        /// Кнопка "D"
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

            #region Кнопки для стирания

            ResizeNumberFont(uI_Button_Operation_C);
            ResizeNumberFont(uI_Button_Operation_CE);
            ResizeNumberFont(uI_Button_Operation_Delete);

            #endregion
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
                uI_Button_Number_ChangeSign.Enabled = false;
                uI_Button_Number_Comma.Enabled = false;

                uI_Button_Operation_Addition.Enabled = false;
                uI_Button_Operation_Division.Enabled = false;
                uI_Button_Operation_Multiplication.Enabled = false;
                uI_Button_Operation_Subtraction.Enabled = false;

                uI_Button_Operation_Hyperbole.Enabled = false;
                uI_Button_Operation_Modulo.Enabled = false;
                uI_Button_Operation_Square.Enabled = false;
                uI_Button_Operation_SquareRoot.Enabled = false;

                uI_Button_Equals.Enabled = false;
            }
            else
            {
                uI_Button_Number_ChangeSign.Enabled = true;
                uI_Button_Number_Comma.Enabled = true;

                uI_Button_Operation_Addition.Enabled = true;
                uI_Button_Operation_Division.Enabled = true;
                uI_Button_Operation_Multiplication.Enabled = true;
                uI_Button_Operation_Subtraction.Enabled = true;

                uI_Button_Operation_Hyperbole.Enabled = true;
                uI_Button_Operation_Modulo.Enabled = true;
                uI_Button_Operation_Square.Enabled = true;
                uI_Button_Operation_SquareRoot.Enabled = true;

                uI_Button_Equals.Enabled = true;
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

        private double GetResult(in string oper, in double var)
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

        private void ResizeControl(in Size size, Control ctrl, in Point origLocation)
        {
            // Отношение нынешних ширин и высот формы к изначальным размерам формы
            double xRatio = (double)Width / (double)originalFormSize.Width;
            double yRatio = (double)Height / (double)originalFormSize.Height;

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

            else if (Width <= 640 || Height <= 480)
                button.Font = new Font(button.Font.FontFamily, 10F);

            else if ((Width <= 1280 && Width >= 640) || (Height <= 720 && Height >= 480))
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

            else if (Width <= 640 || Height <= 480)
                text.Font = new Font(text.Font.FontFamily, 20F);

            else if ((Width <= 1280 && Width >= 640) || (Height <= 720 && Height >= 480))
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
        
        #endregion
    }
}
