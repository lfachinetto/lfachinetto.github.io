using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;

namespace Tratorfix.Models.Repository
{
    public class Repository
    {
        private TratorfixContext context = new TratorfixContext();

        public IEnumerable<Produto> Produtos
        {
            get { return context.Produtos; }
        }

        public IEnumerable<Orçamento> Orçamentos
        {
            get
            {
                return context.Orçamentos
                      .Include(o => o.LinhasOrçamento
                      .Select(ol => ol.Produto));
            }
        }

        public void SaveOrder(Orçamento order)
        {
            if (order.OrçamentoId == 0)
            {
                order = context.Orçamentos.Add(order);

                foreach (LinhaOrçamento line in order.LinhasOrçamento)
                {
                    context.Entry(line.Produto).State = EntityState.Modified;
                }
            }

            else
            {
                Orçamento dbOrder = context.Orçamentos.Find(order.OrçamentoId);
                if (dbOrder != null)
                {
                    //Ver se tem como arrumar por aqui
                    dbOrder.Nome = order.Nome;
                    dbOrder.CNP = order.CNP;
                    dbOrder.Email = order.Email;
                    dbOrder.IE = order.IE;
                    dbOrder.Bairro = order.Bairro;
                    dbOrder.CEP = order.CEP;
                    dbOrder.Cidade = order.Cidade;
                    dbOrder.Estado = order.Estado;
                    dbOrder.CNP = order.CNP;
                    dbOrder.Numero = order.Numero;
                    dbOrder.Rua = order.Rua;
                    //dbOrder.Senha = order.Senha;
                    dbOrder.TelCelular = order.TelCelular;
                    dbOrder.TelFixo = order.TelFixo;
                    dbOrder.PontoRef = order.PontoRef;
                    dbOrder.Enviado = order.Enviado;
                    dbOrder.Observações = order.Observações;
                }
            }
            context.SaveChanges();
        }
    }
}