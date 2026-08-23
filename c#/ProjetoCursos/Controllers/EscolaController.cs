using ProjetoCursos.Models;
using ProjetoCursos.Views;

namespace ProjetoCursos.Controllers
{
    /// <summary>
    /// Camada de controle (Controller): interpreta a opção escolhida pelo
    /// usuário, solicita os dados necessários à View e aciona o Model
    /// (Escola/Curso/Disciplina/Aluno) para executar cada operação.
    /// </summary>
    public class EscolaController
    {
        private readonly Escola escola;
        private readonly ConsoleView view;

        public EscolaController(Escola escola, ConsoleView view)
        {
            this.escola = escola;
            this.view = view;
        }

        public void Executar()
        {
            int opcao;

            do
            {
                opcao = view.ExibirMenuEObterOpcao();

                switch (opcao)
                {
                    case 0:
                        view.ExibirMensagem("Encerrando aplicação...");
                        break;
                    case 1:
                        AdicionarCurso();
                        break;
                    case 2:
                        PesquisarCurso();
                        break;
                    case 3:
                        RemoverCurso();
                        break;
                    case 4:
                        AdicionarDisciplina();
                        break;
                    case 5:
                        PesquisarDisciplina();
                        break;
                    case 6:
                        RemoverDisciplina();
                        break;
                    case 7:
                        MatricularAluno();
                        break;
                    case 8:
                        RemoverAlunoDaDisciplina();
                        break;
                    case 9:
                        PesquisarAluno();
                        break;
                    default:
                        view.ExibirErro("Opção inválida.");
                        break;
                }

                if (opcao != 0)
                {
                    view.AguardarTecla();
                }

            } while (opcao != 0);
        }

        // ---------------------------------------------------------------
        // 1. Adicionar curso
        // ---------------------------------------------------------------
        private void AdicionarCurso()
        {
            view.ExibirTitulo("Adicionar curso");

            if (escola.QtdeCursos >= Escola.MAX_CURSOS)
            {
                view.ExibirErro($"Limite máximo de {Escola.MAX_CURSOS} cursos já foi atingido.");
                return;
            }

            int id = view.LerInteiro("ID do curso: ");
            string descricao = view.LerTexto("Descrição do curso: ");

            Curso curso = new Curso(id, descricao);
            bool adicionado = escola.adicionarCurso(curso);

            if (adicionado)
            {
                view.ExibirSucesso($"Curso '{descricao}' adicionado com sucesso.");
            }
            else
            {
                view.ExibirErro("Não foi possível adicionar: limite de cursos atingido ou ID já utilizado.");
            }
        }

        // ---------------------------------------------------------------
        // 2. Pesquisar curso (mostrando as disciplinas associadas)
        // ---------------------------------------------------------------
        private void PesquisarCurso()
        {
            view.ExibirTitulo("Pesquisar curso");

            int id = view.LerInteiro("ID do curso: ");
            Curso encontrado = LocalizarCurso(id);
            if (encontrado == null)
            {
                return;
            }

            view.ExibirMensagem($"ID: {encontrado.Id}");
            view.ExibirMensagem($"Descrição: {encontrado.Descricao}");
            view.ExibirMensagem($"Disciplinas cadastradas: {encontrado.QtdeDisciplinas}/{Curso.MAX_DISCIPLINAS}");

            if (encontrado.QtdeDisciplinas == 0)
            {
                view.ExibirMensagem("  (nenhuma disciplina cadastrada)");
            }

            for (int i = 0; i < encontrado.QtdeDisciplinas; i++)
            {
                Disciplina d = encontrado.ObterDisciplinaPorPosicao(i);
                view.ExibirMensagem($"  - [{d.Id}] {d.Descricao} ({d.QtdeAlunos}/{Disciplina.MAX_ALUNOS} alunos)");
            }
        }

        // ---------------------------------------------------------------
        // 3. Remover curso (sem disciplinas associadas)
        // ---------------------------------------------------------------
        private void RemoverCurso()
        {
            view.ExibirTitulo("Remover curso");

            int id = view.LerInteiro("ID do curso: ");
            Curso encontrado = LocalizarCurso(id);
            if (encontrado == null)
            {
                return;
            }

            bool removido = escola.removerCurso(encontrado);

            if (removido)
            {
                view.ExibirSucesso($"Curso '{encontrado.Descricao}' removido com sucesso.");
            }
            else
            {
                view.ExibirErro("Não é possível remover: o curso possui disciplinas associadas.");
            }
        }

