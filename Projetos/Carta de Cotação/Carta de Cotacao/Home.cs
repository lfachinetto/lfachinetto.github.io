using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;
using System.Diagnostics;
using System.IO;
using iTextSharp.text.pdf;


namespace CartaDeCotacao
{
    public partial class Home : Form
    {
        private static Status status = new Status();
        public static Preferencias preferencias = new Preferencias();
        public Fill fill = new Fill();
        private string tipoveiculo;
        private bool fipe;
        public static bool excedeuLinhas;

        public Home()
        {
            InitializeComponent();
            try
            {
                Relatório.Home = this;
                for (int i = DateTime.Now.Year; i >= 1981; i--)
                    anoFabric.Items.Add(i.ToString());
                anoFabric.SelectedIndex = -1;
                //tipo1.Checked = true;
                data.Value = DateTime.Now;
                danosMateriais.Text =
                    danosCorporais.Text =
                        danosMorais.Text = APPMorte.Text = APPInvalidez.Text = equipamento.Text = carroceria.Text =
                            franqBasica.Text =
                                vistaBasica.Text = franqReduz.Text = vistaReduz.Text = valorFipe.Text = "0";
                labelValorFipe.Text = "Valor na Fipe no mês " + DateTime.Today.ToString("M/yyyy") + ":";
                List<FipeModel> modelos = new List<FipeModel>();
                anoModeloFipe.DataSource = modelos;
            }
            catch (Exception ex)
            {
                Relatório.ExHandler(ex, "Home_Load");
            }
        }
        
