namespace ProjetoCursos.Models
{
    /// <summary>
    /// Representa um aluno. Não existe, no sistema, um cadastro central de
    /// alunos: a "existência" de um aluno é dada pelas matrículas que ele
    /// possui em disciplinas — por isso a busca por aluno percorre toda a
    /// escola (ver <see cref="Escola.pesquisarAlunoNaEscola"/>).
    /// </summary>
    public class Aluno
    {
        /// <summary>
        /// Quantidade máxima de disciplinas em que um mesmo aluno pode estar
        /// matriculado simultaneamente.
        /// </summary>
        public const int MAX_DISCIPLINAS_SIMULTANEAS = 6;

        private int id;
        private string nome;

        public Aluno(int id, string nome)
        {
            this.id = id;
            this.nome = nome;
        }

        public int Id => id;
        public string Nome => nome;

        /// <summary>
        /// Verifica se o aluno ainda pode se matricular em mais uma
        /// disciplina do curso informado (regra: no máximo 6 disciplinas
        /// simultâneas).
        /// </summary>
        /// <remarks>
        /// O diagrama original descrevia o parâmetro como "Cursos cursos",
        /// mas não existe, no diagrama, nenhuma classe "Cursos" (apenas
        /// "Curso", que representa um único curso, e "Escola", que reúne os
        /// cursos). Interpretamos o parâmetro como o curso ao qual pertence
        /// a disciplina-alvo da matrícula: o método conta quantas
        /// disciplinas DESSE curso o aluno já cursa e confere o limite de 6.
        /// A regra de "matriculado em um único curso" depende de enxergar
        /// TODOS os cursos da escola, então essa parte é validada no
        /// Controller (usando <see cref="Escola.pesquisarAlunoNaEscola"/>)
        /// antes de chamar este método.
        /// </remarks>
        public bool podeMatricular(Curso curso)
        {
            int totalMatriculasNoCurso = 0;

            for (int i = 0; i < curso.QtdeDisciplinas; i++)
            {
                Disciplina disciplina = curso.ObterDisciplinaPorPosicao(i);

                if (disciplina.possuiAluno(this))
                {
                    totalMatriculasNoCurso++;
                }
            }

            return totalMatriculasNoCurso < MAX_DISCIPLINAS_SIMULTANEAS;
        }
    }
}