        // ---------------------------------------------------------------
        // 4. Adicionar disciplina no curso
        // ---------------------------------------------------------------
        private void AdicionarDisciplina()
        {
            view.ExibirTitulo("Adicionar disciplina no curso");

            int cursoId = view.LerInteiro("ID do curso: ");
            Curso curso = LocalizarCurso(cursoId);
            if (curso == null)
            {
                return;
            }

            if (curso.QtdeDisciplinas >= Curso.MAX_DISCIPLINAS)
            {
                view.ExibirErro($"Limite máximo de {Curso.MAX_DISCIPLINAS} disciplinas por curso já foi atingido.");
                return;
            }

            int disciplinaId = view.LerInteiro("ID da disciplina: ");
            string descricao = view.LerTexto("Descrição da disciplina: ");

            Disciplina disciplina = new Disciplina(disciplinaId, descricao);
            bool adicionada = curso.adicionarDisciplina(disciplina);

            if (adicionada)
            {
                view.ExibirSucesso($"Disciplina '{descricao}' adicionada ao curso '{curso.Descricao}'.");
            }
            else
            {
                view.ExibirErro("Não foi possível adicionar: limite de disciplinas atingido ou ID já utilizado neste curso.");
            }
        }

        // ---------------------------------------------------------------
        // 5. Pesquisar disciplina (mostrando os alunos matriculados)
        // ---------------------------------------------------------------
        private void PesquisarDisciplina()
        {
            view.ExibirTitulo("Pesquisar disciplina");

            int cursoId = view.LerInteiro("ID do curso: ");
            Curso curso = LocalizarCurso(cursoId);
            if (curso == null)
            {
                return;
            }

            int disciplinaId = view.LerInteiro("ID da disciplina: ");
            Disciplina encontrada = LocalizarDisciplina(curso, disciplinaId);
            if (encontrada == null)
            {
                return;
            }

            view.ExibirMensagem($"ID: {encontrada.Id}");
            view.ExibirMensagem($"Descrição: {encontrada.Descricao}");
            view.ExibirMensagem($"Curso: [{curso.Id}] {curso.Descricao}");
            view.ExibirMensagem($"Alunos matriculados: {encontrada.QtdeAlunos}/{Disciplina.MAX_ALUNOS}");

            if (encontrada.QtdeAlunos == 0)
            {
                view.ExibirMensagem("  (nenhum aluno matriculado)");
            }

            for (int i = 0; i < encontrada.QtdeAlunos; i++)
            {
                Aluno a = encontrada.ObterAlunoPorPosicao(i);
                view.ExibirMensagem($"  - [{a.Id}] {a.Nome}");
            }
        }

        // ---------------------------------------------------------------
        // 6. Remover disciplina do curso (sem alunos matriculados)
        // ---------------------------------------------------------------
        private void RemoverDisciplina()
        {
            view.ExibirTitulo("Remover disciplina do curso");

            int cursoId = view.LerInteiro("ID do curso: ");
            Curso curso = LocalizarCurso(cursoId);
            if (curso == null)
            {
                return;
            }

            int disciplinaId = view.LerInteiro("ID da disciplina: ");
            Disciplina encontrada = LocalizarDisciplina(curso, disciplinaId);
            if (encontrada == null)
            {
                return;
            }

            bool removida = curso.removerDisciplina(encontrada);

            if (removida)
            {
                view.ExibirSucesso($"Disciplina '{encontrada.Descricao}' removida com sucesso.");
            }
            else
            {
                view.ExibirErro("Não é possível remover: a disciplina possui alunos matriculados.");
            }
        }

