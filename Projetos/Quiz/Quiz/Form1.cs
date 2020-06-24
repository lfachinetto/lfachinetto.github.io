using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.IO;

namespace Quiz
{
    public partial class Form1 : Form
    {
        static HttpListener _httpListener = new HttpListener();
        public Form1()
        {
            InitializeComponent();
            label10.Text = "IP Local: " + GetLocalIPAddress();
            Web();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Resultados res = new Resultados();
            res.Show();
        }

        void Web()
        {
            Console.WriteLine("Starting server...");
            _httpListener.Prefixes.Add("http://*:4000/"); // add prefix "http://localhost:4000/" RUN AS ADMIN! FIREWALL!
            _httpListener.Start(); // start server (Run application as Administrator!)
            Console.WriteLine("Server started.");
            Thread _responseThread = new Thread(ResponseThread);
            _responseThread.Start(); // start the response thread
        }

        void ResponseThread()
        {
            while (true)
            {
                HttpListenerContext context = _httpListener.GetContext(); // get a context
                                                                          // Now, you'll find the request URL in context.Request.Url



                byte[] _responseArray = null;

                _responseArray = Encoding.UTF8.GetBytes("<h1>Bem-vindo ao servidor do quiz!</h1>");

                context.Response.OutputStream.Write(_responseArray, 0, _responseArray.Length); // write bytes to the output stream
                context.Response.KeepAlive = false; // set the KeepAlive bool to false
                context.Response.Close(); // close the connection
                Console.WriteLine("Response given to a request.");

                // Note: The GetContext method blocks while waiting for a request. 
                HttpListenerRequest request = context.Request;
                Console.WriteLine("URL: {0}", request.Url.OriginalString);
                Console.WriteLine("Raw URL: {0}", request.RawUrl);

                switch (request.RawUrl)
                {
                    default:
                        //Respostas
                        switch (request.RawUrl.Substring(13, 1))
                        {
                            case "1":
                                label3.BeginInvoke((MethodInvoker)delegate () { this.label3.Text = "Salvando..."; });
                                Salvar(request.RawUrl.Substring(1, 12));
                                label3.BeginInvoke((MethodInvoker)delegate () { this.label3.Text = "Salvo"; });
                                Thread.Sleep(1000);
                                label3.BeginInvoke((MethodInvoker)delegate () { this.label3.Text = "Resultado"; });
                                break;
                            case "2":
                                label3.BeginInvoke((MethodInvoker)delegate () { this.label6.Text = "Salvando..."; });
                                Salvar(request.RawUrl.Substring(1, 12));
                                label3.BeginInvoke((MethodInvoker)delegate () { this.label6.Text = "Salvo"; });
                                Thread.Sleep(1000);
                                label3.BeginInvoke((MethodInvoker)delegate () { this.label6.Text = "Resultado"; });
                                break;
                            case "3":
                                label3.BeginInvoke((MethodInvoker)delegate () { this.label9.Text = "Salvando..."; });
                                Salvar(request.RawUrl.Substring(1, 12));
                                label3.BeginInvoke((MethodInvoker)delegate () { this.label9.Text = "Salvo"; });
                                Thread.Sleep(1000);
                                label3.BeginInvoke((MethodInvoker)delegate () { this.label9.Text = "Resultado"; });
                                break;
                        }
                        break;
                    case "/favicon.ico":
                    break;
                    case "/con1":
                        label5.BeginInvoke((MethodInvoker)delegate () { this.label2.Text = "Conectado"; });
                        break;
                    case "/ini1":
                        label3.BeginInvoke((MethodInvoker)delegate () { this.label3.Text = "Respondendo"; });
                        break;
                    case "/fim1":
                        label3.BeginInvoke((MethodInvoker)delegate () { this.label3.Text = "Aguardando"; });
                        break;
                    case "/con2":
                        label5.BeginInvoke((MethodInvoker)delegate () { this.label5.Text = "Conectado"; });
                        break;
                    case "/ini2":
                        label3.BeginInvoke((MethodInvoker)delegate () { this.label6.Text = "Respondendo"; });
                        break;
                    case "/fim2":
                        label3.BeginInvoke((MethodInvoker)delegate () { this.label6.Text = "Aguardando"; });
                        break;
                    case "/con3":
                        label5.BeginInvoke((MethodInvoker)delegate () { this.label8.Text = "Conectado"; });
                        break;
                    case "/ini3":
                        label3.BeginInvoke((MethodInvoker)delegate () { this.label9.Text = "Respondendo"; });
                        break;
                    case "/fim3":
                        label3.BeginInvoke((MethodInvoker)delegate () { this.label9.Text = "Aguardando"; });
                        break;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public void Salvar(string resps)
        {
            File.AppendAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Respostas.txt", resps + Environment.NewLine);
        }
    }
}
