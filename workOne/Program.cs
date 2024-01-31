using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace workOne
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Выберите способ подбора пароля:");
            Console.WriteLine("1. Подбор по словарю");
            Console.WriteLine("2. Перебор по заданному набору символов");

            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    DictionaryAttack();
                    break;
                case 2:
                    BruteforceAttack();
                    break;
                default:
                    Console.WriteLine("Некорректный выбор.");
                    break;
            }
        }

        static void DictionaryAttack()
        {
            Console.WriteLine("Введите путь к словарю:");
            string dictionaryPath = Console.ReadLine();
            Console.WriteLine("Введите путь к файлу, для которого нужно подобрать пароль:");
            string filePath = Console.ReadLine();

            List<string> dictionary = new List<string>(File.ReadAllLines(dictionaryPath));

            foreach (string password in dictionary)
            {
                if (CheckPassword(password, filePath))
                {
                    Console.WriteLine("Пароль найден: " + password);
                    // передать пароль другой программе или серверу
                    return;
                }
            }

            Console.WriteLine("Пароль не найден.");
        }

        static void BruteforceAttack()
        {
            Console.WriteLine("Введите набор символов для перебора:");
            string characters = Console.ReadLine();
            Console.WriteLine("Введите длину пароля:");
            int passwordLength = int.Parse(Console.ReadLine());

            char[] password = new char[passwordLength];
            char[] characterSet = characters.ToCharArray();

            for (int i = 0; i < passwordLength; i++)
            {
                password[i] = characterSet[0];
            }

            while (true)
            {
                if (CheckPassword(new string(password), null))
                {
                    Console.WriteLine("Пароль найден: " + new string(password));
                    // передать пароль другой программе или серверу
                    return;
                }

                int index = passwordLength - 1;
                while (index >= 0 && password[index] == characterSet[characterSet.Length - 1])
                {
                    password[index] = characterSet[0];
                    index--;
                }

                if (index < 0)
                    break;

                password[index] = characterSet[Array.IndexOf(characterSet, password[index]) + 1];
            }

            Console.WriteLine("Пароль не найден.");
        }

        static bool CheckPassword(string password, string filePath)
        {
            return false;
        }
    }
}