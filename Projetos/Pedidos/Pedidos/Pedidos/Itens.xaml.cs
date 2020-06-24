using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pedidos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Itens : ContentPage
    {
        public Pedido PedidoEditando => App.Current.MainPage.BindingContext as Pedido;

        public Itens()
        {
            InitializeComponent();
            BindingContext = PedidoEditando;
        }

        private async void EditarItem(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushModalAsync(new Editor(e.Item as Item));
        }

        private async void NovoItem(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Editor(new Item()));
        }
    }
}