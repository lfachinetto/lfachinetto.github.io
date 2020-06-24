using System;

using System.Net.NetworkInformation;

namespace CartaDeCotacao
{
    public class Funcoes
    {
        public static bool CheckForInternetConnection()
        {
            try
            {
                bool funcionou = false;
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                if (reply.Status == IPStatus.Success)
                {
                    funcionou = true;
                }
                return funcionou;
            }
            catch (System.Net.NetworkInformation.PingException)
            {
                Relatorio.AdicionarAoRelatorio("Sem internet");
                return false;
            }
        }
    }
}
