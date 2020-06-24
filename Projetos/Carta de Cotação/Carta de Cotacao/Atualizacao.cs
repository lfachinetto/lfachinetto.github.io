using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;

namespace CartaDeCotacao
{
    public partial class Atualizacao : Form
    {
        private string _url;

        private string _file = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Carta de Cotação\setup.exe";
        public string LocalFile { get { return _file; } set { _file = value; } }
        
        public Atualizacao(string tipo)
        {
            InitializeComponent();
            if (tipo == "normal")
                _url = @"https://drive.google.com/uc?export=download&id=1qcaYe4vNv_FAi_-yITpLXAuF_BCqxBzu";
            if (tipo == "teste")
                _url = @"https://drive.google.com/uc?export=download&id=1YMFnsGqKe6T5m4Djue_hx0bem-Lx4bKO";
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //Faz o download do arquivo
            using (WebClient client = new WebClient())
            {
                //Event handlers do WebClient
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                try
                {
                    client.DownloadFileAsync(new Uri(_url), _file);//download Async
                }
                catch (Exception)
                {
                    MessageBox.Show("Ocorreu um problema ao atualizar o programa.", AppDomain.CurrentDomain.FriendlyName.Replace(".exe", ""), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.Cancel;
                }
            }
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = e.ProgressPercentage.ToString() + "%";

            //A ProgressBar é bugada e tem problema de não ir até o fim, fazer isso melhora:
            if (e.ProgressPercentage > 0)
                progressBar1.Value = e.ProgressPercentage - 1;

            //Se o usuário fechar a janela, cancela o download
            if (this.DialogResult == DialogResult.Cancel)
                (sender as WebClient).CancelAsync();//sender é o objeto que chamou esse metódo (void), que no caso foi o WebClient client
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)//retorna DialogResult OK se completado o download sem erro
            {
                progressBar1.Maximum = 1;//só pra garantir mostrar 100%
                label1.Text = "100%";//idem
                this.Refresh();//idem

                this.DialogResult = DialogResult.OK;//isso fecha a janela automagicamente
            }
            else
            {
                MessageBox.Show("Ocorreu um problema ao atualizar o programa.", AppDomain.CurrentDomain.FriendlyName.Replace(".exe", ""), MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;//isso fecha a janela automagicamente
            }
        }

        public static bool VerificarAtualizacoes(Status atual)
        {
            bool disp = false;
            //Citar maior versão da lista de versões
            if (atual.VersoesFunc.Count != 0 && atual.VersoesFunc[atual.VersoesFunc.Count - 1].VersionNum != Application.ProductVersion)
                disp = true;
            return disp;
        }
    }
}