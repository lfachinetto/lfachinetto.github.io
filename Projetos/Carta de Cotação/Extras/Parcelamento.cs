using System;

namespace CartaDeCotacao
{
    public class Parcelamento
    {
        public string Seguradora { get; set; }
        public string FormaPagto { get; set; }
        public string TipoFranq { get; set; }

        public decimal Basica { get; set; }
        public decimal Reduzida { get; set; }
        private const decimal iof = 7.38m;
        private decimal juro;

        public int NumParcelas { get; set; }
        public string Descricao { get; set; }
        public decimal ValParcela { get; set; }

        public Parcelamento()
        {

        }

        public Parcelamento(string seg, string forma, string tipo, decimal b, decimal r/*, e o que mais for preciso para fazer o cálculo)*/)
        {
            Seguradora = seg;
            FormaPagto = forma;
            TipoFranq = tipo;
            Basica = b;
            Reduzida = r;
        }

        public void CalculaParcela(int numParcelas)
        {
            NumParcelas = numParcelas;
            
            if (TipoFranq == "Básica")
                ValParcela = Basica;
            else
                ValParcela = Reduzida;

            if (numParcelas == 1)
            {
                Descricao = "À vista";
                return;
            }
            else
                Descricao = String.Format("1 + {0}", numParcelas - 1);

            switch (Seguradora)
            {
                case "Allianz":
                    if (TipoFranq == "Básica" && FormaPagto == "Carnê")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                juro = 1.0329m;
                                break;
                            case 6:
                                juro = 1.0627m;
                                break;
                        }
                        ValParcela = Basica * juro / numParcelas;
                    }

