using ProjetoCursos.Controllers;
using ProjetoCursos.Models;
using ProjetoCursos.Views;

namespace ProjetoCursos
{
    internal class Program
    {
        static void Main()
        {
            Escola escola = new Escola();
            ConsoleView view = new ConsoleView();
            EscolaController controller = new EscolaController(escola, view);

            controller.Executar();
        }
    }
}
