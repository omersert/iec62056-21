using System;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace modbuslib
{
    public static class Program
    {
        
        static void Main(string[] args)
        {
            

            /* Regex kullanılmak istenirse parantezin içinin değerini buluyor.
            Regex rx = new Regex(@"\((.*?)\)");
            string s2 = "fasfsdfdsf(000002.314*kWh)1232323132";
            var s1 = Regex.Match(s2, @"\((.*?)\)").Value;
            */
            
            SerialPort port1 = new SerialPort("COM1");
            port1.BaudRate = 300;
            port1.Parity = Parity.Even;
            port1.StopBits = StopBits.One;
            port1.DataBits = 7;
            port1.ReadTimeout = 7000;
            port1.RtsEnable = true;
            port1.DtrEnable = true;

            string SerialNo_Makel = "40782736";
            string SerialNo_Kohler = "21003746";
            
            TariffDevice _device1 = new TariffDevice(port1, SerialNo_Kohler);

            string id = _device1.Identification(""); //Argüman sayacın markası
            System.Diagnostics.Debug.WriteLine(id);

            /*
            var ans1 = _device1.ReadOut();
            System.Diagnostics.Debug.WriteLine(ans1 + "\n");
            */
            
            var ans1 = _device1.ProgrammingMode(""); //Argüman sayacın markası
            System.Diagnostics.Debug.WriteLine(ans1 + "\n");

            string query_res = _device1.GetObisResult("0.9.1"); //Argüman istenilen veriye karşılık gelen Obis kodu
            System.Diagnostics.Debug.WriteLine(query_res);
            
            _device1.CompletelySignOff();
            System.Diagnostics.Debug.WriteLine("");
        }
    }
}
