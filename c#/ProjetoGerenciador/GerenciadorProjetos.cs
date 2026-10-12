using System;
using System.Collections.Generic;
using System.Globalization;

// ============================================================
// Projeto Gerenciador de Projetos - Listas / Console (MVC)
// ============================================================

namespace GerenciadorProjetos
{
    // ======================== MODEL ========================

    public class Tarefa
    {
        public const string Aberta = "Aberta";
        public const string Fechada = "Fechada";
        public const string Cancelada = "Cancelada";
        public static readonly string[] Statuses = { Aberta, Fechada, Cancelada };

        private int id;
        private string titulo;
        private string descricao;
        private int prioridade;      // 1=alta, 2=media, 3=baixa
        private string status;
        private DateTime dataCriacao;
        private DateTime dataConclusao; // DateTime.MinValue = sem conclusao

        public Tarefa(int id, string titulo, string descricao, int prioridade)
        {
            this.id = id;
            this.titulo = titulo;
            this.descricao = descricao;
            this.prioridade = prioridade;
            this.status = Aberta;
            this.dataCriacao = DateTime.Now;
            this.dataConclusao = DateTime.MinValue;
        }

        public int Id { get { return id; } set { id = value; } }
        public string Titulo { get { return titulo; } set { titulo = value; } }
        public string Descricao { get { return descricao; } set { descricao = value; } }
        public int Prioridade { get { return prioridade; } set { prioridade = value; } }
        public string Status { get { return status; } set { status = value; } }
        public DateTime DataCriacao { get { return dataCriacao; } set { dataCriacao = value; } }
        public DateTime DataConclusao { get { return dataConclusao; } set { dataConclusao = value; } }

        // Aberta -> Fechada
        public void concluir()
        {
            if (status != Aberta) return;
            status = Fechada;
            dataConclusao = DateTime.Now;
        }

        // Aberta -> Cancelada
        public void cancelar()
        {
            if (status != Aberta) return;
            status = Cancelada;
        }

        // Fechada/Cancelada -> Aberta
        public void reabrir()
        {
            if (status == Aberta) return;
            status = Aberta;
            dataConclusao = DateTime.MinValue;
        }

        public string nomePrioridade()
        {
            switch (prioridade)
            {
                case 1: return "Alta";
                case 2: return "Media";
                case 3: return "Baixa";
                default: return "?";
            }
        }

        // Duas tarefas sao iguais quando possuem o mesmo id
        public override bool Equals(object obj)
        {
            Tarefa outra = obj as Tarefa;
            if (outra == null) return false;
            return id == outra.id;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }

    public class Projeto
    {
        private int id;
        private string nome;
        private List<Tarefa> tarefas = new List<Tarefa>();
        private int proxIdTarefa = 1;

        public Projeto(int id, string nome)
        {
            this.id = id;
            this.nome = nome;
        }

        public int Id { get { return id; } set { id = value; } }
        public string Nome { get { return nome; } set { nome = value; } }
        public List<Tarefa> Tarefas { get { return tarefas; } }

        // Extra: gera o proximo id de tarefa do projeto (nao reaproveita ids)
        public int novoIdTarefa()
        {
            return proxIdTarefa++;
        }

        public void adicionarTarefa(Tarefa t)
        {
            if (t != null) tarefas.Add(t);
        }

        public bool removerTarefa(Tarefa t)
        {
            if (t == null) return false;
            return tarefas.Remove(t);
        }

        public Tarefa buscarTarefa(Tarefa t)
        {
            if (t == null) return null;
            int pos = tarefas.IndexOf(t);
            return pos >= 0 ? tarefas[pos] : null;
        }

        public List<Tarefa> tarefasPorStatus(string s)
        {
            List<Tarefa> resultado = new List<Tarefa>();
            foreach (Tarefa t in tarefas)
                if (string.Equals(t.Status, s, StringComparison.OrdinalIgnoreCase))
                    resultado.Add(t);
            return resultado;
        }

        public List<Tarefa> tarefasPorPrioridade(int p)
        {
            List<Tarefa> resultado = new List<Tarefa>();
            foreach (Tarefa t in tarefas)
                if (t.Prioridade == p) resultado.Add(t);
            return resultado;
        }

        public int totalAbertas()
        {
            return tarefasPorStatus(Tarefa.Aberta).Count;
        }

