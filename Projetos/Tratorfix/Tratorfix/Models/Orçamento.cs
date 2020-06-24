using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tratorfix.Models
{
    public class Orçamento
    {
        public int OrçamentoId { get; set; }

        //public Cliente Cliente { get; set; }

        //CLIENTE PROVISÓRIO

        public string Nome { get; set; }        
        public int CNP { get; set; }
        public int IE { get; set; }        
        public string TelCelular { get; set; }        
        public string TelFixo { get; set; }

        //Endereço provisório (sem discriminação Entrega/Faturameto), aqui fica os dois endereços
        
        public int CEP { get; set; }
        public string Rua { get; set; }
        public int Numero { get; set; }
        public string Bairro { get; set; }
        public int PontoRef { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        public string Email { get; set; }                
        //public string Senha { get; set; }

        //FIM CLIENTE PROVISÓRIO

        public bool Enviado { get; set; }
        public string Observações { get; set; }
        public virtual List<LinhaOrçamento> LinhasOrçamento { get; set; }
    }

    public class LinhaOrçamento
    {
        public int LinhaOrçamentoId { get; set; }
        public Orçamento Orçamento { get; set; }
        public Produto Produto { get; set; }
        public int Quantidade { get; set; }
        public string Medida { get; set; }
    }
}