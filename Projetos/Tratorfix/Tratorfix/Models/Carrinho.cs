using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tratorfix.Models
{
    public class Carrinho
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Produto product, string measure, string quantity)
        {
            int cartLineId = lineCollection.Count;
                cartLineId++;
                
            lineCollection.Add(new CartLine
            {
                CartLineId = cartLineId,
                Product = product,
                Measure = measure,
                Quantity = quantity
            });
        }

        public void RemoveLine(int cartLineId)
        {
            lineCollection.RemoveAll(l => l.CartLineId == cartLineId);
            int i = 0;
            int res = 1;
            foreach (CartLine item in lineCollection)
            {
                lineCollection[i].CartLineId = res;
                i++;
                res = i + 1;
            }
        }

        public void Update(int cartLineId, string measure, string quantity)
        {
            lineCollection[cartLineId - 1].Measure = measure;
            lineCollection[cartLineId - 1].Quantity = quantity;
        }

        public void Clear()
        {
            lineCollection.Clear();
        }
        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }

    public class CartLine
    {
        public int CartLineId { get ; set; }
        public Produto Product { get; set; }        
        public string Measure { get; set; }
        public string Quantity { get; set; }
    }
}