using System;
using System.IO;
using System.Text;

namespace ControleAcessos
{
    class Program
    {
        static Cadastro cadastro = new Cadastro();

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            // download: carga dos dados ao executar a aplicação
            try
            {
                cadastro.download();
            }
            catch (IOException ex)
            {
                Console.WriteLine("Não foi possível carregar os dados: " + ex.Message);
            }

            int opcao;
            do
            {
                opcao = LerMenu();
                Console.WriteLine();

                switch (opcao)
                {
                    case 0: break;
                    case 1: CadastrarAmbiente(); break;
                    case 2: ConsultarAmbiente(); break;
                    case 3: ExcluirAmbiente(); break;
                    case 4: CadastrarUsuario(); break;
                    case 5: ConsultarUsuario(); break;
                    case 6: ExcluirUsuario(); break;
                    case 7: ConcederPermissao(); break;
                    case 8: RevogarPermissao(); break;
                    case 9: RegistrarAcesso(); break;
                    case 10: ConsultarLogs(); break;
                    default: Console.WriteLine("Opção inválida."); break;
                }
            } while (opcao != 0);

            // upload: persistência dos dados ao encerrar a aplicação
            try
            {
                cadastro.upload();
                Console.WriteLine("Dados salvos. Até logo!");
            }
            catch (IOException ex)
            {
                Console.WriteLine("Não foi possível salvar os dados: " + ex.Message);
            }
        }

        // ------------------------------------------------------------------ Menu

        static int LerMenu()
        {
            Console.WriteLine();
            Console.WriteLine("===== CONTROLE DE ACESSOS =====");
            Console.WriteLine(" 0 - Sair");
            Console.WriteLine(" 1 - Cadastrar ambiente");
            Console.WriteLine(" 2 - Consultar ambiente");
            Console.WriteLine(" 3 - Excluir ambiente");
            Console.WriteLine(" 4 - Cadastrar usuário");
            Console.WriteLine(" 5 - Consultar usuário");
            Console.WriteLine(" 6 - Excluir usuário");
            Console.WriteLine(" 7 - Conceder permissão de acesso ao usuário");
            Console.WriteLine(" 8 - Revogar permissão de acesso ao usuário");
            Console.WriteLine(" 9 - Registrar acesso");
            Console.WriteLine("10 - Consultar logs de acesso");
            return LerInteiro("Opção: ");
        }

        // ------------------------------------------------------------- Leitura

        static int LerInteiro(string mensagem)
        {
            int valor;
            Console.Write(mensagem);
            while (!int.TryParse(Console.ReadLine(), out valor))
                Console.Write("Valor inválido. " + mensagem);
            return valor;
        }

        static string LerTexto(string mensagem)
        {
            string texto;
            Console.Write(mensagem);
            while (string.IsNullOrWhiteSpace(texto = Console.ReadLine()))
                Console.Write("Texto vazio. " + mensagem);
            return texto.Trim();
        }

        // Pede o id e devolve o ambiente cadastrado (ou null, já avisando o usuário)
        static Ambiente LerAmbiente()
        {
            int id = LerInteiro("Id do ambiente: ");
            Ambiente ambiente = cadastro.pesquisarAmbiente(new Ambiente(id, ""));
            if (ambiente == null)
                Console.WriteLine("Ambiente não encontrado.");
            return ambiente;
        }

        static Usuario LerUsuario()
        {
            int id = LerInteiro("Id do usuário: ");
            Usuario usuario = cadastro.pesquisarUsuario(new Usuario(id, ""));
            if (usuario == null)
                Console.WriteLine("Usuário não encontrado.");
            return usuario;
        }

        // ------------------------------------------------------------- Ambientes

        static void CadastrarAmbiente()
        {
            int id = LerInteiro("Id do ambiente: ");
            if (cadastro.pesquisarAmbiente(new Ambiente(id, "")) != null)
            {
                Console.WriteLine("Já existe um ambiente com esse id.");
                return;
            }

            string nome = LerTexto("Nome do ambiente: ");
            cadastro.adicionarAmbiente(new Ambiente(id, nome));
            Console.WriteLine("Ambiente cadastrado.");
        }

