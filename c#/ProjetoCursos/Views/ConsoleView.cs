using System;

namespace ProjetoCursos.Views
{
    /// <summary>
    /// Camada de apresentação (View): concentra toda a interação com o
    /// console (leitura de dados e exibição de mensagens), mantendo o
    /// Controller e os Models livres de código de entrada/saída.
    /// </summary>
    public class ConsoleView
    {
        public void ExibirTitulo(string titulo)
        {
            Console.WriteLine();
            Console.WriteLine($"== {titulo} ==");
        }

        public int ExibirMenuEObterOpcao()
        {
            Console.WriteLine();
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("               PROJETO CURSOS");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("0. Sair");
            Console.WriteLine("1. Adicionar curso");
            Console.WriteLine("2. Pesquisar curso");
            Console.WriteLine("3. Remover curso");
            Console.WriteLine("4. Adicionar disciplina no curso");
            Console.WriteLine("5. Pesquisar disciplina");
            Console.WriteLine("6. Remover disciplina do curso");
            Console.WriteLine("7. Matricular aluno na disciplina");
            Console.WriteLine("8. Remover aluno da disciplina");
            Console.WriteLine("9. Pesquisar aluno");

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

        public void AguardarTecla()
        {
            Console.WriteLine();
            Console.Write("Pressione ENTER para continuar...");
            Console.ReadLine();
        }
    }
}
