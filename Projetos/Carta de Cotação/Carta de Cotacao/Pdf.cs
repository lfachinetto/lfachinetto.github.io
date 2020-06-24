using System;
using System.Collections.Generic;
using System.Windows.Forms;

using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Globalization;
using System.Diagnostics;

namespace CartaDeCotacao
{
    public class Pdf
    {
        private Cotacao Cota { get; set; }
        private string PdfFolder { get; set; }
        string exib;

        public Pdf(Cotacao cot)
        {
            Cota = cot;
            PdfFolder = Home.preferencias.PastaPDF;
            if (PdfFolder == "")
            {
                PdfFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\";
            }
        }

        public void GerarPdf()
        {
            var doc1 = new Document(PageSize.A4, 40.0f, 20.0f, 30.0f, 30.0f);//Margens - Margem direita precisa ser 20 para poder caber valores acima de R$ 100.000,00 nas contratações
            string filename = null;
            PdfWriter writer = null;
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog
                {
                    InitialDirectory = PdfFolder,
                    Filter = "Arquivo PDF|*.pdf",
                    Title = "Salvar como",
                    FileName = Cota.Cliente + ".pdf"
                };
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (saveFileDialog1.FileName != "")
                    {
                        filename = saveFileDialog1.FileName;
                        writer = PdfWriter.GetInstance(doc1, new FileStream(saveFileDialog1.FileName, FileMode.Create));
                        Relatório.SalvaUtilizacao(Cota);
                    }
                }
                else return;

