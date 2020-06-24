using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CartaDeCotacao
{
    [Serializable]
    public class Status
    {
        public List<Seguradora> Segs { get; set; }
        public bool ProgDisp { get; set; }
        public bool Fipe { get; set; }
        public BindingList<Versao> VersoesFunc { get; set; }
        public string Mensagem { get; set; }
        public string CorMsg { get; set; }
        public bool PastaAPPDataExiste;

        public Status()
        {
            VersoesFunc = new BindingList<Versao>();
            ProgDisp = true;
            Fipe = true;
            Mensagem = "";
            CorMsg = "Black";
            Segs = Seguradora.GeraSeguradoras();
       }
    }

    [Serializable]
    public class Versao
    {
        public string VersionNum { get; set; }
    }
}