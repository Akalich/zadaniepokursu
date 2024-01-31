using System;
using System.Collections.Generic;
using System.Linq;
class Category
{
    public string Name { get; set; }
    private List<(decimal amount, string description)> actions;

    public Category(string name)
    {
        Name = name;
        actions = new List<(decimal, string)>();
    }

    public void Deposit(decimal amount, string description = "")
    {
        actions.Add((amount, description));
    }

    public bool Withdraw(decimal amount, string description = "")
    {
        if (GetBalance() >= amount)
        {
            actions.Add((-amount, description));
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Transfer(Category targetCategory, decimal amount)
    {
        if (GetBalance() >= amount)
        {
            actions.Add((-amount, $"Transfer to {targetCategory.Name}"));
            targetCategory.actions.Add((amount, $"Transfer from {Name}"));
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckBalance(decimal amount)
    {
        return GetBalance() >= amount;
    }

    public decimal GetBalance()
    {
        return actions.Sum(a => a.amount);
    }

    public void Print()
    {
        Console.WriteLine($"********{Name}********");

        foreach (var action in actions)
        {
            Console.WriteLine($"{action.description,-20} {action.amount,15:N}");
        }

        Console.WriteLine($"Total: {GetBalance(),15:N}");
        Console.WriteLine();
    }
}

class Wallet
{
    private List<Category> categories;

    public Wallet()
    {
        categories = new List<Category>();
    }

    public Category CreateCategory(string name)
    {
        var category = new Category(name);
        categories.Add(category);
        return category;
    }

    public void Print()
    {
        foreach (var category in categories)
        {
            category.Print();
        }
    }

    public decimal CalculateSpendingPercentage(Category category)
    {
        var totalSpending = categories.Sum(c => Math.Max(0, -c.GetBalance()));

        if (totalSpending == 0)
        {
            return 0;
        }

        var categorySpending = Math.Max(0, -category.GetBalance());

        return Math.Round((categorySpending / totalSpending) * 100, 1);
    }

    public void PrintSpendingPercentage()
    {
        Console.WriteLine("Spending Percentage:");

        int maxPercentage = 100;
        int increment = 10;

        while (maxPercentage >= 0)
        {
            Console.Write($"{maxPercentage,3}| ");

            foreach (var category in categories)
            {
                var percentage = CalculateSpendingPercentage(category);

                if (percentage >= maxPercentage)
                {
                    Console.Write("o  ");
                }
                else
                {
                    Console.Write("   ");
                }
            }

            Console.WriteLine();

            maxPercentage -= increment;
        }

        Console.Write("    ");
        foreach (var category in categories)
        {
            Console.Write($"{category.Name,3} ");
        }

        Console.WriteLine();
    }
}

class Program
{
    static void Main(string[] args)
    {
        Wallet wallet = new Wallet();

        Category foodCategory = wallet.CreateCategory("Food");
        foodCategory.Deposit(100, "Initial deposit");
        foodCategory.Withdraw(50, "Groceries");
        foodCategory.Withdraw(20);
        foodCategory.Transfer(wallet.CreateCategory("Clothes"), 30);

        Category clothesCategory = wallet.CreateCategory("Clothes");
        clothesCategory.Deposit(50, "Initial deposit");
        clothesCategory.Withdraw(20, "Shirt");
        clothesCategory.Withdraw(10);
        clothesCategory.Withdraw(10, "Pants");

        wallet.Print();
        wallet.PrintSpendingPercentage();
    }
}
