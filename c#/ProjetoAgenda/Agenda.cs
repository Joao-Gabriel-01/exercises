using System;
using System.Collections.Generic;
using System.Text;

// ============================================================
// Projeto Agenda (Contatos) - Listas / Console Application
// ============================================================

namespace ProjetoAgenda
{
    // ======================== MODEL ========================

    public class Data
    {
        private int dia;
        private int mes;
        private int ano;

        public Data(int dia, int mes, int ano)
        {
            setData(dia, mes, ano);
        }

        public int Dia { get { return dia; } }
        public int Mes { get { return mes; } }
        public int Ano { get { return ano; } }

        public void setData(int dia, int mes, int ano)
        {
            if (!ehValida(dia, mes, ano))
                throw new ArgumentException("Data invalida.");
            this.dia = dia;
            this.mes = mes;
            this.ano = ano;
        }

        public static bool ehValida(int dia, int mes, int ano)
        {
            if (ano < 1 || ano > 9999 || mes < 1 || mes > 12 || dia < 1) return false;
            return dia <= DateTime.DaysInMonth(ano, mes);
        }

        public override string ToString()
        {
            return dia.ToString("00") + "/" + mes.ToString("00") + "/" + ano.ToString("0000");
        }
    }

    public class Telefone
    {
        private string tipo;
        private string numero;
        private bool principal;

        public Telefone(string tipo, string numero, bool principal)
        {
            this.tipo = tipo;
            this.numero = numero;
            this.principal = principal;
        }

        public string Tipo { get { return tipo; } set { tipo = value; } }
        public string Numero { get { return numero; } set { numero = value; } }
        public bool Principal { get { return principal; } set { principal = value; } }

        public override string ToString()
        {
            return tipo + ": " + numero + (principal ? " (principal)" : "");
        }
    }

    public class Contato
    {
        private string email;
        private string nome;
        private Data dtNasc;
        private List<Telefone> telefones = new List<Telefone>();

        public Contato(string nome, string email, Data dtNasc)
        {
            this.nome = nome;
            this.email = email;
            this.dtNasc = dtNasc;
        }

        public string Email { get { return email; } set { email = value; } }
        public string Nome { get { return nome; } set { nome = value; } }
        public Data DtNasc { get { return dtNasc; } set { dtNasc = value; } }
        public List<Telefone> Telefones { get { return telefones; } }

        public int getIdade()
        {
            DateTime hoje = DateTime.Today;
            int idade = hoje.Year - dtNasc.Ano;
            // ainda nao fez aniversario neste ano
            if (hoje.Month < dtNasc.Mes || (hoje.Month == dtNasc.Mes && hoje.Day < dtNasc.Dia))
                idade--;
            return idade;
        }

        public void adicionarTelefone(Telefone t)
        {
            if (t == null) return;

            // o primeiro telefone sempre e o principal
            if (telefones.Count == 0) t.Principal = true;

            // so pode existir um telefone principal
            if (t.Principal)
            {
                foreach (Telefone outro in telefones)
                    outro.Principal = false;
            }
            telefones.Add(t);
        }

        public string getTelefonePrincipal()
        {
            foreach (Telefone t in telefones)
                if (t.Principal) return t.Numero;
            return "(sem telefone)";
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Nome: " + nome);
            sb.Append(" | E-mail: " + email);
            sb.Append(" | Nascimento: " + dtNasc);
            sb.Append(" (" + getIdade() + " anos)");
            sb.Append(" | Tel. principal: " + getTelefonePrincipal());
            return sb.ToString();
        }

        // Dois contatos sao iguais quando possuem o mesmo nome (sem diferenciar maiusculas)
        public override bool Equals(object obj)
        {
            Contato outro = obj as Contato;
            if (outro == null) return false;
            return string.Equals(nome, outro.nome, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return nome == null ? 0 : nome.ToLowerInvariant().GetHashCode();
        }
    }

    public class Contatos
    {
        private readonly List<Contato> agenda = new List<Contato>();

