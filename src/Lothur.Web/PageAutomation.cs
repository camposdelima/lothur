using OpenQA.Selenium;
using System.Threading.Tasks;

namespace Lothur.Web
{
    public interface IPageAutomation<TTask>:IWebAutomation<TTask>
         where TTask : Task
    {
    }

    public abstract class PageAutomation<TTask>: WebAutomation<TTask>, IPageAutomation<TTask>
        where TTask:Task
    {
        protected System.Uri PageAddress { get; }

        public PageAutomation(System.Uri pageAddress, System.Uri driverServer,ICapabilities capabilities):base(driverServer, capabilities)
        {
            this.PageAddress = pageAddress;
            Navigate();
        }

        protected virtual void Navigate()
        {
            this.Navigate(this.PageAddress.AbsoluteUri);
        }

    }
}