        private async void Home_Load(object sender, EventArgs e)
        {
            try
            {
                //if (!File.Exists(WebFile.AppData + "Status.dat") && !await WebFile.HasInternet())
                //throw new Exception("Sem internet e Status não encontrado");

                await WebFile.CheckAppFilesAsync();
                preferencias = WebFile.Prefs;                
                status = WebFile.Status;
                tipo1.Checked = true;
                opcaoNomeFuncionario.Checked = true;
                //seguradora.DataSource = status.Segs;
                if (preferencias.Validade)
                    opcaoValidadeCot.Checked = true;
                else
                {
                    opcaoValidadeCot.Checked = true;
                    opcaoValidadeCot.Checked = false;
                }

                if (await Relatório.PrimeiroUso())
                {
                    Relatório.MoverDadosAntigos();
                    if (novaVersaoLabel2.Text != "" && novaVersaoLabel2.Text != " ")
                    {
                        novaVersao.Location = new Point(171, 207);
                        novaVersao.Size = new Size(503, 229);
                        novaVersao.Visible = true;
                    }
                }

                if (status.VersoesFunc.FirstOrDefault(v => v.VersionNum == Application.ProductVersion) == null)
                    Relatório.PostMessage(new Relatório(5));

                if (Atualizacao.VerificarAtualizacoes(status))
                    AtualizarPrograma();

                if (status.VersoesFunc.FirstOrDefault(v => v.VersionNum == Application.ProductVersion) == null)
                    Close();
                
                if (!status.ProgDisp)
                {
                    Relatório.PostMessage(new Relatório(6));
                    Environment.Exit(1);
                }

                if (status.Mensagem != "")
                {
                    mensagem.ForeColor = Color.FromName(status.CorMsg);
                    mensagem.Text = status.Mensagem.Replace("|", "\r\n");
                    mensagem.Visible = true;
                }

                if (!status.Fipe)
                {
                    modoManual.Checked = true;
                    modoManual.Visible = false;
                    fipe = false;
                }
                else
                    fipe = true;
                if (File.Exists(WebFile.AppData + "Restauração.bin"))
                {
                    fill = (Fill) WebFile.Deserialize(WebFile.AppData + "Restauração.bin");

                    if (File.GetCreationTime(WebFile.AppData + "Restauração.bin").Date == DateTime.Now.Date &&
                        fill.Cliente != "")
                        if (
                            MessageBox.Show(
                                "Você deseja restaurar o preenchimento salvo\r\nde quando ocorreu um erro no programa?",
                                "Carta de Cotação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                            DialogResult.Yes)
                        {
                            mensagem.ForeColor = Color.Blue;
                            mensagem.Text =
                                "Verifique se o preenchimento restaurado foi\r\ninserido corretamente antes de gerar a carta.";
                            PreencherFill();
                        }

                    File.Delete(WebFile.AppData + "Restauração.bin");
                }
                //Código para destacar novos recursos:
                /*DateTime dataArquivo = File.GetLastWriteTime(WebFile.AppData + "Versão.dat");
                TimeSpan dif = DateTime.Now - dataArquivo;
                if (dif.Days <= 3)
                {
                    label1.Visible = true;
                    arquivoToolStripMenuItem.ForeColor = Color.Blue;
                    editarCartaToolStripMenuItem.ForeColor = Color.Blue;
                }*/
                await Relatório.VerificarEnvio();
                exemploToolStripMenuItem.Enabled = true;
                limparToolStripMenuItem.Enabled = true;
                botaoLimpar.Enabled = true;
                seguradora.Enabled = true;
                carregando.Visible = false;
            }
            catch (Exception ex)
            {
                if (File.Exists(WebFile.AppData + "Status.dat"))
                    Relatório.ExHandler(ex, "Home_Load");
                else
                {
                    MessageBox.Show("ATENÇÃO:\r\nNão foi possível baixar os arquivos necessários para o programa funcionar.\r\n"
                                    + "Verifique se seu Windows está atualizado (IMPORTANTE).\r\n"
                                    + "Verifique sua conexão com a internet ou se o acesso à internet pelo programa está sendo bloqueado.\r\n"                                   
                                    + "\r\nErro interno:\r\n"
                                    + ex.Message, "Carta de Cotação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Relatório.AdicionarAoRelatorio("Status não encontrado");
                    Environment.Exit(1);
                }
            }
        }

        private void AtualizarPrograma()
        {
            if (MessageBox.Show("Uma nova versão do Carta de Cotação está disponível,\r\nvocê deseja atualizá-lo agora?",
                    "Carta de Cotação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Atualizacao downloader = new Atualizacao("normal");
                if (downloader.ShowDialog() == DialogResult.OK) //OK => download não foi cancelado e terminou sem erros
                {
                    Process.Start(downloader.LocalFile); //Executa o arquivo nessa linha
                    Close();
                }
            }
        }

        private void sobreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Relatório.AdicionarAoRelatorio("Sobre aberto");
            Sobre about = new Sobre();
            about.Show();
        }

        private void fecharToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Relatório.AdicionarAoRelatorio("Fechar do menu pressionado");
            Close();
        }

        private void gerarToolStripMenuItem_Click(object sender, EventArgs e) => gerar.PerformClick();

        private void limparToolStripMenuItem_Click(object sender, EventArgs e) => Limpar();

        private void pastaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Relatório.AdicionarAoRelatorio("Botão abrir última pasta pressionado");
            var pasta = preferencias.PastaPDF;
            if (pasta != "")
                Process.Start(pasta);
            else
                MessageBox.Show("Nenhuma pasta está registrada ainda.", "Carta de Cotação", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
        }

        private void textBoxs_KeyDown(object sender, KeyEventArgs e) => CorrigirBackSpaceFormatacaoMoeda(sender as TextBox, e);

        private void textBoxs_TextChanged(object sender, EventArgs e) => FormatarMoeda(sender as TextBox);

        private void FormatarMoeda(TextBox sender)
        {
            int pos = sender.SelectionStart;
            int len = sender.TextLength;

            string digitos =
                new string(
                    sender.Text.Remove(0, sender.Text.IndexOf("$") + 1).Where(c => "0123456789".Contains(c)).ToArray());

            decimal result;
            if (decimal.TryParse(digitos, NumberStyles.Currency, CultureInfo.CurrentCulture, out result))
                sender.Text = (result/100m).ToString("C", CultureInfo.CurrentCulture);
            else
                sender.Text = "0";

            sender.SelectionStart = pos + sender.TextLength - len;
        }

        private void CorrigirBackSpaceFormatacaoMoeda(TextBox sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
                if (sender.SelectionStart != 0 && ".,".Contains(sender.Text[sender.SelectionStart - 1]))
                {
                    sender.SelectionStart--;
                    e.SuppressKeyPress = true;
                }
        }

        private void opcaoValidadeCot_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (opcaoValidadeCot.Checked)
                {
                    //Código refletido na função limpar
                    dataValidadeCot.Enabled = true;
                    DateTime hoje = DateTime.Now;
                    DateTime comdias = hoje.AddDays(5);
                    dataValidadeCot.Value = comdias;
                }
                else
                {
                    dataValidadeCot.Enabled = false;
                }
                preferencias.Validade = opcaoValidadeCot.Checked;
                WebFile.Prefs = preferencias;
            }
            catch (Exception ex)
            {
                Relatório.ExHandler(ex, "Validade pressionada antes do status ser carregado");
            }
        }

