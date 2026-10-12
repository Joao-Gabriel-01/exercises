using System;
using System.Collections.Generic;
using System.Globalization;

// ============================================================
// Projeto Biblioteca - Listas / Console Application (MVC)
// ============================================================

namespace ProjetoBiblioteca
{
    // ======================== MODEL ========================

    public class Emprestimo
    {
        private DateTime dtEmprestimo;
        private DateTime dtDevolucao; // DateTime.MinValue = ainda nao devolvido

        public Emprestimo(DateTime dtEmprestimo)
        {
            this.dtEmprestimo = dtEmprestimo;
            this.dtDevolucao = DateTime.MinValue;
        }

        public DateTime DtEmprestimo { get { return dtEmprestimo; } set { dtEmprestimo = value; } }
        public DateTime DtDevolucao { get { return dtDevolucao; } set { dtDevolucao = value; } }

        public bool EmAberto { get { return dtDevolucao == DateTime.MinValue; } }
    }

    public class Exemplar
    {
        private int tombo;
        private List<Emprestimo> emprestimos = new List<Emprestimo>();

        public Exemplar(int tombo)
        {
            this.tombo = tombo;
        }

        public int Tombo { get { return tombo; } set { tombo = value; } }
        public List<Emprestimo> Emprestimos { get { return emprestimos; } }

        // Um exemplar esta disponivel quando nao tem emprestimo em aberto
        public bool disponivel()
        {
            if (emprestimos.Count == 0) return true;
            return !emprestimos[emprestimos.Count - 1].EmAberto;
        }

        public bool emprestar()
        {
            if (!disponivel()) return false;
            emprestimos.Add(new Emprestimo(DateTime.Now));
            return true;
        }

        public bool devolver()
        {
            if (disponivel()) return false; // nao ha emprestimo em aberto
            emprestimos[emprestimos.Count - 1].DtDevolucao = DateTime.Now;
            return true;
        }

        public int qtdeEmprestimos()
        {
            return emprestimos.Count;
        }
    }

    public class Livro
    {
        private int isbn;
        private string titulo;
        private string autor;
        private string editora;
        private List<Exemplar> exemplares = new List<Exemplar>();

        public Livro(int isbn, string titulo, string autor, string editora)
        {
            this.isbn = isbn;
            this.titulo = titulo;
            this.autor = autor;
            this.editora = editora;
        }

        public int Isbn { get { return isbn; } set { isbn = value; } }
        public string Titulo { get { return titulo; } set { titulo = value; } }
        public string Autor { get { return autor; } set { autor = value; } }
        public string Editora { get { return editora; } set { editora = value; } }
        public List<Exemplar> Exemplares { get { return exemplares; } }

        public void adicionarExemplar(Exemplar exemplar)
        {
            if (exemplar != null) exemplares.Add(exemplar);
        }

        public int qtdeExemplares()
        {
            return exemplares.Count;
        }

        public int qtdeDisponiveis()
        {
            int qtd = 0;
            foreach (Exemplar e in exemplares)
                if (e.disponivel()) qtd++;
            return qtd;
        }

        // Total de emprestimos de todos os exemplares do titulo
        public int qtdeEmprestimos()
        {
            int qtd = 0;
            foreach (Exemplar e in exemplares)
                qtd += e.qtdeEmprestimos();
            return qtd;
        }

        public double percDisponibilidade()
        {
            if (exemplares.Count == 0) return 0;
            return (double)qtdeDisponiveis() / qtdeExemplares() * 100;
        }

        // Dois livros sao iguais quando possuem o mesmo ISBN
        public override bool Equals(object obj)
        {
            Livro outro = obj as Livro;
            if (outro == null) return false;
            return isbn == outro.isbn;
        }

        public override int GetHashCode()
        {
            return isbn.GetHashCode();
        }
    }

    public class Livros
    {
        private List<Livro> acervo = new List<Livro>();

        public List<Livro> Acervo { get { return acervo; } }

        public void adicionar(Livro livro)
        {
            if (livro != null) acervo.Add(livro);
        }

        public Livro pesquisar(Livro livro)
        {
            if (livro == null) return null;
            int pos = acervo.IndexOf(livro);
            return pos >= 0 ? acervo[pos] : null;
        }

        // Extra: localiza um exemplar pelo tombo em todo o acervo
        public Exemplar buscarExemplar(int tombo)
        {
            foreach (Livro l in acervo)
                foreach (Exemplar e in l.Exemplares)
                    if (e.Tombo == tombo) return e;
            return null;
        }
    }

    // ======================== VIEW ========================

    public class BibliotecaView
    {
        private static readonly CultureInfo br = new CultureInfo("pt-BR");

        public int menu()
        {
            Console.WriteLine();
            Console.WriteLine("===== BIBLIOTECA =====");
            Console.WriteLine("1 - Adicionar livro");
            Console.WriteLine("2 - Pesquisar livro (sintetico)");
            Console.WriteLine("3 - Pesquisar livro (analitico)");
            Console.WriteLine("4 - Adicionar exemplar");
            Console.WriteLine("5 - Registrar emprestimo");
            Console.WriteLine("6 - Registrar devolucao");
            Console.WriteLine("0 - Sair");
            return lerInt("Opcao: ");
        }

        public int lerInt(string mensagem)
        {
            while (true)
            {
                Console.Write(mensagem);
                int valor;
                if (int.TryParse(Console.ReadLine(), out valor) && valor >= 0) return valor;
                Console.WriteLine("Valor invalido. Digite um numero inteiro positivo.");
            }
        }