                Home.preferencias.PastaPDF = saveFileDialog1.FileName.Substring(0, saveFileDialog1.FileName.LastIndexOf('\\') + 1);
                WebFile.Prefs = Home.preferencias;
            }
            catch (IOException)
            {
                Relatório.AdicionarAoRelatorio("Há um arquivo de mesmo nome aberto");
                MessageBox.Show("Não foi possível criar o arquivo PDF,\r\nverifique se há um arquivo de mesmo nome aberto.\r\nFeche-o e tente novamente.",
                "Carta de Cotação",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }

            doc1.Open();
            iTextSharp.text.Font normal = FontFactory.GetFont("Helvetica", 11);

            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(ConverteImageParaByteArray(Properties.Resources.Bragaseg));
            jpg.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
            jpg.ScaleToFit(250f, 250f);
            //jpg.SetAbsolutePosition(500f, 750f);

            //Como colocar marca d'água:
            //jpg.SetAbsolutePosition(doc1.PageSize.Width - 500f, 750f);
            //jpg.Rotation = (float)Math.PI / 2;
            //jpg.RotationDegrees = 90f;
            
            doc1.Add(jpg);            
            
            doc1.Add(new Paragraph("\r\n", normal));
            Paragraph pfcity = new Paragraph("Passo Fundo, " + DataPorExtenso(Cota.Data) + ".", normal);
            //pfcity.Alignment = iTextSharp.text.Image.ALIGN_RIGHT;
            //pfcity.IndentationLeft = 230;
            doc1.Add(pfcity);
            doc1.Add(new Paragraph("\r\n", normal));
            if (Home.excedeuLinhas)
                doc1.Add(new Paragraph("A " + Cota.Cliente, normal));
            else
            {
                doc1.Add(new Paragraph("A", normal));
                doc1.Add(new Paragraph(Cota.Cliente, normal));
            }
            doc1.Add(new Paragraph("\r\n", normal));
            doc1.Add(new Paragraph("Estamos encaminhando a cotação do veículo abaixo:", normal));
            doc1.Add(new Paragraph(Cota.Marca + " - " + Cota.Modelo, normal));
            doc1.Add(new Paragraph("Ano de fabricação: " + Cota.AnoFabricacao + "      Ano modelo: " + Cota.AnoModelo, normal));
            doc1.Add(new Paragraph("\r\n", normal));

            //Início das contratações:
            decimal valorfipecont = Cota.ValorFipe * Cota.PorContratada / 100m;
            List<string> contrat = new List<string>();
            List<decimal> listvalores = new List<decimal>();            

            if (Cota.Rcf != true)
            {
                if (Cota.Vd != true)
                    contrat.Add("Casco - " + Cota.PorContratada + "% da Fipe:");
                else
                    contrat.Add("Valor Determinado:");
                listvalores.Add(valorfipecont);
            }
            else
            {
                contrat.Add("Sem casco contratado");
                listvalores.Add(-1.0m);
            }

            contrat.Add("Danos Materiais:");
            listvalores.Add(Cota.DanosMateriais);

            contrat.Add("Danos Corporais:");
            listvalores.Add(Cota.DanosCorporais);

            contrat.Add("Danos Morais:");
            listvalores.Add(Cota.DanosMorais);

            if (Cota.Seguradora != "Itaú")
                contrat.Add("APP Morte:");
            else
                contrat.Add("APP:");
            listvalores.Add(Cota.APPMorte);

            //AQUI: ESSE NÃO APARECE SE FOR ITAÚ
            contrat.Add("APP Invalidez:");
            listvalores.Add(Cota.APPInvalidez);
            
            if (Cota.Tipo == FipeType.Caminhão)
            {
                contrat.Add("Equipamento:");
                listvalores.Add(Cota.Equipamento);

                contrat.Add("Carroceria:");
                listvalores.Add(Cota.Carroceria);
            }

            bool semdecimais = listvalores.Exists(d => d > 999999);

            PdfPTable valores = new PdfPTable(5);
            valores.HorizontalAlignment = 0;
            float[] widthvalores = { 45f, 30f, 9f, 40f, 30f };
            valores.SetWidths(widthvalores);
            valores.DefaultCell.BorderColor = iTextSharp.text.BaseColor.WHITE;

            int c = 0;//contador de valores não zero na listvalores
            int n = 0;//num linhas
            int MODO = 0;//<--MODO 0 divide os itens entre as colunas 1 e 2
            if (MODO == 0)  //MODO 1 sempre preenche a coluna 1 inteira (3 itens ou 4 se caminhão) e depois preenche a coluna 2
            {
                foreach (decimal d in listvalores)
                    if (d != 0.0m)
                    {
                        c++;
                        if (c % 2 != 0)
                            n++;
                    }
            }
            else
            {
                n = 3;//num linhas se não caminhão
                if (Cota.Tipo == FipeType.Caminhão)
                    n = 4;//num linhas se caminhão
            }

            int[] col1 = new int[] { -1, -1, -1, -1 };//posições na coluna 1, inicializa com -1 = não preenchido
            int[] col2 = new int[] { -1, -1, -1, -1 };//posições na coluna 2, inicializa com -1 = não preenchido

            c = 0;//reinicia contador
            int i = 0;//contador de índice da listvalores
            foreach (decimal d in listvalores)
            {
                if (d != 0.0m)
                {
                    if (c < n)//preenchimento da coluna 1
                        col1[c] = i;//guarda a posição de d na listvalores e contrat
                    else//preenchimento da coluna 2, após coluna 1 estar cheia
                        col2[c - n] = i;//guarda a posição de d na listvalores e contrat, c - n porque o índice deve iniciar no zero
                    c++;
                }
                i++;
            }
            
            for (i = 0; i < n; i++)
            {
                if (col1[i] != -1)//testa se a posição na coluna 1 tem valor a ser preenchido
                {
                    valores.AddCell(new Phrase(contrat[col1[i]], normal));//preenche com a string de contrat na posição guardada em col1
                    
                    if (listvalores[col1[i]] != -1.0m)// testa se é o caso "sem casco"
                        if (!semdecimais)
                            valores.AddCell(new Phrase(listvalores[col1[i]].ToString("C2", CultureInfo.CurrentCulture)));//converte valor para string
                        else
                            valores.AddCell(new Phrase(listvalores[col1[i]].ToString("C0", CultureInfo.CurrentCulture)));//converte valor para string
                    else
                        valores.AddCell(new Phrase("", normal));//se for "sem casco" preenche com vazio

                    valores.AddCell(new Phrase("", normal));//preenche a coluna do meio com vazio
                }
                
                if (col2[i] != -1)//testa se a posição na coluna 2 tem valor a ser preenchido
                {
                    valores.AddCell(new Phrase(contrat[col2[i]], normal));//preenche com a string de contrat na posição guardada em col2
                    // como "sem casco" nunca vai cair na coluna 2, não precisa testar
                    if (!semdecimais)
                        valores.AddCell(new Phrase(listvalores[col2[i]].ToString("C2", CultureInfo.CurrentCulture)));//converte valor para string
                    else
                        valores.AddCell(new Phrase(listvalores[col2[i]].ToString("C0", CultureInfo.CurrentCulture)));//converte valor para string
                }
                else
                {
                    valores.AddCell(new Phrase("", normal));//se a posição na coluna 2 não tem valor, preenche com vazio
                    valores.AddCell(new Phrase("", normal));//idem acima para o valor
                }
            }
            doc1.Add(valores);
            //Fim das contratações

            int colunas = 3;
            if (Cota.Seguradora == "Bradesco" || Cota.Seguradora == "Generale" || (Cota.Seguradora == "Mapfre" && Cota.DebitoBB == true))
                colunas = 2;            
            float[] width = { 15f, 15f, 15f};
            float[] width2 = { 15f, 15f};

            //Tabela Franquia Básica
            if (Cota.FranquiaBasica + Cota.ValFranqBasicaAVista != 0)
            {
                PdfPTable table = new PdfPTable(colunas);

                if (colunas == 3)
                    table.SetWidths(width);
                else
                    table.SetWidths(width2);
                table.HorizontalAlignment = 0;
                PdfPCell cell = new PdfPCell(new Phrase(""));
                cell.Colspan = 1;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);                
                doc1.Add(new Paragraph("\r\n", normal));
                if (Cota.Rcf != true)
                {
                    doc1.Add(new Paragraph("Franquia básica: " + Cota.FranquiaBasica.ToString("C2", CultureInfo.CurrentCulture), normal));
                    doc1.Add(new Paragraph("\r\n", normal));
                }

                if (Cota.Seguradora == "Mapfre" && Cota.DebitoBB)
                    table.AddCell(new Phrase("Débito", normal));
                else
                    table.AddCell(new Phrase("Carnê", normal));
                
                if (colunas != 2)
                    if (Cota.Seguradora == "Sul América")
                        table.AddCell(new Phrase("Débito BB", normal));
                    else
                        table.AddCell(new Phrase("Débito", normal));

                foreach (Parcelamento parc in Cota.TabelaParcelamento.FindAll(p => (p.TipoFranq == "Básica" && !(p.Seguradora == "Bradesco" && p.FormaPagto == "Débito")) && /* ADICIONADO */!(p.Seguradora == "Generale" && p.FormaPagto == "Débito") && (p.TipoFranq == "Básica" && !(p.Seguradora == "Mapfre" && p.FormaPagto == "Carnê" && Cota.DebitoBB == true))))
                {
                    if (parc.FormaPagto == "Carnê" || (parc.FormaPagto == "Débito" && Cota.DebitoBB && parc.ValParcela != 0.0m))
                        table.AddCell(new Phrase(parc.Descricao.ToString(), normal));

                    if (parc.ValParcela == 0.0m)
                        table.AddCell(new Phrase("", normal));
                    else
                        table.AddCell(new Phrase(parc.ValParcela.ToString("C2", CultureInfo.CurrentCulture), normal));
                }
                doc1.Add(table);
            }
            //Tabela Franquia Reduzida
            if (Cota.FranquiaReduzida + Cota.ValFranqReduzidaAVista != 0.0m && Cota.Rcf != true)
            {
                PdfPTable table2 = new PdfPTable(colunas);
                if (colunas == 3)
                    table2.SetWidths(width);
                else
                    table2.SetWidths(width2);
                table2.HorizontalAlignment = 0;
                PdfPCell cell2 = new PdfPCell(new Phrase(""))
                {
                    Colspan = 1,
                    HorizontalAlignment = 1
                };
                //0=Left, 1=Centre, 2=Right
                table2.AddCell(cell2);
                doc1.Add(new Paragraph("\r\n"));
                doc1.Add(new Paragraph("Franquia reduzida: " + Cota.FranquiaReduzida.ToString("C2", CultureInfo.CurrentCulture), normal));
                doc1.Add(new Paragraph("\r\n"));

                if (Cota.Seguradora == "Mapfre" && Cota.DebitoBB)
                    table2.AddCell(new Phrase("Débito", normal));
                else
                    table2.AddCell(new Phrase("Carnê", normal));

                if (colunas != 2)
                    if (Cota.Seguradora == "Sul América")
                        table2.AddCell(new Phrase("Débito BB", normal));
                    else
                        table2.AddCell(new Phrase("Débito", normal));

                foreach (Parcelamento parc in Cota.TabelaParcelamento.FindAll(p => (p.TipoFranq == "Reduzida" && !(p.Seguradora == "Bradesco" && p.FormaPagto == "Débito")) && /* ADICIONADO */!(p.Seguradora == "Generale" && p.FormaPagto == "Débito") && (p.TipoFranq == "Reduzida" && !(p.Seguradora == "Mapfre" && p.FormaPagto == "Carnê" && Cota.DebitoBB == true))))
                {
                    if (parc.FormaPagto == "Carnê" || (parc.FormaPagto == "Débito" && Cota.DebitoBB && parc.ValParcela != 0.0m))
                        table2.AddCell(new Phrase(parc.Descricao.ToString(), normal));

                    if (parc.ValParcela == 0.0m)
                        table2.AddCell(new Phrase("", normal));
                    else
                        table2.AddCell(new Phrase(parc.ValParcela.ToString("C2", CultureInfo.CurrentCulture), normal));
                }
                doc1.Add(table2);
            }
            //Final Tabelas

