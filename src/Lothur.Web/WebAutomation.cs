using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using yozepi.Retry;

namespace Lothur.Web
{
    public interface IWebAutomation<TTask> : IDisposable
        where TTask : Task
    {
        TTask Execute();
    }

    public abstract class WebAutomation<TTask> : IWebAutomation<TTask>
         where TTask : Task
    {
        private const int WaitTimeout = 30;
        private const int WaitInterval = 500;

        protected IWebDriver Driver { get; }
        protected IClock Clock { get; } = new SystemClock();

        protected virtual IEnumerable<IMiddlewareAutomation> Middlewares { get; }

        public WebAutomation(System.Uri driverServer, ICapabilities capabilities)
        {
            Driver = new RemoteWebDriver(driverServer, capabilities);
            Init();
        }

        public WebAutomation(IWebDriver driver)
        {
            Driver = driver;
            Init();
        }


        private void Init()
        {
            ExecuteMiddlewares();
        }

        private void ExecuteMiddlewares()
        {
            if (Middlewares == null) return;

            foreach (var middleware in Middlewares)
                middleware.Execute();
        }

        protected virtual void Navigate(System.Uri uri)
        {
            this.Driver.Navigate().GoToUrl(uri.AbsoluteUri);
        }

        protected virtual void Navigate(string url)
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

        protected IWebElement FindByName(string selector)
        {
            return this.Driver.FindElement(By.Name(selector));
        }

        protected IEnumerable<IWebElement> FindMany(By by)
        {
            return this.Driver.FindElements(by);
        }

        protected IEnumerable<IWebElement> FindMany(string selector)
        {
            return this.Driver.FindElements(By.CssSelector(selector));
        }

        protected IWebElement TryFind(string selector)
        {
            try
            {
                return this.Find(selector);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        protected IWebElement WaitForFind(string selector, int timeout = WaitTimeout)
        {
            this.WaitForCondition(() => this.TryFind(selector), timeout);

            return this.Find(selector);
        }

        public IAlert SwitchToAlert()
        {

            try
            {
                return Driver.SwitchTo().Alert();

            }
            catch (NoAlertPresentException)
            {
                return null;
            }
        }

        /// <summary>
        /// Wait for one expected condition.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="timeout">Timeout, in seconds, for condition.</param>
        protected void WaitForCondition(Func<bool> condition, int timeout = WaitTimeout, int interval = WaitInterval)
        {
            //SpinWait.SpinUntil(condition, timeout * 1000);
            this.WaitForCondition<bool>(condition, timeout, interval);
        }

        /// <summary>
        /// Wait for one expected condition.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="timeout">Timeout, in seconds, for condition.</param>
        protected void WaitForCondition<TResult>(Func<TResult> condition, int timeout = WaitTimeout, int interval = WaitInterval)
        {
            new WebDriverWait(this.Clock, this.Driver, TimeSpan.FromSeconds(timeout), TimeSpan.FromMilliseconds(WaitInterval)).Until(driver => condition());
        }

        protected TResult ExecuteScript<TResult>(string command)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)this.Driver;
            return (TResult)js.ExecuteScript(command);
        }

        protected void ExecuteScript(string command)
        {
            ExecuteScript<object>(command);
        }

        public Screenshot GetScreenshot()
        {
            return ((ITakesScreenshot)this.Driver).GetScreenshot();
        }

        protected void Try(Action action, int retries = 3, int delay = 1)
        {
            try
            {
                TryIt.Try(action, retries).UsingDelay(TimeSpan.FromSeconds(delay)).ThenTry(() => { }, 1).Go();
            }
            catch (yozepi.Retry.RetryFailedException ex)
            {
                throw ((List<System.Exception>)ex.ExceptionList).LastOrDefault();
            }
        }

        protected T Try<T>(Func<T> action, int retries = 3, int delay = 1)
        {
            try
            {
                return TryIt.Try<T>(action, retries).UsingDelay(TimeSpan.FromSeconds(delay)).ThenTry(() => default(T), 1).Go();
            }
            catch (yozepi.Retry.RetryFailedException ex)
            {
                throw ((List<System.Exception>)ex.ExceptionList).LastOrDefault();
            }
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
