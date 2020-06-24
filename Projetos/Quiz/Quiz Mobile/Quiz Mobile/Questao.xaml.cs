using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QuizMobile
{
	public partial class Questao : ContentPage
	{
        int num = 0;
        int max;
        Question[] questoes;
        int pontu;

        public Questao(Question[] questions)
		{
			InitializeComponent();
            max = questions.Length;
            questoes = questions;
            perg.Text = questoes[num].Pergunta;            
            resList.ItemsSource = questoes[num].Respostas;
            img.Source = questoes[num].Foto;
            num++;
            if (!Question.Enviar("ini"))
                DisplayAlert("Alerta!", "Tablet desconectado da rede, por favor avise o grupo.", "OK, vou avisar");
        }

        private async void Prox(object sender, EventArgs e)
        {
            if (resList.SelectedIndex != -1)
            {
                questoes[num - 1].Resultado = resList.SelectedIndex;
                pontu += questoes[num - 1].Pontuacoes[resList.SelectedIndex];
                if (num == max)
                {
                    /*int[] respos = null;
                    for (int runs = 0; runs < questoes.Length; runs++)
                    {
                        respos[runs] = questoes[runs].Resultado;
                    }*/

                    string resps = "";
                    for (int i = 0; i < questoes.Length; i++)
                    {
                        resps += questoes[i].Resultado;
                    }

                    await Navigation.PushModalAsync(new Resultado(pontu, resps));
                }
                else
                {
                    perg.Text = questoes[num].Pergunta;
                    resList.ItemsSource = questoes[num].Respostas;
                    img.Source = questoes[num].Foto;
                    num++;
                    if (num == max)
                        proxButton.Text = "Resultado";                    
                }
            }
            else
                await DisplayAlert("Atenção!", "Você precisa selecionar uma resposta.", "OK");
        }

        
    }
}
