using System;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.Mail;
using System.IO;
using System.Threading.Tasks;

namespace CartaDeCotacao
{
    public class Relatório
    {
        static readonly string LocalUtilizacoes = WebFile.AppData + "Utilizações.log";
        static readonly string LocalRelatorio = WebFile.AppData + "Relatório.log";
        static readonly string LocalVersao = WebFile.AppData + "Versão.dat";
        public static Home Home;

        public static void SalvaUtilizacao(Cotacao cot)
        {
            FileStream fs = new FileStream(LocalUtilizacoes, FileMode.Append, FileAccess.Write);
            using (StreamWriter sw = new StreamWriter(fs))
            {
                if (cot.NomeFuncionario == false || cot.Funcionario == "")
                    sw.WriteLine(String.Format("{0} na {1} no tipo {2}", DateTime.Now.ToString("dd/MM/yy"), cot.Seguradora, cot.Tipo.ToString()));
                if (cot.NomeFuncionario == true && cot.Funcionario != "")
                    sw.WriteLine(String.Format("{0} na {1} no tipo {2} por {3}", DateTime.Now.ToString("dd/MM/yy"), cot.Seguradora, cot.Tipo.ToString(), cot.Funcionario));
            }
        }

        public async static Task VerificarEnvio()
        {
            if (File.Exists(LocalUtilizacoes))
            {
                DateTime dataArquivo = File.GetCreationTime(LocalUtilizacoes);
                TimeSpan dif = DateTime.Now - dataArquivo;

                if (dif.Days >= 7)
                    await EnviarRelatorio();
            }
        }

        private async static Task EnviarRelatorio(bool emergencia = false)
        {
            DirectoryInfo appData = new DirectoryInfo(WebFile.AppData);
            FileInfo[] arqsAppData = appData.GetFiles();
            string arquivosAPPData = "\r\n-------------------------------------------------\r\n";
            foreach (FileInfo f in arqsAppData)
                arquivosAPPData += f.Name + " - " + f.Length + " KB - " + f.LastWriteTime + "\r\n";
            arquivosAPPData += "-------------------------------------------------";

            string log = null;
            if (File.Exists(LocalUtilizacoes))
                using (StreamReader leitor = new StreamReader(LocalUtilizacoes))
                {
                    string linha;
                    while ((linha = leitor.ReadLine()) != null)
                    {
                        log += linha;
                        log += "\r\n";
                    }
                }
            else
                log = "Nenhuma utilização registrada.\r\n";

            string log2 = "\r\n";
            if (File.Exists(LocalRelatorio))
            {
                using (StreamReader leitor = new StreamReader(LocalRelatorio))
                {
                    string linha;
                    while ((linha = leitor.ReadLine()) != null)
                    {
                        log2 += linha;
                        log2 += "\r\n";
                    }
                }
            }
            else
                log2 += "Nenhum relatório de uso.\r\n";            
            
            int numLinhas = log.Split('\n').Length - 1;

            string adicionais = "Nome do usuário: " + Environment.UserName + "\r\nVersão: " + Application.ProductVersion + "\r\nSistema operacional: " + SistemaOperacional() +
                                "\r\nNúmero de utilizações: " + numLinhas.ToString() + arquivosAPPData + log2;
            
            var message = new MailMessage(WebFile.Email.Address, WebFile.Email.Address);
            if (!emergencia)
                message.Subject = ("Relatório semanal de " + Environment.UserName);
            else
                message.Subject = ("Relatório emergencial de " + Environment.UserName);
            string mensagem = adicionais + "-------------------------------------------------\r\n" + log;
            message.Body = (mensagem);
            if (await WebFile.SendEmailAsync(message))
            {
                File.Delete(LocalUtilizacoes);
                if (File.Exists(LocalRelatorio))
                    File.Delete(LocalRelatorio);
            }
        }