            doc1.Add(new Paragraph("\r\n", normal));
            doc1.Add(new Paragraph(Cota.Observacoes, normal));

            if (Cota.MostrarValidade == true)
            {
                string textovalidade = "*Validade da cotação: " + Cota.Validade.ToShortDateString();
                doc1.Add(new Paragraph(textovalidade, normal));
            }

            doc1.Add(new Paragraph("\r\n", normal));
            iTextSharp.text.Font negrito = FontFactory.GetFont("Helvetica", 11, iTextSharp.text.Font.BOLD);

            //Testes das exibições
            /*if (Cota.NomeClaudio == true)
                exib += "1";
            else
                exib += "0";

            if (Cota.CelClaudio == true)
                exib += "1";
            else
                exib += "0";

            if (Cota.NomeFuncionario == true && Cota.Funcionario != "")
                exib += "1";
            else
                exib += "0";

            switch (exib)
            {
                case "001":
                    doc1.Add(new Paragraph(Cota.Funcionario, negrito));
                    break;
                case "010":
                    doc1.Add(new Paragraph("(54) 9981-4797", negrito));
                    break;
                case "011":
                    doc1.Add(new Paragraph("(54) 9981-4797 / " + Cota.Funcionario, negrito));
                    break;
                case "100":
                    doc1.Add(new Paragraph("Cláudio Zanchet", negrito));
                    break;
                case "101":
                    doc1.Add(new Paragraph("Cláudio Zanchet / " + Cota.Funcionario, negrito));
                    break;
                case "110":
                    doc1.Add(new Paragraph("Cláudio Zanchet" + " - (54) 9981-4797", negrito));
                    break;
                case "111":
                    doc1.Add(new Paragraph("Cláudio Zanchet" + " - (54) 9981-4797 / " + Cota.Funcionario, negrito));
                    break;                
            }*/
            //Fim dos testes das exibições
            doc1.Add(new Paragraph(Cota.Funcionario, negrito));