                    if (TipoFranq == "Básica" && FormaPagto == "Débito")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = 0.0m;
                                break;
                            case 6:
                                ValParcela = Basica / numParcelas;
                                break;
                        }
                    }

                    if (TipoFranq == "Reduzida" && FormaPagto == "Carnê")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                juro = 1.0329m;
                                break;
                            case 6:
                                juro = 1.0627m;
                                break;
                        }
                        ValParcela = Reduzida * juro / numParcelas;
                    }

                    if (TipoFranq == "Reduzida" && FormaPagto == "Débito")
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = 0.0m;
                                break;
                            case 6:
                                ValParcela = Reduzida / numParcelas;
                                break;
                        }
                    break;



                case "Azul":
                    if (TipoFranq == "Básica" && FormaPagto == "Carnê")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                juro = 7.552m;
                                break;
                            case 5:
                                juro = 10.15m;
                                break;
                            case 6:
                                juro = 12.786m;
                                break;
                            case 7:
                                juro = 15.465m;
                                break;

                        }
                        ValParcela = (((((Basica - iof / 100) * juro / 100) + (Basica - iof / 100)) + iof / 100) / numParcelas) + 0.05m;
                    }
                    if (TipoFranq == "Básica" && FormaPagto == "Débito")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = Basica / numParcelas;
                                break;
                            case 5:
                                ValParcela = 0.0m;
                                break;
                            case 6:
                                ValParcela = 0.0m;
                                break;
                            case 7:
                                ValParcela = 0.0m;
                                break;
                        }
                    }
                    if (TipoFranq == "Reduzida" && FormaPagto == "Carnê")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                juro = 7.552m;
                                break;
                            case 5:
                                juro = 10.15m;
                                break;
                            case 6:
                                juro = 12.786m;
                                break;
                            case 7:
                                juro = 15.465m;
                                break;

                        }
                        ValParcela = (((((Reduzida - iof / 100) * juro / 100) + (Reduzida - iof / 100)) + iof / 100) / numParcelas) + 0.05m;
                    }
                    if (TipoFranq == "Reduzida" && FormaPagto == "Débito")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = Reduzida / numParcelas;
                                break;
                            case 5:
                                ValParcela = 0.0m;
                                break;
                            case 6:
                                ValParcela = 0.0m;
                                break;
                            case 7:
                                ValParcela = 0.0m;
                                break;
                        }
                    }
                    break;


                case "Bradesco":

                    if (TipoFranq == "Básica" && FormaPagto == "Carnê")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                juro = 1.0m;
                                break;
                            case 5:
                                juro = 1.07m;
                                break;
                            case 6:
                                juro = 1.0879m;
                                break;
                            case 7:
                                juro = 1.1061m;
                                break;
                        }
                        ValParcela = ((Basica * juro) / numParcelas);
                    }
                    if (TipoFranq == "Reduzida" && FormaPagto == "Carnê")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                juro = 1.0m;
                                break;
                            case 5:
                                juro = 1.07m;
                                break;
                            case 6:
                                juro = 1.0879m;
                                break;
                            case 7:
                                juro = 1.1061m;
                                break;
                        }
                        ValParcela = ((Reduzida * juro) / numParcelas);
                    }
                    break;                

                case "HDI":
                    if (TipoFranq == "Básica" && FormaPagto == "Carnê")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                juro = 1.067m;//ValParcela = Basica / numParcelas;
                                break;
                            case 5:
                                juro = 1.0899m;
                                break;
                            case 6:
                                juro = 1.1132m;
                                break;
                            case 7:
                                juro = 1.1368m;
                                break;
                        }
                        //if (numParcelas != 4)
                            ValParcela = ((Basica * juro) / numParcelas);
                    }
                    if (TipoFranq == "Básica" && FormaPagto == "Débito")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = Basica / numParcelas;
                                break;
                            case 5:
                                ValParcela = 0.0m;
                                break;
                            case 6:
                                ValParcela = Basica / numParcelas;//0.0m;
                                break;
                            case 7:
                                ValParcela = 0.0m;
                                break;
                        }
                    }
                    if (TipoFranq == "Reduzida" && FormaPagto == "Carnê")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                juro = 1.067m;//ValParcela = Reduzida / numParcelas;
                                break;
                            case 5:
                                juro = 1.0899m;
                                break;
                            case 6:
                                juro = 1.1132m;
                                break;
                            case 7:
                                juro = 1.1368m;
                                break;
                        }
                        //if (numParcelas != 4)
                            ValParcela = ((Reduzida * juro) / numParcelas);
                    }
                    if (TipoFranq == "Reduzida" && FormaPagto == "Débito")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = Reduzida / numParcelas;
                                break;
                            case 5:
                                ValParcela = 0.0m;
                                break;
                            case 6:
                                ValParcela = Reduzida / numParcelas;//0.0m;
                                break;
                            case 7:
                                ValParcela = 0.0m;
                                break;
                        }
                    }
                    break;

                case "Itaú":
                    if (TipoFranq == "Básica" && FormaPagto == "Carnê")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                juro = 7.55m;
                                break;
                            case 5:
                                juro = 10.15m;
                                break;
                            case 6:
                                juro = 12.778m;
                                break;
                            case 7:
                                juro = 15.46m;
                                break;
                        }
                        ValParcela = ((((Basica - iof / 100) * juro / 100) + (Basica - iof / 100)) + iof / 100) / numParcelas;
                    }
                    if (TipoFranq == "Básica" && FormaPagto == "Débito")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = Basica / numParcelas;
                                break;
                            case 5:
                                ValParcela = 0.0m;
                                break;
                            case 6:
                                ValParcela = 0.0m;
                                break;
                            case 7:
                                ValParcela = 0.0m;
                                break;
                        }
                    }
                    if (TipoFranq == "Reduzida" && FormaPagto == "Carnê")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                juro = 7.55m;
                                break;
                            case 5:
                                juro = 10.15m;
                                break;
                            case 6:
                                juro = 12.778m;
                                break;
                            case 7:
                                juro = 15.46m;
                                break;
                        }
                        ValParcela = ((((Reduzida - iof / 100) * juro / 100) + (Reduzida - iof / 100)) + iof / 100) / numParcelas;
                    }
                    if (TipoFranq == "Reduzida" && FormaPagto == "Débito")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = Reduzida / numParcelas;
                                break;
                            case 5:
                                ValParcela = 0.0m;
                                break;
                            case 6:
                                ValParcela = 0.0m;
                                break;
                            case 7:
                                ValParcela = 0.0m;
                                break;
                        }
                    }
                    break;

                case "Liberty":
                    if (TipoFranq == "Básica" && FormaPagto == "Carnê")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = Basica / numParcelas;
                                break;
                            case 5:
                                juro = 8.39m;
                                break;
                            case 6:
                                juro = 10.561m;
                                break;
                            case 7:
                                juro = 12.75m;
                                break;
                        }
                        if (numParcelas != 4)
                            ValParcela = ((((Basica - iof / 100) * juro / 100) + (Basica - iof / 100)) + iof / 100) / numParcelas;
                    }
                    if (TipoFranq == "Básica" && FormaPagto == "Débito")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = Basica / numParcelas;;
                                break;
                            case 5:
                                ValParcela = 0.0m;
                                break;
                            case 6:
                                ValParcela = 0.0m;
                                break;
                            case 7:
                                ValParcela = 0.0m;
                                break;
                        }
                    }
                    if (TipoFranq == "Reduzida" && FormaPagto == "Carnê")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = Reduzida / numParcelas;
                                break;
                            case 5:
                                juro = 8.39m;
                                break;
                            case 6:
                                juro = 10.561m;
                                break;
                            case 7:
                                juro = 12.75m;
                                break;
                        }
                        if (numParcelas != 4)
                            ValParcela = ((((Reduzida - iof / 100) * juro / 100) + (Reduzida - iof / 100)) + iof / 100) / numParcelas;
                    }
                    if (TipoFranq == "Reduzida" && FormaPagto == "Débito")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = Reduzida / numParcelas;;
                                break;
                            case 5:
                                ValParcela = 0.0m;
                                break;
                            case 6:
                                ValParcela = 0.0m;
                                break;
                            case 7:
                                ValParcela = 0.0m;
                                break;
                        }
                    }
                    break;

                case "Mapfre":
                    if (TipoFranq == "Básica" && FormaPagto == "Carnê")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                juro = 1.0m; //Sem juros
                                break;
                            case 5:
                                juro = 1.07m;
                                break;
                            case 6:
                                juro = 1.0879m;
                                break;
                            case 7:
                                juro = 1.1061m;
                                break;
                        }
                        ValParcela = (Basica * juro) / numParcelas;
                    }
                    if (TipoFranq == "Básica" && FormaPagto == "Débito")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = 0.0m;
                                break;
                            case 5:
                                ValParcela = 0.0m;
                                break;
                            case 6:
                                ValParcela = Basica / numParcelas;
                                break;
                            case 7:
                                ValParcela = 0.0m;
                                break;
                        }
                    }
                    if (TipoFranq == "Reduzida" && FormaPagto == "Carnê")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                juro = 1.0m; //Sem juros
                                break;
                            case 5:
                                juro = 1.07m;
                                break;
                            case 6:
                                juro = 1.0879m;
                                break;
                            case 7:
                                juro = 1.1061m;
                                break;
                        }
                        ValParcela = (Reduzida * juro) / numParcelas;
                    }
                    if (TipoFranq == "Reduzida" && FormaPagto == "Débito")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = 0.0m;
                                break;
                            case 5:
                                ValParcela = 0.0m;
                                break;
                            case 6:
                                ValParcela = Reduzida / numParcelas;
                                break;
                            case 7:
                                ValParcela = 0.0m;
                                break;
                        }
                    }
                    break;

                case "Porto Seguro":
                    if (TipoFranq == "Básica" && FormaPagto == "Carnê")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = Basica / numParcelas;
                                break;
                            case 5:
                                juro = 10.139m;
                                break;
                            case 6:
                                juro = 12.77m;
                                break;
                            case 7:
                                juro = 15.445m;
                                break;
                        }
                        if (numParcelas != 4)
                            ValParcela = ((((Basica - iof / 100) * juro / 100) + (Basica - iof / 100)) + iof / 100) / numParcelas;
                    }
                    if (TipoFranq == "Básica" && FormaPagto == "Débito")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = Basica / numParcelas;
                                break;
                            case 5:
                                ValParcela = 0.0m;
                                break;
                            case 6:
                                ValParcela = 0.0m;
                                break;
                            case 7:
                                ValParcela = 0.0m;
                                break;
                        }
                    }
                    if (TipoFranq == "Reduzida" && FormaPagto == "Carnê")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = Reduzida / numParcelas;
                                break;
                            case 5:
                                juro = 10.139m;
                                break;
                            case 6:
                                juro = 12.77m;
                                break;
                            case 7:
                                juro = 15.445m;
                                break;
                        }
                        if (numParcelas != 4)
                            ValParcela = ((((Reduzida - iof / 100) * juro / 100) + (Reduzida - iof / 100)) + iof / 100) / numParcelas;
                    }
                    if (TipoFranq == "Reduzida" && FormaPagto == "Débito")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = Reduzida / numParcelas;
                                break;
                            case 5:
                                ValParcela = 0.0m;
                                break;
                            case 6:
                                ValParcela = 0.0m;
                                break;
                            case 7:
                                ValParcela = 0.0m;
                                break;
                        }
                    }
                    break;

                case "Sompo":
                    if (TipoFranq == "Básica" && FormaPagto == "Carnê")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                juro = 6.215m;
                                break;
                            case 5:
                                juro = 8.346m;
                                break;
                            case 6:
                                juro = 10.495m;
                                break;
                            case 7:
                                juro = 12.68m;
                                break;
                        }
                        ValParcela = ((((Basica - iof / 100) * juro / 100) + (Basica - iof / 100)) + iof / 100) / numParcelas;
                    }
                    if (TipoFranq == "Básica" && FormaPagto == "Débito")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = 0.0m;
                                break;
                            case 5:
                                ValParcela = Basica / numParcelas;
                                break;
                            case 6:
                                ValParcela = 0.0m;
                                break;
                            case 7:
                                ValParcela = 0.0m;
                                break;
                        }
                    }
                    if (TipoFranq == "Reduzida" && FormaPagto == "Carnê")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                juro = 6.215m;
                                break;
                            case 5:
                                juro = 8.346m;
                                break;
                            case 6:
                                juro = 10.495m;
                                break;
                            case 7:
                                juro = 12.68m;
                                break;
                        }
                        ValParcela = ((((Reduzida - iof / 100) * juro / 100) + (Reduzida - iof / 100)) + iof / 100) / numParcelas;
                    }
                    if (TipoFranq == "Reduzida" && FormaPagto == "Débito")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = 0.0m;
                                break;
                            case 5:
                                ValParcela = Reduzida / numParcelas;
                                break;
                            case 6:
                                ValParcela = 0.0m;
                                break;
                            case 7:
                                ValParcela = 0.0m;
                                break;
                        }
                    }
                    break;

                case "Sul América":
                    if (TipoFranq == "Básica" && FormaPagto == "Carnê")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                juro = 6.215m;
                                break;
                            case 5:
                                juro = 8.346m;
                                break;
                            case 6:
                                juro = 10.495m;
                                break;
                            case 7:
                                juro = 12.68m;
                                break;
                        }
                        ValParcela = ((((Basica - iof / 100) * juro / 100) + (Basica - iof / 100)) + iof / 100) / numParcelas;
                    }
                    if (TipoFranq == "Básica" && FormaPagto == "Débito")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = 0.0m;
                                break;
                            case 5:
                                ValParcela = Basica / numParcelas;
                                break;
                            case 6:
                                ValParcela = 0.0m;
                                break;
                            case 7:
                                ValParcela = 0.0m;
                                break;
                        }
                    }
                    if (TipoFranq == "Reduzida" && FormaPagto == "Carnê")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                juro = 6.215m;
                                break;
                            case 5:
                                juro = 8.346m;
                                break;
                            case 6:
                                juro = 10.495m;
                                break;
                            case 7:
                                juro = 12.68m;
                                break;
                        }
                        ValParcela = ((((Reduzida - iof / 100) * juro / 100) + (Reduzida - iof / 100)) + iof / 100) / numParcelas;
                    }
                    if (TipoFranq == "Reduzida" && FormaPagto == "Débito")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = 0.0m;
                                break;
                            case 5:
                                ValParcela = Reduzida / numParcelas;
                                break;
                            case 6:
                                ValParcela = 0.0m;
                                break;
                            case 7:
                                ValParcela = 0.0m;
                                break;
                        }
                    }
                    break;

                case "Tokio Marine":
                    if (TipoFranq == "Básica" && FormaPagto == "Carnê")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = Basica / NumParcelas;
                                break;
                            case 5:
                                juro = 1.07m;
                                break;
                            case 6:
                                juro = 1.088m;
                                break;
                            case 7:
                                juro = 1.1518m;
                                break;
                        }
                        if (numParcelas != 4)
                            ValParcela = ((Basica * juro) / numParcelas);
                    }
                    if (TipoFranq == "Básica" && FormaPagto == "Débito")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = 0.0m;
                                break;
                            case 5:
                                ValParcela = 0.0m;
                                break;
                            case 6:
                                ValParcela = Basica / numParcelas;
                                break;
                            case 7:
                                ValParcela = 0.0m;
                                break;
                        }
                    }
                    if (TipoFranq == "Reduzida" && FormaPagto == "Carnê")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = Reduzida / NumParcelas;
                                break;
                            case 5:
                                juro = 1.07m;
                                break;
                            case 6:
                                juro = 1.088m;
                                break;
                            case 7:
                                juro = 1.1518m;
                                break;
                        }
                        if (numParcelas != 4)
                            ValParcela = ((Reduzida * juro) / numParcelas);
                    }
                    if (TipoFranq == "Reduzida" && FormaPagto == "Débito")
                    {
                        switch (numParcelas)
                        {
                            case 4:
                                ValParcela = 0.0m;
                                break;
                            case 5:
                                ValParcela = 0.0m;
                                break;
                            case 6:
                                ValParcela = Reduzida / numParcelas;
                                break;
                            case 7:
                                ValParcela = 0.0m;
                                break;
                        }
                    }
                    break;
            }
        }
    }
}