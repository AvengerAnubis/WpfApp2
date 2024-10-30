using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace FirstWpfApp
{
    public partial class MainWindow : Window
    {
        string leftop = ""; // Левый операнд
        string operation = ""; // Знак операции
        string rightop = ""; // Правый операнд

        public MainWindow()
        {
            InitializeComponent();
            // Добавляем обработчик для всех кнопок на гриде
            foreach (UIElement c in LayoutRoot.Children)
            {
                if (c is Button)
                {
                    ((Button)c).Click += Button_Click;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Получаем текст кнопки
            string s = (string)((Button)e.OriginalSource).Content;
            // Добавляем его в текстовое поле
            textBlock.Text += s;
            double num;
            // Пытаемся преобразовать его в число
            bool result = double.TryParse(s, out num);
            // Если текст - это число
            if (result)
            {
                // Если операция не задана
                if (operation == "")
                {
                    // Добавляем к левому операнду
                    leftop += s;
                }
                else
                {
                    // Иначе к правому операнду
                    rightop += s;
                }
            }
            // Если было введено не число
            else if (s == "=")
            {
                textBlock.Text = textBlock.Text.Substring(0,textBlock.Text.Length - 1);
                textBlock.Text = new DataTable().Compute(textBlock.Text, null).ToString();
            }
            // Очищаем поле и переменные
            else if (s == "ЧИСТИТЬ")
            {
                leftop = "";
                rightop = "";
                operation = "";
                textBlock.Text = "";
            }
            // Получаем операцию
            else
            {
                // Если правый операнд уже имеется, то присваиваем его значение левому
                // операнду, а правый операнд очищаем
                if (rightop != "")
                {
                    Update_RightOp();
                    leftop = rightop;
                    rightop = "";
                }
                operation = s;
            }
        }

        // Обновляем значение правого операнда
        private void Update_RightOp()
        {
            double num1 = double.Parse(leftop);
            double num2 = double.Parse(rightop);

            switch (operation)
            {
                case "+":
                    rightop = (num1 + num2).ToString(); // Сложение
                    break;
                case "-":
                    rightop = (num1 - num2).ToString(); // Вычитание
                    break;
                case "*":
                    rightop = (num1 * num2).ToString(); // Умножение
                    break;
                case "/":
                    // Проверяем деление на ноль
                    if (num2 != 0)
                    {
                        rightop = (num1 / num2).ToString(); // Деление
                    }
                    else
                    {
                        rightop = "Ошибка"; // Обработка деления на 0
                    }
                    break;
                case "^":
                    rightop = Math.Pow(num1, num2).ToString(); // Возведение в степень
                    break;
                default:
                    rightop = "Неизвестная операция"; // Обработка неизвестной операции
                    break;
            }
        }
    }
}