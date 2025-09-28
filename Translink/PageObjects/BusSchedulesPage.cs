using AventStack.ExtentReports;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.Common;

namespace Translink.PageObjects
{
    public class BusSchedulesPage : PageBase
    {
        public BusSchedulesPage(IPage page, TestContext testContext) : base(page, testContext) { }

        //--------------Element Definitions----------------//
        private ILocator PageIdentifier() => Page.GetByRole(AriaRole.Heading, new() { Name = "Bus Schedules", Exact = true });
        private ILocator SearchByBox() => Page.Locator("#find-schedule-searchbox");
        private ILocator FindScheduleButton() => Page.Locator("//button/span[text()='Find Schedule']");
        private ILocator SearchResultList() => Page.Locator("//output[@class='searchResultsList']");
        private ILocator SearchResult(string resultString) => Page.GetByRole(AriaRole.Link, new() { Name = $"{resultString}" });

        //---------------------Methods---------------------//
        public async Task IsPageAvailableAsync()
        {
            PageIdentifier().WaitForAsync(new() { State = WaitForSelectorState.Visible });
            Assert.IsTrue(PageIdentifier().IsVisibleAsync().GetAwaiter().GetResult(), "Bus Schedules Page Not Found!");
        }

        public async Task SearchByField(string searchString)
        {
            await SearchByBox().SendKeysToElementAsync(searchString);
            await FindScheduleButton().ClickElementAsync();

            SearchResultList().WaitForAsync(new() { State = WaitForSelectorState.Visible });

            var path = await Helpers.AddScreenshotAsync("BusSchedulesPage", TestContext, Page);
            Regression.Node.Pass($"Search for bus schedule", MediaEntityBuilder.CreateScreenCaptureFromPath(path).Build());
        }

        public async Task SelectResult(string resultString)
        {
            await SearchResult(resultString).ClickElementAsync(2000);
        }
    }
}
