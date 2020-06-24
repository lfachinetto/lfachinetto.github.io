using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tratorfix.Models
{
    public class Endereco
    {
        public int EnderecoId { get; set; }

        [Required(ErrorMessage = "Por favor, digite seu CEP")]
        public int CEP { get; set; }

        public string Rua { get; set; }

        [Required(ErrorMessage = "Por favor, digite o número de seu endereço")]
        public int Numero { get; set; }

        public string Bairro { get; set; }
        public int PontoRef { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
    }
}