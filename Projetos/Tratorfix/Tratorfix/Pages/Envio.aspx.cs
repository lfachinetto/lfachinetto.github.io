using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.ModelBinding;
using Tratorfix.Models;
using Tratorfix.Models.Repository;
using Tratorfix.Pages.Helpers;
using System.Threading.Tasks;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Tratorfix.Pages
{
    public partial class Envio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Envio de orçamento - Tratorfix Parafusos";
            envioForm.Visible = true;
            checkoutMessage.Visible = false;
            if (IsPostBack)
            {
                Orçamento myOrder = new Orçamento();
                if (TryUpdateModel(myOrder,
                new FormValueProvider(ModelBindingExecutionContext)))
                {
                    myOrder.LinhasOrçamento = new List<LinhaOrçamento>();
                    Carrinho myCart = SessionHelper.GetCarrinho(Session);
                    foreach (CartLine line in myCart.Lines)
                    {
                        myOrder.LinhasOrçamento.Add(new LinhaOrçamento
                        {
                            Orçamento = myOrder,
                            Produto = line.Product
                        });
                    }
                    new Repository().SaveOrder(myOrder);
                    myCart.Clear();

                    envioForm.Visible = false;
                    EnviarOrçamento(myOrder);
                    checkoutMessage.Visible = true;
                }
            }
        }

        public static void EnviarOrçamento(Orçamento orc)
        {
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
                Subject = "Notificação de orçamento",
                Body = "Orçamento recebido, consulte banco de dados.",
                From = new MailAddress("cartadecotacao@gmail.com", "Site Tratorfix")
            };
            mes.To.Add(new MailAddress("lucasmfachinetto@gmail.com", "Lucas Mezzomo Fachinetto"));
            client.Send(mes);
        }
    }
}