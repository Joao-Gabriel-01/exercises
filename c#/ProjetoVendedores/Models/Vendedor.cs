using System;

namespace ProjetoVendedores.Models
{
    /// <summary>
    /// Representa um vendedor e o histórico de vendas dos 31 dias do mês.
    /// </summary>
    public class Vendedor
    {
        private const int DIAS_NO_MES = 31;

        private int id;
        private string nome;
        private double percComissao;
        private Venda[] asVendas;

        public Vendedor(int id, string nome, double percComissao)
        {
            this.id = id;
            this.nome = nome;
            this.percComissao = percComissao;
            asVendas = new Venda[DIAS_NO_MES];
        }

        public int Id => id;
        public string Nome => nome;
        public double PercComissao => percComissao;

        /// <summary>
        /// Registra (ou substitui) a venda de um determinado dia do mês.
        /// </summary>
        /// <param name="dia">Dia do mês, de 1 a 31.</param>
        public void registrarVenda(int dia, Venda venda)
        {
            if (dia < 1 || dia > DIAS_NO_MES)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(dia), $"O dia deve estar entre 1 e {DIAS_NO_MES}.");
            }

            asVendas[dia - 1] = venda;
        }

        /// <summary>
        /// Valor total de vendas do vendedor, somando todos os dias registrados.
        /// </summary>
        public double valorVendas()
        {
            double total = 0d;

            foreach (Venda venda in asVendas)
            {
                if (venda != null)
                {
                    total += venda.Valor;
                }
            }

            return total;
        }

        /// <summary>
        /// Valor da comissão devida ao vendedor sobre o total de vendas.
        /// </summary>
        public double valorComissao()
        {
            return valorVendas() * (percComissao / 100d);
        }

        /// <summary>
        /// Método auxiliar (não constava no diagrama original, mas é necessário
        /// para atender a consulta de vendedor): valor médio das vendas diárias,
        /// considerando apenas os dias em que houve registro de venda.
        /// </summary>
        public double valorMedioVendasDiarias()
        {
            double somaMedias = 0d;
            int diasComRegistro = 0;

            foreach (Venda venda in asVendas)
            {
                if (venda != null)
                {
                    somaMedias += venda.valorMedio();
                    diasComRegistro++;
                }
            }

            if (diasComRegistro == 0)
            {
                return 0d;
            }

            return somaMedias / diasComRegistro;
        }

        /// <summary>
        /// Método auxiliar necessário para a regra de exclusão: só é possível
        /// excluir um vendedor que não possua nenhuma venda associada.
        /// </summary>
        public bool possuiVendas()
        {
            foreach (Venda venda in asVendas)
            {
                if (venda != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
