using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tratorfix.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "Por favor, digite seu nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Por favor, digite seu CPF/CNPJ")]
        public int CNP { get; set; }

        public int IE { get; set; }

        [Required(ErrorMessage = "Por favor, digite seu telefone celular")]
        public string TelCelular { get; set; }

        [Required(ErrorMessage = "Por favor, digite seu telefone fixo")]
        public string TelFixo { get; set; }

        public Endereco EndEntrega { get; set; }
        public Endereco EndFat { get; set; }

        [Required(ErrorMessage = "Por favor, digite seu e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Por favor, digite uma nova senha")]
        public string Senha { get; set; }
    }
}