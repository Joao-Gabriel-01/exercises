#include <iostream>
#include <string>
#include <sstream>

using namespace std;

struct No {
    int  senha;
    No*  proximo;
};

struct FilaPonteiro {
    No*  inicio;
    No*  fim;
    int  tamanho;
};

void fpInicializar(FilaPonteiro &f) {
    f.inicio  = NULL;
    f.fim     = NULL;
    f.tamanho = 0;
}

bool fpVazia(const FilaPonteiro &f) {
    return f.inicio == NULL;
}

void fpEnfileirar(FilaPonteiro &f, int valor) {
    No* novo      = new No;
    novo->senha   = valor;
    novo->proximo = NULL;

    if (fpVazia(f)) {
        f.inicio = novo;
        f.fim    = novo;
    } else {
        f.fim->proximo = novo;
        f.fim          = novo;
    }
    f.tamanho++;
}

int fpDesenfileirar(FilaPonteiro &f) {
    No*  aux   = f.inicio;
    int  valor = aux->senha;

    f.inicio = f.inicio->proximo;
    if (f.inicio == NULL)
        f.fim = NULL;

    delete aux;
    f.tamanho--;
    return valor;
}

void fpLiberar(FilaPonteiro &f) {
    while (!fpVazia(f))
        fpDesenfileirar(f);
}


const int MAX_VETOR = 200;  

struct FilaVetor {
    int  dados[MAX_VETOR];
    int  inicio;
    int  fim;
    int  tamanho;
};

void fvInicializar(FilaVetor &f) {
    f.inicio  = 0;
    f.fim     = 0;
    f.tamanho = 0;
}

bool fvVazia(const FilaVetor &f) {
    return f.tamanho == 0;
}

bool fvCheia(const FilaVetor &f) {
    return f.tamanho == MAX_VETOR;
}

bool fvEnfileirar(FilaVetor &f, int valor) {
    if (fvCheia(f))
        return false;

    f.dados[f.fim] = valor;
    f.fim          = (f.fim + 1) % MAX_VETOR;
    f.tamanho++;
    return true;
}

int fvDesenfileirar(FilaVetor &f) {
    int valor = f.dados[f.inicio];
    f.inicio  = (f.inicio + 1) % MAX_VETOR;
    f.tamanho--;
    return valor;
}


string intParaString(int n) {
    ostringstream oss;
    oss << n;
    return oss.str();
}

string formatarSenha(int n) {
    string s = intParaString(n);
    while ((int)s.size() < 3)
        s = "0" + s;
    return "S" + s;
}

void separador() {
    cout << "========================================" << endl;
}

int main() {
    FilaPonteiro senhasGeradas;
    fpInicializar(senhasGeradas);
    FilaVetor senhasAtendidas;
    fvInicializar(senhasAtendidas);

    int contadorSenhas = 0;
    int opcao          = -1;

    separador();
    cout << "        SISTEMA DE ATENDIMENTO"         << endl;
    cout << "  senhasGeradas   : Fila por Ponteiros" << endl;
    cout << "  senhasAtendidas : Fila por Vetor"     << endl;
    separador();

    do {
        cout << "\n  Senhas aguardando atendimento: "
             << senhasGeradas.tamanho << endl;
        cout << endl;
        cout << "  [1] Gerar senha"          << endl;
        cout << "  [2] Realizar atendimento" << endl;
        cout << "  [0] Sair"                 << endl;
        cout << "\n  Opcao: ";
        cin  >> opcao;

        switch (opcao) {

            case 1: {
                contadorSenhas++;
                fpEnfileirar(senhasGeradas, contadorSenhas);
                cout << "\n  >> Senha " << formatarSenha(contadorSenhas)
                     << " gerada e enfileirada em senhasGeradas."
                     << endl;
                break;
            }

            case 2: {
                if (fpVazia(senhasGeradas)) {
                    cout << "\n  >> Nao ha senhas aguardando atendimento!"
                         << endl;
                } else {
                    int senhaAtual = fpDesenfileirar(senhasGeradas);

                    if (!fvEnfileirar(senhasAtendidas, senhaAtual)) {

                        cout << "\n  >> ERRO: fila senhasAtendidas cheia!"
                             << endl;
                    } else {
                        cout << "\n  >> Atendendo senha: "
                             << formatarSenha(senhaAtual) << endl;
                        cout << "     Enfileirada em senhasAtendidas."
                             << endl;
                    }
                }
                break;
            }

            case 0: {
                if (!fpVazia(senhasGeradas)) {
                    cout << "\n  >> ATENCAO: Ainda ha "
                         << senhasGeradas.tamanho
                         << " senha(s) aguardando atendimento." << endl;
                    cout << "  >> Atenda todas antes de sair." << endl;
                    opcao = -1;  
                }
                break;
            }

          
            default: {
                cout << "\n  >> Opcao invalida! Tente novamente." << endl;
                break;
            }
        }

        separador();

    } while (opcao != 0);

   
    cout << "\n  SISTEMA ENCERRADO" << endl;
    cout << "  Total de senhas atendidas: "
         << senhasAtendidas.tamanho << endl;
    separador();

   
    fpLiberar(senhasGeradas);

    cout << "\n  Pressione ENTER para fechar..." << endl;
    cin.ignore();
    cin.get();

    return 0;
}
