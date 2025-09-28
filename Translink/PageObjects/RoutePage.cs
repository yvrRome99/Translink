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
        private ILocator SearchResult(string station) => Page.Locator($"(//a/span[text()='{station}'])[1]");
        private ILocator SearchResultTimes(string station) => Page.Locator($"//a/span[text()='{station}']/ancestor::th[1]/following-sibling::td");

        //---------------------Methods---------------------//
        public async Task IsPageAvailableAsync()
        {
            PageIdentifier().WaitForAsync(new() { State = WaitForSelectorState.Visible });
            Assert.IsTrue(await PageIdentifier().IsVisibleAsync(), "Route Page Not Found!");
        }

        public async Task SearchForRouteAsync(string dateString, string startTime, string endTime)
        {
            await StartDateField().SetValueAsync(Page, dateString);
            await StartTimeField().SetValueAsync(Page, startTime);
            await EndTimeField().SetValueAsync(Page, endTime, 2000);

            await SearchButton().ClickElementAsync(5000);

            var path = await Helpers.AddScreenshotAsync("RoutePage", TestContext, Page);
            Regression.Node.Pass($"Search for route", MediaEntityBuilder.CreateScreenCaptureFromPath(path).Build());
        }

        public async Task<int> GetFirstInteractableLocatorIndexAsync(string station)
        {
            SearchResult(station).WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await SearchResult(station).ScrollIntoViewIfNeededAsync();
            var tdElements = SearchResultTimes(station);
            int count = await tdElements.CountAsync();

            for (int i = 0; i < count; i++)
            {
                var td = tdElements.Nth(i);
                
                if (await td.IsVisibleAsync() && await td.IsEnabledAsync())
                {
                    return i;
                }
            }

            return -1;
        }

        public async Task ValidateTimes(string station)
        {
            var index = Convert.ToInt32(await GetFirstInteractableLocatorIndexAsync(station));

            if (index > 0)
            {
                var path = await Helpers.AddScreenshotAsync("RoutePage", TestContext, Page);
                Regression.Node.Info($"Screenshot for reference", MediaEntityBuilder.CreateScreenCaptureFromPath(path).Build());

                var busTime1 = await SearchResultTimes(station).Nth(index).InnerTextAsync();
                var busTime2 = await SearchResultTimes(station).Nth(index + 1).InnerTextAsync();
                var busTime3 = await SearchResultTimes(station).Nth(index + 2).InnerTextAsync();
                var busTime4 = await SearchResultTimes(station).Nth(index + 3).InnerTextAsync();

                bool isInOrder = DateTime.Parse(busTime1) < DateTime.Parse(busTime2) && DateTime.Parse(busTime2) < DateTime.Parse(busTime3) && DateTime.Parse(busTime3) < DateTime.Parse(busTime4);
                if (isInOrder)
                {
                    Regression.Node.Pass($"The bus times appear in the correct order: {busTime1} < {busTime2} < {busTime3} < {busTime4}");
                }
                else
                {
                    Regression.Node.Fail($"The bus times do not appear in the correct order!");
                }

                var time12Variance = (DateTime.Parse(busTime2) - DateTime.Parse(busTime1)).TotalMinutes;
                var time23Variance = (DateTime.Parse(busTime3) - DateTime.Parse(busTime2)).TotalMinutes;
                var time34Variance = (DateTime.Parse(busTime4) - DateTime.Parse(busTime3)).TotalMinutes;

                bool isLessThan60For12 = time12Variance < 60;
                bool isLessThan60For23 = time23Variance < 60;
                bool isLessThan60For34 = time34Variance < 60;

                if (isLessThan60For12 && isLessThan60For23 && isLessThan60For34)
                {
                    Regression.Node.Pass($"The time betwen two busses is less than 60 minutes. Between Bus1 and Bus2 is {time12Variance} min, between Bus2 and Bus3 is {time23Variance} min, and finally between Bus3 and Bus4 is {time34Variance} min");
                }
                else
                {
                    Regression.Node.Fail($"The time between two busses is greater than 60 minutes.");
                }
            }
            else
            {
                var path = await Helpers.AddScreenshotAsync("RoutePage", TestContext, Page);
                Regression.Node.Fail($"Unable to find route times", MediaEntityBuilder.CreateScreenCaptureFromPath(path).Build());
            }
        }

        public async Task SelectResultAsync(string station)
        {
            await SearchResult(station).ClickElementAsync(5000);
        }
    }
}
