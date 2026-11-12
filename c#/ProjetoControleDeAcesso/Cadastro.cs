using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace ControleAcessos
{
    /// <summary>
    /// Repositório de usuários e ambientes + persistência.
    ///
    /// Modelo relacional usado na persistência (um arquivo CSV por tabela, separador ';'):
    ///   ambientes.csv   -> id ; nome                                   (PK: id)
    ///   usuarios.csv    -> id ; nome                                   (PK: id)
    ///   permissoes.csv  -> usuario_id ; ambiente_id                    (PK composta; FKs)
    ///   logs.csv        -> ambiente_id ; usuario_id ; dt_acesso ; tipo (FKs; tipo: 1=autorizado, 0=negado)
    /// </summary>
    public class Cadastro
    {
        private const char Sep = ';';

        private List<Usuario> usuarios;
        private List<Ambiente> ambientes;

        public Cadastro()
        {
            usuarios = new List<Usuario>();
            ambientes = new List<Ambiente>();
        }

        public IReadOnlyList<Usuario> Usuarios { get { return usuarios.AsReadOnly(); } }
        public IReadOnlyList<Ambiente> Ambientes { get { return ambientes.AsReadOnly(); } }

        // ---------------------------------------------------------------- Usuários

        public void adicionarUsuario(Usuario usuario)
        {
            usuarios.Add(usuario);
        }

        /// <summary>
        /// Só remove se o usuário existir e estiver sem nenhuma permissão de acesso.
        /// Os logs do usuário também são removidos dos ambientes (integridade referencial).
        /// </summary>
        public bool removerUsuario(Usuario usuario)
        {
            Usuario existente = pesquisarUsuario(usuario);
            if (existente == null || existente.Ambientes.Count > 0)
                return false;

            foreach (Ambiente a in ambientes)
                a.removerLogsDoUsuario(existente);

            return usuarios.Remove(existente);
        }

        public Usuario pesquisarUsuario(Usuario usuario)
        {
            if (usuario == null)
                return null;

            return usuarios.Find(u => u.Equals(usuario));
        }

        // --------------------------------------------------------------- Ambientes

        public void adicionarAmbiente(Ambiente ambiente)
        {
            ambientes.Add(ambiente);
        }

        /// <summary>
        /// Remove o ambiente e revoga a permissão de todos os usuários que tinham acesso a ele.
        /// </summary>
        public bool removerAmbiente(Ambiente ambiente)
        {
            Ambiente existente = pesquisarAmbiente(ambiente);
            if (existente == null)
                return false;

            foreach (Usuario u in usuarios)
                u.revogarPermissao(existente);

            return ambientes.Remove(existente);
        }

        public Ambiente pesquisarAmbiente(Ambiente ambiente)
        {
            if (ambiente == null)
                return null;

            return ambientes.Find(a => a.Equals(ambiente));
        }

        // -------------------------------------------------------------- Persistência

        private static string Pasta
        {
            get { return Path.Combine(AppContext.BaseDirectory, "dados"); }
        }

        private static string Caminho(string arquivo)
        {
            return Path.Combine(Pasta, arquivo);
        }

        // O separador não pode aparecer dentro do nome
        private static string Limpar(string texto)
        {
            return texto.Replace(Sep, ',');
        }

        /// <summary>
        /// Grava todos os dados nos arquivos (chamado ao encerrar a aplicação).
        /// </summary>
        public void upload()
        {
            Directory.CreateDirectory(Pasta);

            List<string> linhas = new List<string>();

            foreach (Ambiente a in ambientes)
                linhas.Add(a.Id + Sep.ToString() + Limpar(a.Nome));
            File.WriteAllLines(Caminho("ambientes.csv"), linhas);

            linhas.Clear();
            foreach (Usuario u in usuarios)
                linhas.Add(u.Id + Sep.ToString() + Limpar(u.Nome));
            File.WriteAllLines(Caminho("usuarios.csv"), linhas);

            linhas.Clear();
            foreach (Usuario u in usuarios)
                foreach (Ambiente a in u.Ambientes)
                    linhas.Add(u.Id + Sep.ToString() + a.Id);
            File.WriteAllLines(Caminho("permissoes.csv"), linhas);

            // Logs de cada ambiente na ordem da fila (do mais antigo para o mais recente)
            linhas.Clear();
            foreach (Ambiente a in ambientes)
                foreach (Log l in a.Logs)
                    linhas.Add(string.Join(Sep.ToString(), new string[] {
                        a.Id.ToString(),
                        l.Usuario.Id.ToString(),
                        l.DtAcesso.ToString("o", CultureInfo.InvariantCulture),
                        l.TipoAcesso ? "1" : "0"
                    }));
            File.WriteAllLines(Caminho("logs.csv"), linhas);
        }

        /// <summary>
        /// Carrega os dados dos arquivos (chamado ao iniciar a aplicação).
        /// Se os arquivos não existirem, começa vazio. Linhas inválidas são ignoradas.
        /// </summary>
        public void download()
        {
            usuarios.Clear();
            ambientes.Clear();

            // 1) Ambientes
            foreach (string[] c in LerCsv("ambientes.csv", 2))
            {
                int id;
                if (int.TryParse(c[0], out id))
                    ambientes.Add(new Ambiente(id, c[1]));
            }

            // 2) Usuários
            foreach (string[] c in LerCsv("usuarios.csv", 2))
            {
                int id;
                if (int.TryParse(c[0], out id))
                    usuarios.Add(new Usuario(id, c[1]));
            }

            // 3) Permissões (precisam de usuário e ambiente já carregados)
            foreach (string[] c in LerCsv("permissoes.csv", 2))
            {
                int idUsuario, idAmbiente;
                if (!int.TryParse(c[0], out idUsuario) || !int.TryParse(c[1], out idAmbiente))
                    continue;

                Usuario u = pesquisarUsuario(new Usuario(idUsuario, ""));
                Ambiente a = pesquisarAmbiente(new Ambiente(idAmbiente, ""));
                if (u != null && a != null)
                    u.concederPermissao(a);
            }

            // 4) Logs (a ordem do arquivo é a ordem da fila)
            foreach (string[] c in LerCsv("logs.csv", 4))
            {
                int idAmbiente, idUsuario;
                DateTime data;
                if (!int.TryParse(c[0], out idAmbiente) || !int.TryParse(c[1], out idUsuario))
                    continue;
                if (!DateTime.TryParse(c[2], CultureInfo.InvariantCulture,
                        DateTimeStyles.RoundtripKind, out data))
                    continue;

                Ambiente a = pesquisarAmbiente(new Ambiente(idAmbiente, ""));
                Usuario u = pesquisarUsuario(new Usuario(idUsuario, ""));
                if (a != null && u != null)
                    a.registrarLog(new Log(data, u, c[3] == "1"));
            }
        }

        // Lê um CSV e devolve apenas as linhas com o número esperado de colunas
        private static List<string[]> LerCsv(string arquivo, int colunas)
        {
            List<string[]> resultado = new List<string[]>();
            string caminho = Caminho(arquivo);

            if (!File.Exists(caminho))
                return resultado;

            foreach (string linha in File.ReadAllLines(caminho))
            {
                if (string.IsNullOrWhiteSpace(linha))
                    continue;

                string[] partes = linha.Split(Sep);
                if (partes.Length == colunas)
                    resultado.Add(partes);
            }
            return resultado;
        }
    }
}