        public int totalFechadas()
        {
            return tarefasPorStatus(Tarefa.Fechada).Count;
        }

        // Extra: usado no resumo
        public int totalCanceladas()
        {
            return tarefasPorStatus(Tarefa.Cancelada).Count;
        }

        // Dois projetos sao iguais quando possuem o mesmo id
        public override bool Equals(object obj)
        {
            Projeto outro = obj as Projeto;
            if (outro == null) return false;
            return id == outro.id;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }

    public class Projetos
    {
        private List<Projeto> itens = new List<Projeto>();

        public bool adicionar(Projeto p)
        {
            if (p == null || itens.Contains(p)) return false;
            itens.Add(p);
            return true;
        }

        // So remove projeto sem tarefas
        public bool remover(Projeto p)
        {
            if (p == null) return false;
            Projeto existente = buscar(p);
            if (existente == null || existente.Tarefas.Count > 0) return false;
            return itens.Remove(existente);
        }

        public Projeto buscar(Projeto p)
        {
            if (p == null) return null;
            int pos = itens.IndexOf(p);
            return pos >= 0 ? itens[pos] : null;
        }

        public List<Projeto> listar()
        {
            return itens;
        }
    }

    // ======================== VIEW ========================

    public class GerenciadorView
    {
        private static readonly CultureInfo br = new CultureInfo("pt-BR");

