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
    public class RoutePage : PageBase
    {
        public RoutePage(IPage page, TestContext testContext) : base(page, testContext) { }

        //--------------Element Definitions----------------//
        private ILocator PageIdentifier() => Page.Locator("//div[@data-block-name='Route Name Title']");
        private ILocator StartDateField() => Page.Locator("//input[@name='startDate']");
        private ILocator StartTimeField() => Page.Locator("//input[@name='startTime']");
        private ILocator EndTimeField() => Page.Locator("//input[@name='endTime']");
        private ILocator SearchButton() => Page.Locator("//section[@id='schedules_tab']//button[@form='SchedulesTimeFilter']");
        private ILocator SearchResult(string result) => Page.Locator($"(//a/span[text()='{result}'])[1]");

        //---------------------Methods---------------------//
        public async Task IsPageAvailableAsync()
        {
            PageIdentifier().WaitForAsync(new() { State = WaitForSelectorState.Visible });
            Assert.IsTrue(await PageIdentifier().IsVisibleAsync(), "Route Page Not Found!");
        }

        public async Task SearchForRoute(string dateString, string startTime, string endTime)
        {
            await StartDateField().SetValueAsync(Page, dateString);
            await StartTimeField().SetValueAsync(Page, startTime);
            await EndTimeField().SetValueAsync(Page, endTime);

            await SearchButton().ClickElementAsync(5000);

            var path = await Helpers.AddScreenshotAsync("RoutePage", TestContext, Page);
            Regression.Node.Pass($"Search for route", MediaEntityBuilder.CreateScreenCaptureFromPath(path).Build());
        }

        public async Task SelectResult(string station)
        {
            await SearchResult(station).ClickElementAsync(5000);
        }
    }
}
