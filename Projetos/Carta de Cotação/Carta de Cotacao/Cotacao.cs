using System;
using System.Collections.Generic;

using System.Windows.Forms;
using System.Globalization;

namespace CartaDeCotacao
{
    public class Cotacao
        {
            private const int maxParcelas = 7; //Número máximo de parcelas            

            private static List<string> formasPagto = new List<string> { "Carnê", "Débito" };
            private static List<string> tiposFranq = new List<string> { "Básica", "Reduzida" };

            public string Seguradora { get; set; }
            public List<Parcelamento> TabelaParcelamento { get; set; }

            public FipeType Tipo { get; set; }
            public string Cliente { get; set; }
            public DateTime Data { get; set; }
            public string Modelo { get; set; }
            public int AnoFabricacao { get; set; }
            public string AnoModelo { get; set; }
            public string Marca { get; set; }
            public decimal ValorFipe { get; set; }
            public decimal PorContratada { get; set; }
            public decimal DanosMateriais { get; set; }
            public decimal DanosCorporais { get; set; }
            public decimal DanosMorais { get; set; }
            public decimal APPMorte { get; set; }
            public decimal APPInvalidez { get; set; }
            public decimal FranquiaBasica { get; set; }
            public decimal ValFranqBasicaAVista { get; set; }
            public decimal FranquiaReduzida { get; set; }
            public decimal ValFranqReduzidaAVista { get; set; }
            public string Observacoes { get; set; }
            public bool ErroDeFormato { get; set; }
            public bool CelClaudio { get; set; }
            public bool NomeFuncionario { get; set; }
            public string Funcionario { get; set; }
            public bool DebitoBB { get; set; }
            public bool Rcf { get; set; }
            public bool NomeClaudio { get; set; }
            public decimal Equipamento { get; set; }
            public decimal Carroceria { get; set; }
            public bool Vd { get; set; }
            public bool MostrarValidade { get; set; }
            public DateTime Validade { get; set; }
            public Fill Preenchimento { get; set; }
    
