bool runProgram = true;

char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.' };
char[] operations = { '+', '-', '*', '/' };

string inputPreviousValue = "";
string inputCurrentValue = "";
string operation = "";

int relativePosition;

// ================================================================================

void ClearWindowLine(int linePosition)
{
    Console.SetCursorPosition(0, linePosition);

    for (int i = 0; i < Console.WindowWidth; i++)
    {
        Console.Write(" ");
    }

    Console.SetCursorPosition(0, linePosition);
}

void PrintPreviousValue()
{
    ClearWindowLine(relativePosition);
    Console.Write("Valor anterior: {0}", inputPreviousValue);
}

void PrintOperation()
{
    ClearWindowLine(relativePosition + 1);
    Console.Write("Operador      : {0}", operation);
}

void PrintInput()
{
    ClearWindowLine(relativePosition + 2);
    Console.Write("Valor atual   : {0}", inputCurrentValue);
}

void BackspaceInput()
{
    if (inputCurrentValue.Length > 0)
    {
        Console.Write("\b ");
        Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, relativePosition);

        if (inputCurrentValue.Length >= 0)
        {
            inputCurrentValue = inputCurrentValue.Substring(0, inputCurrentValue.Length - 1);
        }
        else
        {
            inputCurrentValue = "";
        }
    }
}

double DoCalc(string operation, double value1, double value2)
{
    double result = 0;

    switch (operation)
    {
        case "+":
            result = value1 + value2;
            break;
        case "-":
            result = value1 - value2;
            break;
        case "*":
            result = value1 * value2;
            break;
        case "/":
            result = value1 / value2;
            break;
    }

    return result;
}

// ================================================================================

Console.WriteLine(@"
Calculadora Simples de Terminal.

Operadores Aritméticos: + Adição; - Subtração; * Multiplicação; / Divisão.

Funcionamento: Digite números e operadores aritméticos no campo de entrada.
               Como se fosse uma calculadora de mão.

Comando de tecla:
- ENTER: Visualizar o resultado.
- C    : Limpar a entrada.
- T    : Inverter o sinal do número. Se o valor atual estiver vazio,
         então o número do valor anterior será invertido.
- ESC  : Sair do programa.
");

relativePosition = Console.GetCursorPosition().Top;

while (runProgram)
{
    PrintPreviousValue();
    PrintOperation();
    PrintInput();

    ConsoleKeyInfo cki = Console.ReadKey(true);

    if (cki.Key == ConsoleKey.Escape)
    {
        runProgram = false;
        break;
    }
    else if (cki.Key == ConsoleKey.Backspace)
    {
        BackspaceInput();
    }
    else
    {
        char ch = cki.KeyChar;
        bool isDigit = digits.Contains(ch);
        bool isOperation = operations.Contains(ch);

        if (isDigit)
        {
            inputCurrentValue += ch;
        }
        else if (isOperation)
        {
            bool operationIsNotEmpty = !String.IsNullOrEmpty(operation);
            bool inputPreviousValueIsNotEmpty = !String.IsNullOrEmpty(inputPreviousValue);
            bool inputCurrentValueIsNotEmpty = !String.IsNullOrEmpty(inputCurrentValue);

            if (operationIsNotEmpty & inputPreviousValueIsNotEmpty & inputCurrentValueIsNotEmpty)
            {
                double result = DoCalc(operation, double.Parse(inputPreviousValue), double.Parse(inputCurrentValue));
                inputPreviousValue = result.ToString();
                inputCurrentValue = "";
            }
            else
            {
                if (inputCurrentValueIsNotEmpty)
                {
                    inputPreviousValue = inputCurrentValue;
                    inputCurrentValue = "";
                }
            }

            operation = ch.ToString();
        }
        else if (cki.Key == ConsoleKey.Enter)
        {
            bool operationIsNotEmpty = !String.IsNullOrEmpty(operation);
            bool inputPreviousValueIsNotEmpty = !String.IsNullOrEmpty(inputPreviousValue);
            bool inputCurrentValueIsNotEmpty = !String.IsNullOrEmpty(inputCurrentValue);

            if (operationIsNotEmpty & inputPreviousValueIsNotEmpty & inputCurrentValueIsNotEmpty)
            {
                double result = DoCalc(operation, double.Parse(inputPreviousValue), double.Parse(inputCurrentValue));
                inputPreviousValue = result.ToString();
                inputCurrentValue = "";
                operation = "";
            }
        }
        else if (cki.Key == ConsoleKey.C)
        {
            inputCurrentValue = "";
            inputPreviousValue = "";
            operation = "";
        }
        else if (cki.Key == ConsoleKey.T)
        {
            if (inputCurrentValue == "")
            {
                double value = double.Parse(inputPreviousValue);
                value = -value;
                inputPreviousValue = value.ToString();
            }
            else
            {
                double value = double.Parse(inputCurrentValue);
                value = -value;
                inputCurrentValue = value.ToString();
            }
        }
    }
}

Console.WriteLine("\n\nFim do programa.\n");