        public int menu()
        {
            Console.WriteLine();
            Console.WriteLine("===== GERENCIADOR DE PROJETOS =====");
            Console.WriteLine(" 1 - Adicionar projeto");
            Console.WriteLine(" 2 - Pesquisar projeto (tarefas por status e totais)");
            Console.WriteLine(" 3 - Remover projeto (apenas se sem tarefas)");
            Console.WriteLine(" 4 - Adicionar tarefa em projeto");
            Console.WriteLine(" 5 - Concluir tarefa");
            Console.WriteLine(" 6 - Cancelar tarefa");
            Console.WriteLine(" 7 - Reabrir tarefa");
            Console.WriteLine(" 8 - Listar tarefas de um projeto");
            Console.WriteLine(" 9 - Filtrar tarefas (status/prioridade) em um projeto");
            Console.WriteLine("10 - Filtrar tarefas (status/prioridade) em todos os projetos");
            Console.WriteLine("11 - Resumo geral");
            Console.WriteLine(" 0 - Sair");
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

        public int lerFaixa(string mensagem, int min, int max)
        {
            while (true)
            {
                int valor = lerInt(mensagem);
                if (valor >= min && valor <= max) return valor;
                Console.WriteLine("Digite um valor entre " + min + " e " + max + ".");
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

        public void mostrarProjetos(List<Projeto> projetos)
        {
            Console.WriteLine();
            Console.WriteLine("Projetos cadastrados:");
            foreach (Projeto p in projetos)
                Console.WriteLine("  [" + p.Id + "] " + p.Nome + " (" + p.Tarefas.Count + " tarefa(s))");
        }

        public void mostrarTarefa(Tarefa t)
        {
            string linha = "  #" + t.Id + " [" + t.nomePrioridade() + "] " + t.Titulo +
                           " - " + t.Status + " | Criada: " + data(t.DataCriacao);
            if (t.DataConclusao != DateTime.MinValue)
                linha += " | Concluida: " + data(t.DataConclusao);
            Console.WriteLine(linha);
            Console.WriteLine("      " + t.Descricao);
        }

        public void mostrarTarefas(List<Tarefa> tarefas)
        {
            if (tarefas.Count == 0)
            {
                Console.WriteLine("  (nenhuma tarefa)");
                return;
            }
            foreach (Tarefa t in tarefas)
                mostrarTarefa(t);
        }

        // Opcao 2: tarefas agrupadas por status + totais
        public void mostrarProjetoPorStatus(Projeto p)
        {
            Console.WriteLine();
            Console.WriteLine("Projeto [" + p.Id + "] " + p.Nome);
            Console.WriteLine("Totais: " + p.totalAbertas() + " aberta(s) | " +
                              p.totalFechadas() + " fechada(s) | " +
                              p.totalCanceladas() + " cancelada(s) | " +
                              p.Tarefas.Count + " no total");
            foreach (string status in Tarefa.Statuses)
            {
                List<Tarefa> lista = p.tarefasPorStatus(status);
                Console.WriteLine();
                Console.WriteLine("-- " + status + "s (" + lista.Count + ") --");
                mostrarTarefas(lista);
            }
        }

        public void mostrarTarefasDoProjeto(Projeto p, List<Tarefa> tarefas)
        {
            Console.WriteLine();
            Console.WriteLine("Projeto [" + p.Id + "] " + p.Nome + " - " + tarefas.Count + " tarefa(s)");
            mostrarTarefas(tarefas);
        }

        public void mostrarResumo(int qtdeProjetos, int abertas, int fechadas, int canceladas)
        {
            Console.WriteLine();
            Console.WriteLine("===== RESUMO GERAL =====");
            Console.WriteLine("Projetos: " + qtdeProjetos);
            Console.WriteLine("Tarefas abertas: " + abertas);
            Console.WriteLine("Tarefas fechadas: " + fechadas);
            Console.WriteLine("Tarefas canceladas: " + canceladas);

            if (abertas > 0)
            {
                double perc = (double)fechadas / abertas * 100;
                Console.WriteLine("Concluidas em relacao as abertas: " + perc.ToString("F2", br) + "%");
            }
            else if (fechadas > 0)
            {
                Console.WriteLine("Concluidas em relacao as abertas: nao ha tarefas abertas");
            }
            else
            {
                Console.WriteLine("Concluidas em relacao as abertas: 0,00%");
            }
        }
    }

    // ===================== CONTROLLER =====================

    public class GerenciadorController
    {
        private Projetos projetos = new Projetos();
        private GerenciadorView view = new GerenciadorView();
        private int proxIdProjeto = 1;

        public void executar()
        {
            int opcao;
            do
            {
                opcao = view.menu();
                switch (opcao)
                {
                    case 1: adicionarProjeto(); break;
                    case 2: pesquisarProjeto(); break;
                    case 3: removerProjeto(); break;
                    case 4: adicionarTarefa(); break;
                    case 5: concluirTarefa(); break;
                    case 6: cancelarTarefa(); break;
                    case 7: reabrirTarefa(); break;
                    case 8: listarTarefas(); break;
                    case 9: filtrarEmProjeto(); break;
                    case 10: filtrarEmTodos(); break;
                    case 11: resumoGeral(); break;
                    case 0: view.mensagem("Encerrando..."); break;
                    default: view.mensagem("Opcao invalida."); break;
                }
            } while (opcao != 0);
        }

        // Mostra os projetos e pede o id; Projeto "modelo" so com o id (Equals compara pelo id)
        private Projeto localizarProjeto()
        {
            List<Projeto> lista = projetos.listar();
            if (lista.Count == 0)
            {
                view.mensagem("Nenhum projeto cadastrado.");
                return null;
            }
            view.mostrarProjetos(lista);
            int id = view.lerInt("Id do projeto: ");
            Projeto achado = projetos.buscar(new Projeto(id, ""));
            if (achado == null) view.mensagem("Projeto nao encontrado.");
            return achado;
        }

        private Tarefa localizarTarefa(Projeto p)
        {
            if (p.Tarefas.Count == 0)
            {
                view.mensagem("O projeto nao possui tarefas.");
                return null;
            }
            view.mostrarTarefas(p.Tarefas);
            int id = view.lerInt("Id da tarefa: ");
            Tarefa achada = p.buscarTarefa(new Tarefa(id, "", "", 3));
            if (achada == null) view.mensagem("Tarefa nao encontrada.");
            return achada;
        }

        private void adicionarProjeto()
        {
            string nome = view.lerTexto("Nome do projeto: ");
            Projeto p = new Projeto(proxIdProjeto, nome);
            if (projetos.adicionar(p))
            {
                proxIdProjeto++;
                view.mensagem("Projeto adicionado. Id: " + p.Id);
            }
            else
            {
                view.mensagem("Nao foi possivel adicionar o projeto.");
            }
        }

        private void pesquisarProjeto()
        {
            Projeto p = localizarProjeto();
            if (p != null) view.mostrarProjetoPorStatus(p);
        }

        private void removerProjeto()
        {
            Projeto p = localizarProjeto();
            if (p == null) return;

            if (p.Tarefas.Count > 0)
            {
                view.mensagem("Nao e possivel remover: o projeto possui " + p.Tarefas.Count + " tarefa(s).");
                return;
            }

            if (projetos.remover(p))
                view.mensagem("Projeto removido.");
            else
                view.mensagem("Nao foi possivel remover o projeto.");
        }

        private void adicionarTarefa()
        {
            Projeto p = localizarProjeto();
            if (p == null) return;

            string titulo = view.lerTexto("Titulo: ");
            string descricao = view.lerTexto("Descricao: ");
            int prioridade = view.lerFaixa("Prioridade (1=alta, 2=media, 3=baixa): ", 1, 3);

            Tarefa t = new Tarefa(p.novoIdTarefa(), titulo, descricao, prioridade);
            p.adicionarTarefa(t);
            view.mensagem("Tarefa adicionada. Id: " + t.Id);
        }

        private void concluirTarefa()
        {
            Projeto p = localizarProjeto();
            if (p == null) return;
            Tarefa t = localizarTarefa(p);
            if (t == null) return;

            if (t.Status != Tarefa.Aberta)
            {
                view.mensagem("Apenas tarefas abertas podem ser concluidas (status atual: " + t.Status + ").");
                return;
            }
            t.concluir();
            view.mensagem("Tarefa concluida.");
        }

        private void cancelarTarefa()
        {
            Projeto p = localizarProjeto();
            if (p == null) return;
            Tarefa t = localizarTarefa(p);
            if (t == null) return;

            if (t.Status != Tarefa.Aberta)
            {
                view.mensagem("Apenas tarefas abertas podem ser canceladas (status atual: " + t.Status + ").");
                return;
            }
            t.cancelar();
            view.mensagem("Tarefa cancelada.");
        }

        private void reabrirTarefa()
        {
            Projeto p = localizarProjeto();
            if (p == null) return;
            Tarefa t = localizarTarefa(p);
            if (t == null) return;

            if (t.Status == Tarefa.Aberta)
            {
                view.mensagem("A tarefa ja esta aberta.");
                return;
            }
            t.reabrir();
            view.mensagem("Tarefa reaberta.");
        }

        private void listarTarefas()
        {
            Projeto p = localizarProjeto();
            if (p != null) view.mostrarTarefasDoProjeto(p, p.Tarefas);
        }

        // Le o criterio (status ou prioridade) e devolve a funcao de filtro
        private Func<Projeto, List<Tarefa>> lerFiltro()
        {
            int tipo = view.lerFaixa("Filtrar por: 1 - Status | 2 - Prioridade: ", 1, 2);
            if (tipo == 1)
            {
                int s = view.lerFaixa("Status: 1 - Aberta | 2 - Fechada | 3 - Cancelada: ", 1, 3);
                string status = Tarefa.Statuses[s - 1];
                return delegate (Projeto p) { return p.tarefasPorStatus(status); };
            }
            else
            {
                int prio = view.lerFaixa("Prioridade (1=alta, 2=media, 3=baixa): ", 1, 3);
                return delegate (Projeto p) { return p.tarefasPorPrioridade(prio); };
            }
        }

        private void filtrarEmProjeto()
        {
            Projeto p = localizarProjeto();
            if (p == null) return;

            Func<Projeto, List<Tarefa>> filtro = lerFiltro();
            view.mostrarTarefasDoProjeto(p, filtro(p));
        }

        private void filtrarEmTodos()
        {
            List<Projeto> lista = projetos.listar();
            if (lista.Count == 0)
            {
                view.mensagem("Nenhum projeto cadastrado.");
                return;
            }

            Func<Projeto, List<Tarefa>> filtro = lerFiltro();
            int total = 0;
            foreach (Projeto p in lista)
            {
                List<Tarefa> resultado = filtro(p);
                total += resultado.Count;
                view.mostrarTarefasDoProjeto(p, resultado);
            }
            Console.WriteLine();
            view.mensagem("Total de tarefas encontradas: " + total);
        }

        private void resumoGeral()
        {
            int abertas = 0, fechadas = 0, canceladas = 0;
            foreach (Projeto p in projetos.listar())
            {
                abertas += p.totalAbertas();
                fechadas += p.totalFechadas();
                canceladas += p.totalCanceladas();
            }
            view.mostrarResumo(projetos.listar().Count, abertas, fechadas, canceladas);
        }
    }

    // ======================== MAIN ========================

    class Program
    {
        static void Main(string[] args)
        {
            GerenciadorController controller = new GerenciadorController();
            controller.executar();
        }
    }
}
