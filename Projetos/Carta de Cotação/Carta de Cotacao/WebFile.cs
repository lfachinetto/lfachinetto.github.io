using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Linq;

namespace CartaDeCotacao
{
    public static class WebFile
    {
        private static string _appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Carta de Cotação\";//nunca usar _appData diretamente, só a propriedade AppData
        public static string AppData { get { if (!Directory.Exists(_appData)) { Directory.CreateDirectory(_appData); } return _appData; } }//agora faz teste de existencia do diretório automagicamente
        public static string DirDrive { get { return @"C:\Users\Lucas\Google Drive\Carta de Cotação\Status.dat"; } }

        private static string _statusUrl = @"https://drive.google.com/uc?export=download&id=1Lrk-Mz7DGrQJf67iUsBxnbFDeoSnFcul";
        private static string _extrasUrl = @"https://drive.google.com/uc?export=download&id=1Ijowi5o2fD96BEsiSplCvNRftJ--M_5-";
        public static Status Status { get { return (Status)Deserialize(AppData + "Status.dat"); } }//chamar ou atribuir Web.Status / Web.StatusDrive / Web.Prefs desserializa ou serializa automaticamente
        public static Status StatusDrive { get { return (Status)Deserialize(DirDrive); } set { Serialize(value, DirDrive); } }//idem
        public static Preferencias Prefs { get { return (Preferencias)Deserialize(AppData + "Preferencias.bin"); } set { Serialize(value, AppData + "Preferencias.bin"); } }//idem
        public static Assembly Extras { get { return Assembly.LoadFile(AppData + "Extras.dll"); } }

        public static MailAddress Email { get { return new MailAddress("cartadecotacao@gmail.com", "Carta de Cotação"); } }
        private static string _password = "cadeco132";

