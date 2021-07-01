using System;
using OpenQA.Selenium;
using System.Threading.Tasks;

namespace GoogleSearcher
{
    public class GoogleSearchAutomation: Lothur.Web.PageAutomation<Task>
    {
        private const string ADDRESS = "http://google.com.br";

        public GoogleSearchAutomation(System.Uri driverServerURI,ICapabilities capabilities):base(new Uri(ADDRESS), driverServerURI, capabilities)
        {}

        public override Task Execute()
        {
            this.Find(".a4bIc input[name='q']").SendKeys("hello world");
            this.Find("input[name='btnK']").Submit();

            return Task.CompletedTask;
        }
    }
}
