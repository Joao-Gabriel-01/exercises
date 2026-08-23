using ProjetoVendedores.Controllers;
using ProjetoVendedores.Models;
using ProjetoVendedores.Views;

namespace ProjetoVendedores
{
    internal class Program
    {
        private const int MAX_VENDEDORES = 10;

        static void Main()
        {
            Vendedores vendedores = new Vendedores(MAX_VENDEDORES);
            ConsoleView view = new ConsoleView();
            VendedorController controller = new VendedorController(vendedores, view);

            controller.Executar();
        }
    }
}
