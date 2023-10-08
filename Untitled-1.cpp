#include <iostream>
#include <ctime>
using namespace std;

int main()
{
    int const size = 10;
    int a[size];
    srand(unsigned(time(NULL)));
    for (int n = 0; n < size; n++) a[n] = rand() % 21;
    for (int n = 0; n < size; n++) cout << a[n] <<
    cout << endl;
    int left = 5, right = 10, max = left;
    for (int n = 0; n < size; n++) if (a[n] > 5 && a[n] < 10 && a[n] > max) max = a[n];
    if (max != left) cout << "Max = " << max << endl;
    else cout << "\a Does not exist!" << endl;
    cin. get();
    return 0;
}