        public static async Task<bool> SendEmailAsync(MailMessage message)
        {
            if (!HasInternet())
                return false;

            SmtpClient client = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Email.Address, _password)
            };

            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            var tcs = new TaskCompletionSource<bool>();
            try
            {
                client.SendCompleted += (s, e) =>
                {
                    if (e.Error != null)
                        tcs.TrySetException(e.Error);
                    else
                        if (e.Cancelled)
                        tcs.TrySetCanceled();
                    else
                        tcs.TrySetResult(true);
                };

                client.SendAsync(message, null);
                return await tcs.Task;
            }
            catch (Exception ex)
            {
                Relatório.ExHandler(ex);
                return false;
            }
        }

        public static async Task CheckAppFilesAsync()//Cria ou atualiza Preferencias.bin / Status.dat / Extras.dll
        {
            string local = AppData + "Preferencias.bin";
            if (!File.Exists(local))
            {
                File.Create(local).Close();
                Prefs = new Preferencias();
            }

            local = AppData + "Status.dat";
            if (!await DownloadFileAsync(_statusUrl, local))
                return;

            local = AppData + "Extras.dll";
            if (!await DownloadFileAsync(_extrasUrl, local))
                return;
        }

        public static object Deserialize(string local)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(local, FileMode.Open, FileAccess.Read, FileShare.Read);

            try { return formatter.Deserialize(stream); }
            catch (Exception ex)
            {
                Relatório.ExHandler(ex);
                return new object();
            }
            finally { stream.Close(); }
        }

        public static void Serialize(object o, string local)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(local, FileMode.Create, FileAccess.Write, FileShare.Read);

            try { formatter.Serialize(stream, o); }
            catch (Exception ex) { Relatório.ExHandler(ex); }
            finally { stream.Close(); }
        }

        /*public static async Task<DateTime> LastModified(string url)
        {
            if (!HasInternet())
                return DateTime.MinValue;

            var tcs = new TaskCompletionSource<WebResponse>();
            try
            {
                HttpWebRequest head = (HttpWebRequest)WebRequest.Create(url);
                head.Method = WebRequestMethods.Http.Head;
                head.BeginGetResponse(iar =>
                {
                    try { tcs.TrySetResult(req.EndGetResponse(iar)); }
                    catch (OperationCanceledException) { tcs.TrySetCanceled(); }
                    catch (Exception ex) { tcs.TrySetException(ex); }
                }, null);

                using (WebResponse response = await tcs.Task)
                {
                    DateTime lastModified;
                    if (DateTime.TryParse(response.Headers.Get("Last-Modified"), out lastModified))
                        return lastModified;
                    else
                        return DateTime.MinValue;
                }
            }
            catch (Exception ex)
            {
                Relatório.ExHandler(ex);
                return DateTime.MinValue;
            }
        }*/

        public static async Task<string> HttpRequestAsync(string url)
        {
            HttpWebRequest post = (HttpWebRequest)WebRequest.Create(url.Substring(0, url.IndexOf("?")));
            post.Method = WebRequestMethods.Http.Post;

            post.Accept = "*/*";
            post.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            post.Referer = @"http://veiculos.fipe.org.br/api/veiculos/";
            post.UserAgent = "Mozilla / 5.0(Windows NT 10.0; WOW64; rv: 40.0) Gecko / 20100101 Firefox / 40.0";
            post.Headers.Add("Accept-Encoding", "gzip, deflate");
            post.Headers.Add("Accept-Language", "pt-BR,pt;q=0.8,en-US;q=0.5,en;q=0.3");
            post.Headers.Add("X-Requested-With", "XMLHttpRequest");

            var tcsRequest = new TaskCompletionSource<Stream>();
            var tcsResponse = new TaskCompletionSource<WebResponse>();
            try
            {
                post.BeginGetRequestStream(iar =>
                {
                    try { tcsRequest.TrySetResult(post.EndGetRequestStream(iar)); }
                    catch (OperationCanceledException) { tcsRequest.TrySetCanceled(); }
                    catch (Exception ex) { tcsRequest.TrySetException(ex); }
                }, null);

                byte[] body = Encoding.UTF8.GetBytes(url.Substring(url.IndexOf("?") + 1));
                using (Stream request = await tcsRequest.Task)
                    request.Write(body, 0, body.Length);

                post.BeginGetResponse(iar =>
                {
                    try { tcsResponse.TrySetResult(post.EndGetResponse(iar)); }
                    catch (OperationCanceledException) { tcsResponse.TrySetCanceled(); }
                    catch (Exception ex) { tcsResponse.TrySetException(ex); }
                }, null);

                string output;
                using (WebResponse response = await tcsResponse.Task)
                using (StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    output = stream.ReadToEnd();

                return output;
            }
            catch (Exception ex)
            {
                Relatório.ExHandler(ex);
                return null;
            }
        }

        /*public static async Task<string> DownloadStringAsync(string url)
        {
            if (!HasInternet())
                return null;

            var tcs = new TaskCompletionSource<string>();
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadStringCompleted += (s, e) =>
                    {
                        if (e.Error != null)
                            tcs.TrySetException(e.Error);
                        else
                            if (e.Cancelled)
                                tcs.TrySetCanceled();
                            else
                                tcs.TrySetResult(e.Result);
                    };
                    client.DownloadStringAsync(new Uri(url));
                }
                return await tcs.Task;
            }
            catch (Exception ex)
            {
                Relatório.ExHandler(ex);
                return null;
            }
        }*/

        public static async Task<bool> DownloadFileAsync(string url, string local)
        {
            if (!HasInternet())
                return false;

            HttpWebRequest get = (HttpWebRequest)WebRequest.Create(url);
            get.Method = WebRequestMethods.Http.Get;

            var tcs = new TaskCompletionSource<WebResponse>();
            try
            {
                get.BeginGetResponse(iar =>
                {
                    try { tcs.TrySetResult(get.EndGetResponse(iar)); }
                    catch (OperationCanceledException) { tcs.TrySetCanceled(); }
                    catch (Exception ex) { tcs.TrySetException(ex); }
                }, null);

                using (WebResponse response = await tcs.Task)
                {
                    using (Stream rs = response.GetResponseStream())
                    {
                        int bytesRead = 0;
                        byte[] buffer = new byte[4096];

                        MemoryStream ms = new MemoryStream();
                        while ((bytesRead = rs.Read(buffer, 0, buffer.Length)) > 0)
                            ms.Write(buffer, 0, bytesRead);

                        ms.Position = 0;

                        var md5 = MD5.Create();
                        byte[] localHash = new byte[16];
                        byte[] remoteHash = md5.ComputeHash(ms);

                        if (File.Exists(local))
                            using (FileStream fs = new FileStream(local, FileMode.Open, FileAccess.Read, FileShare.Read))
                                localHash = md5.ComputeHash(fs);

                        if (!remoteHash.SequenceEqual(localHash))
                        {
                            ms.Position = 0;
                            using (FileStream fs = new FileStream(local, FileMode.Create, FileAccess.Write, FileShare.Read))
                                while ((bytesRead = ms.Read(buffer, 0, buffer.Length)) > 0)
                                    fs.Write(buffer, 0, bytesRead);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Relatório.ExHandler(ex);
                return false;
            }
        }

        public static bool HasInternet()
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send("8.8.8.8", 1000);//ping google public dns server

                    if (reply.Status == IPStatus.Success)
                        return true;
                    else
                        throw new PingException(JsonConvert.SerializeObject(new Relatório(2, reply.Status.ToString())));
                }
            }
            catch (Exception ex)
            {
                Relatório.ExHandler(ex);
                return false;
            }
        }
    }
}