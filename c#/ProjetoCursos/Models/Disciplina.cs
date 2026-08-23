namespace ProjetoCursos.Models
{
    /// <summary>
    /// Representa uma disciplina de um curso, com capacidade máxima de
    /// 15 alunos matriculados.
    /// </summary>
    public class Disciplina
    {
        public const int MAX_ALUNOS = 15;

        private int id;
        private string descricao;
        private Aluno[] alunos;
        private int qtde;

        public Disciplina(int id, string descricao)
        {
            this.id = id;
            this.descricao = descricao;
            alunos = new Aluno[MAX_ALUNOS];
            qtde = 0;
        }

        public int Id => id;
        public string Descricao => descricao;
        public int QtdeAlunos => qtde;

        /// <summary>
        /// Matricula um aluno na disciplina, respeitando o limite de 15
        /// vagas e impedindo matrícula duplicada do mesmo aluno.
        /// </summary>
        public bool matricularAluno(Aluno aluno)
        {
            if (qtde >= MAX_ALUNOS)
            {
                return false; // disciplina lotada
            }

            if (IndiceDoAluno(aluno.Id) != -1)
            {
                return false; // aluno já matriculado nesta disciplina
            }

            alunos[qtde] = aluno;
            qtde++;
            return true;
        }

        /// <summary>
        /// Remove a matrícula de um aluno na disciplina.
        /// </summary>
        public bool desmatricularAluno(Aluno aluno)
        {
            int indice = IndiceDoAluno(aluno.Id);

            if (indice == -1)
            {
                return false; // aluno não estava matriculado
            }

            for (int i = indice; i < qtde - 1; i++)
            {
                alunos[i] = alunos[i + 1];
            }

            alunos[qtde - 1] = null;
            qtde--;
            return true;
        }

        /// <summary>
        /// Método auxiliar (não constava no diagrama original, mas é
        /// necessário para o cálculo de <see cref="Aluno.podeMatricular"/>
        /// e para a busca de aluno na escola): informa se o aluno já está
        /// matriculado nesta disciplina.
        /// </summary>
        public bool possuiAluno(Aluno aluno)
        {
            return IndiceDoAluno(aluno.Id) != -1;
        }

        /// <summary>
        /// Método auxiliar para localizar um aluno matriculado pelo id,
        /// usado na busca de aluno em toda a escola.
        /// </summary>
        public Aluno ObterAlunoPorId(int alunoId)
        {
            int indice = IndiceDoAluno(alunoId);
            return indice == -1 ? null : alunos[indice];
        }

        /// <summary>
        /// Método auxiliar para listar os alunos matriculados (0 a
        /// QtdeAlunos - 1), sem expor o array interno.
        /// </summary>
        public Aluno ObterAlunoPorPosicao(int posicao)
        {
            if (posicao < 0 || posicao >= qtde)
            {
                return null;
            }

            return alunos[posicao];
        }

        private int IndiceDoAluno(int alunoId)
        {
            for (int i = 0; i < qtde; i++)
            {
                if (alunos[i].Id == alunoId)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
