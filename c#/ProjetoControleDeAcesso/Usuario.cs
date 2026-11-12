using System;
using System.Collections.Generic;

namespace ControleAcessos
{
    /// <summary>
    /// Usuário do sistema. Cada usuário só pode ter uma permissão por ambiente.
    /// </summary>
    public class Usuario
    {
        private int id;
        private string nome;
        private List<Ambiente> ambientes;

        public Usuario(int id, string nome)
        {
            this.id = id;
            this.nome = nome;
            this.ambientes = new List<Ambiente>();
        }

        public int Id { get { return id; } }
        public string Nome { get { return nome; } }

        // Somente leitura: alterações passam por concederPermissao/revogarPermissao
        public IReadOnlyList<Ambiente> Ambientes { get { return ambientes.AsReadOnly(); } }

        /// <summary>
        /// Concede a permissão. Retorna false se o ambiente for nulo
        /// ou se o usuário já tiver permissão para ele.
        /// </summary>
        public bool concederPermissao(Ambiente ambiente)
        {
            if (ambiente == null || ambientes.Contains(ambiente))
                return false;

            ambientes.Add(ambiente);
            return true;
        }

        /// <summary>
        /// Revoga a permissão. Retorna false se o usuário não tinha permissão para o ambiente.
        /// </summary>
        public bool revogarPermissao(Ambiente ambiente)
        {
            if (ambiente == null)
                return false;

            return ambientes.Remove(ambiente);
        }

        public bool temPermissao(Ambiente ambiente)
        {
            return ambiente != null && ambientes.Contains(ambiente);
        }

        public override bool Equals(object obj)
        {
            Usuario outro = obj as Usuario;
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
