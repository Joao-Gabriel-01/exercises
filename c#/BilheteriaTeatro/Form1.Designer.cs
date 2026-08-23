namespace BilheteriaTeatro
{
    partial class Form1
    {
        /// <summary>
        /// Variável necessária ao Designer.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpa os recursos em uso.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Boilerplate padrão do Designer. Nenhum componente de interface
        // (poltronas, botão Faturamento, label de resultado) é criado aqui:
        // todos são gerados dinamicamente em Form1.cs, conforme exigido
        // pelo enunciado da atividade.
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        }
    }
}
