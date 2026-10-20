namespace ProjetoAtendimento
{
    public class Guiche
    {
        private int id;
        private Queue<Senha> atendimentos;

        public Guiche()
        {
            atendimentos = new Queue<Senha>();
            id = 0;
        }

        public Guiche(int id)
        {
            atendimentos = new Queue<Senha>();
            this.id = id;
        }

        public int Id
        {
            get { return id; }
        }

        public Queue<Senha> Atendimentos
        {
            get { return atendimentos; }
        }

        // Retira a próxima senha da fila de senhas e a coloca na fila de atendimentos
        public bool chamar(Queue<Senha> filaSenhas)
        {
            if (filaSenhas.Count == 0)
                return false;

            Senha senha = filaSenhas.Dequeue();
            senha.registrarAtendimento();
            atendimentos.Enqueue(senha);
            return true;
        }

        // Usado para exibir o guichê no ComboBox
        public override string ToString()
        {
            return "Guichê " + id;
        }
    }
}
