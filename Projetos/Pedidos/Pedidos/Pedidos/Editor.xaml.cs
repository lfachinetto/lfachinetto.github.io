using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using static System.Enum;
using System.Globalization;

namespace Pedidos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Editor : ContentPage
    {
        
        public Item ItemEditando { get; set; }
        public Pedido PedidoEditando => App.Current.MainPage.BindingContext as Pedido;
        public static IEnumerable<Unidade> Unidades => GetValues(typeof(Unidade)).Cast<Unidade>();

        public Editor(Item i)
        {
            ItemEditando = i;
            
            InitializeComponent();
            pickerUnd.ItemsSource = Unidades.ToList();

            BindingContext = ItemEditando;
        }

        private void SalvarItem(object sender, EventArgs e)
        {
            if (!PedidoEditando.Itens.Contains(ItemEditando))
                PedidoEditando.Itens.Add(ItemEditando);

            PedidoEditando.RefreshTotal();

            Navigation.PopModalAsync();
        }

        private void ApagarItem(object sender, EventArgs e)
        {
            if (PedidoEditando.Itens.Contains(ItemEditando))
                PedidoEditando.Itens.Remove(ItemEditando);

            Navigation.PopModalAsync();
        }        
    }

    public class Comportamento : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            string entryText = entry.Text;
            
            if (entryText.Length > 2 || entryText.Contains("."))
                entryText = entryText.Remove(entryText.Length - 1);

            if (entryText == "")
                entryText = "0";

            entry.Text = entryText;
        }
    }
}
