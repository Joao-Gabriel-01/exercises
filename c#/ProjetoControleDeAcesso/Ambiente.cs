using System;
using System.Collections.Generic;
using System.Linq;

namespace ControleAcessos
{
    /// <summary>
    /// Ambiente (sala) controlado por um dispositivo de acesso.
    /// Guarda, no máximo, 100 logs; ao estourar, o mais antigo é descartado (fila FIFO).
    /// </summary>
    public class Ambiente
    {
        public const int MaxLogs = 100;

        private int id;
        private string nome;
        private Queue<Log> logs;

        public Ambiente(int id, string nome)
        {
            this.id = id;
            this.nome = nome;
            this.logs = new Queue<Log>();
        }

        public int Id { get { return id; } }
        public string Nome { get { return nome; } }
        public int QtdLogs { get { return logs.Count; } }

        // Do mais antigo para o mais recente (ordem da fila)
        public IEnumerable<Log> Logs { get { return logs; } }

        public void registrarLog(Log log)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            // Descarta os mais antigos até haver espaço para o novo
            while (logs.Count >= MaxLogs)
                logs.Dequeue();

            logs.Enqueue(log);
        }

        /// <summary>
        /// Remove do ambiente todos os logs de um usuário (usado quando o usuário é excluído).
        /// Reconstrói a fila mantendo a ordem original.
        /// </summary>
        public void removerLogsDoUsuario(Usuario usuario)
        {
            logs = new Queue<Log>(logs.Where(l => !l.Usuario.Equals(usuario)));
        }

        public override bool Equals(object obj)
        {
            Ambiente outro = obj as Ambiente;
            return outro != null && outro.id == id;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", id, nome);
        }
    }
}
