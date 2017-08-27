using System;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Xunit;

namespace Lothur.Web.Tests
{
    public class WebAutomationTest:Lothur.Web.WebAutomation<Task>
    {
        const string DriverServerAddress = "http://localhost:9515";
        const string TestedAddress = "http://www.google.com.br";

        public WebAutomationTest() : base(
            new Uri(DriverServerAddress),
            new OpenQA.Selenium.Remote.DesiredCapabilities())
        {
            this.Navigate(TestedAddress);
        }

        public override Task Execute()
        {
            return Task.CompletedTask;
        }

        [Fact]
        public void WasFoundElement()
        {
            IWebElement element = this.Find("#lst-ib");

            Assert.NotNull(element);
        }


        private void ChangePage()
        {
            this.Find("#lst-ib").SendKeys("hello world");
            this.Find("input[name='btnK']").Submit();
        }

        private void ChangePageWithDelay()
        {
            Task.Delay(TimeSpan.FromSeconds(5)).ContinueWith((task) => ChangePage());
        }

        [Fact]
        public void WasFoundLateElement()
        {
            ChangePageWithDelay();

            IWebElement element = this.WaitForFind("#hdtb-msb-vis");
            Assert.NotNull(element);
        }
    }
}
