using System;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Xunit;

namespace Lothur.Web.Tests
{
    public class PageAutomationTest:Lothur.Web.PageAutomation<Task>
    {
        const string PageExpectedAddress = "http://www.google.com.br";
        const string DriverServerAddress = "http://localhost:9515";

        public PageAutomationTest() : base(
            new Uri(PageExpectedAddress),
            new Uri(DriverServerAddress),
            new OpenQA.Selenium.Remote.DesiredCapabilities())
        {

        }

        public override Task Execute()
        {
            return Task.CompletedTask;
        }

        [Fact]
        public void IsExpectedHostAddress()
        {
            Assert.Equal(
                new Uri(PageExpectedAddress).Host,
                new Uri(this.Driver.Url).Host
            );
        }

        [Fact]
        public void IsCompleted()
        {
            Task execution = Execute();

            Assert.True(execution.IsCompleted);
        }
    }
}
