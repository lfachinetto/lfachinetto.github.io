using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;

namespace Pedidos
{
    public enum Tipo { Cotação, Pedido }

    public enum Empresa { Tratorfix, Zamil, Totalfix }
    public enum Unidade { Un, Kg }

    public class Pedido : INotifyPropertyChanged
    {
        private Tipo _tipo;
        public Tipo Tipo { get { return _tipo; } set { if (_tipo == value) return; else _tipo = value; OnPropertyChanged(); } }

        private Empresa _empresa;
        public Empresa Empresa { get { return _empresa; } set { if (_empresa == value) return; else _empresa = value; OnPropertyChanged(); } }

        private string _cliente;
        public string Cliente { get { return _cliente; } set { if (_cliente == value) return; else _cliente = value; OnPropertyChanged(); } }

        private string _cidade;
        public string Cidade { get { return _cidade; } set { if (value == _cidade) return; else _cidade = value; OnPropertyChanged(); } }

        private string _condições;
        public string Condições { get { return _condições; } set { if (_condições == value) return; else _condições = value; OnPropertyChanged(); } }

        private string _frete;
        public string Frete { get { return _frete; } set { if (_frete == value) return; else _frete = value; OnPropertyChanged(); } }

        private string _representante = "Clauciano Fachinetto";
        public string Representante { get { return _representante; } set { if (_representante == value) return; else _representante = value; OnPropertyChanged(); } }

        private ObservableCollection<Item> _itens = new ObservableCollection<Item>();
        public ObservableCollection<Item> Itens { get { return _itens; } set { if (_itens.SequenceEqual(value)) return; else _itens = value; OnPropertyChanged(); OnPropertyChanged(nameof(Total)); } }

        public decimal Total => Itens.Sum(i => i.Valor);
        
        public Pedido()
        {
            Itens.CollectionChanged += Itens_CollectionChanged;
        }        

        private void Itens_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Total));
        }
        
        public void RefreshTotal()
        {
            OnPropertyChanged(nameof(Total));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
