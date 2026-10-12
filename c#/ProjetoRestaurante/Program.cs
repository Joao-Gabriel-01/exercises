using System;
using System.Globalization;
using System.Text;

// ============================================================
// Projeto Restaurante - POO / MVC Console Application
// ============================================================

namespace ProjetoRestaurante
{
    // ======================== MODEL ========================

    public class Item
    {
        private int id;
        private string descricao;
        private double preco;

        public Item(int id, string descricao, double preco)
        {
            this.id = id;
            this.descricao = descricao;
            this.preco = preco;
        }

        public int Id { get { return id; } set { id = value; } }
        public string Descricao { get { return descricao; } set { descricao = value; } }
        public double Preco { get { return preco; } set { preco = value; } }
    }

    public class Pedido
    {
        public const int MaxItens = 10;

        private int id;
        private string cliente;
        private Item[] itens = new Item[MaxItens];

        public Pedido(string cliente)
        {
            this.cliente = cliente;
        }

        public int Id { get { return id; } set { id = value; } }
        public string Cliente { get { return cliente; } set { cliente = value; } }

        public bool adicionarItem(Item item)
        {
            if (item == null) return false;
            for (int i = 0; i < itens.Length; i++)
            {
                if (itens[i] == null)
                {
                    itens[i] = item;
                    return true;
                }
            }
            return false; // pedido cheio (10 itens)
        }

        public bool removerItem(Item item)
        {
            if (item == null) return false;
            for (int i = 0; i < itens.Length; i++)
            {
                if (itens[i] != null && itens[i].Id == item.Id)
                {
                    itens[i] = null;
                    return true;
                }
            }
            return false;
        }

        public int quantidadeItens()
        {
            int qtd = 0;
            for (int i = 0; i < itens.Length; i++)
                if (itens[i] != null) qtd++;
            return qtd;
        }

        public double calcularTotal()
        {
            double total = 0;
            for (int i = 0; i < itens.Length; i++)
                if (itens[i] != null) total += itens[i].Preco;
            return total;
        }

        public string dadosDoPedido()
        {
            CultureInfo br = new CultureInfo("pt-BR");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Pedido #" + id);
            sb.AppendLine("Cliente: " + cliente);
            sb.AppendLine("Itens:");
            if (quantidadeItens() == 0)
            {
                sb.AppendLine("  (nenhum item)");
            }
            else
            {
                for (int i = 0; i < itens.Length; i++)
                {
                    if (itens[i] != null)
                        sb.AppendLine("  [" + itens[i].Id + "] " + itens[i].Descricao +
                                      " - " + itens[i].Preco.ToString("C", br));
                }
            }
            sb.Append("Total: " + calcularTotal().ToString("C", br));
            return sb.ToString();
        }
    }

    public class Restaurante
    {
        public const int MaxPedidos = 50;

        private int proxPedido = 1;
        private Pedido[] pedidos = new Pedido[MaxPedidos];

        public Pedido[] Pedidos { get { return pedidos; } }

        public bool novoPedido(Pedido pedido)
        {
            if (pedido == null) return false;
            for (int i = 0; i < pedidos.Length; i++)
            {
                if (pedidos[i] == null)
                {
                    pedido.Id = proxPedido++;
                    pedidos[i] = pedido;
                    return true;
                }
            }
            return false; // limite diario de 50 pedidos
        }

        // Busca pelo id do pedido informado
        public Pedido buscarPedido(Pedido pedido)
        {
            if (pedido == null) return null;
            for (int i = 0; i < pedidos.Length; i++)
            {
                if (pedidos[i] != null && pedidos[i].Id == pedido.Id)
                    return pedidos[i];
            }
            return null;
        }

        public bool cancelarPedido(Pedido pedido)
        {
            if (pedido == null) return false;
            for (int i = 0; i < pedidos.Length; i++)
            {
                if (pedidos[i] != null && pedidos[i].Id == pedido.Id)
                {
                    pedidos[i] = null; // o id nao e reaproveitado
                    return true;
                }
            }
            return false;
        }

        public double somaGeralDoDia()
        {
            double soma = 0;
            for (int i = 0; i < pedidos.Length; i++)
                if (pedidos[i] != null) soma += pedidos[i].calcularTotal();
            return soma;
        }
    }

    // ======================== VIEW ========================

    public class RestauranteView
    {
        private static readonly CultureInfo br = new CultureInfo("pt-BR");

