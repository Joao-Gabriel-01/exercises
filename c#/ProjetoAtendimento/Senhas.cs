namespace ProjetoAtendimento
{
    public class Senhas
    {
        private int proximoAtendimento;
        private Queue<Senha> filaSenhas;

        public Senhas()
        {
            filaSenhas = new Queue<Senha>();
            proximoAtendimento = 1;
        }

        // Acesso à fila (necessário para Guiche.chamar e para listar na interface)
        public Queue<Senha> FilaSenhas
        {
            get { return filaSenhas; }
        }

        public void gerar()
        {
            filaSenhas.Enqueue(new Senha(proximoAtendimento));
            proximoAtendimento++;
        }
    }
}
