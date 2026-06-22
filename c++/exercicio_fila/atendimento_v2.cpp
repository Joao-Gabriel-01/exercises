#include <iostream>
#include <string>
#include <sstream>

using namespace std;

// ============================================================
//  UTILITÁRIOS
// ============================================================

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

// ============================================================
//  FILA POR PONTEIROS  –  senhasGeradas
// ============================================================

struct NoFila {
    int    senha;
    NoFila* proximo;
};

struct FilaPonteiro {
    NoFila* inicio;
    NoFila* fim;
    int     tamanho;
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
    NoFila* novo    = new NoFila;
    novo->senha     = valor;
    novo->proximo   = NULL;

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
    NoFila* aux  = f.inicio;
    int     valor = aux->senha;

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

// ============================================================
//  GUICHÊ  –  cada guichê possui id e sua própria fila (por ponteiros)
// ============================================================

struct Guiche {
    int          id;
    FilaPonteiro senhasAtendidas;  // fila por ponteiros para as senhas atendidas
};

// ============================================================
//  LISTA ENCADEADA DE GUICHÊS
// ============================================================

struct NoLista {
    Guiche   guiche;
    NoLista* proximo;
};

struct ListaGuiches {
    NoLista* cabeca;
    int      quantidade;
};

void lgInicializar(ListaGuiches &l) {
    l.cabeca    = NULL;
    l.quantidade = 0;
}

// Retorna ponteiro para o nó que contém o guichê com o id dado (ou NULL)
NoLista* lgBuscar(ListaGuiches &l, int id) {
    NoLista* atual = l.cabeca;
    while (atual != NULL) {
        if (atual->guiche.id == id)
            return atual;
        atual = atual->proximo;
    }
    return NULL;
}

// Abre (adiciona) um novo guichê na lista; retorna false se o id já existir
bool lgAbrirGuiche(ListaGuiches &l, int id) {
    if (lgBuscar(l, id) != NULL)
        return false;  // id duplicado

    NoLista* novo = new NoLista;
    novo->guiche.id = id;
    fpInicializar(novo->guiche.senhasAtendidas);
    novo->proximo = NULL;

    // Inserção no fim para manter a ordem de abertura
    if (l.cabeca == NULL) {
        l.cabeca = novo;
    } else {
        NoLista* atual = l.cabeca;
        while (atual->proximo != NULL)
            atual = atual->proximo;
        atual->proximo = novo;
    }
    l.quantidade++;
    return true;
}

// Libera toda a memória da lista e das filas internas
void lgLiberar(ListaGuiches &l) {
    NoLista* atual = l.cabeca;
    while (atual != NULL) {
        NoLista* prox = atual->proximo;
        fpLiberar(atual->guiche.senhasAtendidas);
        delete atual;
        atual = prox;
    }
    l.cabeca     = NULL;
    l.quantidade = 0;
}

// Conta o total de senhas atendidas em todos os guichês
int lgTotalAtendidas(const ListaGuiches &l) {
    int total    = 0;
    NoLista* atual = l.cabeca;
    while (atual != NULL) {
        total += atual->guiche.senhasAtendidas.tamanho;
        atual  = atual->proximo;
    }
    return total;
}

// ============================================================
//  MAIN
// ============================================================

int main() {
    FilaPonteiro senhasGeradas;
    fpInicializar(senhasGeradas);

    ListaGuiches listaGuiches;
    lgInicializar(listaGuiches);

    int contadorSenhas = 0;
    int opcao          = -1;

    separador();
    cout << "    SISTEMA DE ATENDIMENTO  v2.0"    << endl;
    cout << "  senhasGeradas : Fila por Ponteiros" << endl;
    cout << "  Guiches       : Lista Encadeada"    << endl;
    separador();

    do {
        cout << "\n  Senhas aguardando atendimento : "
             << senhasGeradas.tamanho << endl;
        cout << "  Guiches abertos               : "
             << listaGuiches.quantidade << endl;
        cout << endl;
        cout << "  [1] Gerar senha"           << endl;
        cout << "  [2] Abrir guiche"          << endl;
        cout << "  [3] Realizar atendimento"  << endl;
        cout << "  [4] Listar senhas atendidas" << endl;
        cout << "  [0] Sair"                  << endl;
        cout << "\n  Opcao: ";
        cin  >> opcao;
        cout << endl;

        switch (opcao) {

            // --------------------------------------------------
            case 1: {
                contadorSenhas++;
                fpEnfileirar(senhasGeradas, contadorSenhas);
                cout << "  >> Senha " << formatarSenha(contadorSenhas)
                     << " gerada e enfileirada em senhasGeradas." << endl;
                break;
            }

            // --------------------------------------------------
            case 2: {
                int idGuiche;
                cout << "  ID do novo guiche: ";
                cin  >> idGuiche;

                if (lgAbrirGuiche(listaGuiches, idGuiche)) {
                    cout << "  >> Guiche " << idGuiche
                         << " aberto e adicionado a lista de guiches." << endl;
                } else {
                    cout << "  >> ERRO: ja existe um guiche com o ID "
                         << idGuiche << "." << endl;
                }
                break;
            }

            // --------------------------------------------------
            case 3: {
                if (listaGuiches.quantidade == 0) {
                    cout << "  >> Nenhum guiche aberto."
                            " Abra um guiche antes de atender." << endl;
                    break;
                }
                if (fpVazia(senhasGeradas)) {
                    cout << "  >> Nao ha senhas aguardando atendimento!" << endl;
                    break;
                }

                int idGuiche;
                cout << "  ID do guiche que esta chamando: ";
                cin  >> idGuiche;

                NoLista* no = lgBuscar(listaGuiches, idGuiche);
                if (no == NULL) {
                    cout << "  >> ERRO: guiche " << idGuiche
                         << " nao encontrado na lista." << endl;
                } else {
                    int senhaAtual = fpDesenfileirar(senhasGeradas);
                    fpEnfileirar(no->guiche.senhasAtendidas, senhaAtual);
                    cout << "  >> Guiche " << idGuiche
                         << " atendeu a senha " << formatarSenha(senhaAtual)
                         << "." << endl;
                }
                break;
            }

            // --------------------------------------------------
            case 4: {
                if (listaGuiches.quantidade == 0) {
                    cout << "  >> Nenhum guiche aberto." << endl;
                    break;
                }

                int idGuiche;
                cout << "  ID do guiche para consulta: ";
                cin  >> idGuiche;

                NoLista* no = lgBuscar(listaGuiches, idGuiche);
                if (no == NULL) {
                    cout << "  >> ERRO: guiche " << idGuiche
                         << " nao encontrado." << endl;
                } else {
                    FilaPonteiro &fila = no->guiche.senhasAtendidas;
                    cout << "  >> Guiche " << idGuiche
                         << " atendeu " << fila.tamanho
                         << " senha(s):" << endl;

                    if (fpVazia(fila)) {
                        cout << "     (nenhuma senha atendida ainda)" << endl;
                    } else {
                        // Percorre a fila apenas para leitura (sem remover)
                        NoFila* atual = fila.inicio;
                        int pos = 1;
                        while (atual != NULL) {
                            cout << "     [" << pos << "] "
                                 << formatarSenha(atual->senha) << endl;
                            atual = atual->proximo;
                            pos++;
                        }
                    }
                }
                break;
            }

            // --------------------------------------------------
            case 0: {
                if (!fpVazia(senhasGeradas)) {
                    cout << "  >> ATENCAO: Ainda ha "
                         << senhasGeradas.tamanho
                         << " senha(s) aguardando atendimento." << endl;
                    cout << "  >> Atenda todas as senhas antes de sair."
                         << endl;
                    opcao = -1;  // impede encerramento
                }
                break;
            }

            // --------------------------------------------------
            default: {
                cout << "  >> Opcao invalida! Tente novamente." << endl;
                break;
            }
        }

        separador();

    } while (opcao != 0);

    // ----------------------------------------------------------
    //  Encerramento
    // ----------------------------------------------------------
    cout << "\n  SISTEMA ENCERRADO" << endl;
    cout << "  Total de senhas atendidas: "
         << lgTotalAtendidas(listaGuiches) << endl;
    separador();

    // Libera memória
    fpLiberar(senhasGeradas);
    lgLiberar(listaGuiches);

    cout << "\n  Pressione ENTER para fechar..." << endl;
    cin.ignore();
    cin.get();

    return 0;
}
