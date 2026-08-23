namespace ProjetoVendedores.Models
{
    /// <summary>
    /// Coleção que gerencia os vendedores cadastrados, com um limite máximo
    /// de vagas (definido na criação da coleção).
    /// </summary>
    public class Vendedores
    {
        private Vendedor[] osVendedores;
        private int max;
        private int qtde;

        public Vendedores(int max)
        {
            this.max = max;
            osVendedores = new Vendedor[max];
            qtde = 0;
        }

        public int Qtde => qtde;
        public int Max => max;

        /// <summary>
        /// Adiciona um vendedor, respeitando o limite máximo e a unicidade do id.
        /// </summary>
        public bool addVendedor(Vendedor v)
        {
            if (qtde >= max)
            {
                return false; // limite máximo de vendedores atingido
            }

            if (searchVendedor(v) != null)
            {
                return false; // já existe um vendedor cadastrado com esse id
            }

            osVendedores[qtde] = v;
            qtde++;
            return true;
        }

        /// <summary>
        /// Remove um vendedor, desde que ele não possua nenhuma venda associada.
        /// </summary>
        public bool delVendedor(Vendedor v)
        {
            int indice = IndiceDoVendedor(v);

            if (indice == -1)
            {
                return false; // vendedor não encontrado
            }

            if (osVendedores[indice].possuiVendas())
            {
                return false; // possui vendas associadas, exclusão não permitida
            }

            for (int i = indice; i < qtde - 1; i++)
            {
                osVendedores[i] = osVendedores[i + 1];
            }

            osVendedores[qtde - 1] = null;
            qtde--;

            return true;
        }

        /// <summary>
        /// Busca um vendedor pelo id (o objeto recebido serve apenas como chave de busca).
        /// </summary>
        public Vendedor searchVendedor(Vendedor v)
        {
            int indice = IndiceDoVendedor(v);
            return indice == -1 ? null : osVendedores[indice];
        }

        public double valorVendas()
        {
            double total = 0d;

            for (int i = 0; i < qtde; i++)
            {
                total += osVendedores[i].valorVendas();
            }

            return total;
        }

        public double valorComissao()
        {
            double total = 0d;

            for (int i = 0; i < qtde; i++)
            {
                total += osVendedores[i].valorComissao();
            }

            return total;
        }

        /// <summary>
        /// Método auxiliar para permitir a listagem dos vendedores cadastrados
        /// sem expor diretamente o array interno.
        /// </summary>
        public Vendedor ObterPorPosicao(int posicao)
        {
            if (posicao < 0 || posicao >= qtde)
            {
                return null;
            }

            return osVendedores[posicao];
        }

        private int IndiceDoVendedor(Vendedor v)
        {
            for (int i = 0; i < qtde; i++)
            {
                if (osVendedores[i].Id == v.Id)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