        // ---------------------------------------------------------------
        // 7. Matricular aluno na disciplina
        // ---------------------------------------------------------------
        private void MatricularAluno()
        {
            view.ExibirTitulo("Matricular aluno na disciplina");

            int cursoId = view.LerInteiro("ID do curso: ");
            Curso curso = LocalizarCurso(cursoId);
            if (curso == null)
            {
                return;
            }

            int disciplinaId = view.LerInteiro("ID da disciplina: ");
            Disciplina disciplina = LocalizarDisciplina(curso, disciplinaId);
            if (disciplina == null)
            {
                return;
            }

            int alunoId = view.LerInteiro("ID do aluno: ");
            ResultadoBuscaAluno resultado = escola.pesquisarAlunoNaEscola(alunoId);

            Aluno aluno;

            if (resultado != null)
            {
                if (resultado.Curso.Id != curso.Id)
                {
                    view.ExibirErro(
                        $"Este aluno já está matriculado em disciplinas do curso '{resultado.Curso.Descricao}' " +
                        "e só pode estar matriculado em um único curso.");
                    return;
                }

                // Reaproveita o mesmo objeto/nome já usado nas outras matrículas do aluno.
                aluno = resultado.Aluno;
            }
            else
            {
                string nome = view.LerTexto("Nome do aluno: ");
                aluno = new Aluno(alunoId, nome);
            }

            if (!aluno.podeMatricular(curso))
            {
                view.ExibirErro($"O aluno já atingiu o limite de {Aluno.MAX_DISCIPLINAS_SIMULTANEAS} disciplinas simultâneas.");
                return;
            }

            bool matriculado = disciplina.matricularAluno(aluno);

            if (matriculado)
            {
                view.ExibirSucesso($"Aluno '{aluno.Nome}' matriculado com sucesso em '{disciplina.Descricao}'.");
            }
            else
            {
                view.ExibirErro("Não foi possível matricular: aluno já matriculado nesta disciplina ou disciplina lotada.");
            }
        }

        // ---------------------------------------------------------------
        // 8. Remover aluno da disciplina
        // ---------------------------------------------------------------
        private void RemoverAlunoDaDisciplina()
        {
            view.ExibirTitulo("Remover aluno da disciplina");

            int cursoId = view.LerInteiro("ID do curso: ");
            Curso curso = LocalizarCurso(cursoId);
            if (curso == null)
            {
                return;
            }

            int disciplinaId = view.LerInteiro("ID da disciplina: ");
            Disciplina disciplina = LocalizarDisciplina(curso, disciplinaId);
            if (disciplina == null)
            {
                return;
            }

            int alunoId = view.LerInteiro("ID do aluno: ");
            Aluno probe = new Aluno(alunoId, string.Empty);

            bool removido = disciplina.desmatricularAluno(probe);

            if (removido)
            {
                view.ExibirSucesso("Aluno removido da disciplina com sucesso.");
            }
            else
            {
                view.ExibirErro("Aluno não encontrado nesta disciplina.");
            }
        }

        // ---------------------------------------------------------------
        // 9. Pesquisar aluno (nome e disciplinas em que está matriculado)
        // ---------------------------------------------------------------
        private void PesquisarAluno()
        {
            view.ExibirTitulo("Pesquisar aluno");

            int id = view.LerInteiro("ID do aluno: ");
            ResultadoBuscaAluno resultado = escola.pesquisarAlunoNaEscola(id);

            if (resultado == null)
            {
                view.ExibirErro("Aluno não encontrado (nenhuma matrícula localizada).");
                return;
            }

            view.ExibirMensagem($"ID: {resultado.Aluno.Id}");
            view.ExibirMensagem($"Nome: {resultado.Aluno.Nome}");
            view.ExibirMensagem($"Curso: [{resultado.Curso.Id}] {resultado.Curso.Descricao}");
            view.ExibirMensagem("Disciplinas em que está matriculado:");

            foreach (Disciplina d in resultado.Disciplinas)
            {
                view.ExibirMensagem($"  - [{d.Id}] {d.Descricao}");
            }
        }

        // ---------------------------------------------------------------
        // Auxiliares de localização (com mensagem de erro já tratada)
        // ---------------------------------------------------------------
        private Curso LocalizarCurso(int id)
        {
            Curso encontrado = escola.pesquisarCurso(new Curso(id, string.Empty));

            if (encontrado == null)
            {
                view.ExibirErro("Curso não encontrado.");
            }

            return encontrado;
        }

        private Disciplina LocalizarDisciplina(Curso curso, int id)
        {
            Disciplina encontrada = curso.pesquisarDisciplina(new Disciplina(id, string.Empty));

            if (encontrada == null)
            {
                view.ExibirErro("Disciplina não encontrada neste curso.");
            }

            return encontrada;
        }
    }
}