        public string lerTexto(string mensagem)
        {
            while (true)
            {
                Console.Write(mensagem);
                string txt = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(txt)) return txt.Trim();
                Console.WriteLine("O campo nao pode ficar vazio.");
            }
        }

        public void mensagem(string texto)
        {
            Console.WriteLine(texto);
        }

        private string data(DateTime d)
        {
            return d.ToString("dd/MM/yyyy HH:mm", br);
        }

        public void mostrarSintetico(Livro l)
        {
            Console.WriteLine();
            Console.WriteLine("ISBN: " + l.Isbn);
            Console.WriteLine("Titulo: " + l.Titulo);
            Console.WriteLine("Autor: " + l.Autor);
            Console.WriteLine("Editora: " + l.Editora);
            Console.WriteLine("Total de exemplares: " + l.qtdeExemplares());
            Console.WriteLine("Exemplares disponiveis: " + l.qtdeDisponiveis());
            Console.WriteLine("Total de emprestimos: " + l.qtdeEmprestimos());
            Console.WriteLine("Disponibilidade: " + l.percDisponibilidade().ToString("F2", br) + "%");
        }

        public void mostrarAnalitico(Livro l)
        {
            mostrarSintetico(l);
            Console.WriteLine();
            Console.WriteLine("--- Exemplares ---");
            if (l.qtdeExemplares() == 0)
            {
                Console.WriteLine("(nenhum exemplar cadastrado)");
                return;
            }

            foreach (Exemplar e in l.Exemplares)
            {
                Console.WriteLine("Tombo " + e.Tombo + " | " +
                                  (e.disponivel() ? "Disponivel" : "Emprestado") +
                                  " | Emprestimos: " + e.qtdeEmprestimos());
                if (e.qtdeEmprestimos() == 0)
                {
                    Console.WriteLine("    (sem emprestimos)");
                    continue;
                }
                foreach (Emprestimo emp in e.Emprestimos)
                {
                    Console.WriteLine("    Emprestimo: " + data(emp.DtEmprestimo) +
                                      " | Devolucao: " +
                                      (emp.EmAberto ? "(em aberto)" : data(emp.DtDevolucao)));
                }
            }
        }
    }

    // ===================== CONTROLLER =====================

    public class BibliotecaController
    {
        private Livros livros = new Livros();
        private BibliotecaView view = new BibliotecaView();

        public void executar()
        {
            int opcao;
            do
            {
                opcao = view.menu();
                switch (opcao)
                {
                    case 1: adicionarLivro(); break;
                    case 2: pesquisarSintetico(); break;
                    case 3: pesquisarAnalitico(); break;
                    case 4: adicionarExemplar(); break;
                    case 5: registrarEmprestimo(); break;
                    case 6: registrarDevolucao(); break;
                    case 0: view.mensagem("Encerrando..."); break;
                    default: view.mensagem("Opcao invalida."); break;
                }
            } while (opcao != 0);
        }

        // Livro "modelo" so com o ISBN, usado nas buscas (Equals compara pelo ISBN)
        private Livro localizarLivro()
        {
            int isbn = view.lerInt("ISBN: ");
            Livro achado = livros.pesquisar(new Livro(isbn, "", "", ""));
            if (achado == null) view.mensagem("Livro nao encontrado.");
            return achado;
        }

        private void adicionarLivro()
        {
            int isbn = view.lerInt("ISBN: ");
            if (livros.pesquisar(new Livro(isbn, "", "", "")) != null)
            {
                view.mensagem("Ja existe um livro com esse ISBN.");
                return;
            }
            string titulo = view.lerTexto("Titulo: ");
            string autor = view.lerTexto("Autor: ");
            string editora = view.lerTexto("Editora: ");

            livros.adicionar(new Livro(isbn, titulo, autor, editora));
            view.mensagem("Livro adicionado.");
        }

        private void pesquisarSintetico()
        {
            Livro l = localizarLivro();
            if (l != null) view.mostrarSintetico(l);
        }

        private void pesquisarAnalitico()
        {
            Livro l = localizarLivro();
            if (l != null) view.mostrarAnalitico(l);
        }

        private void adicionarExemplar()
        {
            Livro l = localizarLivro();
            if (l == null) return;

            int tombo = view.lerInt("Tombo do exemplar: ");
            if (livros.buscarExemplar(tombo) != null)
            {
                view.mensagem("Ja existe um exemplar com esse tombo.");
                return;
            }

            l.adicionarExemplar(new Exemplar(tombo));
            view.mensagem("Exemplar adicionado ao livro \"" + l.Titulo + "\".");
        }

        private void registrarEmprestimo()
        {
            int tombo = view.lerInt("Tombo do exemplar: ");
            Exemplar e = livros.buscarExemplar(tombo);
            if (e == null)
            {
                view.mensagem("Exemplar nao encontrado.");
                return;
            }

            if (e.emprestar())
                view.mensagem("Emprestimo registrado.");
            else
                view.mensagem("Exemplar indisponivel: ja esta emprestado.");
        }

        private void registrarDevolucao()
        {
            int tombo = view.lerInt("Tombo do exemplar: ");
            Exemplar e = livros.buscarExemplar(tombo);
            if (e == null)
            {
                view.mensagem("Exemplar nao encontrado.");
                return;
            }

            if (e.devolver())
                view.mensagem("Devolucao registrada.");
            else
                view.mensagem("Este exemplar nao esta emprestado.");
        }
    }

    // ======================== MAIN ========================

    class Program
    {
        static void Main(string[] args)
        {
            BibliotecaController controller = new BibliotecaController();
            controller.executar();
        }
    }
}
