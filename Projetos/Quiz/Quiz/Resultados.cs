using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Quiz
{
    public partial class Resultados : Form
    {
        bool time = true;
        int perga = 0;
        int pergb = 0;
        int pergc = 0;
        int times = 1;
        Question[] questions;

        public Resultados()
        {
            InitializeComponent();
            questions = new Question[10];
            questions[0] = new Question("Costuma ingerir refrigerantes quantas vezes na semana?", "a.jpg", new string[] { "0", "1 ou 2", "3 ou mais" }, new int[] { 2, 1, 0 });
            questions[1] = new Question("Quantas frutas você ingere por dia?", "b.jpg", new string[] { "0", "1 a 3", "Mais de 3" }, new int[] { 0, 1, 2 });
            questions[2] = new Question("Durante as refeições, seu prato contém vegetais/legumes?", "c.jpg", new string[] { "Sim", "Às vezes", "Nunca" }, new int[] { 2, 1, 0 });
            questions[3] = new Question("Antes de ingerir o alimento, costuma verificar suas propriedades?", "d.jpg", new string[] { "Sim", "Às vezes", "Nunca" }, new int[] { 2, 1, 0 });
            questions[4] = new Question("Costuma comer sobremesa depois do almoço/jantar?", "e.jpg", new string[] { "Sim", "Às vezes", "Nunca" }, new int[] { 0, 1, 2 });
            questions[5] = new Question("Quantas refeições faz ao dia?", "f.jpg", new string[] { "1 a 3", "4 a 6", "6 ou mais" }, new int[] { 0, 2, 1 });
            questions[6] = new Question("Costuma ingerir alimentos pré-prontos/congelados?", "g.jpg", new string[] { "Sim", "Às vezes", "Nunca" }, new int[] { 0, 1, 2 });
            questions[7] = new Question("Costuma tentar seguir a pirâmide alimentar?", "h.jpg", new string[] { "Sim", "Às vezes", "Nunca" }, new int[] { 2, 1, 0 });
            questions[8] = new Question("Quantos litros de água por dia costuma beber?", "i.jpg", new string[] { "0 a 2", "2 a 3", "3 ou mais" }, new int[] { 0, 2, 1 });
            questions[9] = new Question("Está disposto a buscar uma alimentação saudável?", "j.jpg", new string[] { "Sim", "Talvez", "Não" }, new int[] { 2, 1, 0 });
            Atualizar();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Atualizar();
        }

        public void Atualizar()
        {
            string[] resps = File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Respostas.txt");

            int ruim = 0;
            int bom = 0;
            int muitobom = 0;

            for (int i = 0; i < resps.Length; i++)
            {
                int pont = Int32.Parse(resps[i].Substring(10, 2));
                if (pont <= 5)
                    ruim++;
                if (pont >= 6 && pont <= 14)
                    bom++;
                if (pont >= 15 && pont <= 20)
                    muitobom++;
            }

            //reset your chart series and legends
            chart1.Series.Clear();
            chart1.Legends.Clear();

            //Add a new Legend(if needed) and do some formating
            chart1.Legends.Add("Legenda");
            chart1.Legends[0].LegendStyle = LegendStyle.Table;
            chart1.Legends[0].Docking = Docking.Bottom;
            chart1.Legends[0].Alignment = StringAlignment.Center;
            chart1.Legends[0].Title = "Resultado";
            chart1.Legends[0].BorderColor = Color.Black;

            //Add a new chart-series
            string seriesname = "Série";
            chart1.Series.Add(seriesname);
            //set the chart-type to "Pie"
            chart1.Series[seriesname].ChartType = SeriesChartType.Pie;

            //Add some datapoints so the series. in this case you can pass the values to this method
            if (ruim != 0)
                chart1.Series[seriesname].Points.AddXY("Ruim" + "\r\n" + ((double)ruim / (ruim + bom + muitobom) * 100).ToString("0") + "%", ruim);
            if (bom != 0)
                chart1.Series[seriesname].Points.AddXY("Média" + "\r\n" + ((double)bom / (ruim + bom + muitobom) * 100).ToString("0") + "%", bom);
            if (muitobom != 0)
                chart1.Series[seriesname].Points.AddXY("Muito boa" + "\r\n" + ((double)muitobom / (ruim + bom + muitobom) * 100).ToString("0") + "%", muitobom);
            chart1.Series[seriesname].Font = new Font("Microsoft Sans Sherif", 18);

            //Escolhe três perguntas aleatórias

            Random r = new Random();
            

            if (time)
            {
                perga = r.Next(0, 9);
                pergb = r.Next(0, 9);
                while (pergb == perga)
                    pergb = r.Next(0, 9);
                pergc = r.Next(0, 9);
                while (pergc == perga || pergc == pergb)
                    pergc = r.Next(0, 9);
                time = false;
            }



            //Pergunta 1

            int a = 0;
            int aa = 0;
            int aaa = 0;

            for (int i = 0; i < resps.Length; i++)
            {
                if (Int32.Parse(resps[i].Substring(perga, 1)) == 0)
                    a++;

                if (Int32.Parse(resps[i].Substring(perga, 1)) == 1)
                    aa++;

                if (Int32.Parse(resps[i].Substring(perga, 1)) == 2)
                    aaa++;
            }



            chart2.Series.Clear();
            chart2.Legends.Clear();

            chart2.Series.Add("Série");

            chart2.Series[0].ChartType = SeriesChartType.StackedColumn100;

            chart2.Series[0].Points.AddXY("", a);
            chart2.Series[0].Points[0].Label = questions[perga].Respostas[0] + "\r\n" + ((double)a / (a + aa + aaa) * 100).ToString("0") + "%";
            label3.Text = questions[perga].Pergunta;

            chart2.Series.Add("Sériee");

            chart2.Series[1].ChartType = SeriesChartType.StackedColumn100;

            chart2.Series[1].Points.AddXY("",aa);
            chart2.Series[1].Points[0].Label = questions[perga].Respostas[1] + "\r\n" + ((double)aa / (a + aa + aaa) * 100).ToString("0") + "%";

            chart2.Series.Add("Sérieee");

            chart2.Series[2].ChartType = SeriesChartType.StackedColumn100;

            chart2.Series[2].Points.AddXY("",aaa);
            chart2.Series[2].Points[0].Label = questions[perga].Respostas[2] + "\r\n" + ((double)aaa / (a + aa + aaa) * 100).ToString("0") + "%";

            chart2.Series[0].Font = new Font("Microsoft Sans Sherif", 18);
            chart2.Series[1].Font = new Font("Microsoft Sans Sherif", 18);
            chart2.Series[2].Font = new Font("Microsoft Sans Sherif", 18);

            //Pergunta 2

            int b = 0;
            int bb = 0;
            int bbb = 0;

            for (int i = 0; i < resps.Length; i++)
            {
                if (Int32.Parse(resps[i].Substring(pergb, 1)) == 0)
                    b++;

                if (Int32.Parse(resps[i].Substring(pergb, 1)) == 1)
                    bb++;

                if (Int32.Parse(resps[i].Substring(pergb, 1)) == 2)
                    bbb++;
            }



            chart3.Series.Clear();
            chart3.Legends.Clear();

            chart3.Series.Add("Série");

            chart3.Series[0].ChartType = SeriesChartType.StackedColumn100;

            chart3.Series[0].Points.AddXY("", b);
            chart3.Series[0].Points[0].Label = questions[pergb].Respostas[0] + "\r\n" + ((double)b / (b + bb + bbb) * 100).ToString("0") + "%";
            label4.Text = questions[pergb].Pergunta;

            chart3.Series.Add("Sériee");

            chart3.Series[1].ChartType = SeriesChartType.StackedColumn100;

            chart3.Series[1].Points.AddXY("", bb);
            chart3.Series[1].Points[0].Label = questions[pergb].Respostas[1] + "\r\n" + ((double)bb / (b + bb + bbb) * 100).ToString("0") + "%";

            chart3.Series.Add("Sérieee");

            chart3.Series[2].ChartType = SeriesChartType.StackedColumn100;

            chart3.Series[2].Points.AddXY("", bbb);
            chart3.Series[2].Points[0].Label = questions[pergb].Respostas[2] + "\r\n" + ((double)bbb / (b + bb + bbb) * 100).ToString("0") + "%";

            chart3.Series[0].Font = new Font("Microsoft Sans Sherif", 18);
            chart3.Series[1].Font = new Font("Microsoft Sans Sherif", 18);
            chart3.Series[2].Font = new Font("Microsoft Sans Sherif", 18);

            //Pergunta 3

            int c = 0;
            int cc = 0;
            int ccc = 0;

            for (int i = 0; i < resps.Length; i++)
            {
                if (Int32.Parse(resps[i].Substring(pergc, 1)) == 0)
                    c++;

                if (Int32.Parse(resps[i].Substring(pergc, 1)) == 1)
                    cc++;

                if (Int32.Parse(resps[i].Substring(pergc, 1)) == 2)
                    ccc++;
            }



            chart4.Series.Clear();
            chart4.Legends.Clear();

            chart4.Series.Add("Série");

            chart4.Series[0].ChartType = SeriesChartType.StackedColumn100;

            chart4.Series[0].Points.AddXY("", c);
            chart4.Series[0].Points[0].Label = questions[pergc].Respostas[0] + "\r\n" + ((double)c / (c + cc + ccc) * 100).ToString("0") + "%";
            label5.Text = questions[pergc].Pergunta;

            chart4.Series.Add("Sériee");

            chart4.Series[1].ChartType = SeriesChartType.StackedColumn100;

            chart4.Series[1].Points.AddXY("", cc);
            chart4.Series[1].Points[0].Label = questions[pergc].Respostas[1] + "\r\n" + ((double)cc / (c + cc + ccc) * 100).ToString("0") + "%";

            chart4.Series.Add("Sérieee");

            chart4.Series[2].ChartType = SeriesChartType.StackedColumn100;

            chart4.Series[2].Points.AddXY("", ccc);
            chart4.Series[2].Points[0].Label = questions[pergc].Respostas[2] + "\r\n" + ((double)ccc / (c + cc + ccc) * 100).ToString("0") + "%";

            chart4.Series[0].Font = new Font("Microsoft Sans Sherif", 18);
            chart4.Series[1].Font = new Font("Microsoft Sans Sherif", 18);
            chart4.Series[2].Font = new Font("Microsoft Sans Sherif", 18);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (times == 1)
                label2.Text = "Atualizando dados em tempo real.";
            if (times == 2)
                label2.Text = "Atualizando dados em tempo real..";
            if (times == 3)
                label2.Text = "Atualizando dados em tempo real...";
            if (times != 3)
                times++;
            else
                times = 0;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            time = true;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
