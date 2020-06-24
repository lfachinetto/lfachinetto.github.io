using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Routing;
using Tratorfix.Models.Repository;

namespace Tratorfix.Controls
{
    public partial class ListaProdutos : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected IEnumerable<string> GetProdutos()
        {
            return new Repository().Produtos
                .Select(p => p.Nome)
                .Distinct()
                .OrderBy(n  => n);
        }

        protected string CreateHomeLinkHtml()
        {
            string path = RouteTable.Routes.GetVirtualPath(null, null).VirtualPath;
            return string.Format("<a href='{0}'>Todos</a>", path);
        }

        protected string CreateLinkHtml(string produto)
        {

            string path = RouteTable.Routes.GetVirtualPath(null, null, new RouteValueDictionary() { { "produto", produto }, { "page", "1" } }).VirtualPath;
            string.Format("<a href='{0}'>{1}</a>", path, produto);

            return string.Format("<a href='{0}'>{1}</a>", path, produto);
        }
    }
}