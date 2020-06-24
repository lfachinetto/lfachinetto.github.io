using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tratorfix.Models
{
    public class Produto
    {
        public int ProdutoId { get; set; }
        public Linha Linha { get; set; }
        public string Nome { get; set; }
        public string Descrição { get; set; }
        public string Imagem { get; set; }
    }
}