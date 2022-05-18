using DevNcore.Automation.WebCrawler;
using System.Reflection;

namespace TestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ChromeDriver Start");

            var cm = new Chrome();            

            cm.Url("https://www.google.com");

            Console.ReadLine();
            cm.Exit();
        }
    }
}