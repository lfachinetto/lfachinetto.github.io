﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace QuizMobile
{
    public class Question
    {
        public string Pergunta { get; set; }
        public string Foto { get; set; }
        public string[] Respostas { get; set; }
        public int[] Pontuacoes { get; set; }
        public int Resultado { get; set; }

        public Question(string perg, string fot, string[] resps, int[] ponts)
        {
            Pergunta = perg;
            Foto = fot;
            Respostas = resps;
            Pontuacoes = ponts;
        }

        public static bool Enviar(string comando)
        {
            try
            {
                WebClient myClient = new WebClient();
                Stream response = myClient.OpenRead(MainPage.ip + comando + MainPage.numero);
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }            
        }
    }
}
