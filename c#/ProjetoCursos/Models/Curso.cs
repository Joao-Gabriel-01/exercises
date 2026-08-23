namespace ProjetoCursos.Models
{
    /// <summary>
    /// Representa um curso, com capacidade máxima de 12 disciplinas.
    /// </summary>
    public class Curso
    {
        public const int MAX_DISCIPLINAS = 12;

        private int id;
        private string descricao;
        private Disciplina[] disciplinas;
        private int qtde;

        public Curso(int id, string descricao)
        {
            this.id = id;
            this.descricao = descricao;
            disciplinas = new Disciplina[MAX_DISCIPLINAS];
            qtde = 0;
        }

        public int Id => id;
        public string Descricao => descricao;
        public int QtdeDisciplinas => qtde;

        /// <summary>
        /// Adiciona uma disciplina ao curso, respeitando o limite de 12
        /// disciplinas e impedindo id duplicado dentro do mesmo curso.
        /// </summary>
        public bool adicionarDisciplina(Disciplina disciplina)
        {
            if (qtde >= MAX_DISCIPLINAS)
            {
                return false; // limite de disciplinas do curso atingido
            }

            if (IndiceDaDisciplina(disciplina.Id) != -1)
            {
                return false; // já existe disciplina com esse id neste curso
            }

            disciplinas[qtde] = disciplina;
            qtde++;
            return true;
        }

        /// <summary>
        /// Busca uma disciplina do curso pelo id (o objeto recebido serve
        /// apenas como chave de busca).
        /// </summary>
        public Disciplina pesquisarDisciplina(Disciplina disciplina)
        {
            int indice = IndiceDaDisciplina(disciplina.Id);
            return indice == -1 ? null : disciplinas[indice];
        }

        /// <summary>
        /// Remove uma disciplina do curso, desde que ela não possua nenhum
        /// aluno matriculado.
        /// </summary>
        public bool removerDisciplina(Disciplina disciplina)
        {
            int indice = IndiceDaDisciplina(disciplina.Id);

            if (indice == -1)
            {
                return false; // disciplina não encontrada
            }

            if (disciplinas[indice].QtdeAlunos > 0)
            {
                return false; // possui alunos matriculados, remoção não permitida
            }

            for (int i = indice; i < qtde - 1; i++)
            {
                disciplinas[i] = disciplinas[i + 1];
            }

            disciplinas[qtde - 1] = null;
            qtde--;
            return true;
        }

        /// <summary>
        /// Método auxiliar para listar/percorrer as disciplinas cadastradas
        /// (0 a QtdeDisciplinas - 1), sem expor o array interno.
        /// </summary>
        public Disciplina ObterDisciplinaPorPosicao(int posicao)
        {
            if (posicao < 0 || posicao >= qtde)
            {
                return null;
            }

            return disciplinas[posicao];
        }

        private int IndiceDaDisciplina(int disciplinaId)
        {
            for (int i = 0; i < qtde; i++)
            {
                if (disciplinas[i].Id == disciplinaId)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
