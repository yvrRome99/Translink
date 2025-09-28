using AventStack.ExtentReports;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Translink.Common;

namespace Translink.PageObjects
{
    public class WelcomeToTransLinkPage : PageBase
    {
        public WelcomeToTransLinkPage(IPage page, TestContext testContext) : base(page, testContext) { }

        //--------------Element Definitions----------------//
        private ILocator PageIdentifier() => Page.GetByRole(AriaRole.Heading, new() { Name = "Welcome to TransLink" });
        private ILocator SchedulesAndMapsLink() => Page.GetByRole(AriaRole.Link, new() { Name = "Schedules and Maps" });
        private ILocator BusMenuItem() => Page.GetByRole(AriaRole.Link, new() { Name = "Bus", Exact = true });

        //---------------------Methods---------------------//
        public async Task IsPageAvailableAsync()
        {
            await PageIdentifier().WaitForAsync(new() { State = WaitForSelectorState.Visible });
            Assert.IsTrue(await PageIdentifier().IsVisibleAsync(), "Welcome to TransLink Page Not Found!");

            var path = await Helpers.AddScreenshotAsync("WelcomeToTransLinkPage", TestContext, Page);
            Regression.Node.Pass("TransLink page successfully opened", MediaEntityBuilder.CreateScreenCaptureFromPath(path).Build());
        }

        public async Task NavigateToBusSchedules()
        {
            await SchedulesAndMapsLink().HoverAsync();
            await BusMenuItem().ClickElementAsync();
        }
    }
}
