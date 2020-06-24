using System;
using System.Collections.Generic;

namespace CartaDeCotacao
{
    [Serializable]
    public class Fill
    {
        public bool Tipo1 { get; set; }
        public bool Tipo2 { get; set; }
        public bool Tipo3 { get; set; }
        public string Cliente { get; set; }
        public DateTime Data { get; set; }
        public string Modelo { get; set; }
        public int AnoFabricacao { get; set; }
        public string AnoModelo { get; set; }
        public string Marca { get; set; }
        public string ValorFipe { get; set; }
        public string CodigoFipe { get; set; }
        public List<FipeModel> AnoModeloFipe { get; set; }
        public int AnoModeloSelec { get; set; }
        public bool ModoManual { get; set; }
        public bool Vd { get; set; }
        public string Seguradora { get; set; }
        public bool DebitoBB { get; set; }
        public decimal PorContratada { get; set; }
        public string DanosMateriais { get; set; }
        public string DanosCorporais { get; set; }
        public string DanosMorais { get; set; }
        public string APPMorte { get; set; }
        public string APPInvalidez { get; set; }
        public string FranquiaBasica { get; set; }
        public string ValFranqBasicaAVista { get; set; }
        public string FranquiaReduzida { get; set; }
        public string ValFranqReduzidaAVista { get; set; }
        public string Observacoes { get; set; }
        public bool BoolNomeFuncionario { get; set; }
        public string NomeFuncionario { get; set; }
        public bool Rcf { get; set; }
        public string Equipamento { get; set; }
        public string Carroceria { get; set; }
        public bool BoolValidade { get; set; }
        public DateTime Validade { get; set; }        
    }
}