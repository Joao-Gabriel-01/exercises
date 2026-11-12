using System;

namespace ControleAcessos
{
    /// <summary>
    /// Registro de uma tentativa de acesso a um ambiente.
    /// tipoAcesso: true = Autorizado, false = Negado.
    /// </summary>
    public class Log
    {
        private DateTime dtAcesso;
        private Usuario usuario;
        private bool tipoAcesso;

        public Log(DateTime dtAcesso, Usuario usuario, bool tipoAcesso)
        {
            this.dtAcesso = dtAcesso;
            this.usuario = usuario;
            this.tipoAcesso = tipoAcesso;
        }

        public DateTime DtAcesso { get { return dtAcesso; } }
        public Usuario Usuario { get { return usuario; } }
        public bool TipoAcesso { get { return tipoAcesso; } }

        public override string ToString()
        {
            return string.Format("{0:dd/MM/yyyy HH:mm:ss} | {1} (id {2}) | {3}",
                dtAcesso, usuario.Nome, usuario.Id,
                tipoAcesso ? "AUTORIZADO" : "NEGADO");
        }
    }
}
