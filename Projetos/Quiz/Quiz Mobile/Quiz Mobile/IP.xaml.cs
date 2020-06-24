using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuizMobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IP : ContentPage
	{
		public IP ()
		{
			InitializeComponent ();
		}

        private void Entry_Completed(object sender, EventArgs e)
        {
            if (NumberPicker.SelectedItem != null && IPEntry.Text != "")
            {
                MainPage.numero = Int32.Parse(NumberPicker.SelectedItem.ToString());
                MainPage.ip = "http://" + IPEntry.Text + ":4000/";
                if (!Question.Enviar("con"))
                    DisplayAlert("Alerta!", "Tablet desconectado da rede, por favor avise o grupo.", "OK, vou avisar");
                Navigation.PopModalAsync();
            }
            else
                DisplayAlert("Alerta!", "Preencha todos os campos antes de prosseguir.", "OK");
        }
    }
}