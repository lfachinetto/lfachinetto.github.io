using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.ModelBinding;
using Tratorfix.Models;
using Tratorfix.Models.Repository;
namespace Tratorfix.Pages.Admin
{
    public partial class Orçamentos : System.Web.UI.Page
    {
        private Repository repo = new Repository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                int dispatchID;
                if (int.TryParse(Request.Form["dispatch"], out dispatchID))
                {
                    Orçamento myOrder = repo.Orçamentos
                    .Where(o => o.OrçamentoId == dispatchID)
                    .FirstOrDefault();
                    if (myOrder != null)
                    {
                        myOrder.Enviado = true;
                        repo.SaveOrder(myOrder);
                    }
                }
            }
        }

        public IEnumerable<Orçamento> GetOrçamentos([Control] bool showDispatched)
        {
            if (showDispatched)
            {
                return repo.Orçamentos;
            }

            else
            {
                return repo.Orçamentos.Where(o => !o.Enviado);
            }
        }
    }
}