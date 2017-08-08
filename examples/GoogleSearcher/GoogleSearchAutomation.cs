using System;
using OpenQA.Selenium;

namespace GoogleSearcher
{
    public class GoogleSearchAutomation: Lothur.Web.PageAutomation
    {
        private const string ADDRESS = "http://google.com.br";

        public GoogleSearchAutomation(System.Uri driverServerURI,ICapabilities capabilities):base(ADDRESS, driverServerURI, capabilities)
        {}

        public override void Execute()
        {
            this.Find("#lst-ib").SendKeys("hello world");
            this.Find("input[name='btnK']").Submit();
        }
    }
}
