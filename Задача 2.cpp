﻿// Задача2.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include <iostream>
using namespace std;
int a,mx,mn,n;
int i = 0;
int main()
{
	setlocale(LC_ALL, "RU");
	cout << "Введите кол-во чисел" << endl;
	cin >> n;
	while (n <= 0)
	{
		cout << "Введите кол-во чисел" << endl;
		cin >> n;
	}
	cout << "Введите первое число" << endl;
	cin >> a;
	mx = a;
	mn = a;
	cout << "Вводите числа последовательности начиная с первого :" << endl;
	while (i <= n)
	{
		i++;
		cin >> a;
		if (mx < a)
		{
			mx = a;
		}
		if (mn > a)
		{
			mn = a;
		}
		
	}
	cout << "Максимальное число:" << mx << endl;
	cout << "Минимальное число :" << mn << endl;
	cout << "Сумма максимально и минимального :" << mx + mn << endl;
	return 0;
}

// Запуск программы: CTRL+F5 или меню "Отладка" > "Запуск без отладки"
// Отладка программы: F5 или меню "Отладка" > "Запустить отладку"

// Советы по началу работы 
//   1. В окне обозревателя решений можно добавлять файлы и управлять ими.
//   2. В окне Team Explorer можно подключиться к системе управления версиями.
//   3. В окне "Выходные данные" можно просматривать выходные данные сборки и другие сообщения.
//   4. В окне "Список ошибок" можно просматривать ошибки.
//   5. Последовательно выберите пункты меню "Проект" > "Добавить новый элемент", чтобы создать файлы кода, или "Проект" > "Добавить существующий элемент", чтобы добавить в проект существующие файлы кода.
//   6. Чтобы снова открыть этот проект позже, выберите пункты меню "Файл" > "Открыть" > "Проект" и выберите SLN-файл.
