namespace ProjetoAtendimento
{
    public class Senha
    {
        private int id;
        private DateTime dataGerac;
        private DateTime horaGerac;
        private DateTime dataAtend;
        private DateTime horaAtend;

        // Instancia o objeto inicializando id, dataGerac e horaGerac
        public Senha(int id)
        {
            this.id = id;
            this.dataGerac = DateTime.Now;
            this.horaGerac = DateTime.Now;
        }

        // Método auxiliar (não consta no diagrama): registra o momento do atendimento,
        // pois dataAtend e horaAtend são privados.
        public void registrarAtendimento()
        {
            this.dataAtend = DateTime.Now;
            this.horaAtend = DateTime.Now;
        }

        public string dadosParciais()
        {
            return id + "-" + dataGerac.ToString("dd/MM/yyyy") + "-" + horaGerac.ToString("HH:mm:ss");
        }

        public string dadosCompletos()
        {
            return id + "-" + dataGerac.ToString("dd/MM/yyyy") + "-" + horaGerac.ToString("HH:mm:ss")
                 + "-" + dataAtend.ToString("dd/MM/yyyy") + "-" + horaAtend.ToString("HH:mm:ss");
        }
    }
}
