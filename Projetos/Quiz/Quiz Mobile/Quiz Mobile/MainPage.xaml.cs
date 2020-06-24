using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QuizMobile
{
	public partial class MainPage : ContentPage
	{
        public static int numero;
        public static string ip;
        Question[] questions;

        public MainPage()
		{
			InitializeComponent();
            questions = new Question[10];
            questions[0] = new Question("Costuma ingerir refrigerantes quantas vezes na semana?", "a.jpg", new string[] { "0", "1 ou 2", "3 ou mais" }, new int[] { 2, 1, 0});
            questions[1] = new Question("How many fruits do you eat a day?\r\n\r\n-> Quantas frutas você ingere por dia?", "b.jpg", new string[] { "0", "1 a 3", "Mais de 3" }, new int[] { 0, 1, 2 });
            questions[2] = new Question("Durante as refeições, seu prato contém vegetais/legumes?", "c.jpg", new string[] { "Sim", "Às vezes", "Nunca" }, new int[] { 2, 1, 0 });
            questions[3] = new Question("Antes de ingerir o alimento, costuma verificar suas propriedades?", "d.jpg", new string[] { "Sim", "Às vezes", "Nunca" }, new int[] { 2, 1, 0 });
            questions[4] = new Question("Costuma comer sobremesa depois do almoço/jantar?", "e.jpg", new string[] { "Sim", "Às vezes", "Nunca" }, new int[] { 0, 1, 2 });
            questions[5] = new Question("Quantas refeições faz ao dia?", "f.jpg", new string[] { "1 a 3", "4 a 6", "6 ou mais" }, new int[] { 0, 2, 1 });
            questions[6] = new Question("Costuma ingerir alimentos pré-prontos/congelados?", "g.jpg", new string[] { "Sim", "Às vezes", "Nunca" }, new int[] { 0, 1, 2 });
            questions[7] = new Question("Costuma tentar seguir a pirâmide alimentar?", "h.jpg", new string[] { "Sim", "Às vezes", "Nunca" }, new int[] { 2, 1, 0 });
            questions[8] = new Question("¿Cuántos litros de agua por día suele beber?\r\n\r\n-> Quantos litros de água por dia costuma beber?", "i.jpg", new string[] { "0 a 2", "2 a 3", "3 ou mais" }, new int[] { 0, 2, 1 });
            questions[9] = new Question("Está disposto a buscar uma alimentação saudável?", "j.jpg", new string[] { "Sim", "Talvez", "Não" }, new int[] { 2, 1, 0 });
            Navigation.PushModalAsync(new IP());
        }

        private async void AbrirQuestao(object sender, EventArgs e)
        {
            Logo.ScaleTo(2, 1000);
            await Navigation.PushModalAsync(new Questao(questions));
            Logo.ScaleTo(1, 1000);
            //await Navigation.PushModalAsync(new Resultado(15, "111111111"));
        }        
    }
}