        public async static Task EnviarErro(string nome, string descricao, string local)
        {
            string adicionais = "Nome do usuário: " + Environment.UserName + "\r\nVersão: " +  Application.ProductVersion + "\r\nSistema operacional: " + SistemaOperacional() + "\r\n-------------------------------------------------";
            var message = new MailMessage(WebFile.Email.Address, WebFile.Email.Address)
            {
                Subject = ("Relatório de erro de " + Environment.UserName),
                Body = (adicionais + "\r\n\r\n" + "Nome: " + nome + "\r\n" + "Descrição: " + descricao + "\r\n")
            };
            if (local != null)
            {
                var attachment = new Attachment(local);
                message.Attachments.Add(attachment);
            }
            if (await WebFile.SendEmailAsync(message))
                MessageBox.Show("Problema enviado com sucesso!", "Carta de Cotação", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
                MessageBox.Show("Não foi possível enviar o problema.", "Carta de Cotação", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        public static void AdicionarAoRelatorio(string descricao)
        {
            string[] linhas = { "" };
            bool achou = false;

            if (File.Exists(LocalRelatorio))
            {
                linhas = File.ReadAllLines(LocalRelatorio);

                int i = 0;
                foreach (string linha in linhas)
                {
                    if (linha.Contains(descricao))
                    {
                        string line = linha;
                        string[] splited = line.Split(':');
                        int numero = int.Parse(splited[1].Remove(0,1));
                        int numeronovo = numero + 1;
                        linhas[i] = splited[0] + ": " + numeronovo;
                        achou = true;
                    }
                    i++;
                }

                if (!achou)
                {
                    int ultimaLinha = linhas.Length - 1;
                    linhas[ultimaLinha] += "\r\n" + descricao + ": 1";
                }
            }
            else
                linhas[0] = descricao + ": 1";

            File.WriteAllLines(LocalRelatorio, linhas);
        }

        public static string SistemaOperacional()
        {
            string osname;
            switch (Environment.OSVersion.ToString())
            {
                case "Microsoft Windows NT 6.1.7601 Service Pack 1":
                    osname = "Windows 7 Service Pack 1";
                    break;
                case "Microsoft Windows NT 6.2.9200.0":
                    osname = "Windows 8";
                    break;
                case "Microsoft Windows NT 6.3.9600.0":
                    osname = "Windows 8.1";
                    break;
                case "Microsoft Windows NT 10.0.10586.0":
                    osname = "Windows 10";
                    break;
                case "Microsoft Windows NT 10.0.14393.0":
                    osname = "Windows 10 Anniversary Update";
                    break;
                case "Microsoft Windows NT 10.0.17134.0":
                    osname = "Windows 10";
                    break;                    
                default:
                    osname = Environment.OSVersion.ToString();
                    break;
            }
            return osname;
        }

        public async static Task<bool> PrimeiroUso()
        {
            bool primeira = false;
            string verArq = null;            
            if (File.Exists(LocalVersao))
            {
                using (StreamReader leitor = new StreamReader(LocalVersao))
                {
                    string linha;
                    while ((linha = leitor.ReadLine()) != null)
                    {
                        verArq = linha;
                    }
                }

                if (double.Parse(verArq) != double.Parse(Application.ProductVersion))
                    primeira = true;

                FileStream fs = new FileStream(LocalVersao, FileMode.Create, FileAccess.Write);
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine($"{Application.ProductVersion}");
                }
            }
            else
            {
                FileStream fs = new FileStream(LocalVersao, FileMode.Create, FileAccess.Write);
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine($"{Application.ProductVersion}");
                }
                primeira = true;
            }
            if (primeira)
            {
                string info = "Informações:\r\nNome do usuário: " + Environment.UserName + "\r\nVersão: " + Application.ProductVersion + "\r\nSistema operacional: " + SistemaOperacional();
                var message = new MailMessage(WebFile.Email.Address, WebFile.Email.Address)
                {
                    Subject = ("Nova versão instalada em " + Environment.UserName),
                    Body = (info)
                };
                
                await WebFile.SendEmailAsync(message);
            }
            return primeira;
        }

        public static void MoverDadosAntigos()
        {
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Carta de Cotação.log") && !File.Exists(LocalUtilizacoes))
                File.Move(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Carta de Cotação.log", LocalUtilizacoes);
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Carta de Cotação\Carta de Cotação.log") && !File.Exists(LocalUtilizacoes))
                File.Move(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Carta de Cotação\Carta de Cotação.log", LocalUtilizacoes);
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Carta de Cotação\Status.txt"))
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Carta de Cotação\Status.txt");
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Carta de Cotação\Status.bin"))
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Carta de Cotação\Status.bin");
        }

        private static readonly string[][] Messages = {
            //1- Administrador; 2 - Mensagem
            //Não colocar dois pontos
            new[] { "Erro Fipe -", "A Fipe está indisponível no momento,\r\ntente novamente mais tarde ou\r\ndigite os dados manualmente." },//0 => reservado para mensagens de erro Fipe
            new[] { "Exceção do .Net -", "Ocorreu um erro inesperado,\r\no programa será encerrado." },//1
            new[] { "Sem acesso à Internet. IP Status -", "" },//2
            new[] { "Código Fipe inválido", "Código Fipe inválido." },//3 => reservado para código inválido Fipe
            new[] { "O servidor não respondeu", "O servidor não respondeu." },//4
            new[] { "Versão expirada", "A versão do programa instalada está expirada,\r\npor favor atualize para a última versão." },//5
            new[] { "Carta de Cotação indisponível", "O Carta de Cotação está indisponível no momento." }//6
        };
        private static readonly MessageBoxIcon[] UserMessageTypes = new MessageBoxIcon[] { MessageBoxIcon.Error, MessageBoxIcon.Error, MessageBoxIcon.Error, MessageBoxIcon.Warning, MessageBoxIcon.Error,
            MessageBoxIcon.Warning, MessageBoxIcon.Exclamation };

        public int Codigo { get; set; }//não alterar nome => json Fipe
        public string Erro { get; set; }//não alterar nome => json Fipe
        public string UserMessage { get; set; }

        public Relatório(int code, string adm = "", string user = "")//adm e user = informação extra
        {
            Codigo = code;
            Erro = Messages[code][0] + " " + adm;//junta mensagem padrão + info extra adm
            UserMessage = Messages[code][1] + " " + user;//junta mensagem padrão + info extra user
        }

        public async static void ExHandler(Exception ex, string adm = "")//Usar num catch
        {
            Relatório report;

            if (ex.Message.StartsWith("{"))//se for json é erro da Fipe ou Exceção nossa 
            {
                report = JsonConvert.DeserializeObject<Relatório>(ex.Message);
                PostMessage(report);
            }
            else
            {
                report = new Relatório(1, ex.Message + adm);//se não for json então é Exceção do .Net                
                PostMessage(report);
                //Home.Hide();
                await EnviarRelatorio(true);
                if (adm != "Home_Load")
                {
                    Home.AtualizarFill();
                    if (!File.Exists(WebFile.AppData + "Restauração.bin"))
                        File.Create(WebFile.AppData + "Restauração.bin").Close();
                    WebFile.Serialize(Home.fill, WebFile.AppData + "Restauração.bin");
                }
                //Environment.Exit(1);
            }
        }

        public static void PostMessage(Relatório report)
        {
            if (report.Erro != " ")//vazio = não adicionar ao Report
                AdicionarAoRelatorio(report.Erro);
        
            if (report.UserMessage != " ")//vazio = não mostrar para o usuário
                MessageBox.Show(report.UserMessage + "\r\n\r\nErro interno:\r\n" + report.Erro, AppDomain.CurrentDomain.FriendlyName.Replace(".exe", ""), MessageBoxButtons.OK, UserMessageTypes[report.Codigo]);
        }
    }
}