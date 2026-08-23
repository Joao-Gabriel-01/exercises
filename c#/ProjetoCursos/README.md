# Projeto Cursos

## Como abrir no Visual Studio

1. Abra o Visual Studio 2022 (ou superior).
2. **Arquivo > Abrir > Pasta...** e selecione a pasta `ProjetoCursos`, ou
   dê duplo clique no arquivo `ProjetoCursos.csproj`.
3. Pressione **F5** para compilar e executar.

O projeto usa .NET 8 (Console App). Se sua máquina só tiver .NET Framework,
troque `<TargetFramework>net8.0</TargetFramework>` por `net48` (ou a versão
disponível) no `.csproj` — o código não usa recursos exclusivos do .NET 8.

## Estrutura (MVC)

- **Models/** — `Aluno`, `Disciplina`, `Curso`, `Escola` (fiéis ao
  diagrama) e `ResultadoBuscaAluno` (DTO auxiliar, explicado abaixo).
- **Views/ConsoleView.cs** — toda a entrada/saída no console (menu,
  leitura validada, mensagens de erro/sucesso).
- **Controllers/EscolaController.cs** — interpreta a opção do menu e
  orquestra View ↔ Model.
- **Program.cs** — monta Model, View e Controller e inicia o menu.

## Sobre o método `Aluno.podeMatricular`

O diagrama descrevia a assinatura como `podeMatricular(Cursos cursos): bool`,
mas o diagrama não define nenhuma classe `Cursos` — apenas `Curso` (um curso
específico) e `Escola` (que reúne os cursos). Foi necessário interpretar essa
assinatura:

- **`podeMatricular(Curso curso)`** conta quantas disciplinas *desse curso*
  o aluno já cursa e verifica o limite de 6 disciplinas simultâneas.
- A regra de **"matriculado em um único curso"** exige enxergar *todos* os
  cursos da escola (não só um), então essa verificação foi implementada no
  `EscolaController.MatricularAluno()`, usando `Escola.pesquisarAlunoNaEscola`
  para descobrir se o aluno já estuda em outro curso antes de liberar a
  matrícula.

Se a intenção original do diagrama for outra, meça sinta-se à vontade
para pedir ajuste — a lógica está isolada nesses dois pontos (o método
`podeMatricular` e o trecho correspondente do Controller), fáceis de
adaptar.

## Outras decisões

- Como não existe uma classe de cadastro central de alunos no diagrama, um
  aluno só "existe" através das matrículas registradas nas disciplinas.
  A busca por aluno (opção 9) percorre todos os cursos e disciplinas da
  escola.
- `Curso`, `Disciplina` e `Escola` usam o mesmo padrão de array de tamanho
  fixo + contador (`qtde`) com compactação na remoção, para manter os dados
  sempre nas posições `0` a `qtde - 1`.
- Todas as buscas (curso, disciplina, aluno) usam apenas o `id` como chave.
