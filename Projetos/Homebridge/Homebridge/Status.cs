using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homebridge
{
    public class Status
    {
        /*public int TempAmb { get; set; }
        public int ACState { get; set; }
        public int ACTemp { get; set; }
        public bool LEDState { get; set; }
        public string LEDColor { get; set; }*/
        public string TempAmb { get; set; }
        public string ACState { get; set; }
        public string ACTemp { get; set; }
        public string LEDState { get; set; }
        //public string LEDColor { get; set; }

        public Status (string tempAmb, string aCState, string aCTemp, string lEDState)
        {
            /*TempAmb = Int32.Parse(tempAmb.Substring(0, 2)); //+ 2?
            ACState = Int32.Parse(aCState);
            ACTemp = Int32.Parse(aCTemp);
            if (Int32.Parse(lEDState) == 1)
                LEDState = true;
            else
                LEDState = false;
            LEDColor = lEDColor.Substring(0, 6);*/
            TempAmb = (Int32.Parse(tempAmb) - 2).ToString();
            ACState = aCState;
            ACTemp = aCTemp;
            LEDState = lEDState;
            //LEDColor = lEDColor;
        }
    }
}
