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
    public class MyFavouritesPage : PageBase
    {
        public MyFavouritesPage(IPage page, TestContext testContext) : base(page, testContext) { }

        //--------------Element Definitions----------------//
        private ILocator PageIdentifier() => Page.GetByRole(AriaRole.Heading, new() { Name = "My Favourites", Exact = true });
        private ILocator FavouriteLink(string link) => Page.GetByRole(AriaRole.Link, new() { Name = $"{link}" });

        //---------------------Methods---------------------//
        public async Task IsPageAvailableAsync()
        {
            PageIdentifier().WaitForAsync(new() { State = WaitForSelectorState.Visible });
            Assert.IsTrue(await PageIdentifier().IsVisibleAsync(), "Bus Schedules Page Not Found!");
        }

        public async Task ValidateFavourite(string linkName)
        {
            Assert.IsTrue(await FavouriteLink(linkName).IsVisibleAsync(), $"'{linkName}' favourite was not found!");

            var path = await Helpers.AddScreenshotAsync("MyFavouritesPage", TestContext, Page);
            Regression.Node.Pass($"Validate '{linkName}' added to favourites", MediaEntityBuilder.CreateScreenCaptureFromPath(path).Build());
        }
    }
}
