using System;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;


public static class Program
{

    static void Main(string[] args)
    {
        SerialPort port1 = new SerialPort("COM1");
        port1.BaudRate = 300;
        port1.Parity = Parity.Even;
        port1.StopBits = StopBits.One;
        port1.DataBits = 7;
        port1.ReadTimeout = 3000;
        port1.RtsEnable = true;
        port1.DtrEnable = true;

        string SerialNo_Makel = "40782736";
        string SerialNo_Kohler = "21003746";

        TariffDevice _device1 = new TariffDevice(port1, SerialNo_Kohler);


        string id = _device1.Identification2(); 
        System.Diagnostics.Debug.WriteLine(id);

        var ans1 = _device1.ProgrammingMode2(); 
        System.Diagnostics.Debug.WriteLine(ans1 + "\n");

        string[] arr1 = { "0.9.1", "0.9.2", "1.8.0" };

        foreach (string a in arr1)
        {
            System.Diagnostics.Debug.WriteLine($"{a} {_device1.GetObisResult(a)}");
        }

        _device1.CompletelySignOff();
        System.Diagnostics.Debug.WriteLine("");


        Thread.Sleep(250);

        _device1.SerialNo = SerialNo_Makel;

        string id2 = _device1.Identification2();
        System.Diagnostics.Debug.WriteLine(id2);

        var ans2 = _device1.ProgrammingMode2();
        System.Diagnostics.Debug.WriteLine(ans2 + "\n");


        foreach (string a in arr1)
        {
            System.Diagnostics.Debug.WriteLine($"{a} {_device1.GetObisResult(a)}");
        }

        _device1.CompletelySignOff();
        System.Diagnostics.Debug.WriteLine("");




        _device1.SerialNo = SerialNo_Kohler;

        string id3 = _device1.Identification2();
        System.Diagnostics.Debug.WriteLine(id3);

        var ans3 = _device1.ProgrammingMode2();
        System.Diagnostics.Debug.WriteLine(ans3 + "\n");


        foreach (string a in arr1)
        {
            System.Diagnostics.Debug.WriteLine($"{a} {_device1.GetObisResult(a)}");
        }

        _device1.CompletelySignOff();
        System.Diagnostics.Debug.WriteLine("");



    }
}

