using System;
using System.Reflection;
using System.Windows.Forms;

namespace CartaDeCotacao
{
    public partial class Sobre : Form
    {
        public Sobre()
        {
            InitializeComponent();
        }

        private void Sobre_Load(object sender, EventArgs e)
        {
            label7.Text = "Versão do parcelamento: " + WebFile.Extras.GetName().Version.ToString(2);
            label6.Text = "Versão do programa: " + Application.ProductVersion;
            label5.Text = "Deselvolvido por Lucas Mezzomo Fachinetto\r\nCopyright © 2014-" + DateTime.Now.Year.ToString();
        }
    }
}
