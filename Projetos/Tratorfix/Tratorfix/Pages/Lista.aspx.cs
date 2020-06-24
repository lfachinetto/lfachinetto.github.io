using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tratorfix.Models;
using Tratorfix.Models.Repository;
using Tratorfix.Pages.Helpers;
using System.Web.Routing;


namespace Tratorfix.Pages
{
    public partial class Lista : System.Web.UI.Page
    {
        private Repository repo = new Repository();
        int pageSize;

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Produtos - Tratorfix Parafusos";
            if (IsPostBack)
            {
                if (int.TryParse(Request.Form["add"], out int selectedProductId))
                {
                    Produto selectedProduct = repo.Produtos.Where(p => p.ProdutoId == selectedProductId).FirstOrDefault();
                    if (selectedProduct != null)
                    {
                        SessionHelper.GetCarrinho(Session).AddItem(selectedProduct, Request.Form["medida" + selectedProductId], Request.Form["quantidade" + selectedProductId]);
                        SessionHelper.Set(Session, SessionKey.RETURN_URL, Request.RawUrl);

                        //Response.Redirect(RouteTable.Routes.GetVirtualPath(null, "cart", null).VirtualPath);
                        lblMessage.Visible = true;
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                        //Response.Redirect(@"#" + selectedProductId);
                    }
                }
            }
        }

        public IEnumerable<Produto> GetProdutos()
        {
            pageSize = 50;
            return repo.Produtos
                .OrderBy(p => p.ProdutoId)
                .Skip((CurrentPage - 1) * pageSize)
                .Take(pageSize);
        }

        protected int CurrentPage
        {
            get
            {
                int page = GetPageFromRequest();
                return page; //> MaxPage ? MaxPage : page;
            }
        }

        /*protected int MaxPage
        {
            get
            {
                int prodCount = repo.Produtos.Count();
                return (int)Math.Ceiling((decimal)prodCount / pageSize);
            }
        }*/

        private int GetPageFromRequest()
        {
            int page;
            string reqValue = (string)RouteData.Values["page"] ?? Request.QueryString["page"];
            return reqValue != null && int.TryParse(reqValue, out page) ? page : 1;
        }
    }
}