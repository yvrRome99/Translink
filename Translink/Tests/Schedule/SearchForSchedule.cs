using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using Translink.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Playwright;
using Translink.PageObjects;

namespace Translink.Tests.Schedule
{
    public static class SearchForSchedule
    {
        public static async Task RunTest(string browserName, TestContext testContext, string dataSource)
        {
            // Setup
            var playwright = await Playwright.CreateAsync();
            var browser = await Helpers.GetBrowserAsync(playwright, browserName, Convert.ToBoolean(testContext.Properties["Headless"].ToString()));
            var browsercontext = await browser.NewContextAsync();
            var page = await browsercontext.NewPageAsync();

            var testName = $"{testContext.TestName}_{browserName}";

            try
            {
                Regression.Test = Regression.Report.CreateTest(testContext.TestName, "This test will search for a bus schedule and save it as a favourite.");

                dynamic userData = Helpers.GetCSVData(testContext.TestName, dataSource);

                Pages.Init(page, testContext);

                // Load the site
                Regression.Node = Regression.Test.CreateNode($"Launch the browser for {testName}");
                await page.GotoAsync(testContext.Properties["SiteURL"].ToString());
                await Pages.WelcomeToTransLink().IsPageAvailableAsync();
                await Pages.WelcomeToTransLink().NavigateToBusSchedules();

                Regression.Node = Regression.Test.CreateNode($"Search for bus schedule");
                await Pages.BusSchedule().IsPageAvailableAsync();
                await Pages.BusSchedule().SearchByField(userData.Search);
                await Pages.BusSchedule().SelectResult(userData.Result);

                Regression.Node = Regression.Test.CreateNode($"Search for route");
                await Pages.Route().SearchForRouteAsync(Helpers.GetDate(Convert.ToInt32(userData.Date), 0, 0, 0, "yyyy-MM-dd"), userData.StartTime, userData.EndTime);
                await Pages.Route().ValidateTimes(userData.Station);
                await Pages.Route().SelectResultAsync(userData.Station);

                Regression.Node = Regression.Test.CreateNode($"Save route as favourite");
                await Pages.Stop().AddToFavourites(userData.NewName);
                await Pages.Stop().ValidateFavouritesSaved(userData.NewName);

                Regression.Test.Pass($"{testName} passed!");
            }
            catch (Exception ex)
            {
                var path = await Helpers.AddScreenshotAsync("WelcomeToTransLinkPage", testContext, page);
                Regression.Test.Fail($"SearchForSchedule failed! {ex}", MediaEntityBuilder.CreateScreenCaptureFromPath(path).Build());

                throw;
            }

        }
    }
}
