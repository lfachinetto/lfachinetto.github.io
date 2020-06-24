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

namespace Homebridge
{
    public partial class Form1 : Form
    {
        //int ligado = 0;
        //int temperatura = 23;
        //int tempAmb;
        //private bool allowVisible;
        Status status;
        int times = 0;
        int timess = 0;
        static HttpListener _httpListener = new HttpListener();

        public Form1()
        {
            InitializeComponent();
            Web();
            label1.Text = "Sistema funcionando";
            numericUpDown1.Value = 23;
        }

        /*protected override void SetVisibleCore(bool value)
        {
            if (!allowVisible)
            {
                value = false;
                if (!this.IsHandleCreated) CreateHandle();
            }
            base.SetVisibleCore(value);
        }*/

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.Open();
            serialPort1.Write("1");
            serialPort1.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serialPort1.Open();
            serialPort1.Write("2");
            serialPort1.Close();
        }

        void Web()
        {
            Console.WriteLine("Starting server...");
            _httpListener.Prefixes.Add("http://*:5000/"); // add prefix "http://localhost:5000/"
            _httpListener.Start(); // start server (Run application as Administrator!)
            Console.WriteLine("Server started.");
            Thread _responseThread = new Thread(ResponseThread);
            _responseThread.Start(); // start the response thread
        }

