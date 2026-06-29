#include <iostream>
using namespace std;

void insertionSort(int v[], int n) {
    int i, j, chave;

    for (i = 1; i < n; i++) {
        chave = v[i];
        j = i - 1;

        while (j >= 0 && v[j] > chave) {
            v[j + 1] = v[j];
            j--;
        }

        v[j + 1] = chave;
    }
}

void exibirVetor(int v[], int n) {
    int i;
    for (i = 0; i < n; i++) {
        cout << v[i];
        if (i < n - 1)
            cout << " - ";
    }
    cout << endl;
}

int main() {
    int v[] = {49, 38, 58, 87, 34, 93, 26, 13};
    int n = 8;

    cout << "Vetor antes da ordenacao:" << endl;
    exibirVetor(v, n);

    insertionSort(v, n);

    cout << "Vetor apos a ordenacao:" << endl;
    exibirVetor(v, n);

    return 0;
}
