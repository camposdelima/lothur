using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Threading.Tasks;

namespace Lothur.Web
{
    public interface IWebAutomation<TTask>: IDisposable
        where TTask: Task
    {
        TTask Execute();
    }

    public abstract class WebAutomation<TTask> : IWebAutomation<TTask>
         where TTask : Task
    {
        protected IWebDriver Driver { get; }

        public WebAutomation(System.Uri driverServer, ICapabilities capabilities)
        {
            Driver = new RemoteWebDriver(driverServer, capabilities);
        }

        protected void Navigate(string url)
        {
            this.Driver.Navigate().GoToUrl(url);
        }

        public abstract TTask Execute();

        protected IWebElement Find(By by)
        {
            return this.Driver.FindElement(by);
        }

        protected IWebElement Find(string selector)
        {
            return this.Driver.FindElement(By.CssSelector(selector));
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.Driver.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~WebAutomation() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
