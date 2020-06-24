using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace CartaDeCotacao
{
    public enum FipeType { Automóvel = 1, Moto, Caminhão };

    public class FipeMake
    {
        public string Label { get; set; }
        public string Value { get; set; }        
    }

    [Serializable]
    public class FipeModel
    {
        private FipeType _tipoVeiculo;
        public FipeType TipoVeiculo { get { return _tipoVeiculo; } set { _tipoVeiculo = value; } }
        public string TipoVeiculoNome { get { return FipeQuery.TipoVeiculoNomes.ElementAt((int)_tipoVeiculo); } }

        public bool IsAuto { get { if (_tipoVeiculo == FipeType.Automóvel) return true; else return false; } }
        public bool IsMoto { get { if (_tipoVeiculo == FipeType.Moto) return true; else return false; } }
        public bool IsTruck { get { if (_tipoVeiculo == FipeType.Caminhão) return true; else return false; } }

        private string _codigoFipe;
        public string CodigoFipe { get { return _codigoFipe; } set { _codigoFipe = value.Replace("  ", ""); } }

        public string Modelo { get; set; }
        public string Marca { get; set; }

        private string _label;
        public string Label { get { return _label; } set { _label = value.Replace("32000", "Zero KM"); } }

        private string _value;
        public string Value { get { return _value; } set { _value = value; } }

        private string _anoModelo;
        public string AnoModelo { get { if (_anoModelo == null) return _value.Substring(0, _value.Length - 2); else return _anoModelo; } set { _anoModelo = value; } }
        public string TipoCombustivel { get { return _value.Substring(_value.Length - 1); } }
        public string Combustivel { get; set; }
        public string SiglaCombustivel { get; set; }

        private string _anoFabricacao;
        public string AnoFabricacao { get { if (_anoFabricacao == null) return ""; else return _anoFabricacao; } set { _anoFabricacao = value; } }

        private string _valor;
        public string Valor { get { return _valor; } set { _valor = value; } }
        public decimal PrecoMedio { get { if (_valor != null) return decimal.Parse(_valor, NumberStyles.Currency); else return 0.0m; } }

        public string MesReferencia { get; set; }
        public string DataConsulta { get; set; }

        public string Autenticacao { get; set; }
    }

    public class FipeQuery
    {
        private static string _refTable = /*"0"*/(224 + (DateTime.Today.Year - 2017) * 12 + DateTime.Today.Month - 12).ToString();//Dec2017 = 224

        private static readonly List<string> _queryTypes = new List<string>() { "codigo" };
        private static readonly List<string> _vehicleTypes = new List<string>() { "none", "carro", "moto", "caminhao" };
        private static readonly List<string> _vehicleTypeNames = new List<string>() { "Nenhum", "Automóvel", "Moto", "Caminhão" };

        /*private static string _getRefTableUrl = @"http://fipe.org.br/pt-br/indices/veiculos";*/
        private static string _getModelsUrl = @"http://veiculos.fipe.org.br/api/veiculos/ConsultarAnoModeloPeloCodigoFipe?codigoTabelaReferencia=$&codigoMarca=&codigoModelo=&codigoTipoVeiculo=$&anoModelo=&codigoTipoCombustivel=&tipoVeiculo=$&modeloCodigoExterno=$&tipoConsulta=$";
        private static string _getMakesUrl = @"http://veiculos.fipe.org.br/api/veiculos/ConsultarMarcas?codigoTabelaReferencia=$&codigoTipoVeiculo=$";
        private static string _queryByFipeCodeUrl = @"http://veiculos.fipe.org.br/api/veiculos/ConsultarValorComTodosParametros?codigoTabelaReferencia=$&codigoMarca=&codigoModelo=&codigoTipoVeiculo=$&anoModelo=$&codigoTipoCombustivel=$&tipoVeiculo=$&modeloCodigoExterno=$&tipoConsulta=$";

        public static List<string> TipoVeiculoNomes { get { return _vehicleTypeNames; } }

        private async Task<List<FipeModel>> GetModels(string code)
        {
            try
            {
                string url = /*await*/ UrlBuilder(_getModelsUrl, new string[] { _refTable, "1", _vehicleTypes[1], code, _queryTypes[0] });
                //if (_refTable == "0")
                //  throw new WebException(JsonConvert.SerializeObject(new Relatório(0, "Fipe indisponível")));

                List<Task<string>> requests = new List<Task<string>>();
                foreach (int type in Enum.GetValues(typeof(FipeType)).OfType<object>().Where(o => (int)o > 0))//Autodetection FipeType
                {
                    url = /*await*/ UrlBuilder(_getModelsUrl, new string[] { _refTable, type.ToString(), _vehicleTypes[type], code, _queryTypes[0] });
                    requests.Add(WebFile.HttpRequestAsync(url));
                }

                string[] responses = await TaskEx.WhenAll(requests);
                if (responses.Length == 0 || String.IsNullOrEmpty(responses[0]) || String.IsNullOrEmpty(responses[1]) || String.IsNullOrEmpty(responses[2]))
                    throw new WebException(JsonConvert.SerializeObject(new Relatório(2, "", "Sem acesso à internet")));

                List<FipeModel> models = new List<FipeModel>();
                foreach (string jsonList in responses)
                {
                    if (jsonList.StartsWith("["))//tests if string is a json list (stars with '[')... "{\"codigo\":\"3\",\"erro\":\"CÃ³digo fipe invÃ¡lido\"}" , "{\"codigo\":\"0\",\"erro\":\"Nada encontrado\"}")
                    {
                        models = JsonConvert.DeserializeObject<List<FipeModel>>(jsonList);
                        foreach (FipeModel model in models)
                            model.TipoVeiculo = (FipeType)(Array.IndexOf(responses, jsonList) + 1);
                    }
                    else
                    {
                        Relatório report = JsonConvert.DeserializeObject<Relatório>(jsonList);
                        if (report.Codigo != 0)
                            throw new WebException(jsonList);
                    }
                }
                if (models.Count == 0)
                    throw new WebException(JsonConvert.SerializeObject(new Relatório(3)));
                else
                    return models;
            }
            catch (Exception ex)
            {
                Relatório.ExHandler(ex);
                return new List<FipeModel>();
            }
        }

        public async Task<List<FipeMake>> GetMakes(FipeType type)
        {
            try
            {
                List<FipeMake> makes = new List<FipeMake>();
                string url = /*await*/ UrlBuilder(_getMakesUrl, new string[] { _refTable, ((int)type).ToString() });
                string jsonList = await WebFile.HttpRequestAsync(url);

                if (String.IsNullOrEmpty(jsonList))
                    throw new WebException(JsonConvert.SerializeObject(new Relatório(2, "", "Sem acesso à internet")));
                else
                {
                    if (jsonList.StartsWith("["))//tests if string is a json list (starts with '[')... "{\"codigo\":\"3\",\"erro\":\"CÃ³digo fipe invÃ¡lido\"}" , "{\"codigo\":\"0\",\"erro\":\"Nada encontrado\"}")
                        makes = JsonConvert.DeserializeObject<List<FipeMake>>(jsonList);
                    else
                        throw new WebException(jsonList);
                }
                return makes;
            }
            catch (Exception ex)
            {
                Relatório.ExHandler(ex);
                return new List<FipeMake>();
            }
        }

        public async Task<List<FipeModel>> QueryYear_modelsByFipeCode(string code)//QueryByFipeCode(Tipo t, string code)
        {
            try
            {
                int numcode = -1;
                if (code == "" || !Int32.TryParse(code.Replace("-", ""), out numcode) || numcode == 0 || numcode > 9999999)
                    throw new WebException(JsonConvert.SerializeObject(new Relatório(3)));

                code = numcode.ToString("000000-0");

                List<FipeModel> models = await GetModels(code);

                if (models.Count == 0)
                    return models;

                List<Task<string>> requests = new List<Task<string>>();
                foreach (FipeModel model in models)
                {
                    string url = /*await*/ UrlBuilder(_queryByFipeCodeUrl, new string[] { _refTable, ((int)model.TipoVeiculo).ToString(), model.AnoModelo, model.TipoCombustivel, _vehicleTypes[(int)model.TipoVeiculo], code, _queryTypes[0] });
                    requests.Add(WebFile.HttpRequestAsync(url));
                }

                string[] responses = await TaskEx.WhenAll(requests);
                if (responses.Length == 0 || String.IsNullOrEmpty(responses[0]))
                    throw new WebException(JsonConvert.SerializeObject(new Relatório(4)));

                foreach (string json in responses)
                {
                    if (Array.IndexOf(responses, json) == 0)//insert mockup in position 0
                    {
                        FipeModel mockup = JsonConvert.DeserializeObject<FipeModel>(json);//mockup: "Selecione o ano/modelo:"
                        mockup.Label = "Selecione o ano/modelo:";
                        mockup.Value = "0";
                        mockup.Valor = null;
                        models.Insert(0, mockup);
                    }

                    FipeModel model = JsonConvert.DeserializeObject<FipeModel>(json);
                    model.Label = models.ElementAt(Array.IndexOf(responses, json) + 1).Label;
                    model.Value = models.ElementAt(Array.IndexOf(responses, json) + 1).Value;
                    models[Array.IndexOf(responses, json) + 1] = model;
                }
                return models;
            }
            catch (Exception ex)
            {
                Relatório.ExHandler(ex);
                return new List<FipeModel>();
            }
        }

        public FipeModel QueryValuationByFipeModel(FipeModel model)
        {
            return model;
        }

        /*private static async Task GetCurrentRefTable()
        {
            try
            {
                string html = await WebFile.DownloadStringAsync(_getRefTableUrl);
                if (String.IsNullOrEmpty(html))
                    throw new WebException(JsonConvert.SerializeObject(new Relatório(2)));

                int posInicial = html.IndexOf("<option value=\"") + "<option value=\"".Length;//procura no código html a posição onde está o valor da tabela do mês atual (é a primeira tag <option value="...") 
                int posFinal = html.IndexOf("\"", posInicial);//procura a posição final (fecha aspas), a partir da posição inicial (abre aspas)
                _refTable = html.Substring(posInicial, posFinal - posInicial);//o valor da tabela está a partir da posição inicial, contando tantos caracteres até fechar aspas = posFinal - posInicial
                int i = 0;
                if (!Int32.TryParse(_refTable, out i))
                    _refTable = "0";
            }
            catch (Exception ex)
            {
                Relatório.ExHandler(ex);
                _refTable = "0";
            }
        }*/

        private static string /*async Task<string>*/ UrlBuilder(string url, string[] args)
        {
            /*if (_refTable == "0")
            {
                await GetCurrentRefTable();
                args[0] = _refTable;
            }*/

            StringBuilder sb = new StringBuilder();
            string[] parts = url.Split('$');

            for (int i = 0; i < args.Length; i++)
                sb.Append(parts[i] + args[i]);

            return sb.ToString();
        }
    }
}