        public List<Contato> Agenda { get { return agenda; } }

        public bool adicionar(Contato c)
        {
            if (c == null || agenda.Contains(c)) return false; // nao permite duplicados
            agenda.Add(c);
            return true;
        }

        public Contato pesquisar(Contato c)
        {
            if (c == null) return null;
            int pos = agenda.IndexOf(c);
            return pos >= 0 ? agenda[pos] : null;
        }

        // Substitui os dados do contato existente pelos dados de c
        public bool alterar(Contato c)
        {
            if (c == null) return false;
            int pos = agenda.IndexOf(c);
            if (pos < 0) return false;
            agenda[pos] = c;
            return true;
        }

        public bool remover(Contato c)
        {
            if (c == null) return false;
            return agenda.Remove(c);
        }
    }

    // ======================== VIEW ========================

    public class AgendaView
    {
        public int menu()
        {
            Console.WriteLine();
            Console.WriteLine("===== AGENDA =====");
            Console.WriteLine("1 - Adicionar contato");
            Console.WriteLine("2 - Pesquisar contato");
            Console.WriteLine("3 - Alterar contato");
            Console.WriteLine("4 - Remover contato");
            Console.WriteLine("5 - Listar contatos");
            Console.WriteLine("0 - Sair");
            return lerInt("Opcao: ");
        }

