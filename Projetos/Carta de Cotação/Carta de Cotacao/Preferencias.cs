using System;

namespace CartaDeCotacao
{
    [Serializable]
    public class Preferencias
    {
        public string PastaPDF { get; set; }
        public string NomeFuncionario { get; set; }
        public bool Validade { get; set; }

        public Preferencias()
        {
            PastaPDF = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Validade = true;
        }
    }
}
