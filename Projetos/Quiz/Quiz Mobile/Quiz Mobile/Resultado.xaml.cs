using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuizMobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Resultado : ContentPage
	{
        public Resultado(int pont, string respos)
        {
            InitializeComponent();

            if (pont <= 7)
                labelResult.Text = "Sua alimentação precisa melhorar. Busque alimentos menos calóricos\r\ne também mais frutas, verduras e menos industrializados.";
            if (pont >= 8 && pont <= 14)
            {
                star2.Source = "star.png";
                labelResult.Text = "Você se alimenta bem, mas ainda pode melhorar! Tente balancear mais\r\nsuas refeições e ingerir alimentos mais naturais.";
            }
            if (pont >= 15 && pont <= 20)
            {
                star2.Source = "star.png";
                star3.Source = "star.png";
                labelResult.Text = "Parabéns! Sua alimentação é boa, mas sempre podemos melhorar! Tente manter\r\nsua alimentação balanceada aliada a uma rotina de exercícios!";
            }

            /*string respos = "";
            for(int i = 0; i < resps.Length; i++)
            {
                respos += resps[i].ToString();
            }*/

            star1.ScaleTo(3, 3000);
            star2.ScaleTo(3, 3000);
            star3.ScaleTo(3, 3000);

            if (!Question.Enviar(respos + pont.ToString("00")))
                DisplayAlert("Alerta!", "Tablet desconectado da rede, por favor avise o grupo.", "OK, vou avisar");
        }

        private void Fim(object sender, EventArgs e)
        {
            if (!Question.Enviar("fim"))
                DisplayAlert("Alerta!", "Tablet desconectado da rede, por favor avise o grupo.", "OK, vou avisar");
            Navigation.PopModalAsync();
            Navigation.PopModalAsync();
        }
    }
}