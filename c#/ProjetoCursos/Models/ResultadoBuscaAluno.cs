using System.Collections.Generic;

namespace ProjetoCursos.Models
{
    /// <summary>
    /// Resultado da busca de um aluno em toda a escola (não fazia parte do
    /// diagrama original, mas é necessário para a opção "Pesquisar aluno",
    /// que precisa informar o nome do aluno e em quais disciplinas ele está
    /// matriculado).
    /// </summary>
    public class ResultadoBuscaAluno
    {
        public Aluno Aluno { get; set; }
        public Curso Curso { get; set; }
        public List<Disciplina> Disciplinas { get; set; } = new List<Disciplina>();
    }
}
