using System;
using System.Collections.Generic;

namespace CartaDeCotacao
{
    [Serializable]
    public class Seguradora
    {
        private static List<string> seguradoras = new List<string> { "", "Allianz", "Azul", "Bradesco", "HDI", "Itaú", "Liberty", "Mapfre", "Porto Seguro", "Sompo", "Sul América", "Tokio Marine"};

        public string Nome { get; set; }
        public bool Disp { get; set; }
        public List<string> Observacoes { get; set; }
        public string ObservacoesMC { get; set; }
        public bool Moto { get; set; }
        public bool Caminhao { get; set; }

        public Seguradora(string nome)
        {
            Nome = nome;
            Disp = true;
            Observacoes = new List<string>();
            ObservacoesMC = "Assistência 24 horas.";
        }

        public static List<Seguradora> GeraSeguradoras()
        {
            List<Seguradora> result = new List<Seguradora>();            
            foreach (string nome in seguradoras)
            {
                Seguradora seg = new Seguradora(nome);
                switch (nome)
                {
                    case "Allianz":
                        seg.Caminhao = true;
                        seg.Observacoes.Add("Pagamento em 1 + 5 pelo preço à vista, se for no débito (exceto Banrisul e Sicredi).");
                        seg.Observacoes.Add("Assistência 24 horas s/limite de KM.");
                        seg.Observacoes.Add("Cobertura de vidros, faróis, lanternas e retrovisores.");
                        seg.Observacoes.Add("Carro reserva 20 dias com ar condicionado em oficinas referenciadas.");
                        break;

                    case "Azul":
                        seg.Observacoes.Add("Pagamento em até 1 + 3 pelo preço à vista, se for no débito (exceto Banrisul e Sicredi).");
                        seg.Observacoes.Add("Assistência 24 horas 400 KM.");
                        seg.Observacoes.Add("Cobertura de vidros, retrovisores, faróis e lanternas.");
                        break;

                    case "Bradesco":
                        seg.Moto = true;
                        seg.Caminhao = true;                        
                        seg.Observacoes.Add("Pagamento em 1 + 3 pelo preço à vista.");
                        seg.Observacoes.Add("Assistência 24 horas 400 KM de guincho.");
                        seg.Observacoes.Add("Cobertura vidros, faróis, lanternas e retrovisores.");
                        seg.Observacoes.Add("Carro reserva por 10 dias.");
                        break;

                    case "HDI":
                        seg.Caminhao = true;
                        seg.Observacoes.Add("Pagamento em 1 + 3 sem juros no carnê ou débito (exceto no Banco do Brasil e Itaú).");
                        seg.Observacoes.Add("Assistência 24 horas 200 Km de guincho.");
                        seg.Observacoes.Add("Cobertura de vidros, faróis, lanternas e retrovisores.");
                        seg.Observacoes.Add("Carro reserva 7 dias.");
                        break;

                    case "Itaú":
                        seg.Caminhao = true;
                        seg.Observacoes.Add("Pagamento em até 1 + 3 pelo preço à vista, se for no débito (exceto Banrisul, Caixa e Sicredi).");
                        seg.Observacoes.Add("Assistência 24 horas sem limite de KM.");
                        seg.Observacoes.Add("Cobertura de vidros, faróis, lenternas e retrovisores.");
                        seg.Observacoes.Add("10 dias de carro reserva.");
                        break;

                    case "Liberty":
                        seg.Caminhao = true;
                        seg.Observacoes.Add("Pagamento em até 1 + 3 pelo preço à vista, se for no débito (exceto Sicredi).");
                        seg.Observacoes.Add("Assistência 24 horas sem limite de KM.");
                        seg.Observacoes.Add("Cobertura de vidros, faróis, lanternas e retrovisores.");
                        seg.Observacoes.Add("Carta verde por um ano.");
                        seg.Observacoes.Add("Carro reserva 7 dias.");
                        break;

                    case "Mapfre":                        
                        seg.Moto = true;
                        seg.Caminhao = true;
                        seg.Observacoes.Add("Pagamento em até 1 + 5 pelo preço à vista, se for no débito (exceto Banrisul).");
                        seg.Observacoes.Add("Assistência 24 horas - 250 Km de guincho.");
                        seg.Observacoes.Add("Cobertura de vidros, faróis, lanternas e retrovisores.");
                        seg.Observacoes.Add("Carro reserva 7 dias.");
                        break;

                    case "Porto Seguro":
                        seg.Moto = true;
                        seg.Observacoes.Add("Pagamento em até 1 + 3 pelo preço à vista (exceto Banrisul e Sicredi).");
                        seg.Observacoes.Add("Assistência 24 horas sem limite de KM.");
                        seg.Observacoes.Add("Cobertura de vidros, faróis, lanternas e retrovisores.");
                        seg.Observacoes.Add("Carro reserva 15 dias ou desconto de 25% na franquia, limitado a 500 reais,");
                        seg.Observacoes.Add("em sinistro parcial indenizável, se o veículo for reparado em oficina referenciada.");
                        break;

                    case "Sompo":
                        seg.Caminhao = true;
                        seg.Observacoes.Add("Indisponível");
                        break;

                    case "Sul América":
                        seg.Caminhao = true;
                        seg.Moto = true;
                        seg.Observacoes.Add("Pagamento em até 1 + 4 pelo preço à vista, se for no débito (exceto Banrisul).");
                        seg.Observacoes.Add("Caso o débito não seja no Banco do Brasil, o valor à vista será de: XXX.");
                        seg.Observacoes.Add("Assistência 24 horas sem limite de Km.");
                        seg.Observacoes.Add("Cobertura de vidros, retrovisores, faróis e lanternas.");
                        seg.Observacoes.Add("Carro reserva 7 dias.");
                        break;

                    case "Tokio Marine":
                        seg.Caminhao = true;
                        seg.Observacoes.Add("Pagamento em até 1 + 5 pelo preço à vista, se for no débito (exceto Banrisul e Sicredi).");
                        seg.Observacoes.Add("Assistência 24 horas 300 KM.");
                        seg.Observacoes.Add("Cobertura de vidros, retrovisores, faróis e lanternas.");
                        seg.Observacoes.Add("Carro reserva 7 dias.");
                        seg.Observacoes.Add("Desconto na franquia de 30%, se o veículo for reparado em oficina referenciada e o");
                        seg.Observacoes.Add("valor do conserto ficar superior ao valor da franquia estipulada na apólice.");
                        break;
                    case "":
                        seg.Moto = true;
                        seg.Caminhao = true;
                        seg.ObservacoesMC = "";
                        break;
                }
                result.Add(seg);
            }
            return result;
        }
    }
}