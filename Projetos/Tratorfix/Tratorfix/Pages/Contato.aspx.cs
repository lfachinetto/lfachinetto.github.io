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

namespace Tratorfix.Pages
{
    public partial class Contato : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Contato - Tratorfix Parafusos";
            contato.Visible = true;
            checkoutMessage.Visible = false;            
        }

        protected void enviar_Click(object sender, EventArgs ev)
        {
            string mensagem =
                "Nome: " + string.Format("{0}", Request.Form["nome"]) + "\r\n" +
                "E-mail: " + string.Format("{0}", Request.Form["email"]) + "\r\n" +
                "Telefone: " + string.Format("{0}", Request.Form["telefone"]) + "\r\n\r\n" +
                "Assunto: " + string.Format("{0}", Request.Form["assunto"]) + "\r\n\r\n" +
                "Mensagem:\r\n" +
                string.Format("{0}", Request.Form["mensagem"]);
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
            client.SendCompleted += (s, e) =>
            {
                if (e.Error != null)
                    tcs.TrySetException(e.Error);
                else
                    if (e.Cancelled)
                    tcs.TrySetCanceled();
                else
                    tcs.TrySetResult(true);
            };
            var mes = new MailMessage
            {
                Subject = "Nova mensagem enviada pelo site: " + string.Format("{0}", Request.Form["assunto"]),
                Body = mensagem,
                From = new MailAddress("cartadecotacao@gmail.com", "Carta de Cotação")
            };
            mes.To.Add(new MailAddress("lucasmfachinetto@gmail.com", "Lucas Mezzomo Fachinetto"));
            client.Send(mes);
            contato.Visible = false;
            checkoutMessage.Visible = true;
        }
    }
}