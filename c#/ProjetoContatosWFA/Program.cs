using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

// ============================================================
// Projeto Agenda (Contatos) - Windows Forms Application
// Toda a interface e criada em codigo (sem Designer).
// ============================================================

namespace ProjetoAgendaForms
{
    // ======================== MODEL ========================

    public class Data
    {
        private int dia;
        private int mes;
        private int ano;

        public Data(int dia, int mes, int ano)
        {
            setData(dia, mes, ano);
        }

        public int Dia { get { return dia; } }
        public int Mes { get { return mes; } }
        public int Ano { get { return ano; } }

        public void setData(int dia, int mes, int ano)
        {
            if (!ehValida(dia, mes, ano))
                throw new ArgumentException("Data invalida.");
            this.dia = dia;
            this.mes = mes;
            this.ano = ano;
        }

        public static bool ehValida(int dia, int mes, int ano)
        {
            if (ano < 1 || ano > 9999 || mes < 1 || mes > 12 || dia < 1) return false;
            return dia <= DateTime.DaysInMonth(ano, mes);
        }

        public override string ToString()
        {
            return dia.ToString("00") + "/" + mes.ToString("00") + "/" + ano.ToString("0000");
        }
    }

    public class Telefone
    {
        private string tipo;
        private string numero;
        private bool principal;

        public Telefone(string tipo, string numero, bool principal)
        {
            this.tipo = tipo;
            this.numero = numero;
            this.principal = principal;
        }

        public string Tipo { get { return tipo; } set { tipo = value; } }
        public string Numero { get { return numero; } set { numero = value; } }
        public bool Principal { get { return principal; } set { principal = value; } }

        public override string ToString()
        {
            return tipo + ": " + numero + (principal ? " (principal)" : "");
        }
    }

    public class Contato
    {
        private string email;
        private string nome;
        private Data dtNasc;
        private List<Telefone> telefones = new List<Telefone>();

        public Contato(string nome, string email, Data dtNasc)
        {
            this.nome = nome;
            this.email = email;
            this.dtNasc = dtNasc;
        }

        public string Email { get { return email; } set { email = value; } }
        public string Nome { get { return nome; } set { nome = value; } }
        public Data DtNasc { get { return dtNasc; } set { dtNasc = value; } }
        public List<Telefone> Telefones { get { return telefones; } }

        public int getIdade()
        {
            DateTime hoje = DateTime.Today;
            int idade = hoje.Year - dtNasc.Ano;
            if (hoje.Month < dtNasc.Mes || (hoje.Month == dtNasc.Mes && hoje.Day < dtNasc.Dia))
                idade--;
            return idade;
        }

        public void adicionarTelefone(Telefone t)
        {
            if (t == null) return;

            // o primeiro telefone sempre e o principal
            if (telefones.Count == 0) t.Principal = true;

            // so pode existir um telefone principal
            if (t.Principal)
            {
                foreach (Telefone outro in telefones)
                    outro.Principal = false;
            }
            telefones.Add(t);
        }

        public string getTelefonePrincipal()
        {
            foreach (Telefone t in telefones)
                if (t.Principal) return t.Numero;
            return "(sem telefone)";
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Nome: " + nome);
            sb.Append(" | E-mail: " + email);
            sb.Append(" | Nascimento: " + dtNasc);
            sb.Append(" (" + getIdade() + " anos)");
            sb.Append(" | Tel. principal: " + getTelefonePrincipal());
            return sb.ToString();
        }

        // Dois contatos sao iguais quando possuem o mesmo nome (sem diferenciar maiusculas)
        public override bool Equals(object obj)
        {
            Contato outro = obj as Contato;
            if (outro == null) return false;
            return string.Equals(nome, outro.nome, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return nome == null ? 0 : nome.ToLowerInvariant().GetHashCode();
        }
    }

    public class Contatos
    {
        private readonly List<Contato> agenda = new List<Contato>();

        public List<Contato> Agenda { get { return agenda; } }

        public bool adicionar(Contato c)
        {
            if (c == null || agenda.Contains(c)) return false;
            agenda.Add(c);
            return true;
        }

        public Contato pesquisar(Contato c)
        {
            if (c == null) return null;
            int pos = agenda.IndexOf(c);
            return pos >= 0 ? agenda[pos] : null;
        }

        public bool alterar(Contato c)
        {
            if (c == null) return false;
            int pos = agenda.IndexOf(c);
            if (pos < 0) return false;
            agenda[pos] = c;
            return true;
        }

        public bool remover(Contato c)
        {
            if (c == null) return false;
            return agenda.Remove(c);
        }
    }

