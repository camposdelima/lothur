using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;

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
        private const int WaitTimeout = 10;

        protected IWebDriver Driver { get; }

        public WebAutomation(System.Uri driverServer, ICapabilities capabilities)
        {
            Driver = new RemoteWebDriver(driverServer, capabilities);
        }

        protected WebDriverWait WaitForIt(int timeout = 10)
        {
            return new WebDriverWait(this.Driver, TimeSpan.FromSeconds(timeout));
        }

        protected void Navigate(string url)
        {
            this.Driver.Navigate().GoToUrl(url);
        }

        protected void TryNavigateToFrameAndSwitchIt(string frame, int timeout = WaitTimeout)
        {
            var wait = WaitForIt(timeout);
            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(frame));
        }

        protected IWebElement TryClickElement(By by, int timeout = WaitTimeout)
        {
            var wait = WaitForIt(timeout);
            return wait.Until(ExpectedConditions.ElementToBeClickable(by));
        }

        protected bool CheckSelectedElement(By by, int timeout = WaitTimeout)
        {
            var wait = WaitForIt(timeout);
            return wait.Until(ExpectedConditions.ElementToBeSelected(by));
        }
        protected IWebElement CheckElementIsVisible(By by, int timeout = WaitTimeout)
        {
            var wait = WaitForIt(timeout);
            return wait.Until(ExpectedConditions.ElementIsVisible(by));
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

        protected IWebElement TryFind(string selector)
        {
            try
            {
                return this.Find(selector);
            } catch(NoSuchElementException)
            {
                return null;
            }
        }

        protected IWebElement WaitForFind(string selector, int timeout = WaitTimeout)
        {
            this.WaitForCondition(() => this.TryFind(selector) != null, timeout);

            return this.Find(selector);
        }

        /// <summary>
        /// Wait for one expected condition.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="timeout">Timeout, in seconds, for condition.</param>
        protected void WaitForCondition(Func<bool> condition, int timeout = WaitTimeout)
        {
            SpinWait.SpinUntil(condition, timeout * 1000);
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
