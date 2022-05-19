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

            

            Console.ReadLine();
            cm.Exit();
        }
    }

    public class Voice
    {
        Chrome cm { get; set; }

        public Voice()
        {
            cm = new Chrome();
            cm.Url("https://www.youtube.com");

            string step = "";
            
        }


        public void Test()
        {

        }
    }




}