        public Cotacao (FipeType tiposeg, string cliente, DateTime data, string seguradora, string modelo, string anoFabricacao, string anoModelo,
            string marca, string valorFipe, string porContratada, string danosMateriais, string danosCorporais, string danosMorais,
                string appMorte, string appInvalidez, string franquiaBasica,string valFranqBasicaAVista, string franquiaReduzida,
                    string valFranqReduzidaAVista, string observacoes, bool nomeFuncionario, string funcionario,
                        bool debitobb, bool rcf, string equipamento, string carroceria, bool vd, bool mostrarvalidade, DateTime validade, Fill preenchimento)
        {
            try
            {
                var segerrada = new FormatException();
                Tipo = tiposeg;
                Cliente = cliente;
                Data = data;
                if (seguradora != "")
                    Seguradora = seguradora;
                else            
                    throw segerrada;
                Modelo = modelo;
                AnoFabricacao = Convert.ToInt32(anoFabricacao);
                AnoModelo = anoModelo;
                Marca = marca;
                ValorFipe = Decimal.Parse(valorFipe, NumberStyles.Currency, CultureInfo.CurrentCulture);
                if (porContratada != "")
                    PorContratada = Decimal.Parse(porContratada);
                else
                    PorContratada = 0.0m;
                Rcf = rcf;
                DanosMateriais = Decimal.Parse(danosMateriais, NumberStyles.Currency, CultureInfo.CurrentCulture);
                DanosCorporais = Decimal.Parse(danosCorporais, NumberStyles.Currency, CultureInfo.CurrentCulture);
                DanosMorais = Decimal.Parse(danosMorais, NumberStyles.Currency, CultureInfo.CurrentCulture);
                APPMorte = Decimal.Parse(appMorte, NumberStyles.Currency, CultureInfo.CurrentCulture);
                APPInvalidez = Decimal.Parse(appInvalidez, NumberStyles.Currency, CultureInfo.CurrentCulture);                
                FranquiaBasica = Decimal.Parse(franquiaBasica, NumberStyles.Currency, CultureInfo.CurrentCulture);
                ValFranqBasicaAVista = Decimal.Parse(valFranqBasicaAVista, NumberStyles.Currency, CultureInfo.CurrentCulture);
                FranquiaReduzida = Decimal.Parse(franquiaReduzida, NumberStyles.Currency, CultureInfo.CurrentCulture);
                ValFranqReduzidaAVista = Decimal.Parse(valFranqReduzidaAVista, NumberStyles.Currency, CultureInfo.CurrentCulture);
                if (FranquiaBasica + ValFranqBasicaAVista + FranquiaReduzida + ValFranqReduzidaAVista == 0.0m)
                    throw segerrada;

                NomeFuncionario = nomeFuncionario;
                Funcionario = funcionario;
                Observacoes = observacoes;

                DebitoBB = debitobb;
                ErroDeFormato = false;

                if (nomeFuncionario == true)
                {
                    Home.preferencias.NomeFuncionario = funcionario;
                    WebFile.Prefs =  Home.preferencias;
                }

                Equipamento = Decimal.Parse(equipamento, NumberStyles.Currency, CultureInfo.CurrentCulture);
                Carroceria = Decimal.Parse(carroceria, NumberStyles.Currency, CultureInfo.CurrentCulture);
                Vd = vd;
                MostrarValidade = mostrarvalidade;
                Validade = validade;
                Preenchimento = preenchimento;
            }
            catch (FormatException)
            {
                Relatório.AdicionarAoRelatorio("Campos preenchidos incorretamente");
                MessageBox.Show("Algum campo foi preenchido incorretamente.\r\nCorrija-o e tente novamente.",
                "Carta de Cotação",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                ErroDeFormato = true;
            }

            TabelaParcelamento = new List<Parcelamento>();
            int max = maxParcelas;
            if (Seguradora == "Allianz")
                max = 6; //Ajusta o número máximo de parcelas conforme a seguradora;
            
            for (int i = 0; i < max; i++)
            {
                foreach (string tipo in tiposFranq)
                    foreach (string forma in formasPagto)
                    {
                        try
                        {
                            object o = WebFile.Extras.CreateInstance("CartaDeCotacao.Parcelamento");
                            o.GetType().GetConstructors()[1].Invoke(o, new object[] { Seguradora, forma, tipo, ValFranqBasicaAVista, ValFranqReduzidaAVista });
                            o.GetType().GetMethod("CalculaParcela").Invoke(o, new object[] { 1 + i });

                            Parcelamento parc = new Parcelamento()
                            {
                                Seguradora = (string)o.GetType().GetProperty("Seguradora").GetValue(o, null),
                                FormaPagto = (string)o.GetType().GetProperty("FormaPagto").GetValue(o, null),
                                TipoFranq = (string)o.GetType().GetProperty("TipoFranq").GetValue(o, null),
                                Basica = (decimal)o.GetType().GetProperty("Basica").GetValue(o, null),
                                Reduzida = (decimal)o.GetType().GetProperty("Reduzida").GetValue(o, null),
                                NumParcelas = (int)o.GetType().GetProperty("NumParcelas").GetValue(o, null),
                                Descricao = (string)o.GetType().GetProperty("Descricao").GetValue(o, null),
                                ValParcela = (decimal)o.GetType().GetProperty("ValParcela").GetValue(o, null)
                            };

                            TabelaParcelamento.Add(parc);
                        }
                        catch (Exception ex)
                        {
                            Relatório.ExHandler(ex, "Uso do parcelamento");
                        }
                    }

                if (i == 0) //Começa no zero (Á vista)
                    i++;

                if (i == 1)
                    i++;

                if (Seguradora == "Allianz" && i == 3)
                    i++;
            }
        }
    }
}