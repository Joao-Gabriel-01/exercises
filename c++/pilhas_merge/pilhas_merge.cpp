#include <iostream>
using namespace std;

struct No {
    int valor;
    No* prox;
};

struct PilhaLista {
    No* topo;
    int tamanho;
};

void iniciarLista(PilhaLista& p) {
    p.topo = NULL;
    p.tamanho = 0;
}

void empilharLista(PilhaLista& p, int v) {
    No* novo = new No;
    novo->valor = v;
    novo->prox = p.topo;
    p.topo = novo;
    p.tamanho++;
}

int desempilharLista(PilhaLista& p) {
    int v = p.topo->valor;
    No* tmp = p.topo;
    p.topo = p.topo->prox;
    delete tmp;
    p.tamanho--;
    return v;
}

bool vaziaLista(PilhaLista& p) { return p.topo == NULL; }

const int CAP = 30;

struct PilhaVetor {
    int dados[CAP];
    int topo;
};

void iniciarVetor(PilhaVetor& p) {
    p.topo = -1;
}

void empilharVetor(PilhaVetor& p, int v) {
    p.dados[++p.topo] = v;
}

int desempilharVetor(PilhaVetor& p) {
    return p.dados[p.topo--];
}

bool vaziaVetor(PilhaVetor& p) { return p.topo == -1; }

void lerNumeros(int nums[], int qtd) {
    cout << "\n=== Digite " << qtd << " numeros inteiros em ordem CRESCENTE ===\n";
    for (int i = 0; i < qtd; i++) {
        while (true) {
            cout << "  [" << i + 1 << "/" << qtd << "] Numero: ";
            cin >> nums[i];
            if (i == 0 || nums[i] > nums[i - 1]) break;
            cout << "  ERRO: o numero deve ser MAIOR que o anterior ("
                 << nums[i - 1] << "). Tente novamente.\n";
        }
    }
}

void demoPilhaLista(int nums[], int qtd) {
    cout << "\n=== IMPLEMENTACAO: LISTA ENCADEADA ===\n";

    PilhaLista pares, impares;
    iniciarLista(pares);
    iniciarLista(impares);

    cout << "\n--- Empilhando ---\n";
    for (int i = 0; i < qtd; i++) {
        if (nums[i] % 2 == 0) {
            empilharLista(pares, nums[i]);
            cout << "  " << nums[i] << " -> pilha PARES   (topo=" << pares.topo->valor << ")\n";
        } else {
            empilharLista(impares, nums[i]);
            cout << "  " << nums[i] << " -> pilha IMPARES (topo=" << impares.topo->valor << ")\n";
        }
    }

    cout << "\n--- Desempilhando (ordem decrescente) ---\n";
    cout << "  Impares: ";
    while (!vaziaLista(impares))
        cout << desempilharLista(impares) << " ";
    cout << "\n  Pares  : ";
    while (!vaziaLista(pares))
        cout << desempilharLista(pares) << " ";
    cout << "\n";
}

void demoPilhaVetor(int nums[], int qtd) {
    cout << "\n=== IMPLEMENTACAO: VETOR ===\n";

    PilhaVetor pares, impares;
    iniciarVetor(pares);
    iniciarVetor(impares);

    cout << "\n--- Empilhando ---\n";
    for (int i = 0; i < qtd; i++) {
        if (nums[i] % 2 == 0) {
            empilharVetor(pares, nums[i]);
            cout << "  " << nums[i] << " -> pilha PARES   (topo=dados[" << pares.topo << "]=" << pares.dados[pares.topo] << ")\n";
        } else {
            empilharVetor(impares, nums[i]);
            cout << "  " << nums[i] << " -> pilha IMPARES (topo=dados[" << impares.topo << "]=" << impares.dados[impares.topo] << ")\n";
        }
    }

    cout << "\n--- Desempilhando (ordem decrescente) ---\n";
    cout << "  Impares: ";
    while (!vaziaVetor(impares))
        cout << desempilharVetor(impares) << " ";
    cout << "\n  Pares  : ";
    while (!vaziaVetor(pares))
        cout << desempilharVetor(pares) << " ";
    cout << "\n";
}

int main() {
    const int QTD = 30;
    int nums[QTD];

    lerNumeros(nums, QTD);

    demoPilhaLista(nums, QTD);
    demoPilhaVetor(nums, QTD);

    cout << "\n=== Programa encerrado ===\n";
    return 0;
}