    // ================== FORMULARIO DO CONTATO ==================
    // Usado para Adicionar (existente == null) e Alterar (existente != null)

    public class ContatoForm : Form
    {
        public Contato Resultado { get; private set; }

        private TextBox txtNome;
        private TextBox txtEmail;
        private DateTimePicker dtpNasc;
        private ComboBox cbTipo;
        private TextBox txtNumero;
        private CheckBox chkPrincipal;
        private Button btnAddTel;
        private Button btnRemTel;
        private ListBox lbTelefones;
        private Button btnOk;
        private Button btnCancelar;

        // Contato de trabalho: guarda os telefones enquanto o formulario esta aberto
        private Contato rascunho;

        public ContatoForm(Contato existente)
        {
            bool editando = existente != null;

            Text = editando ? "Alterar contato" : "Novo contato";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(440, 430);

            // ---- Dados pessoais ----
            Controls.Add(new Label { Text = "Nome:", Left = 15, Top = 20, Width = 80 });
            txtNome = new TextBox { Left = 100, Top = 17, Width = 320 };
            Controls.Add(txtNome);

            Controls.Add(new Label { Text = "E-mail:", Left = 15, Top = 55, Width = 80 });
            txtEmail = new TextBox { Left = 100, Top = 52, Width = 320 };
            Controls.Add(txtEmail);

            Controls.Add(new Label { Text = "Nascimento:", Left = 15, Top = 90, Width = 80 });
            dtpNasc = new DateTimePicker
            {
                Left = 100,
                Top = 87,
                Width = 120,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                MinDate = new DateTime(1900, 1, 1),
                MaxDate = DateTime.Today,
                Value = new DateTime(2000, 1, 1)
            };
            Controls.Add(dtpNasc);

            // ---- Telefones ----
            GroupBox gb = new GroupBox { Text = "Telefones", Left = 15, Top = 125, Width = 405, Height = 240 };
            Controls.Add(gb);

            gb.Controls.Add(new Label { Text = "Tipo:", Left = 10, Top = 27, Width = 45 });
            cbTipo = new ComboBox { Left = 60, Top = 24, Width = 130, DropDownStyle = ComboBoxStyle.DropDownList };
            cbTipo.Items.AddRange(new object[] { "Celular", "Residencial", "Trabalho", "Outro" });
            cbTipo.SelectedIndex = 0;
            gb.Controls.Add(cbTipo);

            gb.Controls.Add(new Label { Text = "Numero:", Left = 205, Top = 27, Width = 55 });
            txtNumero = new TextBox { Left = 262, Top = 24, Width = 130 };
            gb.Controls.Add(txtNumero);

            chkPrincipal = new CheckBox { Text = "Telefone principal", Left = 60, Top = 55, Width = 150 };
            gb.Controls.Add(chkPrincipal);

            btnAddTel = new Button { Text = "Adicionar telefone", Left = 215, Top = 52, Width = 177 };
            btnAddTel.Click += AdicionarTelefone;
            gb.Controls.Add(btnAddTel);

            lbTelefones = new ListBox { Left = 10, Top = 90, Width = 382, Height = 105 };
            gb.Controls.Add(lbTelefones);

            btnRemTel = new Button { Text = "Remover telefone selecionado", Left = 10, Top = 203, Width = 382 };
            btnRemTel.Click += RemoverTelefone;
            gb.Controls.Add(btnRemTel);

            // ---- Botoes ----
            btnOk = new Button { Text = "Salvar", Left = 245, Top = 385, Width = 85 };
            btnOk.Click += Salvar;
            Controls.Add(btnOk);

            btnCancelar = new Button { Text = "Cancelar", Left = 335, Top = 385, Width = 85, DialogResult = DialogResult.Cancel };
            Controls.Add(btnCancelar);

            AcceptButton = btnOk;
            CancelButton = btnCancelar;

            // ---- Carrega os dados no modo alteracao ----
            if (editando)
            {
                txtNome.Text = existente.Nome;
                txtNome.ReadOnly = true; // o nome identifica o contato (Equals)
                txtEmail.Text = existente.Email;
                dtpNasc.Value = new DateTime(existente.DtNasc.Ano, existente.DtNasc.Mes, existente.DtNasc.Dia);

                rascunho = new Contato(existente.Nome, existente.Email, existente.DtNasc);
                foreach (Telefone t in existente.Telefones)
                    rascunho.Telefones.Add(new Telefone(t.Tipo, t.Numero, t.Principal));
            }
            else
            {
                rascunho = new Contato("", "", new Data(1, 1, 2000));
            }

            AtualizarTelefones();
        }

