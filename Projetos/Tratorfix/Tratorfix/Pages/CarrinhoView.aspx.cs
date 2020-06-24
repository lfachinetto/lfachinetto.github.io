using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tratorfix.Models;
using Tratorfix.Pages.Helpers;
using Tratorfix.Models.Repository;
using System.Web.Routing;

namespace Tratorfix.Pages
{
    public partial class CarrinhoView : System.Web.UI.Page
    {
        public string medidaAtual;
        public string quantidadeAtual;

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Orçamento online - Tratorfix Parafusos";
            if (IsPostBack)
            {
                Repository repo = new Repository();
                if (int.TryParse(Request.Form["remove"], out int lineId))
                {
                        SessionHelper.GetCarrinho(Session).RemoveLine(lineId);
                }

                if (int.TryParse(Request.Form["alterar"], out int lineIdStart))
                {
                    var a = GetCartLines().ToList();
                    medidaAtual = GetCartLines().ToList()[lineIdStart - 1].Measure;
                    quantidadeAtual = GetCartLines().ToList()[lineIdStart - 1].Quantity;

                    area.InnerHtml = @"Medida:<input type =""text"" class=""form-control"" style=""color: black"" name=""medidaAlterada"" runat=""server"" value=""" + medidaAtual + @""" /><br />";
                    area.InnerHtml = area.InnerHtml + @"Quantidade:<input type = ""text"" class=""form-control"" style=""color: black"" name=""quantidadeAlterada"" runat=""server"" value=""" + quantidadeAtual + @""" /><br/>";
                    area.InnerHtml = area.InnerHtml + @"<a href=""alterarItem""><button type = ""submit"" class=""btn btn-success"" name=""alterarItem"" value=""" + lineIdStart.ToString() + @""">Alterar item</button></a><br/>";

                    area.Visible = true;
                }

                if (int.TryParse(Request.Form["alterarItem"], out int lineIdTwo))
                {
                        SessionHelper.GetCarrinho(Session).Update(lineIdTwo, Request.Form["medidaAlterada"], Request.Form["quantidadeAlterada"]);
                    area.Visible = false;
                                                
                }

                /*if (int.TryParse(Request.Form["addNew"], out int selectedProductId))
                {
                    Produto selectedProduct = repo.Produtos.Where(p => p.ProdutoId == selectedProductId).FirstOrDefault();
                    if (selectedProduct != null)
                    {
                        SessionHelper.GetCarrinho(Session).AddItem(selectedProduct,);
                        SessionHelper.Set(Session, SessionKey.RETURN_URL, Request.RawUrl);

                        Response.Redirect(RouteTable.Routes.GetVirtualPath(null, "cart", null).VirtualPath);
                    }
                }*/
            }
        }

        public IEnumerable<CartLine> GetCartLines()
        {
            return SessionHelper.GetCarrinho(Session).Lines;
        }

        public string ReturnUrl
        {
            get
            {
                return SessionHelper.Get<string>(Session, SessionKey.RETURN_URL);
            }
        }

        public string ContatoUrl
        {
            get
            {
                return RouteTable.Routes.GetVirtualPath(null, "contato", null).VirtualPath;
            }
        }

        public string CheckoutUrl
        {
            get
            {
                return RouteTable.Routes.GetVirtualPath(null, "enviar", null).VirtualPath;
            }
        }

        protected void Unnamed_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Texto alterado");
        }
    }
}