        public int menu()
        {
            Console.WriteLine();
            Console.WriteLine("===== RESTAURANTE =====");
            Console.WriteLine("1 - Criar novo pedido");
            Console.WriteLine("2 - Adicionar item ao pedido");
            Console.WriteLine("3 - Remover item do pedido");
            Console.WriteLine("4 - Consultar pedido");
            Console.WriteLine("5 - Cancelar pedido");
            Console.WriteLine("6 - Listar todos os pedidos");
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

        public double lerDouble(string mensagem)
        {
            while (true)
            {
                Console.Write(mensagem);
                string txt = (Console.ReadLine() ?? "").Replace(',', '.');
                double valor;
                if (double.TryParse(txt, NumberStyles.Float, CultureInfo.InvariantCulture, out valor)
                    && valor >= 0)
                    return valor;
                Console.WriteLine("Valor invalido. Digite um numero positivo.");
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

        public void mostrarPedido(Pedido pedido)
        {
            Console.WriteLine();
            Console.WriteLine(pedido.dadosDoPedido());
        }

        public void listarPedidos(Pedido[] pedidos, double somaGeral)
        {
            Console.WriteLine();
            Console.WriteLine("===== PEDIDOS DO DIA =====");
            bool algum = false;
            for (int i = 0; i < pedidos.Length; i++)
            {
                if (pedidos[i] != null)
                {
                    algum = true;
                    Console.WriteLine("Pedido #" + pedidos[i].Id + " | " + pedidos[i].Cliente +
                                      " | Itens: " + pedidos[i].quantidadeItens() +
                                      " | Total: " + pedidos[i].calcularTotal().ToString("C", br));
                }
            }
            if (!algum) Console.WriteLine("Nenhum pedido registrado.");
            Console.WriteLine("--------------------------");
            Console.WriteLine("SOMA GERAL DO DIA: " + somaGeral.ToString("C", br));
        }
    }

    // ===================== CONTROLLER =====================

    public class RestauranteController
    {
        private Restaurante restaurante = new Restaurante();
        private RestauranteView view = new RestauranteView();

        public void executar()
        {
            int opcao;
            do
            {
                opcao = view.menu();
                switch (opcao)
                {
                    case 1: criarPedido(); break;
                    case 2: adicionarItem(); break;
                    case 3: removerItem(); break;
                    case 4: consultarPedido(); break;
                    case 5: cancelarPedido(); break;
                    case 6: listarPedidos(); break;
                    case 0: view.mensagem("Encerrando..."); break;
                    default: view.mensagem("Opcao invalida."); break;
                }
            } while (opcao != 0);
        }

        // Cria um Pedido "modelo" apenas com o id para usar nas buscas
        private Pedido localizar()
        {
            int id = view.lerInt("Numero do pedido: ");
            Pedido busca = new Pedido("");
            busca.Id = id;
            Pedido encontrado = restaurante.buscarPedido(busca);
            if (encontrado == null) view.mensagem("Pedido nao encontrado.");
            return encontrado;
        }

        private void criarPedido()
        {
            string cliente = view.lerTexto("Nome do cliente: ");
            Pedido pedido = new Pedido(cliente);
            if (restaurante.novoPedido(pedido))
                view.mensagem("Pedido criado com sucesso! Numero: " + pedido.Id);
            else
                view.mensagem("Limite de " + Restaurante.MaxPedidos + " pedidos do dia atingido.");
        }

        private void adicionarItem()
        {
            Pedido pedido = localizar();
            if (pedido == null) return;

            if (pedido.quantidadeItens() >= Pedido.MaxItens)
            {
                view.mensagem("O pedido ja possui " + Pedido.MaxItens + " itens.");
                return;
            }

            int idItem = view.lerInt("Id do item: ");
            string descricao = view.lerTexto("Descricao: ");
            double preco = view.lerDouble("Preco (R$): ");

            if (pedido.adicionarItem(new Item(idItem, descricao, preco)))
                view.mensagem("Item adicionado.");
            else
                view.mensagem("Nao foi possivel adicionar o item.");
        }

        private void removerItem()
        {
            Pedido pedido = localizar();
            if (pedido == null) return;

            int idItem = view.lerInt("Id do item a remover: ");
            if (pedido.removerItem(new Item(idItem, "", 0)))
                view.mensagem("Item removido.");
            else
                view.mensagem("Item nao encontrado no pedido.");
        }

        private void consultarPedido()
        {
            Pedido pedido = localizar();
            if (pedido != null) view.mostrarPedido(pedido);
        }

        private void cancelarPedido()
        {
            Pedido pedido = localizar();
            if (pedido == null) return;

            if (restaurante.cancelarPedido(pedido))
                view.mensagem("Pedido cancelado.");
            else
                view.mensagem("Nao foi possivel cancelar o pedido.");
        }

        private void listarPedidos()
        {
            view.listarPedidos(restaurante.Pedidos, restaurante.somaGeralDoDia());
        }
    }

    // ======================== MAIN ========================

    class Program
    {
        static void Main(string[] args)
        {
            RestauranteController controller = new RestauranteController();
            controller.executar();
        }
    }
}
