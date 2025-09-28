using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Translink.PageObjects
{
    public abstract class PageBase
    {
        protected readonly IPage Page;
        protected readonly TestContext TestContext;

        protected PageBase(IPage page, TestContext testContext)
        {
            Page = page;
            TestContext = testContext;
        }
    }
}
