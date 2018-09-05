using OpenQA.Selenium;
using System.Threading.Tasks;

namespace Lothur.Web
{
    public interface IMiddlewareAutomation : IWebAutomation<Task>
    {
    }

    public abstract class MiddlewareAutomation : WebAutomation<Task>, IMiddlewareAutomation
    {

        public MiddlewareAutomation(IWebDriver driver) : base(driver)
        {
        }
    }
}
