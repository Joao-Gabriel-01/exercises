namespace ProjetoVendedores.Models
{
    /// <summary>
    /// Representa o registro de vendas de um único dia:
    /// quantas vendas foram feitas e o valor total arrecadado nesse dia.
    /// </summary>
    public class Venda
    {
        private int qtde;
        private double valor;

        public Venda(int qtde, double valor)
        {
            this.qtde = qtde;
            this.valor = valor;
        }

        public int Qtde => qtde;
        public double Valor => valor;

        /// <summary>
        /// Valor médio por venda realizada neste dia (valor total / quantidade).
        /// </summary>
        public double valorMedio()
        {
            if (qtde == 0)
            {
                return 0d;
            }

            return valor / qtde;
        }
    }
}
