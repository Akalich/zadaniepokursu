using System;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("1 - Подбор по словарю");
        Console.WriteLine("2 - Перебор по заданному набору символов");
        int choice = int.Parse(Console.ReadLine());
        
        if(choice == 1)
        {
            Console.WriteLine("Введите пароль для проверки:");
            string password = Console.ReadLine();
            
            Console.WriteLine("Введите количество символов в пароле:");
            int length = int.Parse(Console.ReadLine());
            
            Console.WriteLine("Начинаем подбор по словарю...");
            
            string[] dictionary = { "password", "qwerty", "123456", "admin", "love" };
            
            foreach(string word in dictionary)
            {
                if(word.Length == length && word == password)
                {
                    Console.WriteLine("Пароль найден: " + password);
                    break;
                }
                else
                {
                    Console.WriteLine("Пароль не найден.");
                    break;
                }
            }
            
        }
        else if(choice == 2)
        {
            Console.WriteLine("Введите количество символов в пароле:");
            int length = int.Parse(Console.ReadLine());
            
            Console.WriteLine("Введите набор символов для перебора:");
            string characters = Console.ReadLine();
            
            StringBuilder sb = new StringBuilder(length);
            Random rnd = new Random();
            
            while (true)
            {
                for (int i = 0; i < length; i++)
                {
                    sb.Append(characters[rnd.Next(characters.Length)]);
                }
                
                Console.WriteLine("Попытка: " + sb.ToString());
                
                if(sb.ToString() == "password")
                {
                    Console.WriteLine("Пароль найден: password");
                    break;
                }
                
                sb.Clear();
            }
        }
        else
        {
            Console.WriteLine("Неверный выбор.");
        }
    }
}
