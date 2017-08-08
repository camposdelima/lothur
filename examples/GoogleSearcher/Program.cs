using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;

namespace GoogleSearcher
{
    class Program
    {
        static void Main(string[] args)
        {
            using (GoogleSearchAutomation search = CreateSearchAutomation())
            {
                search.Execute();
                Console.WriteLine("Done!");
            }
        }


        private static GoogleSearchAutomation CreateSearchAutomation()
        {
            Uri driverServerURI = new Uri("http://192.168.1.103:4444");
            ICapabilities driverCapabilities = new DesiredCapabilities();

            return new GoogleSearchAutomation(driverServerURI, driverCapabilities);
        }
    }
}