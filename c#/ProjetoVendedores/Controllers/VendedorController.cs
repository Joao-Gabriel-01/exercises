using System;
using ProjetoVendedores.Models;
using ProjetoVendedores.Views;

namespace ProjetoVendedores.Controllers
{
    /// <summary>
    /// Camada de controle (Controller): interpreta a opção escolhida pelo
    /// usuário, solicita os dados necessários à View e aciona o Model
    /// (Vendedores/Vendedor/Venda) para executar cada operação.
    /// </summary>
    public class VendedorController
    {
        private readonly Vendedores vendedores;
        private readonly ConsoleView view;

        public VendedorController(Vendedores vendedores, ConsoleView view)
        {
            this.vendedores = vendedores;
            this.view = view;
        }

        public void Executar()
        {
            int opcao;

            do
            {
                opcao = view.ExibirMenuEObterOpcao();

                switch (opcao)
                {
                    case 0:
                        view.ExibirMensagem("Encerrando aplicação...");
                        break;
                    case 1:
                        CadastrarVendedor();
                        break;
                    case 2:
                        ConsultarVendedor();
                        break;
                    case 3:
                        ExcluirVendedor();
                        break;
                    case 4:
                        RegistrarVenda();
                        break;
                    case 5:
                        ListarVendedores();
                        break;
                    default:
                        view.ExibirErro("Opção inválida.");
                        break;
                }

                if (opcao != 0)
                {
                    view.AguardarTecla();
                }

            } while (opcao != 0);
        }

        private void CadastrarVendedor()
        {
            view.ExibirTitulo("Cadastrar vendedor");

            if (vendedores.Qtde >= vendedores.Max)
            {
                view.ExibirErro($"Limite máximo de {vendedores.Max} vendedores já foi atingido.");
                return;
            }

            int id = view.LerInteiro("ID do vendedor: ");
            string nome = view.LerTexto("Nome do vendedor: ");
            double percComissao = view.LerDouble("Percentual de comissão (ex.: 5 para 5%): ");

            Vendedor vendedor = new Vendedor(id, nome, percComissao);
            bool adicionado = vendedores.addVendedor(vendedor);

            if (adicionado)
            {
                view.ExibirSucesso($"Vendedor '{nome}' cadastrado com sucesso.");
            }
            else
            {
                view.ExibirErro("Não foi possível cadastrar: limite de vendedores atingido ou ID já utilizado.");
            }
        }

        private void ConsultarVendedor()
        {
            view.ExibirTitulo("Consultar vendedor");

            int id = view.LerInteiro("ID do vendedor: ");
            Vendedor encontrado = vendedores.searchVendedor(new Vendedor(id, string.Empty, 0));

            if (encontrado == null)
            {
                view.ExibirErro("Vendedor não encontrado.");
                return;
            }

            view.ExibirMensagem($"ID: {encontrado.Id}");
            view.ExibirMensagem($"Nome: {encontrado.Nome}");
            view.ExibirMensagem($"Valor total das vendas: {view.FormatarMoeda(encontrado.valorVendas())}");
            view.ExibirMensagem($"Valor da comissão devida: {view.FormatarMoeda(encontrado.valorComissao())}");
            view.ExibirMensagem($"Valor médio das vendas diárias: {view.FormatarMoeda(encontrado.valorMedioVendasDiarias())}");
        }

        private void ExcluirVendedor()
        {
            view.ExibirTitulo("Excluir vendedor");

            int id = view.LerInteiro("ID do vendedor: ");
            Vendedor probe = new Vendedor(id, string.Empty, 0);

            Vendedor encontrado = vendedores.searchVendedor(probe);
            if (encontrado == null)
            {
                view.ExibirErro("Vendedor não encontrado.");
                return;
            }

            bool excluido = vendedores.delVendedor(probe);

            if (excluido)
            {
                view.ExibirSucesso($"Vendedor '{encontrado.Nome}' excluído com sucesso.");
            }
            else
            {
                view.ExibirErro("Não é possível excluir: o vendedor possui vendas associadas.");
            }
        }

        private void RegistrarVenda()
        {
            view.ExibirTitulo("Registrar venda");

            int id = view.LerInteiro("ID do vendedor: ");
            Vendedor encontrado = vendedores.searchVendedor(new Vendedor(id, string.Empty, 0));

            if (encontrado == null)
            {
                view.ExibirErro("Vendedor não encontrado.");
                return;
            }

            int dia = view.LerInteiro("Dia do mês (1 a 31): ");
            int qtdeVendas = view.LerInteiro("Quantidade de vendas no dia: ");
            double valorVendas = view.LerDouble("Valor total das vendas no dia: ");

            try
            {
                Venda venda = new Venda(qtdeVendas, valorVendas);
                encontrado.registrarVenda(dia, venda);
                view.ExibirSucesso("Venda registrada com sucesso.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                view.ExibirErro(ex.Message);
            }
        }

        private void ListarVendedores()
        {
            view.ExibirTitulo("Listar vendedores");

            if (vendedores.Qtde == 0)
            {
                view.ExibirMensagem("Nenhum vendedor cadastrado.");
                return;
            }

            double totalVendas = 0d;
            double totalComissao = 0d;

            for (int i = 0; i < vendedores.Qtde; i++)
            {
                Vendedor v = vendedores.ObterPorPosicao(i);

                double valorVendasVendedor = v.valorVendas();
                double valorComissaoVendedor = v.valorComissao();

                view.ExibirMensagem(
                    $"ID: {v.Id} | Nome: {v.Nome} | Vendas: {view.FormatarMoeda(valorVendasVendedor)} | Comissão: {view.FormatarMoeda(valorComissaoVendedor)}");

                totalVendas += valorVendasVendedor;
                totalComissao += valorComissaoVendedor;
            }

            view.ExibirMensagem("---------------------------------------");
            view.ExibirMensagem($"TOTAL DE VENDAS:    {view.FormatarMoeda(totalVendas)}");
            view.ExibirMensagem($"TOTAL DE COMISSÃO:  {view.FormatarMoeda(totalComissao)}");
        }
    }
}
