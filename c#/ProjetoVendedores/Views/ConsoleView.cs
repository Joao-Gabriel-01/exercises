using System;
using System.Globalization;

namespace ProjetoVendedores.Views
{
    /// <summary>
    /// Camada de apresentação (View): concentra toda a interação com o
    /// console (leitura de dados e exibição de mensagens), mantendo o
    /// Controller e os Models livres de código de entrada/saída.
    /// </summary>
    public class ConsoleView
    {
        private static readonly CultureInfo CulturaBR = CultureInfo.GetCultureInfo("pt-BR");

        public void ExibirTitulo(string titulo)
        {
            Console.WriteLine();
            Console.WriteLine($"== {titulo} ==");
        }

        public int ExibirMenuEObterOpcao()
        {
            Console.WriteLine();
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("          PROJETO VENDEDORES");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("0. Sair");
            Console.WriteLine("1. Cadastrar vendedor");
            Console.WriteLine("2. Consultar vendedor");
            Console.WriteLine("3. Excluir vendedor");
            Console.WriteLine("4. Registrar venda");
            Console.WriteLine("5. Listar vendedores");

            return LerInteiro("Escolha uma opção: ");
        }

        public string LerTexto(string mensagem)
        {
            Console.Write(mensagem);
            return Console.ReadLine() ?? string.Empty;
        }

        public int LerInteiro(string mensagem)
        {
            Console.Write(mensagem);

            while (!int.TryParse(Console.ReadLine(), out int valor))
            {
                Console.Write("Valor inválido, digite um número inteiro: ");
            }

            return valor;
        }

        public double LerDouble(string mensagem)
        {
            Console.Write(mensagem);

            while (!double.TryParse(
                Console.ReadLine()?.Replace(',', '.'),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out double valor))
            {
                Console.Write("Valor inválido, digite um número: ");
            }

            return valor;
        }

        public void ExibirMensagem(string mensagem)
        {
            Console.WriteLine(mensagem);
        }

        public void ExibirErro(string mensagem)
        {
            ConsoleColor corOriginal = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(mensagem);
            Console.ForegroundColor = corOriginal;
        }

        public void ExibirSucesso(string mensagem)
        {
            ConsoleColor corOriginal = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(mensagem);
            Console.ForegroundColor = corOriginal;
        }

        public string FormatarMoeda(double valor)
        {
            return valor.ToString("C2", CulturaBR);
        }

        public void AguardarTecla()
        {
            Console.WriteLine();
            Console.Write("Pressione ENTER para continuar...");
            Console.ReadLine();
        }
    }
}