        static void ConsultarAmbiente()
        {
            Ambiente ambiente = LerAmbiente();
            if (ambiente == null)
                return;

            Console.WriteLine("Ambiente: " + ambiente);
            Console.WriteLine("Logs registrados: " + ambiente.QtdLogs + " (máx. " + Ambiente.MaxLogs + ")");
            Console.WriteLine("Usuários com permissão:");

            bool algum = false;
            foreach (Usuario u in cadastro.Usuarios)
            {
                if (u.temPermissao(ambiente))
                {
                    Console.WriteLine("  " + u);
                    algum = true;
                }
            }
            if (!algum)
                Console.WriteLine("  (nenhum)");
        }

        static void ExcluirAmbiente()
        {
            Ambiente ambiente = LerAmbiente();
            if (ambiente == null)
                return;

            if (cadastro.removerAmbiente(ambiente))
                Console.WriteLine("Ambiente excluído (as permissões dele foram revogadas).");
            else
                Console.WriteLine("Não foi possível excluir o ambiente.");
        }

        // -------------------------------------------------------------- Usuários

        static void CadastrarUsuario()
        {
            int id = LerInteiro("Id do usuário: ");
            if (cadastro.pesquisarUsuario(new Usuario(id, "")) != null)
            {
                Console.WriteLine("Já existe um usuário com esse id.");
                return;
            }

            string nome = LerTexto("Nome do usuário: ");
            cadastro.adicionarUsuario(new Usuario(id, nome));
            Console.WriteLine("Usuário cadastrado.");
        }

        static void ConsultarUsuario()
        {
            Usuario usuario = LerUsuario();
            if (usuario == null)
                return;

            Console.WriteLine("Usuário: " + usuario);
            Console.WriteLine("Ambientes com permissão:");

            if (usuario.Ambientes.Count == 0)
                Console.WriteLine("  (nenhum)");
            foreach (Ambiente a in usuario.Ambientes)
                Console.WriteLine("  " + a);
        }

        static void ExcluirUsuario()
        {
            Usuario usuario = LerUsuario();
            if (usuario == null)
                return;

            if (cadastro.removerUsuario(usuario))
                Console.WriteLine("Usuário excluído.");
            else
                Console.WriteLine("Não foi possível excluir: o usuário ainda possui permissão de acesso. Revogue as permissões antes.");
        }

        // ------------------------------------------------------------ Permissões

        static void ConcederPermissao()
        {
            Ambiente ambiente = LerAmbiente();
            if (ambiente == null)
                return;
            Usuario usuario = LerUsuario();
            if (usuario == null)
                return;

            if (usuario.concederPermissao(ambiente))
                Console.WriteLine("Permissão concedida.");
            else
                Console.WriteLine("O usuário já possui permissão para esse ambiente.");
        }

        static void RevogarPermissao()
        {
            Ambiente ambiente = LerAmbiente();
            if (ambiente == null)
                return;
            Usuario usuario = LerUsuario();
            if (usuario == null)
                return;

            if (usuario.revogarPermissao(ambiente))
                Console.WriteLine("Permissão revogada.");
            else
                Console.WriteLine("O usuário não possui permissão para esse ambiente.");
        }

        // ---------------------------------------------------------------- Acessos

        static void RegistrarAcesso()
        {
            Ambiente ambiente = LerAmbiente();
            if (ambiente == null)
                return;
            Usuario usuario = LerUsuario();
            if (usuario == null)
                return;

            // O dispositivo verifica a permissão e registra o resultado no log
            bool autorizado = usuario.temPermissao(ambiente);
            ambiente.registrarLog(new Log(DateTime.Now, usuario, autorizado));

            Console.WriteLine(autorizado ? "Acesso AUTORIZADO." : "Acesso NEGADO.");
        }

        static void ConsultarLogs()
        {
            Ambiente ambiente = LerAmbiente();
            if (ambiente == null)
                return;

            Console.WriteLine("Filtro: 1 - Todos | 2 - Autorizados | 3 - Negados");
            int filtro = LerInteiro("Filtro: ");
            while (filtro < 1 || filtro > 3)
                filtro = LerInteiro("Filtro inválido (1 a 3): ");

            Console.WriteLine();
            Console.WriteLine("Logs de " + ambiente + " (do mais antigo para o mais recente):");

            int qtd = 0;
            foreach (Log log in ambiente.Logs)
            {
                if (filtro == 2 && !log.TipoAcesso) continue;
                if (filtro == 3 && log.TipoAcesso) continue;

                Console.WriteLine("  " + log);
                qtd++;
            }

            if (qtd == 0)
                Console.WriteLine("  (nenhum log)");
            else
                Console.WriteLine("Total listado: " + qtd);
        }
    }
}
