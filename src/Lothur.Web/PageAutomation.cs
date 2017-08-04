using OpenQA.Selenium;

namespace Lothur.Web
{
    public interface IPageAutomation:IWebAutomation
    {
    }

    public abstract class PageAutomation: WebAutomation, IPageAutomation
    {
        protected string PageAddress { get; }

        public PageAutomation(string pageAddress, System.Uri uri,ICapabilities capabilities):base(uri, capabilities)
        {
            this.PageAddress = pageAddress;
            Navigate();
        }

        protected virtual void Navigate()
        {
            this.Navigate(this.PageAddress);
        }

    }
}
