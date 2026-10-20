namespace ProjetoAtendimento
{
    public class Guiches
    {
        private List<Guiche> listaGuiches;

        public Guiches()
        {
            listaGuiches = new List<Guiche>();
        }

        public List<Guiche> ListaGuiches
        {
            get { return listaGuiches; }
        }

        public void adicionar(Guiche guiche)
        {
            listaGuiches.Add(guiche);
        }
    }
}
