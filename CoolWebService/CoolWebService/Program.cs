using System;
using System.Diagnostics;
using ServiceStack.Text;

namespace CoolWebService
{
    class Program
    {
        static void Main(string[] args)
        {
            new AppHost().Init().Start("http://localhost:8088/");
            "ServiceStack Self Host with Razor listening at http://localhost:8088".Print();
            Process.Start("http://localhost:8088/");

            Console.ReadLine();
        }
    }
}
