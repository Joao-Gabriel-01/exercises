namespace ProjetoCursos.Models
{
    /// <summary>
    /// Escola: coleção que gerencia os cursos oferecidos, com um limite
    /// máximo de 5 cursos.
    /// </summary>
    public class Escola
    {
        public const int MAX_CURSOS = 5;

        private Curso[] cursos;
        private int qtde;

        public Escola()
        {
            cursos = new Curso[MAX_CURSOS];
            qtde = 0;
        }

        public int QtdeCursos => qtde;

        /// <summary>
        /// Adiciona um curso, respeitando o limite máximo e a unicidade do id.
        /// </summary>
        public bool adicionarCurso(Curso curso)
        {
            if (qtde >= MAX_CURSOS)
            {
                return false; // limite máximo de cursos atingido
            }

            if (IndiceDoCurso(curso.Id) != -1)
            {
                return false; // já existe um curso cadastrado com esse id
            }

            cursos[qtde] = curso;
            qtde++;
            return true;
        }

        /// <summary>
        /// Busca um curso pelo id (o objeto recebido serve apenas como
        /// chave de busca).
        /// </summary>
        public Curso pesquisarCurso(Curso curso)
        {
            int indice = IndiceDoCurso(curso.Id);
            return indice == -1 ? null : cursos[indice];
        }

        /// <summary>
        /// Remove um curso, desde que ele não possua nenhuma disciplina
        /// associada.
        /// </summary>
        public bool removerCurso(Curso curso)
        {
            int indice = IndiceDoCurso(curso.Id);

            if (indice == -1)
            {
                return false; // curso não encontrado
            }

            if (cursos[indice].QtdeDisciplinas > 0)
            {
                return false; // possui disciplinas associadas, remoção não permitida
            }

            for (int i = indice; i < qtde - 1; i++)
            {
                cursos[i] = cursos[i + 1];
            }

            cursos[qtde - 1] = null;
            qtde--;
            return true;
        }

        /// <summary>
        /// Método auxiliar para listar/percorrer os cursos cadastrados
        /// (0 a QtdeCursos - 1), sem expor o array interno.
        /// </summary>
        public Curso ObterCursoPorPosicao(int posicao)
        {
            if (posicao < 0 || posicao >= qtde)
            {
                return null;
            }

            return cursos[posicao];
        }

        /// <summary>
        /// Percorre todos os cursos e disciplinas da escola em busca de um
        /// aluno pelo id. Necessário porque não existe um cadastro central
        /// de alunos: a única forma de localizar um aluno é varrendo as
        /// matrículas registradas em cada disciplina.
        /// </summary>
        public ResultadoBuscaAluno pesquisarAlunoNaEscola(int alunoId)
        {
            ResultadoBuscaAluno resultado = null;

            for (int i = 0; i < qtde; i++)
            {
                Curso curso = cursos[i];

                for (int j = 0; j < curso.QtdeDisciplinas; j++)
                {
                    Disciplina disciplina = curso.ObterDisciplinaPorPosicao(j);
                    Aluno aluno = disciplina.ObterAlunoPorId(alunoId);

                    if (aluno != null)
                    {
                        if (resultado == null)
                        {
                            resultado = new ResultadoBuscaAluno
                            {
                                Aluno = aluno,
                                Curso = curso
                            };
                        }

                        resultado.Disciplinas.Add(disciplina);
                    }
                }
            }

            return resultado;
        }

        private int IndiceDoCurso(int cursoId)
        {
            for (int i = 0; i < qtde; i++)
            {
                if (cursos[i].Id == cursoId)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