        private void seguradora_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Teste se a seguradora está com a tabela correta
            if (seguradora.Text != "" && nome.Text != "Teste 123" && !(seguradora.SelectedItem as Seguradora).Disp)
            {
                MessageBox.Show(
                    "A seguradora " + seguradora.Text +
                    " mudou o seu parcelamento,\r\nem breve o programa será atualizado.", "Carta de Cotação",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                seguradora.SelectedIndex = 0;
                //Precisa ser depois da mensagem para não sumir o nome da seguradora antes dele aparecer na mensagem
                return;
            }

            //Características da seguradora
            if (seguradora.Text == "Mapfre" && tipoveiculo == "Automóvel")
                opcaoDebitoBB.Visible = true;
            else
            {
                opcaoDebitoBB.Visible = false;
                opcaoDebitoBB.Checked = false;
            }

            if (seguradora.Text == "Itaú")
            {
                labelAPPMorte.Text = "APP:";
                labelAPPInvalidez.Visible = false;
                APPInvalidez.Visible = false;
                APPInvalidez.Text = "000";
            }
            else
            {
                labelAPPMorte.Text = "APP Morte:";
                labelAPPInvalidez.Visible = true;
                APPInvalidez.Visible = true;
            }

            //Texto das observações

            observacoes.Clear();
            if (tipoveiculo == "Automóvel")
                foreach (string s in (seguradora.SelectedItem as Seguradora).Observacoes)
                {
                    observacoes.Text += s;
                    if ((seguradora.SelectedItem as Seguradora).Observacoes.IndexOf(s) !=
                        (seguradora.SelectedItem as Seguradora).Observacoes.Count - 1)
                        observacoes.Text += "\r\n";
                }
            else
                observacoes.Text = (seguradora.SelectedItem as Seguradora).ObservacoesMC;
        }

