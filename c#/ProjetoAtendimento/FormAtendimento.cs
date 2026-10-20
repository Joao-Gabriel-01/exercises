namespace ProjetoAtendimento
{
    public class FormAtendimento : Form
    {
        private Senhas senhas = new Senhas();
        private Guiches guiches = new Guiches();

        private Button btnGerar;
        private Button btnAdicionar;
        private Button btnChamar;
        private NumericUpDown nudQuantidade;
        private ComboBox cmbGuiche;
        private ListBox lstSenhas;
        private ListBox lstAtendimentos;

        public FormAtendimento()
        {
            Text = "Central de Atendimento";
            ClientSize = new Size(640, 420);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            // Linha 1: gerar senha e adicionar guichês
            btnGerar = new Button { Text = "Gerar", Location = new Point(20, 20), Size = new Size(110, 30) };
            btnGerar.Click += btnGerar_Click;

            Label lblQtd = new Label { Text = "Qtd. guichês:", Location = new Point(160, 26), AutoSize = true };
            nudQuantidade = new NumericUpDown
            {
                Location = new Point(245, 23),
                Size = new Size(60, 25),
                Minimum = 1,
                Maximum = 50,
                Value = 1
            };
            btnAdicionar = new Button { Text = "Adicionar", Location = new Point(320, 20), Size = new Size(110, 30) };
            btnAdicionar.Click += btnAdicionar_Click;

            // Linha 2: escolher guichê e chamar
            Label lblGuiche = new Label { Text = "Guichê:", Location = new Point(20, 76), AutoSize = true };
            cmbGuiche = new ComboBox
            {
                Location = new Point(75, 72),
                Size = new Size(130, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbGuiche.SelectedIndexChanged += (s, e) => atualizarListas();

            btnChamar = new Button { Text = "Chamar", Location = new Point(225, 69), Size = new Size(110, 30) };
            btnChamar.Click += btnChamar_Click;

            // Listas
            Label lblSenhas = new Label { Text = "Senhas (aguardando)", Location = new Point(20, 115), AutoSize = true };
            lstSenhas = new ListBox { Location = new Point(20, 137), Size = new Size(290, 260) };

            Label lblAtend = new Label { Text = "Atendimentos do guichê selecionado", Location = new Point(330, 115), AutoSize = true };
            lstAtendimentos = new ListBox { Location = new Point(330, 137), Size = new Size(290, 260) };

            Controls.AddRange(new Control[]
            {
                btnGerar, lblQtd, nudQuantidade, btnAdicionar,
                lblGuiche, cmbGuiche, btnChamar,
                lblSenhas, lstSenhas, lblAtend, lstAtendimentos
            });
        }

        private void btnGerar_Click(object sender, EventArgs e)
        {
            senhas.gerar();
            atualizarListas();
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            int quantidade = (int)nudQuantidade.Value;
            for (int i = 0; i < quantidade; i++)
            {
                int novoId = guiches.ListaGuiches.Count + 1;
                guiches.adicionar(new Guiche(novoId));
            }

            int selecionado = cmbGuiche.SelectedIndex;
            cmbGuiche.Items.Clear();
            foreach (Guiche g in guiches.ListaGuiches)
                cmbGuiche.Items.Add(g);

            cmbGuiche.SelectedIndex = selecionado >= 0 ? selecionado : 0;
            atualizarListas();
        }

        private void btnChamar_Click(object sender, EventArgs e)
        {
            Guiche guiche = cmbGuiche.SelectedItem as Guiche;
            if (guiche == null)
            {
                MessageBox.Show("Cadastre e selecione um guichê.", "Atenção");
                return;
            }

            if (!guiche.chamar(senhas.FilaSenhas))
                MessageBox.Show("Não há senhas na fila de espera.", "Atenção");

            atualizarListas();
        }

        private void atualizarListas()
        {
            lstSenhas.Items.Clear();
            foreach (Senha s in senhas.FilaSenhas)
                lstSenhas.Items.Add(s.dadosParciais());

            lstAtendimentos.Items.Clear();
            Guiche guiche = cmbGuiche.SelectedItem as Guiche;
            if (guiche != null)
            {
                foreach (Senha s in guiche.Atendimentos)
                    lstAtendimentos.Items.Add(s.dadosCompletos());
            }
        }
    }
}
