using System;
using System.Windows.Forms;

namespace CartaDeCotacao
{
    public partial class Informar : Form
    {
        string anexo;

        public Informar()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (WebFile.HasInternet())
            {
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                button1.Enabled = false;
                button2.Enabled = false;
                button1.Text = "Enviando...";
                Cursor = Cursors.WaitCursor;

                await Relatório.EnviarErro(textBox1.Text, textBox2.Text, anexo);                
                Close();
            }
            else
                MessageBox.Show("Sem conexão com a internet,\r\npor favor tente mais tarde.", "Carta de Cotação", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string PdfFolder = Home.preferencias.PastaPDF;
            if (PdfFolder == "")
            {
                PdfFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\";
            }
                openFileDialog1.InitialDirectory = PdfFolder;
                openFileDialog1.Filter = "Arquivo PDF|*.pdf";
                openFileDialog1.Title = "Abrir";
                openFileDialog1.FileName = "";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (openFileDialog1.FileName != "")
                    {
                        if (openFileDialog1.SafeFileName.Length <= 35)
                            label3.Text = "Cotação anexada (" + openFileDialog1.SafeFileName + ")";
                        else
                            label3.Text = "Cotação anexada";

                        anexo = openFileDialog1.FileName;
                    }
                }
        }

        private void Erro_Load(object sender, EventArgs e) => textBox1.Text = Home.preferencias.NomeFuncionario;
    }
}
