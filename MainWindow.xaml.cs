using System.Windows;
using System.Windows.Controls;

namespace Project004_SimpleCalc
{
    public partial class MainWindow : Window
    {
        double firstNumber = 0;
        double secondNumber = 0;
        double result = 0;
        int maxLength = 16;
        int defaultFontSize = 30;
        int minFontSize = 15;
        bool needsDisplayClearing = false;
        bool isOperatorSelected = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        void CheckIfPeriodExists()
        {
            if (!NumberDisplay.Text.Contains(","))
            {
                NumberDisplay.Text += ",";
            }
        }

        void SetDisplayToDefault()
        {
            NumberDisplay.Text = "0";
            AdjustFontSize();
            PreviousNumberDisplay.Text = "";
            OperationTypeDisplay.Text = "";
            firstNumber = 0;
            secondNumber = 0;
            needsDisplayClearing = false;
            isOperatorSelected = false;
        }

        void AdjustFontSize()
        {
            if (NumberDisplay.Text.Length > maxLength)
            {
                double newFontSize = defaultFontSize - (NumberDisplay.Text.Length - maxLength) * 1.5;
                NumberDisplay.FontSize = newFontSize;
                if (NumberDisplay.FontSize <= minFontSize)
                {
                    NumberDisplay.FontSize = minFontSize;
                }
            }
            else
            {
                NumberDisplay.FontSize = defaultFontSize;
            }
        }

        private void NumberClicked(object sender, RoutedEventArgs e)
        {
            // Wyciągamy cyfrę z klikniętego przycisku
            string digit = (sender as Button).Content.ToString();

            // Jeśli jest samo 0 to zastąpmy je pustym napisem
            if (NumberDisplay.Text == "0")
            {
                NumberDisplay.Text = "";
            }

            // Czy wymaga czyszczenia?
            if (needsDisplayClearing)
            {
                NumberDisplay.Text = "";
                needsDisplayClearing = false;
            }

            // Doklejamy cyfrę do wyświetlacza
            NumberDisplay.Text += digit;

            // Limit do 16 cyfr
            if (NumberDisplay.Text.Length > maxLength)
            {
                NumberDisplay.Text = NumberDisplay.Text.Substring(0, 16);
            }

            AdjustFontSize();
        }

        private void OperatorClicked(object sender, RoutedEventArgs e)
        {
            // Wyciągamy operator z klikniętego przycisku (+ - * /)
            string operation = (sender as Button).Content.ToString();

            // Pozwala na liczenie w trakcie klikania innych operatorów
            if (isOperatorSelected)
            {
                EqualClicked("=", e);
            }

            firstNumber = double.Parse(NumberDisplay.Text);
            PreviousNumberDisplay.Text = $"{firstNumber} {operation}";
            isOperatorSelected = true;

            // Wyświetlamy operator
            OperationTypeDisplay.Text = operation;

            // Oznaczymy, że wyświetlacz będzie wymagał wyczyszczenia
            needsDisplayClearing = true;
        }

        private void EqualClicked(object sender, RoutedEventArgs e)
        {
            // Zabezpieczenie przed wyświetleniem poprzednich liczb których nie ma
            if (!isOperatorSelected)
            {
                return;
            }

            // Zapamiętamy drugą liczbę
            secondNumber = double.Parse(NumberDisplay.Text);

            // Wykonujemy działanie
            switch (OperationTypeDisplay.Text)
            {
                case "+":
                    result = firstNumber + secondNumber;
                    break;
                case "-":
                    result = firstNumber - secondNumber;
                    break;
                case "*":
                    result = firstNumber * secondNumber;
                    break;
                case "/":
                    if (secondNumber == 0)
                    {
                        SetDisplayToDefault();
                        NumberDisplay.FontSize = 24;
                        NumberDisplay.Text = "Nie można dzielić przez 0";
                        return;
                    }
                    result = firstNumber / secondNumber;
                    break;
            }

            // Wyświetlamy wynik i resetujemy operator
            NumberDisplay.Text = result.ToString();
            PreviousNumberDisplay.Text = $"{firstNumber} {OperationTypeDisplay.Text} {secondNumber} =";
            OperationTypeDisplay.Text = "";

            // Ustawianie pod dalsze liczenie
            isOperatorSelected = false;
            needsDisplayClearing = true;

            AdjustFontSize();
        }

        private void PeriodClicked(object sender, RoutedEventArgs e)
        {
            CheckIfPeriodExists();
            AdjustFontSize();
        }

        private void ClearClicked(object sender, RoutedEventArgs e)
        {
            SetDisplayToDefault();
        }

        private void PercentClicked(object sender, RoutedEventArgs e)
        {
            double percentValue = 0;
            secondNumber = double.Parse(NumberDisplay.Text);

            switch (OperationTypeDisplay.Text)
            {
                case "+":
                    percentValue = firstNumber * secondNumber / 100;
                    break;
                case "-":
                    percentValue = firstNumber * secondNumber / 100;
                    break;
                case "*":
                    percentValue = secondNumber / 100;
                    break;
                case "/":
                    percentValue = secondNumber / 100;
                    break;
            }

            PreviousNumberDisplay.Text = $"{firstNumber} {OperationTypeDisplay.Text} {percentValue}";
            NumberDisplay.Text = percentValue.ToString();
            AdjustFontSize();
        }

        private void ReverseNumberClicked(object sender, RoutedEventArgs e)
        {
            if (NumberDisplay.Text == "0")
            {
                NumberDisplay.Text = "0";
            }
            else
            {
                double reversedNumber = double.Parse(NumberDisplay.Text) * -1;
                NumberDisplay.Text = reversedNumber.ToString();
            }
            AdjustFontSize();
        }

        private void BackspaceClicked(object sender, RoutedEventArgs e)
        {
            int lastNumber = NumberDisplay.Text.Length - 1;
            NumberDisplay.Text = NumberDisplay.Text.Substring(0, lastNumber);

            if (NumberDisplay.Text.Length < 1)
            {
                NumberDisplay.Text = "0";
            }
            AdjustFontSize();
        }
    }
}