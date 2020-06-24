using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pedidos
{
    public class Item : INotifyPropertyChanged
    {
        private int _quantidade;
        public int Quantidade { get { return _quantidade; }
                                set { if (_quantidade == value)
                                        return;
                                      else
                                        _quantidade = value;
                                      OnPropertyChanged();
                                      OnPropertyChanged(nameof(Valor)); } }

        private Unidade _unidade;
        public Unidade Unidade { get { return _unidade; } set { if (_unidade == value) return; else _unidade = value; OnPropertyChanged(); OnPropertyChanged(nameof(Valor)); } }

        private string _produto;
        public string Produto { get { return _produto; } set { if (_produto == value) return; else _produto = value; OnPropertyChanged(); } }

        private decimal _preço;
        public decimal Preço { get { return _preço; } set { if (_preço == value) return; else _preço = value; OnPropertyChanged(); OnPropertyChanged(nameof(Valor)); } }
        
        private int _desconto;
        public int Desconto { get { return _desconto; } set { if (_desconto == value) return; else _desconto = value; OnPropertyChanged(); OnPropertyChanged(nameof(Valor)); } }
        
        public decimal Valor => (Quantidade * Preço) - (Quantidade * Preço * Desconto / 100);

        public Item()
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}