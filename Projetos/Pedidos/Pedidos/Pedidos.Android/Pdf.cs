using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
using Android.Graphics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Pedidos.Droid
{
    public class Pdf
    {
        private Pedido Ped;
        private string path = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;//Usar como path + "/PastaExemplo"

        public Pdf(Pedido ped)
        {
            Ped = ped;
        }

        public async Task GerarPdf()
        {
            var doc1 = new Document(PageSize.A4, 40.0f, 20.0f, 30.0f, 30.0f);//Margens
            PdfWriter writer = null;
            //try
            //{
            if (!Directory.Exists(path + "/Cotações"))
                Directory.CreateDirectory(path + "/Cotações");
            if (!Directory.Exists(path + "/Pedidos"))
                Directory.CreateDirectory(path + "/Pedidos");
            string completepath;

            if (Ped.Tipo == Tipo.Cotação)
                completepath = path + "/Cotações/" + Ped.Cliente + "-" + DateTime.Now.Millisecond.ToString() + ".pdf";
            else
                completepath = path + "/Pedidos/" + Ped.Cliente + "-" + DateTime.Now.Millisecond.ToString() + ".pdf";

            writer = PdfWriter.GetInstance(doc1, new FileStream(completepath, FileMode.Create));
            //Path.Combine(path.ToString(), "Cotação/myfile.pdf"), FileMode.Create));
            //}
            //catch (IOException)
            //{
            //MessageBoxOK("Não foi possível criar o arquivo PDF,\r\nverifique se há um arquivo de mesmo nome aberto.\r\nFeche-o e tente novamente.");
            //}

            doc1.Open();
            iTextSharp.text.Font normal = FontFactory.GetFont("Arial", 12);

            /*var logo = new Xamarin.Forms.Image();

            if (Ped.Empresa == Empresa.Zamil)
                logo = Resource.Drawable.zamil;
            if (Ped.Empresa == Empresa.Totalfix)
                logo = Resource.Drawable.totalfix;
            if (Ped.Empresa == Empresa.Totalpinos)
                logo.Source = ImageSource.FromResource("Pedidos.totalpinos.png");

            var bitmap = await GetBitmapFromImageSourceAsync(logo.Source, null);

            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(bitmap);
            jpg.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
            jpg.ScaleToFit(250f, 250f);*/
            //jpg.SetAbsolutePosition(500f, 750f);

            //doc1.Add(jpg);

            doc1.Add(new Paragraph(Ped.Empresa.ToString(), normal));

            doc1.Add(new Paragraph(Enum.GetName(typeof(Tipo), Ped.Tipo), normal));            

            doc1.Add(new Paragraph("\r\n", normal));
            doc1.Add(new Paragraph("Cliente: " + Ped.Cliente, normal));

            doc1.Add(new Paragraph("\r\n", normal));
            doc1.Add(new Paragraph("Cidade: " + Ped.Cidade, normal));

            doc1.Add(new Paragraph("\r\n", normal));
            doc1.Add(new Paragraph("Condições: " + Ped.Condições, normal));

            doc1.Add(new Paragraph("\r\n", normal));
            doc1.Add(new Paragraph("Frete: " + Ped.Frete, normal));

            doc1.Add(new Paragraph("\r\n", normal));
            doc1.Add(new Paragraph("Produtos: ", normal));

            doc1.Add(new Paragraph("\r\n", normal));
            PdfPTable table = new PdfPTable(6);
            //table.SetWidths(width);
            //table.HorizontalAlignment = 0;
            table.AddCell(new Phrase("Quantidade", normal));
            table.AddCell(new Phrase("Unidade", normal));
            table.AddCell(new Phrase("Produto", normal));
            table.AddCell(new Phrase("Preço un.", normal));
            table.AddCell(new Phrase("Desconto", normal));
            table.AddCell(new Phrase("Valor", normal));

            foreach (Item i in Ped.Itens)
            {
                table.AddCell(new Phrase(i.Quantidade.ToString(), normal));
                table.AddCell(new Phrase(Enum.GetName(typeof(Unidade), i.Unidade), normal));
                table.AddCell(new Phrase(i.Produto, normal));
                table.AddCell(new Phrase(i.Preço.ToString("C2"), normal));
                table.AddCell(new Phrase(i.Desconto.ToString() + "%", normal));
                table.AddCell(new Phrase(i.Valor.ToString("C2"), normal));
            }

            doc1.Add(table);

            doc1.Add(new Paragraph("\r\n", normal));
            doc1.Add(new Paragraph("Total: " + Ped.Total.ToString("C2"), normal));

            doc1.AddCreator("Pedidos");
            //File.Create(WebFile.AppData + "TempFill.bin").Close();
            //WebFile.Serialize(Cota.Preenchimento, WebFile.AppData + "TempFill.bin");
            //PdfFileSpecification pfs = PdfFileSpecification.FileEmbedded(writer, WebFile.AppData + "TempFill.bin", "TempFill.bin", null);
            //writer.AddFileAttachment(pfs);
            doc1.Close();
            //File.Delete(WebFile.AppData + "TempFill.bin");

            //Toast.MakeText(null, "Arquivo PDF gerado com sucesso!", ToastLength.Short).Show();

            /*if (MessageBoxYesOrNo("Arquivo PDF gerado com sucesso!\r\nVocê deseja visualizá-lo agora?"))
            {
                Process.Start(filename);
            }*/
        }

        /*private string DataPorExtenso(DateTime data)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            DateTimeFormatInfo dtfi = culture.DateTimeFormat;
            int dia = data.Day;
            int ano = data.Year;
            string mes = culture.TextInfo.ToTitleCase(dtfi.GetMonthName(data.Month));
            string dataextenso = dia.ToString("00") + " de " + mes + " de " + ano;
            return dataextenso;
        }*/

        public static async Task<byte[]> GetBitmapFromImageSourceAsync(ImageSource source, Context context)
        {
            var handler = new Xamarin.Forms.Platform.Android.FileImageSourceHandler();
            var bitmap = await handler.LoadImageAsync(source, context);

            byte[] bitmapData;
            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 0, stream);
                bitmapData = stream.ToArray();
            }
            return bitmapData;
        }

        /*public void MessageBoxOK(string MyMessage)
        {

            AlertDialog.Builder builder;
            builder = new AlertDialog.Builder(null);
            builder.SetTitle("Pedidos");
            builder.SetMessage(MyMessage);
            builder.SetCancelable(false);
            builder.SetPositiveButton("OK", delegate { });
            Dialog dialog = builder.Create();
            dialog.Show();
            return;
        }

        public bool MessageBoxYesOrNo(string MyMessage)
        {
            AlertDialog.Builder builder;
            builder = new AlertDialog.Builder(null);
            builder.SetTitle("Pedidos");
            builder.SetMessage(MyMessage);
            builder.SetCancelable(false);
            builder.SetPositiveButton("Yes", delegate { answer = true; });
            builder.SetNegativeButton("No", delegate { answer = false; });
            Dialog dialog = builder.Create();
            dialog.Show();
            return answer;
        }*/
    }
}