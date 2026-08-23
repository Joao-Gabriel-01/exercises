using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace BilheteriaTeatro
{
   
    public partial class Form1 : Form
    {
      
        private const int FILEIRAS = 15;
        private const int COLUNAS = 40;

       
        private const int TAMANHO_BOTAO = 22;
        private const int ESPACAMENTO = 2;
        private const int MARGEM_PAINEL = 15;

        private enum EstadoPoltrona
        {
            Vaga,
            Inteira,
            Meia
        }

        private class Poltrona
        {
            public int Fileira;   
            public int Coluna;    
            public EstadoPoltrona Estado;
            public Button Botao;
        }

       
        private readonly Poltrona[,] poltronas = new Poltrona[FILEIRAS, COLUNAS];

        
        private Panel painelPoltronas;
        private Button btnFaturamento;
        private Label lblFaturamento;
        private ToolTip dicaPoltrona;

        public Form1()
        {
            InitializeComponent();
            ConfigurarFormulario();
            CriarMapaDePoltronas();
            CriarControlesDeFaturamento();
        }

        private void ConfigurarFormulario()
        {
            Text = "Projeto Teatro - Bilheteria";
            StartPosition = FormStartPosition.CenterScreen;

            int larguraPainel = COLUNAS * (TAMANHO_BOTAO + ESPACAMENTO) + ESPACAMENTO;
            int alturaPainel = FILEIRAS * (TAMANHO_BOTAO + ESPACAMENTO) + ESPACAMENTO;

            ClientSize = new Size(
                larguraPainel + 2 * MARGEM_PAINEL,
                alturaPainel + 2 * MARGEM_PAINEL + 90);

            dicaPoltrona = new ToolTip();
        }

       
        private void CriarMapaDePoltronas()
        {
            int larguraPainel = COLUNAS * (TAMANHO_BOTAO + ESPACAMENTO) + ESPACAMENTO;
            int alturaPainel = FILEIRAS * (TAMANHO_BOTAO + ESPACAMENTO) + ESPACAMENTO;

            painelPoltronas = new Panel
            {
                Location = new Point(MARGEM_PAINEL, MARGEM_PAINEL),
                Size = new Size(larguraPainel, alturaPainel),
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(painelPoltronas);

            for (int fileira = 0; fileira < FILEIRAS; fileira++)
            {
                for (int coluna = 0; coluna < COLUNAS; coluna++)
                {
                    Button botao = new Button
                    {
                        Location = new Point(
                            ESPACAMENTO + coluna * (TAMANHO_BOTAO + ESPACAMENTO),
                            ESPACAMENTO + fileira * (TAMANHO_BOTAO + ESPACAMENTO)),
                        Size = new Size(TAMANHO_BOTAO, TAMANHO_BOTAO),
                        Text = (coluna + 1).ToString(),
                        Font = new Font("Segoe UI", 6f),
                        Margin = Padding.Empty,
                        Tag = (fileira, coluna),
                        UseVisualStyleBackColor = false
                    };
                    botao.Click += BotaoPoltrona_Click;

                    Poltrona poltrona = new Poltrona
                    {
                        Fileira = fileira,
                        Coluna = coluna,
                        Estado = EstadoPoltrona.Vaga,
                        Botao = botao
                    };

                    poltronas[fileira, coluna] = poltrona;
                    AtualizarVisualPoltrona(poltrona);

                    dicaPoltrona.SetToolTip(botao,
                        $"Fileira {fileira + 1}, Poltrona {coluna + 1} - Valor cheio: {FormatarMoeda(ValorCheioFileira(fileira))}");

                    painelPoltronas.Controls.Add(botao);
                }
            }
        }


        private void CriarControlesDeFaturamento()
        {
            btnFaturamento = new Button
            {
                Text = "Faturamento",
                Location = new Point(MARGEM_PAINEL, painelPoltronas.Bottom + 15),
                Size = new Size(140, 30)
            };
            btnFaturamento.Click += BtnFaturamento_Click;
            Controls.Add(btnFaturamento);

            lblFaturamento = new Label
            {
                Text = "Qtde de lugares ocupados: 0\r\nValor da bilheteria: R$ 0,00",
                Location = new Point(btnFaturamento.Right + 15, painelPoltronas.Bottom + 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            Controls.Add(lblFaturamento);
        }

        private void BotaoPoltrona_Click(object sender, EventArgs e)
        {
            Button botao = (Button)sender;
            var posicao = ((int Fileira, int Coluna))botao.Tag;

            if (!CoordenadaValida(posicao.Fileira, posicao.Coluna))
            {
                return; 
            }

            Poltrona poltrona = poltronas[posicao.Fileira, posicao.Coluna];

            if (poltrona.Estado == EstadoPoltrona.Vaga)
            {
                EstadoPoltrona? tipoEscolhido = PerguntarTipoDeEntrada(posicao.Fileira, posicao.Coluna);

                if (tipoEscolhido.HasValue)
                {
                    poltrona.Estado = tipoEscolhido.Value;
                    AtualizarVisualPoltrona(poltrona);
                }
            }
            else
            {
                string tipo = poltrona.Estado == EstadoPoltrona.Inteira ? "inteira" : "meia entrada";

                MessageBox.Show(
                    $"A poltrona Fileira {posicao.Fileira + 1}, Poltrona {posicao.Coluna + 1} já está ocupada ({tipo}).",
                    "Poltrona ocupada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }


        private EstadoPoltrona? PerguntarTipoDeEntrada(int fileira, int coluna)
        {
            decimal valorCheio = ValorCheioFileira(fileira);
            decimal valorMeia = valorCheio / 2m;

            DialogResult resultado = MessageBox.Show(
                $"Reservar poltrona - Fileira {fileira + 1}, Poltrona {coluna + 1}\n\n" +
                $"Inteira: {FormatarMoeda(valorCheio)}\n" +
                $"Meia entrada: {FormatarMoeda(valorMeia)}\n\n" +
                "Sim = Inteira\nNão = Meia entrada\nCancelar = Não reservar",
                "Tipo de reserva",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                return EstadoPoltrona.Inteira;
            }

            if (resultado == DialogResult.No)
            {
                return EstadoPoltrona.Meia;
            }

            return null; 
        }

      
        private void AtualizarVisualPoltrona(Poltrona poltrona)
        {
            switch (poltrona.Estado)
            {
                case EstadoPoltrona.Vaga:
                    poltrona.Botao.BackColor = Color.WhiteSmoke;
                    poltrona.Botao.ForeColor = Color.Black;
                    break;

                case EstadoPoltrona.Inteira:
                    poltrona.Botao.BackColor = Color.IndianRed;
                    poltrona.Botao.ForeColor = Color.White;
                    break;

                case EstadoPoltrona.Meia:
                    poltrona.Botao.BackColor = Color.Orange;
                    poltrona.Botao.ForeColor = Color.Black;
                    break;
            }
        }

     
        private void BtnFaturamento_Click(object sender, EventArgs e)
        {
            int qtdeOcupadas = 0;
            decimal valorTotal = 0m;

            for (int fileira = 0; fileira < FILEIRAS; fileira++)
            {
                decimal valorCheio = ValorCheioFileira(fileira);

                for (int coluna = 0; coluna < COLUNAS; coluna++)
                {
                    Poltrona poltrona = poltronas[fileira, coluna];

                    if (poltrona.Estado == EstadoPoltrona.Inteira)
                    {
                        qtdeOcupadas++;
                        valorTotal += valorCheio;
                    }
                    else if (poltrona.Estado == EstadoPoltrona.Meia)
                    {
                        qtdeOcupadas++;
                        valorTotal += valorCheio / 2m;
                    }
                }
            }

            lblFaturamento.Text =
                $"Qtde de lugares ocupados: {qtdeOcupadas}\r\n" +
                $"Valor da bilheteria: {FormatarMoeda(valorTotal)}";
        }


        private decimal ValorCheioFileira(int fileira)
        {
            int fileiraHumana = fileira + 1; 

            if (fileiraHumana >= 1 && fileiraHumana <= 5)
            {
                return 50m;
            }

            if (fileiraHumana >= 6 && fileiraHumana <= 10)
            {
                return 30m;
            }

           
            return 15m;
        }

      
        // Validação de coordenadas
        private bool CoordenadaValida(int fileira, int coluna)
        {
            return fileira >= 0 && fileira < FILEIRAS
                && coluna >= 0 && coluna < COLUNAS;
        }

        // Formatação de valores monetários no padrão "R$ 9999,99"
        private static string FormatarMoeda(decimal valor)
        {
            return "R$ " + valor.ToString("0.00", CultureInfo.GetCultureInfo("pt-BR"));
        }
    }
}