        void ResponseThread()
        {            
            while (true)
            {                
                try
                {
                    HttpListenerContext context = _httpListener.GetContext(); // get a context
                                                                              // Now, you'll find the request URL in context.Request.Url

                    

                    byte[] _responseArray = null;
                    switch (context.Request.RawUrl)
                    {
                        default:
                            _responseArray = Encoding.UTF8.GetBytes("<h1>Bem-vindo ao servidor Homebridge!</h1>");
                            break;
                        case "/getledstatus":
                            if (times == 0)
                                GetStatus();
                            times++;
                            if (times == 5)
                                times = 0;
                            _responseArray = Encoding.UTF8.GetBytes(status.LEDState);
                            break;
                        case "/getcolor":
                            _responseArray = Encoding.UTF8.GetBytes("116100100");//status.LEDColor);
                            break;
                        case "/status":
                            //_responseArray = Encoding.UTF8.GetBytes("targetHeatingCoolingState: 0,\r\ntargetTemperature: 25.5,\r\ntargetRelativeHumidity: 25.5,\r\ncurrentHeatingCoolingState: 2,\r\ncurrentTemperature: 25.5,\r\ncurrentRelativeHumidity: 23");

                            if (times == 0)
                                GetStatus();
                            times++;
                            if (times == 4)
                                times = 0;

                            _responseArray = Encoding.UTF8.GetBytes("{ \"targetHeatingCoolingState\": " + status.ACState + ", \"targetTemperature\": " + status.ACTemp +", \"currentHeatingCoolingState\": " + status.ACState + ", \"currentTemperature\": " + status.TempAmb + "}");

                            Console.WriteLine(status.TempAmb);
                            break;
                    }
                    context.Response.OutputStream.Write(_responseArray, 0, _responseArray.Length); // write bytes to the output stream
                    context.Response.KeepAlive = false; // set the KeepAlive bool to false
                    context.Response.Close(); // close the connection
                    Console.WriteLine("Respone given to a request.");

                    // Note: The GetContext method blocks while waiting for a request. 
                    HttpListenerRequest request = context.Request;

                    Console.WriteLine("URL: {0}", request.Url.OriginalString);
                    Console.WriteLine("Raw URL: {0}", request.RawUrl);

                    //serialPort1.Open();
                    //serialPort1.Write("200");
                    //Thread.Sleep(100);
                    //int temperatura = Int32.Parse(serialPort1.ReadLine());
                    //serialPort1.Close();
                    //Solucionar código acima
                    int temperatura = 25;
                    int tempini = temperatura;
                    switch (request.RawUrl)
                    {
                        case "/offled":
                            //Thread.Sleep(1000);
                            serialPort1.Open();
                            serialPort1.Write("4");
                            serialPort1.Close();                            
                            break;
                        case "/onled":
                            //Thread.Sleep(1000);
                            serialPort1.Open();
                            serialPort1.Write("3");
                            serialPort1.Close();
                            break;
                        case "/off":
                            //Thread.Sleep(1000);
                            serialPort1.Open();
                            serialPort1.Write("2");
                            serialPort1.Close();
                            break;
                        case "/comfort":
                            //Thread.Sleep(1000);
                            serialPort1.Open();
                            serialPort1.Write("1");
                            serialPort1.Close();
                            break;
                        case "/no-frost": //Indiferente
                            //Thread.Sleep(1000);
                            serialPort1.Open();
                            serialPort1.Write("1");
                            serialPort1.Close();
                            break;
                        case "/auto": //Indiferente
                            //Thread.Sleep(1000);
                            serialPort1.Open();
                            serialPort1.Write("1");
                            serialPort1.Close();
                            break;
                        //Cores personalizadas                        
                        case "/cor=FF034A"://Vermelho
                            serialPort1.Open();
                            serialPort1.Write("101");
                            serialPort1.Close();
                            break;
                        case "/cor=0015FF"://Azul
                            serialPort1.Open();
                            serialPort1.Write("301");
                            serialPort1.Close();
                            break;
                        case "/cor=11FF00"://Verde
                            serialPort1.Open();
                            serialPort1.Write("201");
                            serialPort1.Close();
                            break;
                        case "/cor=FF0090"://Rosa
                            serialPort1.Open();
                            serialPort1.Write("305");
                            serialPort1.Close();
                            break;
                        case "/cor=FFEA00"://Amarelo
                            serialPort1.Open();
                            serialPort1.Write("105");
                            serialPort1.Close();
                            break;
                        case "/cor=7D52FF"://Roxo
                            serialPort1.Open();
                            serialPort1.Write("303");
                            serialPort1.Close();
                            break;
                        //Cores da Siri
                        case "/cor=FF0000"://Vermelho
                            serialPort1.Open();
                            serialPort1.Write("101");
                            serialPort1.Close();
                            break;
                        case "/cor=0000FF"://Azul
                            serialPort1.Open();
                            serialPort1.Write("301");
                            serialPort1.Close();
                            break;
                        case "/cor=00FF00"://Verde
                            serialPort1.Open();
                            serialPort1.Write("201");
                            serialPort1.Close();
                            break;
                        case "/cor=FF8095"://Rosa
                            serialPort1.Open();
                            serialPort1.Write("305");
                            serialPort1.Close();
                            break;
                        case "/cor=FFFF00"://Amarelo
                            serialPort1.Open();
                            serialPort1.Write("105");
                            serialPort1.Close();
                            break;
                        case "/cor=FF00FF"://Roxo
                            serialPort1.Open();
                            serialPort1.Write("303");
                            serialPort1.Close();
                            break;
                        /*case string s when (Int32.Parse(request.RawUrl.Substring(6)) >= 18 && Int32.Parse(request.RawUrl.Substring(6)) <= 31):                        
                            break;*/
                        //case string r when r.Substring(0, 6) == "/modo=":
                        //trial = Int32.TryParse(request.RawUrl.Substring(6, 2), out temperatura);
                        //break;
                        default:
                            if (request.RawUrl.Length >= 19 && request.RawUrl.Substring(0, 19) == "/targetTemperature/")
                                Int32.TryParse(request.RawUrl.Substring(19, 2), out temperatura);
                            else if (request.RawUrl.Length >= 6 && request.RawUrl.Substring(0, 6) == "/cor=")
                            {
                                request.RawUrl.Substring(6, 2);
                                serialPort1.Open();
                                serialPort1.Write("1");
                                serialPort1.Close();
                            }
                            else
                                if (request.RawUrl != "/comfort" && request.RawUrl != "/status" && request.RawUrl != "/getledstatus" && request.RawUrl != "/getcolor" && request.RawUrl != "/favicon.ico")
                                MessageBox.Show("Comando desconhecido: " + request.RawUrl);
                            break;
                    }
                    if (tempini != temperatura)
                    {
                        //Thread.Sleep(1000);
                        serialPort1.Open();
                        serialPort1.Write(temperatura.ToString());
                        serialPort1.Close();
                    }                    
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Não foi possível se comunicar com o Arduino.");
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*{
                //if (!allowClose)
                {
                    this.Hide();
                    e.Cancel = true;
                }
                base.OnFormClosing(e);
            }*/
            Environment.Exit(0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int temperatura;
            temperatura = Convert.ToInt32(numericUpDown1.Value);
            serialPort1.Open();
            serialPort1.Write(temperatura.ToString());
            serialPort1.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            serialPort1.Open();
            serialPort1.Write(textBox2.Text);
            serialPort1.Close();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            /*if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                this.Hide();
            }

            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }*/
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            //allowVisible = true;
            //Show();
        }

        void GetStatus()
        {
            //Thread.Sleep(1000);
            serialPort1.Open();
            serialPort1.Write("5");            
            string result = serialPort1.ReadLine();
            serialPort1.Close(); //Ex.: 17.092232
            status = new Status(result.Substring(0, 2), result.Substring(2, 1), result.Substring(3, 2), result.Substring(5, 1));
            Console.WriteLine(result);
        }
    }
}