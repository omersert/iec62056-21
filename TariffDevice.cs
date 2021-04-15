using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace modbuslib
{
    class TariffDevice
    {
        public SerialPort SerPort { get; set; }
        
        public string SerialNo { get; set; }

        public bool isItMakel { get; set; }

        public TariffDevice(SerialPort SerPort,  string SerialNo)
        {
            this.SerPort = SerPort;
            this.SerialNo = SerialNo;
            this.isItMakel = true;
        }
        public string Identification(string Brand)
        {
            string command = Brand == "Makel" ? $"/?MSY{SerialNo}!{Environment.NewLine}" : $"/?{SerialNo}!{Environment.NewLine}";
            this.SerPort.Open();
            this.SerPort.WriteLine(command);
            string answer = this.SerPort.ReadLine();

            return answer;

        }
        public string Identification2()
        {
            string command1 = $"/?MSY{SerialNo}!{Environment.NewLine}";
            string command2 = $"/?{SerialNo}!{Environment.NewLine}";
            this.SerPort.Open();
            string answer = "";
            try
            {
                this.SerPort.WriteLine(command1);
                answer = this.SerPort.ReadLine();
            }
            catch
            {
                this.SerPort.WriteLine(command2);
                answer = this.SerPort.ReadLine();
                this.isItMakel = false;
            }
            
            return answer;
        }

        public string ReadOut()
        {
            Thread.Sleep(250);

            string command = (char)6 + "050" + Environment.NewLine;
            SerPort.WriteLine(command);

            Thread.Sleep(250);
            SerPort.BaudRate = 9600;
            string answer = SerPort.ReadTo(Convert.ToString((char)3));

            return answer;
        }

        public string ProgrammingMode(string Brand)
        {
            Thread.Sleep(250);

            string command = (char)6 + "051" + Environment.NewLine;
            string answer = "";

            SerPort.WriteLine(command);

            Thread.Sleep(250);

            SerPort.BaudRate = 9600;

            if (Brand == "Makel")
            {
                 answer = SerPort.ReadTo(Convert.ToString((char)3));
            }
            else
            {
                 answer = "ACK";
            }

            return answer;
        }

        public string ProgrammingMode2()
        {
            Thread.Sleep(250);

            string command = (char)6 + "051" + Environment.NewLine;
            string answer = "";

            SerPort.WriteLine(command);

            Thread.Sleep(250);

            SerPort.BaudRate = 9600;
            if (this.isItMakel == false)
            {
                answer = "ACK";
            }
            else
            {
                answer = SerPort.ReadTo(Convert.ToString((char)3));
            }
            return answer;
        }


        public string GetObisResult(string Obis)
        {
            Thread.Sleep(250);
            string command3 = $"\u0001R2\u0002{Obis}()\u0003";
            string command2 = ComputeBCC(command3);
            SerPort.Write(command2);
            string query_result = SerPort.ReadTo(Convert.ToString((char)3));

            Regex rx = new Regex(@"\((.*?)\)");
            var s1 = Regex.Match(query_result, @"\((.*?)\)").Value;

            s1 = s1.TrimStart('(');
            s1 = s1.Remove(s1.Length - 1);

            return s1;
        }

        public string UreticiyeOzel()
        {
            Thread.Sleep(250);
            string command = (char)6 + "057" + Environment.NewLine;
            string answer = SerPort.ReadTo(Convert.ToString((char)3));
            
            return answer;
        }

        public void CompletelySignOff()
        {
            string command = "\u0001B0\u0003\u0071";

            Thread.Sleep(250);

            SerPort.Write(command);
        }

        public byte GetBCC(byte[] inputStream)
        {
            byte bcc = 0;

            if (inputStream != null && inputStream.Length > 0)
            {

                for (int i = 1; i < inputStream.Length; i++)
                {
                    bcc ^= inputStream[i];
                }
            }

            return bcc;
        }

        public string ComputeBCC(String inputstr)
        {
            byte[] ba = Encoding.Default.GetBytes(inputstr);
            var hex = BitConverter.ToString(ba);
            hex = hex.Replace("-", "");
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);

            var bccdec = GetBCC(bytes);
            string result = $"{inputstr}{(char)bccdec}";
            return result;
        }
    }
}
