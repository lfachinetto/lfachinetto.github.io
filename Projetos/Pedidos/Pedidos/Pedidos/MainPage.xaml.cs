using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using System.Collections.ObjectModel;
using static System.Enum;

namespace Pedidos
{
    public partial class MainPage : ContentPage
    {

        public Pedido PedidoEditando { get; set; } = new Pedido(); //{ Empresa = Empresa.Zamil, Cliente = "Lucas", Cidade = "Passo Fundo", Condições = "Nenhuma", Frete = "Grátis",
        //Itens = new ObservableCollection<Item>() { new Item() { Quantidade = 2, Unidade = Unidade.Kg, Produto = "Prego", Preço = 12.00m, Desconto = 20 } } };
        public static IEnumerable<Empresa> Empresas => GetValues(typeof(Empresa)).Cast<Empresa>();
        
        public MainPage()
        {
            InitializeComponent();
            BindingContext = PedidoEditando;
        }

        private async void AbrirLista(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Itens());
        }

        private void GerarCotação(object sender, EventArgs e)
        {
            PedidoEditando.Tipo = Tipo.Cotação;
            DependencyService.Get<IGerarPdf>().Gerar(PedidoEditando);
        }

        private void GerarPedido(object sender, EventArgs e)
        {
            PedidoEditando.Tipo = Tipo.Pedido;
            DependencyService.Get<IGerarPdf>().Gerar(PedidoEditando);
        }

        private void Limpar(object sender, EventArgs e)
        {
            PedidoEditando = new Pedido();
            BindingContext = PedidoEditando;
        }
    }

    public interface IGerarPdf
    {
        Task Gerar(Pedido ped);
    }
}
