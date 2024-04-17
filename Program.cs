using System;
using System.Collections.Generic;
using System.Linq;

public class Wallet
{
    private const string TOP_UP = "Пополнение";
    private const string TOP_DOWN = "Снятие";
    private Dictionary<string, Dictionary<string, object>> categories;

    public Wallet()
    {
        categories = new Dictionary<string, Dictionary<string, object>>();
    }

    public bool AddCategory(string category, double balance = 0.0)
    {
        if (categories.ContainsKey(category))
        {
            Console.WriteLine("Данная категория уже добавлена.");
            return false;
        }

        categories[category] = new Dictionary<string, object>
        {
            { "operations", new List<Tuple<string, string, double>>() },
            { "balance", balance }
        };

        if (balance != 0.0)
        {
            ((List<Tuple<string, string, double>>)categories[category]["operations"]).Add(new Tuple<string, string, double>("Создание новой категории", TOP_UP, balance));
        }

        Console.WriteLine("Новая категория успешно добавлена.");
        return true;
    }

    public bool ChangeBalance(string category, double amount, string operation, string description)
    {
        double currentBalance = (double)categories[category]["balance"];

        if (operation == TOP_UP)
        {
            currentBalance += amount;
        }
        else if (operation == TOP_DOWN)
        {
            currentBalance -= amount;
        }

        if (currentBalance < 0)
        {
            Console.WriteLine("Недостаточно средств.");
            return false;
        }

        categories[category]["balance"] = currentBalance;
        ((List<Tuple<string, string, double>>)categories[category]["operations"]).Add(new Tuple<string, string, double>(description, operation, amount));
        Console.WriteLine($"Операция {operation} успешна. Баланс в категории {category}: {currentBalance}");
        return true;
    }

    public bool TopUp(string category, double amount, string description = "")
    {
        if (!categories.ContainsKey(category))
        {
            Console.WriteLine("Категория не найдена.");
            return false;
        }

        return ChangeBalance(category, amount, TOP_UP, description);
    }

    public bool TopDown(string category, double amount, string description = "")
    {
        if (!categories.ContainsKey(category))
        {
            Console.WriteLine($"Категория {category} не найдена.");
            return false;
        }

        return ChangeBalance(category, amount, TOP_DOWN, description);
    }

    public void SendToCategory(string fromCategory, string toCategory, double amount)
    {
        TopDown(fromCategory, amount, $"Перевод в категорию {toCategory}");
        TopUp(toCategory, amount, $"Перевод из категории {fromCategory}");

        Console.WriteLine($"Перевод из категории {fromCategory} в категорию {toCategory} успешно завершен");
    }

    public void CheckBalance(string category)
    {
        double currentBalance = (double)categories[category]["balance"];
        Console.WriteLine($"Ваш баланс в категории составляет: {currentBalance}");
    }

    public void PrintCategoryStats(string category)
    {
        if (!categories.ContainsKey(category))
        {
            Console.WriteLine("Категория не найдена.");
            return;
        }

        Console.WriteLine($"Статистика для категории {category}: ");
        foreach (var operation in (List<Tuple<string, string, double>>)categories[category]["operations"])
        {
            Console.WriteLine($"Описание: {operation.Item1}, Тип: {operation.Item2}, Сумма: {operation.Item3}");
        }
        Console.WriteLine($"Итоговый баланс: {(double)categories[category]["balance"]}");
    }

    public void CalculatePercentSpend()
    {
        double totalTopUp = categories.Values.SelectMany(c => ((List<Tuple<string, string, double>>)c["operations"]).Where(o => o.Item2 == TOP_UP).Select(o => o.Item3)).Sum();

        if (totalTopUp == 0.0)
        {
            Console.WriteLine("Нет операций пополнения.");
            return;
        }

        Console.WriteLine("Процент расходов по категориям:");
        foreach (var category in categories)
        {
            double totalTopDown = ((List<Tuple<string, string, double>>)category.Value["operations"]).Where(o => o.Item2 == TOP_DOWN).Select(o => o.Item3).Sum();
            double percent = Math.Round((totalTopDown / totalTopUp) * 100, 2);
            Console.WriteLine($"{category.Key}: {percent}%");
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Wallet wallet = new Wallet();
        while (true)
        {
            Console.WriteLine("Что хотите сделать?\n" +
                              "1 - создать категорию\n" +
                              "2 - пополнить категорию\n" +
                              "3 - списать деньги с категории\n" +
                              "4 - посмотреть статистику для категории\n" +
                              "5 - посмотреть потраченных денег для всех категории\n" +
                              "6 - посмотреть баланс категории\n" +
                              "7 - сделать перевод из категории в категорию");

            Console.Write("Введите цифру: ");
            int prompt = int.Parse(Console.ReadLine());
            Console.Write("Введите название для категории: ");
            string category = Console.ReadLine();

            switch (prompt)
            {
                case 1:
                    Console.Write("Введите начальный баланс. (По умолчанию - 0): ");
                    double balance = double.Parse(Console.ReadLine() ?? "0.0");
                    wallet.AddCategory(category, balance);
                    break;
                case 2:
                    Console.Write("Введите сумму: ");
                    double amount = double.Parse(Console.ReadLine());
                    Console.Write("Введите описание для операции: ");
                    string description = Console.ReadLine();
                    wallet.TopUp(category, amount, description);
                    break;
                case 3:
                    Console.Write("Введите сумму: ");
                    double amountDown = double.Parse(Console.ReadLine());
                    Console.Write("Введите описание для операции: ");
                    string descriptionDown = Console.ReadLine();
                    wallet.TopDown(category, amountDown, descriptionDown);
                    break;
                case 4:
                    wallet.PrintCategoryStats(category);
                    break;
                case 5:
                    wallet.CalculatePercentSpend();
                    break;
                case 6:
                    wallet.CheckBalance(category);
                    break;
                case 7:
                    Console.Write("Введите категорию, в которую необходимо сделать перевод: ");
                    string destination = Console.ReadLine();
                    Console.Write("Введите сумму: ");
                    double amountTransfer = double.Parse(Console.ReadLine());
                    wallet.SendToCategory(category, destination, amountTransfer);
                    break;
                default:
                    Console.WriteLine("Я не понимаю о чем вы.");
                    break;
            }
        }
    }
}
