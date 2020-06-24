using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Tratorfix.Pages
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string HomeUrl
        {
            get
            {
                return generateURL("home");
            }
        }

        public string ListaUrl
        {
            get
            {
                return generateURL("lista");
            }
        }

        public string OrçamentoUrl
        {
            get
            {
                return generateURL("cart");
            }
        }

        public string SobreUrl
        {
            get
            {
                return generateURL("sobre");
            }
        }

        public string ContatoUrl
        {
            get
            {
                return generateURL("contato");
            }
        }        

        private string generateURL(string routeName)
        {
            return RouteTable.Routes.GetVirtualPath(null, routeName, null).VirtualPath;
        }
    }
}