        private void metadeBasica_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Relatório.AdicionarAoRelatorio("Metade da básica pressionado");
            try
            {
                decimal metade = decimal.Parse(franqBasica.Text, NumberStyles.Currency, CultureInfo.CurrentCulture)/2;
                franqReduz.Text = Math.Round(metade, 2).ToString("C", CultureInfo.CurrentCulture);
                //“C” de Currency (Moeda)
            }
            catch (FormatException)
            {
                MessageBox.Show(
                    "Não foi possível calcular a metade\r\nda franquia básica, verifique se\r\npreencheu corretamente os campos\r\ne tente novamente!",
                    "Carta de Cotação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void opcaoNomeFuncionario_CheckedChanged(object sender, EventArgs e)
        {
            if (opcaoNomeFuncionario.Checked)
            {
                Relatório.AdicionarAoRelatorio("Nome do funcionário selecionado");
                nomeFuncionario.Enabled = true;
                nomeFuncionario.Text = preferencias.NomeFuncionario;
            }
            else
            {
                Relatório.AdicionarAoRelatorio("Nome do funcionário desselecionado");
                nomeFuncionario.Enabled = false;
                preferencias.NomeFuncionario = nomeFuncionario.Text;
                WebFile.Prefs = preferencias;
                nomeFuncionario.Text = "";
            }
        }

        private void observacoes_TextChanged(object sender, EventArgs e) => VerificarLinhas();

        private void VerificarLinhas()
        {
            int maximo = 6;
            if (franqBasica.Text == "R$ 0,00" && vistaBasica.Text == "R$ 0,00")
                maximo = 14;
            if (franqReduz.Text == "R$ 0,00" && vistaReduz.Text == "R$ 0,00")
                maximo = 14;
            if (opcaoRCF.Checked)
                maximo = 16;
            if (tipoveiculo == "Caminhão")
                maximo--;

            maximo--;
            if (observacoes.Lines.Length > maximo)
                excedeuLinhas = true;
            else
                excedeuLinhas = false;
            maximo++;

            if (observacoes.Lines.Length > maximo)
            {
                mensagemObservacoes.Visible = true;
                mensagemObservacoes.Text = string.Format("As observações excedem o limite de {0} linhas.", maximo);
            }
            else
            {
                mensagemObservacoes.Visible = false;
            }
        }

        private void tipo1_CheckedChanged(object sender, EventArgs e)
        {
            if (tipo1.Checked)
            {
                tipoveiculo = "Automóvel";
                VerificarLinhas();
                opcaoVD.Visible = false;
                opcaoVD.Checked = false;
                seguradora.DataSource = status.Segs.Where(s => s.Nome.Length >= 0).ToList();
                labelEquipamento.Visible = false;
                equipamento.Visible = false;
                labelCarroceria.Visible = false;
                carroceria.Visible = false;

                groupCotacao.Size = new Size(346, 314);
            }
        }

        private void tipo2_CheckedChanged(object sender, EventArgs e)
        {
            if (tipo2.Checked)
            {
                tipoveiculo = "Moto";
                VerificarLinhas();
                opcaoVD.Visible = false;
                opcaoVD.Checked = false;
                seguradora.DataSource = status.Segs.Where(s => s.Moto).ToList();

                labelEquipamento.Visible = false;
                equipamento.Visible = false;
                labelCarroceria.Visible = false;
                carroceria.Visible = false;

                groupCotacao.Size = new Size(346, 314);
            }
        }

        private void tipo3_CheckedChanged(object sender, EventArgs e)
        {
            if (tipo3.Checked)
            {
                tipoveiculo = "Caminhão";
                VerificarLinhas();
                opcaoVD.Visible = true;
                seguradora.DataSource = status.Segs.Where(s => s.Caminhao).ToList();

                labelEquipamento.Visible = true;
                equipamento.Visible = true;
                labelCarroceria.Visible = true;
                carroceria.Visible = true;

                groupCotacao.Size = new Size(346, 356);
            }
        }

        private void opcaoRCF_CheckedChanged(object sender, EventArgs e)
        {
            if (opcaoRCF.Checked)
            {
                Relatório.AdicionarAoRelatorio("RCF");
                labelVistaBasica.Text = "Valor à vista se RCF:";
                labelPorcentFipe.Visible = porcentFipe.Visible = simboloPorcentFipe.Visible = false;

                labelFranqBasica.Visible =
                    franqBasica.Visible = labelFranqReduz.Visible = franqReduz.Visible = metadeBasica.Visible =
                        labelVistaReduz.Visible = vistaReduz.Visible = false;

                labelVistaBasica.Location = new Point(154, 78);
                vistaBasica.Location = new Point(157, 94);
            }
            else
            {
                labelVistaBasica.Text = "Valor à vista com franquia básica:";

                if (opcaoVD.Checked != true)
                {
                    labelPorcentFipe.Visible = true;
                    porcentFipe.Visible = true;
                    simboloPorcentFipe.Visible = true;
                }

                labelFranqBasica.Visible =
                    franqBasica.Visible = labelFranqReduz.Visible = franqReduz.Visible = metadeBasica.Visible =
                        labelVistaReduz.Visible = vistaReduz.Visible = true;

                labelVistaBasica.Location = new Point(154, 120);
                vistaBasica.Location = new Point(157, 136);
            }
        }

        private void opcaoVD_CheckedChanged(object sender, EventArgs e)
        {
            if (opcaoVD.Checked)
            {
                Relatório.AdicionarAoRelatorio("VD");
                modoManual.Checked = false;
                modoManual.Visible = false;
                HabilitarFipe(false);

                labelValorFipe.Text = "Valor do veículo:";

                labelPorcentFipe.Visible = false;
                porcentFipe.Visible = false;
                simboloPorcentFipe.Visible = false;

                porcentFipe.Text = "100";
            }
            else
            {
                if (fipe)
                {
                    modoManual.Visible = true;
                    HabilitarFipe(true);
                }
                valorFipe.Text = "000";
                labelValorFipe.Text = string.Format("Valor na Fipe no mês {0}/{1}", DateTime.Today.Month,
                    DateTime.Today.Year);
                if (!fipe)
                {
                    modoManual.Checked = true;
                }
                if (opcaoRCF.Checked != true)
                {
                    labelPorcentFipe.Visible = true;
                    porcentFipe.Visible = true;
                    simboloPorcentFipe.Visible = true;
                }
            }
        }

        private void opcaoDebitoBB_CheckedChanged(object sender, EventArgs e)
        {
            if (opcaoDebitoBB.Checked)
            {
                Relatório.AdicionarAoRelatorio("Débito BB");
                if (tipoveiculo == "Automóvel")
                    observacoes.Text = observacoes.Text.Replace("se for no débito (exceto Banrisul).",
                        "exclusivo para débito no Banco do Brasil.");
            }
            else
            {
                if (tipoveiculo == "Automóvel")
                    observacoes.Text = observacoes.Text.Replace("exclusivo para débito no Banco do Brasil.",
                        "se for no débito (exceto Banrisul).");
                else
                    observacoes.Text = "Assistência 24 horas.\r\n";
            }
        }

        private void CodFipe_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) 13 && botaobuscar.BackgroundImage != Properties.Resources.CarregandoParado)
            {
                botaobuscar_Click(botaobuscar, null);
            }
        }

