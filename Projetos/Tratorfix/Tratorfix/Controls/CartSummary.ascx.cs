using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Routing;
using Tratorfix.Models;
using Tratorfix.Pages.Helpers;

namespace Tratorfix.Controls
{
    public partial class WebUserControl1 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Carrinho myCart = SessionHelper.GetCarrinho(Session);
            csQuantity.InnerText = myCart.Lines.Count().ToString();
            csLink.HRef = RouteTable.Routes.GetVirtualPath(null, "cart", null).VirtualPath;
        }
    }
}