        private void AtualizarTelefones()
        {
            lbTelefones.Items.Clear();
            foreach (Telefone t in rascunho.Telefones)
                lbTelefones.Items.Add(t.ToString());
        }

        private void AdicionarTelefone(object sender, EventArgs e)
        {
            string numero = txtNumero.Text.Trim();
            if (numero == "")
            {
                MessageBox.Show("Informe o numero do telefone.", "Atencao",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNumero.Focus();
                return;
            }

            rascunho.adicionarTelefone(new Telefone(cbTipo.Text, numero, chkPrincipal.Checked));
            txtNumero.Clear();
            chkPrincipal.Checked = false;
            AtualizarTelefones();
            txtNumero.Focus();
        }

        private void RemoverTelefone(object sender, EventArgs e)
        {
            int idx = lbTelefones.SelectedIndex;
            if (idx < 0)
            {
                MessageBox.Show("Selecione um telefone na lista.", "Atencao",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Telefone removido = rascunho.Telefones[idx];
            rascunho.Telefones.RemoveAt(idx);

            // se removeu o principal, o primeiro que sobrar passa a ser o principal
            if (removido.Principal && rascunho.Telefones.Count > 0)
                rascunho.Telefones[0].Principal = true;

            AtualizarTelefones();
        }

        private void Salvar(object sender, EventArgs e)
        {
            string nome = txtNome.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (nome == "")
            {
                MessageBox.Show("Informe o nome.", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                return;
            }
            if (email == "" || !email.Contains("@"))
            {
                MessageBox.Show("Informe um e-mail valido.", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            DateTime d = dtpNasc.Value;
            Contato c = new Contato(nome, email, new Data(d.Day, d.Month, d.Year));
            foreach (Telefone t in rascunho.Telefones)
                c.Telefones.Add(t); // ja respeita a regra do principal

            Resultado = c;
            DialogResult = DialogResult.OK;
            Close();
        }
    }

    // ===================== FORMULARIO PRINCIPAL =====================

    public class MainForm : Form
    {
        private Contatos contatos = new Contatos();

        private TextBox txtBusca;
        private Button btnPesquisar;
        private ListView lvContatos;
        private ListBox lbTelefones;
        private Label lblTotal;
        private Button btnAdicionar;
        private Button btnAlterar;
        private Button btnRemover;
        private Button btnSair;

        public MainForm()
        {
            Text = "Agenda de Contatos";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(760, 500);
            MinimumSize = new Size(700, 450);

            // Lista (centro) - adicionada primeiro, pois o Fill ocupa o que sobrar
            lvContatos = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                MultiSelect = false,
                HideSelection = false
            };
            lvContatos.Columns.Add("Nome", 200);
            lvContatos.Columns.Add("E-mail", 200);
            lvContatos.Columns.Add("Nascimento", 90);
            lvContatos.Columns.Add("Idade", 50);
            lvContatos.Columns.Add("Tel. principal", 140);
            lvContatos.SelectedIndexChanged += delegate { MostrarTelefones(); };
            lvContatos.DoubleClick += delegate { AlterarContato(); };
            Controls.Add(lvContatos);

            // Telefones do contato selecionado (parte de baixo)
            GroupBox gbTel = new GroupBox
            {
                Text = "Telefones do contato selecionado",
                Dock = DockStyle.Bottom,
                Height = 110,
                Padding = new Padding(8, 4, 8, 8)
            };
            lbTelefones = new ListBox { Dock = DockStyle.Fill };
            gbTel.Controls.Add(lbTelefones);
            Controls.Add(gbTel);

            // Barra de botoes
            Panel pnlBotoes = new Panel { Dock = DockStyle.Bottom, Height = 50 };
            btnAdicionar = new Button { Text = "Adicionar", Left = 10, Top = 10, Width = 100, Height = 30 };
            btnAlterar = new Button { Text = "Alterar", Left = 120, Top = 10, Width = 100, Height = 30 };
            btnRemover = new Button { Text = "Remover", Left = 230, Top = 10, Width = 100, Height = 30 };
            btnSair = new Button { Text = "Sair", Top = 10, Width = 100, Height = 30, Anchor = AnchorStyles.Top | AnchorStyles.Right };
            btnSair.Left = pnlBotoes.Width - 110;
            lblTotal = new Label { Left = 345, Top = 17, Width = 200, Text = "0 contato(s)" };
            btnAdicionar.Click += delegate { AdicionarContato(); };
            btnAlterar.Click += delegate { AlterarContato(); };
            btnRemover.Click += delegate { RemoverContato(); };
            btnSair.Click += delegate { Close(); };
            pnlBotoes.Controls.AddRange(new Control[] { btnAdicionar, btnAlterar, btnRemover, lblTotal, btnSair });
            Controls.Add(pnlBotoes);

            // Barra de busca (topo)
            Panel pnlBusca = new Panel { Dock = DockStyle.Top, Height = 45 };
            pnlBusca.Controls.Add(new Label { Text = "Nome:", Left = 10, Top = 15, Width = 45 });
            txtBusca = new TextBox { Left = 60, Top = 12, Width = 260 };
            btnPesquisar = new Button { Text = "Pesquisar", Left = 330, Top = 10, Width = 100, Height = 27 };
            btnPesquisar.Click += delegate { PesquisarContato(); };
            txtBusca.KeyDown += delegate (object s, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    PesquisarContato();
                }
            };
            pnlBusca.Controls.AddRange(new Control[] { txtBusca, btnPesquisar });
            Controls.Add(pnlBusca);

            AtualizarLista();
        }

        // ---------- Utilitarios ----------

        private Contato ContatoSelecionado()
        {
            if (lvContatos.SelectedItems.Count == 0) return null;
            return (Contato)lvContatos.SelectedItems[0].Tag;
        }

        private void Selecionar(Contato c)
        {
            if (c == null) return;
            foreach (ListViewItem item in lvContatos.Items)
            {
                if (c.Equals(item.Tag))
                {
                    item.Selected = true;
                    item.EnsureVisible();
                    lvContatos.Focus();
                    return;
                }
            }
        }

        // Opcao 5 - Listar contatos
        private void AtualizarLista()
        {
            lvContatos.Items.Clear();
            foreach (Contato c in contatos.Agenda)
            {
                ListViewItem item = new ListViewItem(new string[]
                {
                    c.Nome,
                    c.Email,
                    c.DtNasc.ToString(),
                    c.getIdade().ToString(),
                    c.getTelefonePrincipal()
                });
                item.Tag = c;
                lvContatos.Items.Add(item);
            }
            lblTotal.Text = contatos.Agenda.Count + " contato(s)";
            MostrarTelefones();
        }

        private void MostrarTelefones()
        {
            lbTelefones.Items.Clear();
            Contato c = ContatoSelecionado();
            if (c == null) return;
            foreach (Telefone t in c.Telefones)
                lbTelefones.Items.Add(t.ToString());
        }

        // ---------- Opcoes do menu ----------

        // Opcao 1 - Adicionar contato
        private void AdicionarContato()
        {
            using (ContatoForm f = new ContatoForm(null))
            {
                if (f.ShowDialog(this) != DialogResult.OK) return;

                if (contatos.adicionar(f.Resultado))
                {
                    AtualizarLista();
                    Selecionar(f.Resultado);
                }
                else
                {
                    MessageBox.Show("Ja existe um contato com esse nome.", "Atencao",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // Opcao 2 - Pesquisar contato
        private void PesquisarContato()
        {
            string nome = txtBusca.Text.Trim();
            if (nome == "")
            {
                MessageBox.Show("Digite o nome do contato.", "Atencao",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBusca.Focus();
                return;
            }

            // Contato "modelo" so com o nome (Equals compara pelo nome)
            Contato achado = contatos.pesquisar(new Contato(nome, "", new Data(1, 1, 2000)));
            if (achado == null)
            {
                MessageBox.Show("Contato nao encontrado.", "Pesquisa",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Selecionar(achado);
        }

        // Opcao 3 - Alterar contato
        private void AlterarContato()
        {
            Contato atual = ContatoSelecionado();
            if (atual == null)
            {
                MessageBox.Show("Selecione um contato na lista.", "Atencao",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (ContatoForm f = new ContatoForm(atual))
            {
                if (f.ShowDialog(this) != DialogResult.OK) return;

                if (contatos.alterar(f.Resultado))
                {
                    AtualizarLista();
                    Selecionar(f.Resultado);
                }
                else
                {
                    MessageBox.Show("Nao foi possivel alterar o contato.", "Erro",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Opcao 4 - Remover contato
        private void RemoverContato()
        {
            Contato c = ContatoSelecionado();
            if (c == null)
            {
                MessageBox.Show("Selecione um contato na lista.", "Atencao",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult r = MessageBox.Show("Remover o contato \"" + c.Nome + "\"?", "Confirmar",
                                             MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            if (contatos.remover(c))
                AtualizarLista();
            else
                MessageBox.Show("Nao foi possivel remover o contato.", "Erro",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ======================== MAIN ========================

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
