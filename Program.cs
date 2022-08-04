using System.Text;

namespace Calculator;

class Program
{
    static bool runProgram = true;

    static char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.' };
    static char[] operations = { '+', '-', '*', '/' };

    static string previousValue = "";
    static string currentValue = "";
    static string operation = "";

    static List<string> calculatorHistory = new List<string>();

    static int markedCursorPositionX;
    static int markedCursorPositionY;

    static void Main(string[] args)
    {
        while (runProgram)
        {
            DrawProgram();
            ListenKeyPress();
        }

        Console.Write("\n\n\n\n\n\n\n\n\n");
        Console.WriteLine("Fim do programa.\n");
    }

    static void DrawProgram()
    {
        Console.Clear();
        PrintDescription();
        PrintPreviousValue();
        PrintOperation();
        PrintCurrentValue();
        PrintCalculatorHistory();
    }

    static void ListenKeyPress()
    {
        var cki = Console.ReadKey(true);

        switch (cki.Key)
        {
            case ConsoleKey.Enter:
                ShowResult();
                break;
            case ConsoleKey.Backspace:
                BackspaceCurrentValue();
                break;
            case ConsoleKey.R:
                ResetValues();
                break;
            case ConsoleKey.T:
                ReverseValueSign();
                break;
            case ConsoleKey.Escape:
                ExitProgram();
                break;
            default:
                // If other keys have been pressed 

                char ch = cki.KeyChar;

                bool isDigit = digits.Contains(ch);
                bool isOperation = operations.Contains(ch);

                if (isDigit) currentValue += ch;
                else if (isOperation) ChangeOperation(ch);
                break;
        }
    }

    delegate void DoCalcCallback(double result);
    static void DoCalc(DoCalcCallback? callback)
    {
        var IsEmpty = String.IsNullOrEmpty;

        bool canDoCalc = !IsEmpty(operation) & !IsEmpty(previousValue) & !IsEmpty(currentValue);

        if (canDoCalc)
        {
            double result = 0;

            switch (operation)
            {
                case "+":
                    result = double.Parse(previousValue) + double.Parse(currentValue);
                    break;
                case "-":
                    result = double.Parse(previousValue) - double.Parse(currentValue);
                    break;
                case "*":
                    result = double.Parse(previousValue) * double.Parse(currentValue);
                    break;
                case "/":
                    result = double.Parse(previousValue) / double.Parse(currentValue);
                    break;
            }

            if (calculatorHistory.Count == 5)
            {
                calculatorHistory.RemoveAt(0);
            }

            calculatorHistory.Add($"{previousValue} {operation} {currentValue} = {result}");

            if (callback != null) callback(result);
        }
    }

    static void ChangeOperationDoCalcCb(double result)
    {
        previousValue = result.ToString();
        currentValue = "";
    }
    static void ChangeOperation(char operation)
    {
        var IsEmpty = String.IsNullOrEmpty;

        if (IsEmpty(Program.operation))
        {
            if (!IsEmpty(currentValue))
            {
                previousValue = currentValue;
                currentValue = "";

                // If a new calculation begins, clear the calculator history.
                if (calculatorHistory.Count > 0) calculatorHistory.Clear();
            }
        }
        else
        {
            DoCalc(ChangeOperationDoCalcCb);
        }

        Program.operation = operation.ToString();
    }

    static void ShowResultDoCalcCb(double result)
    {
        previousValue = result.ToString();
        currentValue = "";
        operation = "";
    }
    static void ShowResult()
    {
        DoCalc(ShowResultDoCalcCb);
    }

    static void BackspaceCurrentValue()
    {
        if (currentValue.Length > 0)
        {
            if (currentValue.Length >= 0)
            {
                currentValue = currentValue.Substring(0, currentValue.Length - 1);
            }
            else
            {
                currentValue = "";
            }
        }
    }

    static void ResetValues()
    {
        previousValue = "";
        currentValue = "";
        operation = "";
        calculatorHistory.Clear();
    }

    static void ReverseValueSign()
    {
        var IsEmpty = String.IsNullOrEmpty;

        if (IsEmpty(currentValue))
        {
            double value = double.Parse(previousValue);
            value = -value;
            previousValue = value.ToString();
        }
        else
        {
            double value = double.Parse(currentValue);
            value = -value;
            currentValue = value.ToString();
        }
    }

    static void ExitProgram()
    {
        runProgram = false;
    }

    static void PrintCalculatorHistory()
    {
        Console.Write("\n\n");
        Console.WriteLine("Histórico (5 últimos cálculos):");

        for (int i = calculatorHistory.Count - 1; i >= 0; i--)
        {
            Console.WriteLine(calculatorHistory[i]);
        }

        // Back to current value position
        Console.SetCursorPosition(markedCursorPositionX, markedCursorPositionY);
    }

    static void PrintDescription()
    {
        StringBuilder description = new StringBuilder();
        description
            .Append("Calculadora Simples para Terminal.\n\n")
            .Append("Operadores Aritméticos: + Adição; - Subtração; * Multiplicação; / Divisão.\n\n")
            .Append("Funcionamento: Digite números e operadores aritméticos no campo de entrada.\n")
            .Append("               Como se fosse uma calculadora de mão.\n\n")
            .Append("Comando de tecla:\n")
            .Append("- ENTER: Visualizar o resultado.\n")
            .Append("- R    : Resetar valores.\n")
            .Append("- T    : Inverter o sinal do número. Se o valor atual estiver vazio,.\n")
            .Append("         então o número do valor anterior será invertido.\n")
            .Append("- ESC  : Sair do programa.\n\n");
        Console.Write(description);
    }

    static void PrintPreviousValue()
    {
        Console.Write("Valor anterior: {0}\n", previousValue);
    }

    static void PrintOperation()
    {
        Console.Write("Operador      : {0}\n", operation);
    }

    static void PrintCurrentValue()
    {
        Console.Write("Valor atual   : {0}", currentValue);

        markedCursorPositionX = Console.GetCursorPosition().Left;
        markedCursorPositionY = Console.GetCursorPosition().Top;
    }
}
