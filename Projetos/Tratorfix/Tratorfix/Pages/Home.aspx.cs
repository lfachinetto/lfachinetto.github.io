using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tratorfix.Models;
using Tratorfix.Models.Repository;

namespace Tratorfix.Pages
{
    public partial class Home : System.Web.UI.Page
    {
        private Repository repo = new Repository();
        public int i = 1;
        int pageSize;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public IEnumerable<Produto> GetProdutos()
        {
            pageSize = 6;
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

        protected void cadastrar_Click(object sender, EventArgs e)
        {
            string mensagem =
                "Nome: " + string.Format("{0}", Request.Form["nome"]) + "\r\n" +
                "E-mail: " + string.Format("{0}", Request.Form["email"]);
            SmtpClient client = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("cartadecotacao@gmail.com", "cadeco132")
            };

            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            var tcs = new TaskCompletionSource<bool>();
            client.SendCompleted += (s, a) =>
            {
                if (a.Error != null)
                    tcs.TrySetException(a.Error);
                else
                    if (a.Cancelled)
                    tcs.TrySetCanceled();
                else
                    tcs.TrySetResult(true);
            };
            var mes = new MailMessage
            {
                Subject = "Novo e-mail cadastrado no newstaller",
                Body = mensagem,
                From = new MailAddress("cartadecotacao@gmail.com", "Carta de Cotação")
            };
            mes.To.Add(new MailAddress("lucasmfachinetto@gmail.com", "Lucas Mezzomo Fachinetto"));
            client.Send(mes);
            newstaller.Visible = false;
            successMessage.Visible = true;
        }

        public string ProdutosUrl
        {
            get
            {
                return RouteTable.Routes.GetVirtualPath(null, "lista", null).VirtualPath;
            }
        }        
    }
}