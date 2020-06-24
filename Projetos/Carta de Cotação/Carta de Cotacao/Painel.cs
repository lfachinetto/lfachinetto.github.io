using System;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Cryptography;
using System.IO;
using System.Collections.Generic;

namespace CartaDeCotacao
{
    public partial class Painel : Form
    {
        private Status atual = null;
        public Painel()
        {
            InitializeComponent();
        }

        private void Painel_Load(object sender, EventArgs e)
        {
            try
            {
                Process.Start(@"C:\Program Files\Google\Drive\googledrivesync.exe");
            }
            catch(Exception)
            {
                MessageBox.Show("Não foi possível abrir o programa Google Drive.",
                "Painel de Controle CDC",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
                Close();
                return;
            }

            if (!File.Exists(WebFile.DirDrive))
                File.Create(WebFile.DirDrive).Close();

            if (new FileInfo(WebFile.DirDrive).Length == 0)
                atual = new Status();
            else
                atual = WebFile.StatusDrive;

            foreach (Seguradora s in Seguradora.GeraSeguradoras())
            {
                atual.Segs.Find(sl => sl.Nome == s.Nome).Observacoes = s.Observacoes;
                atual.Segs.Find(sl => sl.Nome == s.Nome).ObservacoesMC = s.ObservacoesMC;
                atual.Segs.Find(sl => sl.Nome == s.Nome).Moto = s.Moto;
                atual.Segs.Find(sl => sl.Nome == s.Nome).Caminhao = s.Caminhao;
            }

            checkBox1.DataBindings.Add("Checked", atual.Segs[1], "Disp");
            checkBox2.DataBindings.Add("Checked", atual.Segs[2], "Disp");
            checkBox3.DataBindings.Add("Checked", atual.Segs[3], "Disp");
            checkBox4.DataBindings.Add("Checked", atual.Segs[4], "Disp");
            checkBox5.DataBindings.Add("Checked", atual.Segs[5], "Disp");
            checkBox6.DataBindings.Add("Checked", atual.Segs[6], "Disp");
            checkBox7.DataBindings.Add("Checked", atual.Segs[7], "Disp");
            checkBox8.DataBindings.Add("Checked", atual.Segs[8], "Disp");
            checkBox9.DataBindings.Add("Checked", atual.Segs[9], "Disp");
            checkBox10.DataBindings.Add("Checked", atual.Segs[10], "Disp");
            checkBox11.DataBindings.Add("Checked", atual.Segs[11], "Disp");
            
            checkBox13.DataBindings.Add("Checked", atual, "ProgDisp");
            checkBox14.DataBindings.Add("Checked", atual, "Fipe");
            dataGridView1.DataSource = atual.VersoesFunc;
            textBox1.DataBindings.Add("Text", atual, "Mensagem");
            textBox2.DataBindings.Add("Text", atual, "CorMsg");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] data = Encoding.Unicode.GetBytes(textBox4.Text);
            byte[] result;
            SHA512 shaM = new SHA512Managed();
            textBox4.Text = "";
            result = shaM.ComputeHash(data);
            string senha = "Î'¬K‘ùZ×ñŒìe`yÊa)(ö“Dð4Ä=G³?;oó€V„]E ªÒ=-ƒS+¦/ÀÒ?­ZmJí>1jö:";

            if (Encoding.Default.GetString(result) == senha)
            {
                WebFile.StatusDrive = atual;
                MessageBox.Show("Status salvo com sucesso.",
                "Painel de Controle CDC",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("Erro de autenticação!", "Painel de Controle CDC",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
            }            
        }
    }
}