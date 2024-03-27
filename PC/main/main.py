import itertools
import string

def brute_force_password(length, alphabet):
    for password in itertools.product(alphabet, repeat=length):
        yield ''.join(password)

def main():
    length = int(input("Введите длину пароля: "))
    choice = input("Выберите режим работы (1 - перебор по заданному набору символов, 2 - перебор по словарю): ")

    if choice == '1':
        alphabet = input("Введите набор символов: ")
        passwords = brute_force_password(length, alphabet)
    elif choice == '2':
        with open('dictionary.txt', 'r') as file:
            words = file.read().split('\n')
        passwords = itertools.product(words, repeat=length)
    else:
        print("Некорректный выбор режима работы.")
        return

    for password in passwords:
        print("Попытка взлома с использованием пароля:", password)

if __name__ == '__main__':
    main()
