using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Tratorfix
{
    public class RouteConfig
    {

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute(null, "produtos/{category}/{page}", "~/Pages/Lista.aspx");
            routes.MapPageRoute(null, "produtos/{page}", "~/Pages/Lista.aspx");
            routes.MapPageRoute(null, "", "~/Pages/Home.aspx");
            routes.MapPageRoute("home", "", "~/Pages/Home.aspx");
            routes.MapPageRoute(null, "produtos", "~/Pages/Lista.aspx");
            routes.MapPageRoute("lista", "produtos", "~/Pages/Lista.aspx");
            routes.MapPageRoute("cart", "orçamento", "~/Pages/CarrinhoView.aspx");
            routes.MapPageRoute("enviar", "envio", "~/Pages/Envio.aspx");
            routes.MapPageRoute("sobre", "sobre", "~/Pages/Sobre.aspx");
            routes.MapPageRoute("contato", "contato", "~/Pages/Contato.aspx");
            routes.MapPageRoute("admin_orders", "admin/orçamentos", "~/Pages/Admin/Orçamentos.aspx");
            routes.MapPageRoute("admin_products", "admin/produtos", "~/Pages/Admin/Produtos.aspx");            
        }
    }
}