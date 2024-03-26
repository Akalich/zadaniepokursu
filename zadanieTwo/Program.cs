using System;
using System.Collections.Generic;
using System.Text;

class Category
{
    private string name;
    private List<string> actions;
    private decimal totalBalance;

    public Category(string name)
    {
        this.name = name;
        this.actions = new List<string>();
        this.totalBalance = 0;
    }

    public void AddMoney(decimal amount, string description = "")
    {
        this.totalBalance += amount;
        string action = $"внесение средств".PadRight(20) + amount.ToString("#,0.00");
        this.actions.Add(action);
        if (!String.IsNullOrEmpty(description))
        {
            action = description.PadRight(20) + amount.ToString("#,0.00");
            this.actions.Add(action);
        }
    }

    public bool WithdrawMoney(decimal amount, string description = "")
    {
        if (this.totalBalance < amount)
        {
            return false; 
        }
        else
        {
            this.totalBalance -= amount;
            string action = description.PadRight(20) + amount.ToString("#,0.00");
            this.actions.Add(action);
            return true;
        }
    }

    public bool Transfer(Category targetCategory, decimal amount)
    {
        if (this.totalBalance < amount)
        {
            return false;
        }
        else
        {
            this.totalBalance -= amount;
            targetCategory.AddMoney(amount, $"перевод из {this.name}");
            return true;
        }
    }

    public bool CheckBalance(decimal amount)
    {
        if(this.totalBalance < amount)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Print()
    {
        Console.WriteLine($"****************{this.name}****************");
        foreach(string action in this.actions)
        {
            Console.WriteLine(action);
        }
        Console.WriteLine($"Всего: {this.totalBalance.ToString("#,0.00")}");
    }
}

class Program
{
    static decimal CalculateSpentPercent(List<Category> categories, Category targetCategory)
    {
        decimal totalSpent = 0;
        foreach(Category category in categories)
        {
            totalSpent += category.TotalBalance;
        }
        decimal targetSpent = targetCategory.TotalBalance;
        return Math.Round(targetSpent / totalSpent * 100, 0);
    }

    static void Main()
    {
        Category food = new Category("Еда");
        food.AddMoney(1000, "зарпата");
        food.AddMoney(500, "подарок");

        Category clothes = new Category("Одежда");
        clothes.AddMoney(2000, "новая куртка");

        food.Transfer(clothes, 300);

        food.Print();
        clothes.Print();

        List<Category> allCategories = new List<Category> { food, clothes };
        Console.WriteLine("Потрачено в процентах:");
        for(int percent = 100; percent >= 10; percent -= 10)
        {
            Console.Write(percent.ToString().PadLeft(3) + "| ");
            foreach(Category category in allCategories)
            {
                decimal spentPercent = CalculateSpentPercent(allCategories, category);
                if(spentPercent >= percent)
                {
                    Console.Write("o".PadLeft(3));
                }
                else
                {
                    Console.Write("  ".PadLeft(3)); 
                }
            }
            Console.WriteLine();
        }
    }
}