        private async void botaobuscar_Click(object sender, EventArgs e)
        {
            botaobuscar.Enabled = false;
            botaobuscar.BackgroundImage = Properties.Resources.CarregandoParado;

            if (WebFile.HasInternet())
            {
                FipeQuery query = new FipeQuery();
                List<FipeModel> modelos = await query.QueryYear_modelsByFipeCode(codFipe.Text);
                if (modelos.Count > 0)
                    switch (modelos[0].TipoVeiculo)
                    {
                        case FipeType.Automóvel:
                            tipo1.Checked = true;
                            break;
                        case FipeType.Moto:
                            tipo2.Checked = true;
                            break;
                        case FipeType.Caminhão:
                            tipo3.Checked = true;
                            break;
                    }
                anoModeloFipe.DisplayMember = "Label";
                anoModeloFipe.ValueMember = "Value";
                anoModeloFipe.DataSource = modelos;
            }
            else
            {
                MessageBox.Show("Sem acesso à internet!", "Carta de Cotação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            botaobuscar.Enabled = true;
            botaobuscar.BackgroundImage = Properties.Resources.Buscar;
        }

        private void nome_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Shift && (e.KeyValue == 'P' || e.KeyValue == 'p'))
            {
                if (MessageBox.Show("Lembrete: Você está rodando o Carta de Cotação dentro do Visual Studio?",
                           "Carta de Cotação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Painel p = new Painel();
                    p.Show();
                }
                
            }
            if (e.Control && e.Shift && (e.KeyValue == 'T' || e.KeyValue == 't'))
            {
                if (
                    MessageBox.Show("Você deseja testar a nova versão\r\nexperimental do Carta de Cotação?",
                        "Carta de Cotação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Atualizacao downloader = new Atualizacao("teste");
                    if (downloader.ShowDialog() == DialogResult.OK)
                        //OK => download não foi cancelado e terminou sem erros
                    {
                        Process.Start(downloader.LocalFile); //Executa o arquivo nessa linha
                        this.Close();
                    }
                }
            }
        }

        private void AutoPreencher()
        {
            Limpar();
            Random r = new Random();
            nome.Text += (char) r.Next(65, 91);
            for (int i = 0; i < 9; i++)
            {
                nome.Text += (char) r.Next(97, 123);
            }
            data.Value = DateTime.Today;
            //
            anoModeloFipe.DisplayMember = "Label";
            anoModeloFipe.ValueMember = "Value";
            anoModeloFipe.DataSource = new List<FipeModel>() { new FipeModel { Label = "Zero KM Gasolina", Value = "0", Modelo = "", Marca = "", AnoFabricacao = "", Valor = "0" } };
            anoModeloFipe.SelectedIndex = 0;
            //
            if (tipoveiculo == "Automóvel")
            {
                codFipe.Text = "001307-2";
                modelo.Text = "Uno Vivace Celeb. 1.0 Evo F.Flex 8V 5P";
                marca.Text = "Fiat";
                valorFipe.Text = "2612400";
            }
            if (tipoveiculo == "Moto")
            {
                codFipe.Text = "811125-1";
                modelo.Text = "Ctx 700N";
                marca.Text = "Honda";
                valorFipe.Text = "2974800";
            }
            if (tipoveiculo == "Caminhão")
            {
                codFipe.Text = "515150-3";
                modelo.Text = "10-160 E Delivery 2P (Diesel)(E5)";
                marca.Text = "Volkswagen";
                valorFipe.Text = "11946600";
            }            
            anoFabric.SelectedIndex = 0;
            seguradora.SelectedIndex = 1;
            porcentFipe.Value = 100;
                danosMateriais.Text =
                    danosCorporais.Text =
                        danosMorais.Text =
                            APPMorte.Text = APPInvalidez.Text = equipamento.Text = carroceria.Text = franqBasica.Text =
                                vistaBasica.Text = franqReduz.Text = vistaReduz.Text = "100000";
        }

        private void gerar_Click(object sender, EventArgs e)
        {
            if (mensagem.Text != status.Mensagem.Replace("|", "\r\n"))
                mensagem.Text = status.Mensagem.Replace("|", "\r\n");

            if (!mensagemObservacoes.Visible)
            {
                AtualizarFill();
                Cotacao cota = new Cotacao((FipeType) Enum.Parse(typeof (FipeType), tipoveiculo), nome.Text, data.Value,
                    seguradora.Text, modelo.Text,
                    anoFabric.Text, anoModeloTexto.Text, marca.Text, valorFipe.Text, porcentFipe.Value.ToString(),
                    danosMateriais.Text,
                    danosCorporais.Text, danosMorais.Text, APPMorte.Text, APPInvalidez.Text, franqBasica.Text,
                    vistaBasica.Text, franqReduz.Text, vistaReduz.Text, observacoes.Text,
                    opcaoNomeFuncionario.Checked, nomeFuncionario.Text, opcaoDebitoBB.Checked, opcaoRCF.Checked,
                    equipamento.Text,
                    carroceria.Text, opcaoVD.Checked, opcaoValidadeCot.Checked, dataValidadeCot.Value, fill);
                if (cota.ErroDeFormato == false)
                {
                    var pdf = new Pdf(cota);
                    pdf.GerarPdf();
                }
            }
            else
            {
                Relatório.AdicionarAoRelatorio("Linhas das observações excederam limite");
                MessageBox.Show("As observações excedem o limite de linhas.",
                    "Carta de Cotação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void botaoLimpar_Click(object sender, EventArgs e) => Limpar();

        private void Limpar()
        {
            Relatório.AdicionarAoRelatorio("Limpar pressionado");
            nome.Text = "";
            data.Value = DateTime.Now;
            codFipe.Text = "";
            anoModeloFipe.DataSource = new List<FipeModel>();
            anoModeloFipe.SelectedIndex = -1;
            modelo.Text = marca.Text = anoModeloTexto.Text = "";
            valorFipe.Text = "000"; //TENTAR ALTERAR PARA VAZIO SEM DAR ERRO
            anoFabric.SelectedIndex = -1;
            if (fipe)
                modoManual.Checked = false;
            opcaoVD.Checked = false;
            opcaoDebitoBB.Checked = false;
            seguradora.SelectedIndex = 0;
            porcentFipe.Value = 100;
            opcaoRCF.Checked = false;
            danosMateriais.Text =
                danosCorporais.Text =
                    danosMorais.Text =
                        APPMorte.Text =
                            APPInvalidez.Text =
                                equipamento.Text = carroceria.Text = franqBasica.Text = vistaBasica.Text =
                                    franqReduz.Text = vistaReduz.Text = "0";
            observacoes.Text = "";
            DateTime hoje = DateTime.Now;
            DateTime comdias = hoje.AddDays(5);
            dataValidadeCot.Value = comdias;
            opcaoNomeFuncionario.Checked = true;
        }

        private void modoManual_CheckedChanged(object sender, EventArgs e)
        {
            if (modoManual.Checked)
            {
                Relatório.AdicionarAoRelatorio("Digitar manualmente");
                HabilitarFipe(false);
            }
            else
                HabilitarFipe(true);
        }

        private void fecharMensagem_Click(object sender, EventArgs e)
        {
            novaVersao.Visible = false;
        }

        private void anoModeloFipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (anoModeloFipe.SelectedIndex >= 0)
            {
                botaobuscar.BackgroundImage = Properties.Resources.CarregandoParado;
                FipeQuery query = new FipeQuery();
                FipeModel model = query.QueryValuationByFipeModel(anoModeloFipe.SelectedItem as FipeModel);
                modelo.Text = model.Modelo;
                marca.Text = model.Marca;
                valorFipe.Text = model.PrecoMedio.ToString("C2", CultureInfo.CurrentCulture);
                anoModeloTexto.Text = model.Label;
                botaobuscar.BackgroundImage = Properties.Resources.Buscar;
            }
        }

        private void exemploToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Relatório.AdicionarAoRelatorio("Exemplo de preenchimento visualizado");
            if (nome.Text != "")
            {
                if (
                    MessageBox.Show("Você deseja limpar o programa e visualizar\r\num exemplo de preenchimento?",
                        "Carta de Cotação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    novaVersao.Visible = false;
                    AutoPreencher();
                }
            }
            else
            {
                novaVersao.Visible = false;
                AutoPreencher();
            }
        }

        private void informarProblemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Informar problema = new Informar();
            problema.Show();
        }

        private void verificarAtualizacoesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Atualizacao.VerificarAtualizacoes(status))
                AtualizarPrograma();
            else
                MessageBox.Show("Nenhuma atualização está disponível no momento.", "Carta de Cotação",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Informar inform = new Informar();
            inform.Show();
        }

        public void AtualizarFill()
        {
            fill.Tipo1 = tipo1.Checked;
            fill.Tipo2 = tipo2.Checked;
            fill.Tipo3 = tipo3.Checked;
            fill.Cliente = nome.Text;
            fill.Data = data.Value;
            fill.Modelo = modelo.Text;
            fill.AnoFabricacao = anoFabric.SelectedIndex;
            fill.AnoModelo = anoModeloTexto.Text;
            fill.Marca = marca.Text;
            fill.ValorFipe = valorFipe.Text;
            fill.CodigoFipe = codFipe.Text;
            fill.ModoManual = modoManual.Checked;
            fill.Vd = opcaoVD.Checked;
            fill.Seguradora = seguradora.Text;
            fill.DebitoBB = opcaoDebitoBB.Checked;
            fill.PorContratada = porcentFipe.Value;
            fill.DanosMateriais = danosMateriais.Text;
            fill.DanosCorporais = danosCorporais.Text;
            fill.DanosMorais = danosMorais.Text;
            fill.APPMorte = APPMorte.Text;
            fill.APPInvalidez = APPInvalidez.Text;
            fill.FranquiaBasica = franqBasica.Text;
            fill.ValFranqBasicaAVista = vistaBasica.Text;
            fill.FranquiaReduzida = franqReduz.Text;
            fill.ValFranqReduzidaAVista = vistaReduz.Text;
            fill.Observacoes = observacoes.Text;
            fill.BoolNomeFuncionario = opcaoNomeFuncionario.Checked;
            fill.NomeFuncionario = nomeFuncionario.Text;
            fill.Rcf = opcaoRCF.Checked;
            fill.Equipamento = equipamento.Text;
            fill.Carroceria = carroceria.Text;
            fill.BoolValidade = opcaoValidadeCot.Checked;
            fill.Validade = dataValidadeCot.Value;
        }

        private void PreencherFill()
        {
            tipo1.Checked = fill.Tipo1;
            tipo2.Checked = fill.Tipo2;
            tipo3.Checked = fill.Tipo3;
            nome.Text = fill.Cliente;
            data.Value = fill.Data;
            opcaoVD.Checked = fill.Vd;
            opcaoRCF.Checked = fill.Rcf;
            modelo.Text = fill.Modelo;
            anoFabric.SelectedIndex = fill.AnoFabricacao;
            anoModeloTexto.Text = fill.AnoModelo;
            
            anoModeloFipe.DisplayMember = "Label";
            anoModeloFipe.ValueMember = "Value";
            anoModeloFipe.DataSource = new List<FipeModel>() { new FipeModel { Label = fill.AnoModelo, Value = "0", Modelo = fill.Modelo, Marca = fill.Marca, AnoFabricacao = fill.AnoFabricacao.ToString(), Valor = fill.ValorFipe } };
            anoModeloFipe.SelectedIndex = 0;
            
            marca.Text = fill.Marca;
            valorFipe.Text = fill.ValorFipe;
            codFipe.Text = fill.CodigoFipe;
            if (fipe)
                modoManual.Checked = fill.ModoManual;
            seguradora.Text = fill.Seguradora;
            opcaoDebitoBB.Checked = fill.DebitoBB;
            porcentFipe.Value = fill.PorContratada;
            danosMateriais.Text = fill.DanosMateriais;
            danosCorporais.Text = fill.DanosCorporais;
            danosMorais.Text = fill.DanosMorais;
            APPMorte.Text = fill.APPMorte;
            APPInvalidez.Text = fill.APPInvalidez;
            franqBasica.Text = fill.FranquiaBasica;
            vistaBasica.Text = fill.ValFranqBasicaAVista;
            franqReduz.Text = fill.FranquiaReduzida;
            vistaReduz.Text = fill.ValFranqReduzidaAVista;
            observacoes.Text = fill.Observacoes;
            opcaoNomeFuncionario.Checked = fill.BoolNomeFuncionario;
            nomeFuncionario.Text = fill.NomeFuncionario;
            equipamento.Text = fill.Equipamento;
            carroceria.Text = fill.Carroceria;
            opcaoValidadeCot.Checked = fill.BoolValidade;
            dataValidadeCot.Value = fill.Validade;
        }

        private void editarCartaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = preferencias.PastaPDF,
                Filter = "Arquivo PDF|*.pdf",
                Title = "Abrir"
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog1.FileName != "")
                {
                    string filename = openFileDialog1.FileName;
                    PdfDictionary documentNames;
                    PdfDictionary embeddedFiles;
                    PdfDictionary fileArray;
                    PdfDictionary file;
                    PRStream stream;
                    using (PdfReader reader = new PdfReader(filename))
                    {
                        PdfDictionary catalog = reader.Catalog;

                        documentNames = (PdfDictionary) PdfReader.GetPdfObject(catalog.Get(PdfName.NAMES));

                        if (documentNames != null)
                        {
                            embeddedFiles =
                                (PdfDictionary) PdfReader.GetPdfObject(documentNames.Get(PdfName.EMBEDDEDFILES));
                            if (embeddedFiles != null)
                            {
                                PdfArray filespecs = embeddedFiles.GetAsArray(PdfName.NAMES);

                                for (int i = 0; i < filespecs.Size; i++)
                                {
                                    i++;
                                    fileArray = filespecs.GetAsDict(i);
                                    file = fileArray.GetAsDict(PdfName.EF);

                                    foreach (PdfName key in file.Keys)
                                    {
                                        stream = (PRStream) PdfReader.GetPdfObject(file.GetAsIndirectObject(key));
                                        //string attachedFileName = fileArray.GetAsString(key).ToString();
                                        byte[] attachedFileBytes = PdfReader.GetStreamBytes(stream);

                                        File.WriteAllBytes(WebFile.AppData + "TempFill.bin", attachedFileBytes);
                                        fill = (Fill) WebFile.Deserialize(WebFile.AppData + "TempFill.bin");
                                        Limpar();
                                        PreencherFill();
                                        File.Delete(WebFile.AppData + "TempFill.bin");
                                        Relatório.AdicionarAoRelatorio("Editar carta");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Relatório.AdicionarAoRelatorio("Tentativa de editar carta antiga");
                    MessageBox.Show(
                        "Somente as cartas geradas a partir da versão atual\r\ndo programa podem ser editadas.",
                        "Carta de Cotação",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Relatório.AdicionarAoRelatorio("Tentativa de editar carta antiga");
                }
            }
        }

        private void HabilitarFipe(bool habilitar)
        {
            if (habilitar)
            {
                anoModeloFipe.Visible = true;
                anoModeloTexto.Visible = false;
                modelo.ReadOnly = true;
                marca.ReadOnly = true;
                valorFipe.ReadOnly = true;
                valorFipe.Text = "0";
                codFipe.ReadOnly = false;
                botaobuscar.Enabled = true;
            }
            else
            {
                List<FipeModel> modelos = new List<FipeModel>();
                anoModeloFipe.DataSource = modelos;
                anoModeloFipe.Visible = false;
                anoModeloTexto.Visible = true;
                modelo.ReadOnly = false;
                marca.ReadOnly = false;
                valorFipe.ReadOnly = false;
                valorFipe.Text = "0";
                codFipe.Text = "";
                codFipe.ReadOnly = true;
                botaobuscar.Enabled = false;
            }
        }

        private void botaobuscar_EnabledChanged(object sender, EventArgs e)
        {
            if (botaobuscar.Enabled)
                botaobuscar.BackgroundImage = Properties.Resources.Buscar;
            else
                botaobuscar.BackgroundImage = Properties.Resources.Buscar_Bloqueado;
        }
    }
}