using OpenQA.Selenium;
using System;
using System.Threading.Tasks;

namespace Lothur.Web
{
    public class LambdaMiddleware : MiddlewareAutomation
    {
        private Action Action { get; }

        public LambdaMiddleware(IWebDriver driver, Action action) : base(driver)
        {
            this.Action = action;
        }

        public override Task Execute()
        {
            this.Action();
            return Task.CompletedTask;
        }
    }
}