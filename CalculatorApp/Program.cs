using System;
using System.Globalization;

class Calculator
{
    static double memory = 0.0;
    static double current = 0.0;

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Калькулятор (консоль). Введите 'help' для списка команд.");

        bool running = true;
        while (running)
        {
            Console.Write($"\nТекущее значение: {current}\n> ");
            string? line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) continue;

            var parts = line.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            var cmd = parts[0].ToLowerInvariant();
            string arg = parts.Length > 1 ? parts[1].Trim() : "";

            try
            {
                switch (cmd)
                {
                    case "help":
                        PrintHelp();
                        break;
                    case "set": // set <number>
                        current = ParseDoubleArg(arg);
                        break;
                    case "add": // add <number>
                        current = current + ParseDoubleArg(arg);
                        break;
                    case "sub": // sub <number>
                        current = current - ParseDoubleArg(arg);
                        break;
                    case "mul": // mul <number>
                        current = current * ParseDoubleArg(arg);
                        break;
                    case "div": // div <number>
                        {
                            double v = ParseDoubleArg(arg);
                            if (v == 0) { Console.WriteLine("ОШИБКА: деление на ноль"); }
                            else current = current / v;
                        }
                        break;
                    case "mod": // mod <number> - остаток от деления
                        {
                            double v = ParseDoubleArg(arg);
                            if (v == 0) { Console.WriteLine("ОШИБКА: модуль по нулю"); }
                            else current = current % v;
                        }
                        break;
                    case "percent": // percent  -> current = current / 100
                        current = current / 100.0;
                        break;
                    case "inv": // 1/x
                        if (current == 0) Console.WriteLine("ОШИБКА: нельзя взять 1/0");
                        else current = 1.0 / current;
                        break;
                    case "sq": // x^2
                        current = current * current;
                        break;
                    case "sqrt": // sqrt(x)
                        if (current < 0) Console.WriteLine("ОШИБКА: корень из отрицательного числа");
                        else current = Math.Sqrt(current);
                        break;
                    case "m+": // M+ добавить текущее в память
                        memory += current;
                        Console.WriteLine($"Memory = {memory}");
                        break;
                    case "m-": // M- вычесть текущее из памяти
                        memory -= current;
                        Console.WriteLine($"Memory = {memory}");
                        break;
                    case "mr": // MR показать память
                        Console.WriteLine($"Memory = {memory}");
                        current = memory;
                        break;
                    case "mc": // очистить память
                        memory = 0.0;
                        Console.WriteLine("Memory очищена");
                        break;
                    case "c": // очистить текущее
                        current = 0.0;
                        Console.WriteLine("Текущее значение очищено");
                        break;
                    case "exit":
                    case "quit":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Неизвестная команда. Введите 'help'.");
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("ОШИБКА: неверный формат числа.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("ОШИБКА: переполнение числа.");
            }
        }

        Console.WriteLine("Выход. До свидания.");
    }

    static double ParseDoubleArg(string arg)
    {
        if (string.IsNullOrWhiteSpace(arg))
            throw new FormatException();
        // поддержка запятой и точки
        return double.Parse(arg, CultureInfo.InvariantCulture);
    }

    static void PrintHelp()
    {
        Console.WriteLine(
@"Список команд:
 set <x>       — установить текущее значение x
 add <x>       — current + x
 sub <x>       — current - x
 mul <x>       — current * x
 div <x>       — current / x
 mod <x>       — остаток current % x
 percent       — current := current / 100
 inv           — 1/current (если current ≠ 0)
 sq            — current^2
 sqrt          — sqrt(current) (если >=0)
 m+            — memory += current
 m-            — memory -= current
 mr            — recall memory (и присвоить current)
 mc            — clear memory
 c             — clear current (0)
 help          — показать это
 exit / quit   — выход"
        );
    }
}