            if (Cota.NomeFuncionario)
                doc1.Add(new Paragraph("Bragaseg Corretora de Seguros", normal));
            else
                doc1.Add(new Paragraph("Bragaseg Corretora de Seguros", negrito));
            doc1.Add(new Paragraph("Rua Uruguai, 1763 - Bairro Centro", normal));
            doc1.Add(new Paragraph("Passo Fundo - RS    CEP: 99010-112", normal));
            doc1.Add(new Paragraph("Telefone: (54) 3045-5300", normal));

            doc1.AddCreator("Carta de Cotação");
            File.Create(WebFile.AppData + "TempFill.bin").Close();
            WebFile.Serialize(Cota.Preenchimento, WebFile.AppData + "TempFill.bin");
            PdfFileSpecification pfs = PdfFileSpecification.FileEmbedded(writer, WebFile.AppData + "TempFill.bin", "TempFill.bin", null);
            writer.AddFileAttachment(pfs);
            doc1.Close();
            File.Delete(WebFile.AppData + "TempFill.bin");

            if (MessageBox.Show("Arquivo PDF gerado com sucesso!\r\nVocê deseja visualizá-lo agora?", "Carta de Cotação", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                Process.Start(filename);
            }
        }

        private string DataPorExtenso(DateTime data)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            DateTimeFormatInfo dtfi = culture.DateTimeFormat;
            int dia = data.Day;
            int ano = data.Year;
            string mes = culture.TextInfo.ToTitleCase(dtfi.GetMonthName(data.Month));
            string dataextenso = dia.ToString("00") + " de " + mes + " de " + ano;
            return dataextenso;
        }

        private static byte[] ConverteImageParaByteArray(System.Drawing.Image imagem)
        {
            using (var stream = new MemoryStream())
            {
                imagem.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return stream.ToArray();
            }
        }
    }
}