        public int lerInt(string mensagem)
        {
            while (true)
            {
                Console.Write(mensagem);
                int valor;
                if (int.TryParse(Console.ReadLine(), out valor)) return valor;
                Console.WriteLine("Valor invalido. Digite um numero inteiro.");
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

        // Retorna "" se o usuario apenas apertar Enter
        public string lerTextoOpcional(string mensagem)
        {
            Console.Write(mensagem);
            return (Console.ReadLine() ?? "").Trim();
        }

        public bool lerSimNao(string mensagem)
        {
            while (true)
            {
                Console.Write(mensagem + " (s/n): ");
                string r = (Console.ReadLine() ?? "").Trim().ToLower();
                if (r == "s") return true;
                if (r == "n") return false;
                Console.WriteLine("Digite s ou n.");
            }
        }

        // Le dd/mm/aaaa ate ficar valido
        public Data lerData(string mensagem)
        {
            while (true)
            {
                Console.Write(mensagem + " (dd/mm/aaaa): ");
                Data d = interpretarData(Console.ReadLine());
                if (d != null) return d;
                Console.WriteLine("Data invalida.");
            }
        }

        public Data interpretarData(string txt)
        {
            if (txt == null) return null;
            string[] partes = txt.Trim().Split('/');
            if (partes.Length != 3) return null;
            int dia, mes, ano;
            if (!int.TryParse(partes[0], out dia) ||
                !int.TryParse(partes[1], out mes) ||
                !int.TryParse(partes[2], out ano)) return null;
            if (!Data.ehValida(dia, mes, ano)) return null;
            return new Data(dia, mes, ano);
        }

        public Telefone lerTelefone(bool primeiro)
        {
            string tipo = lerTexto("Tipo (celular, residencial, trabalho...): ");
            string numero = lerTexto("Numero: ");
            bool principal = primeiro ? true : lerSimNao("E o telefone principal?");
            return new Telefone(tipo, numero, principal);
        }

        public void mensagem(string texto)
        {
            Console.WriteLine(texto);
        }

        public void mostrarContato(Contato c)
        {
            Console.WriteLine();
            Console.WriteLine(c);
            Console.WriteLine("Telefones:");
            if (c.Telefones.Count == 0)
            {
                Console.WriteLine("  (nenhum)");
            }
            else
            {
                foreach (Telefone t in c.Telefones)
                    Console.WriteLine("  - " + t);
            }
        }

        public void listarContatos(List<Contato> contatos)
        {
            Console.WriteLine();
            Console.WriteLine("===== CONTATOS (" + contatos.Count + ") =====");
            if (contatos.Count == 0)
            {
                Console.WriteLine("Agenda vazia.");
                return;
            }
            foreach (Contato c in contatos)
                Console.WriteLine(c);
        }
    }

    // ===================== CONTROLLER =====================

    public class AgendaController
    {
        private Contatos contatos = new Contatos();
        private AgendaView view = new AgendaView();

        public void executar()
        {
            int opcao;
            do
            {
                opcao = view.menu();
                switch (opcao)
                {
                    case 1: adicionar(); break;
                    case 2: pesquisar(); break;
                    case 3: alterar(); break;
                    case 4: remover(); break;
                    case 5: view.listarContatos(contatos.Agenda); break;
                    case 0: view.mensagem("Encerrando..."); break;
                    default: view.mensagem("Opcao invalida."); break;
                }
            } while (opcao != 0);
        }

        // Contato "modelo" so com o nome, usado nas buscas (Equals compara pelo nome)
        private Contato modeloDeBusca(string nome)
        {
            return new Contato(nome, "", new Data(1, 1, 2000));
        }

        private Contato localizar()
        {
            string nome = view.lerTexto("Nome do contato: ");
            Contato encontrado = contatos.pesquisar(modeloDeBusca(nome));
            if (encontrado == null) view.mensagem("Contato nao encontrado.");
            return encontrado;
        }

        private void lerTelefones(Contato c)
        {
            bool outro = view.lerSimNao("Deseja cadastrar um telefone?");
            while (outro)
            {
                c.adicionarTelefone(view.lerTelefone(c.Telefones.Count == 0));
                outro = view.lerSimNao("Cadastrar outro telefone?");
            }
        }

        private void adicionar()
        {
            string nome = view.lerTexto("Nome: ");
            if (contatos.pesquisar(modeloDeBusca(nome)) != null)
            {
                view.mensagem("Ja existe um contato com esse nome.");
                return;
            }
            string email = view.lerTexto("E-mail: ");
            Data nasc = view.lerData("Data de nascimento");

            Contato c = new Contato(nome, email, nasc);
            lerTelefones(c);

            if (contatos.adicionar(c))
                view.mensagem("Contato adicionado.");
            else
                view.mensagem("Nao foi possivel adicionar o contato.");
        }

        private void pesquisar()
        {
            Contato c = localizar();
            if (c != null) view.mostrarContato(c);
        }

        private void alterar()
        {
            Contato atual = localizar();
            if (atual == null) return;

            view.mostrarContato(atual);
            view.mensagem("(Enter mantem o valor atual)");

            string email = view.lerTextoOpcional("Novo e-mail: ");
            Data nasc = atual.DtNasc;
            while (true)
            {
                string txt = view.lerTextoOpcional("Nova data de nascimento (dd/mm/aaaa): ");
                if (txt == "") break;
                Data d = view.interpretarData(txt);
                if (d != null) { nasc = d; break; }
                view.mensagem("Data invalida.");
            }

            Contato novo = new Contato(atual.Nome, email == "" ? atual.Email : email, nasc);

            if (view.lerSimNao("Deseja redefinir os telefones?"))
            {
                lerTelefones(novo);
            }
            else
            {
                foreach (Telefone t in atual.Telefones)
                    novo.Telefones.Add(t);
            }

            if (contatos.alterar(novo))
                view.mensagem("Contato alterado.");
            else
                view.mensagem("Nao foi possivel alterar o contato.");
        }

        private void remover()
        {
            Contato c = localizar();
            if (c == null) return;

            if (view.lerSimNao("Remover " + c.Nome + "?"))
            {
                if (contatos.remover(c))
                    view.mensagem("Contato removido.");
                else
                    view.mensagem("Nao foi possivel remover o contato.");
            }
        }
    }

    // ======================== MAIN ========================

    class Program
    {
        static void Main(string[] args)
        {
            AgendaController controller = new AgendaController();
            controller.executar();
        }
    }
}
