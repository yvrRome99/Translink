using AventStack.ExtentReports;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.Common;

namespace Translink.PageObjects
{
    public class StopPage : PageBase
    {
        public StopPage(IPage page, TestContext testContext) : base(page, testContext) { }

        //--------------Element Definitions----------------//
        private ILocator PageIdentifier() => Page.Locator("//h2/span[text()='Stop # ']");
        private ILocator AddToFavouritesLink() => Page.Locator("//article[@data-infocard-name='Information']//button[@data-infowindow='Add to Favourites']/img");
        private ILocator AddToFavouritesButton() => Page.Locator("//dialog[@data-infowindow-name='Add to Favourites']//button[text()='Add to favourites']");
        private ILocator NameField() => Page.Locator("//input[@name='gtfsFavouriteKey']");
        private ILocator ManageMyFavouritesLink() => Page.GetByRole(AriaRole.Link, new() { Name = "Manage my favourites" });
        private ILocator AllRoutesContent() => Page.Locator("#AllRoutesForStop");

        //---------------------Methods---------------------//
        public async Task IsPageAvailableAsync()
        {
            PageIdentifier().WaitForAsync(new() { State = WaitForSelectorState.Visible });
            Assert.IsTrue(await PageIdentifier().IsVisibleAsync(), "Stop Page Not Found!");
        }

        public async Task AddToFavourites(string newName)
        {
            AllRoutesContent().WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await AddToFavouritesLink().ClickElementAsync(2000);
            await NameField().SendKeysToElementAsync(newName);

            var path = await Helpers.AddScreenshotAsync("StopPage", TestContext, Page);
            Regression.Node.Pass($"Add {newName} to favourites", MediaEntityBuilder.CreateScreenCaptureFromPath(path).Build());

            await AddToFavouritesButton().ClickElementAsync(2000);
        }

        public async Task ValidateFavouritesSaved(string favourite)
        {
            await ManageMyFavouritesLink().ClickElementAsync(3000);
            await Pages.MyFavourite().ValidateFavourite(favourite);
        }
    }
}
