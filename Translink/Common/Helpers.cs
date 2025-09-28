using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink.Common
{
    public static class Helpers
    {
        public static async Task<string> AddScreenshotAsync(string name, TestContext testContext, IPage page)
        {
            var filename = $"{DateTime.Now.ToString("MMddyyyyHHmmss", CultureInfo.InvariantCulture)}_{name}.png";
            var filenameWithPath = Path.Combine(testContext.TestRunResultsDirectory, testContext.ManagedMethod, filename);

            await page.ScreenshotAsync(new()
            {
                Path = filenameWithPath
            });

            testContext.AddResultFile(filenameWithPath);

            return filenameWithPath;
        }

        /// <summary>
        /// This method will retrieve the row in the csv file that contains the test data
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="csvFile"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static dynamic GetCSVData(string keyName, string csvFile, string delimiter = ",")
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter,
                HasHeaderRecord = true,
            };

            List<dynamic> records;
            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, csvFile)))
            using (var csv = new CsvReader(reader, config))
            {
                records = csv.GetRecords<dynamic>().ToList();
            }

            var lookup = records.ToLookup(x => x.Key);

            return lookup[keyName].FirstOrDefault();
        }

        /// <summary>
        /// This method will get the appropriate browser for the test
        /// </summary>
        /// <param name="playwright"></param>
        /// <param name="browserName"></param>
        /// <param name="headless"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static async Task<IBrowser> GetBrowserAsync(IPlaywright playwright, string browserName, bool headless = false)
        {
            return browserName.ToLower() switch
            {
                "chromium" => await playwright.Chromium.LaunchAsync(new() { Headless = headless }),
                "firefox" => await playwright.Firefox.LaunchAsync(new() { Headless = headless }),
                "webkit" => await playwright.Webkit.LaunchAsync(new() { Headless = headless }),
                _ => throw new ArgumentException($"Unsupported browser: {browserName}")
            };
        }

        /// <summary>
        /// This method adds the days, weeks, month, years to the current date
        /// Month: M
        /// Day: d
        /// Year: y
        /// MM-dd-yyyy = "07-05-2025"
        /// M/d/yyyy = "7/5/2025"
        /// MMddyyyy = "07052025"
        /// </summary>
        /// <param name="noOfDays"></param>
        /// <param name="noOfWeeks"></param>
        /// <param name="noOfMonths"></param>
        /// <param name="noOfYears"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetDate(int noOfDays, int noOfWeeks, int noOfMonths, int noOfYears, string format)
        {
            double Weeks = noOfWeeks * 7;
            DateTime date = DateTime.Today.AddDays(noOfDays).AddDays(Weeks).AddMonths(noOfMonths).AddYears(noOfYears);

            return FormatDate(date, format);
        }

        /// <summary>
        /// This method will format the datetime
        /// M - Month
        /// d - day
        /// y - year
        /// ie; FormatDate(date, "MM/dd/yyyy") which returns 02/27/2025
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string FormatDate(DateTime date, string format)
        {
            return date.ToString(format, CultureInfo.InvariantCulture);
        }

        public static async Task SetValueAsync(this ILocator locator, IPage page, string value, int delayInMilliseconds = 1000)
        {
            var elementHandle = await locator.ElementHandleAsync();
            await page.EvaluateAsync($"el => el.setAttribute('value', '{value}')", elementHandle);

            if (delayInMilliseconds > 0)
            {
                await Task.Delay(delayInMilliseconds);
            }
        }

        public static async Task SendKeysToElementAsync(this ILocator locator, string value, int delayInMilliseconds = 1000)
        {
            locator.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await locator.ScrollIntoViewIfNeededAsync();
            await locator.FillAsync(value);

            if (delayInMilliseconds > 0)
            {
                await Task.Delay(delayInMilliseconds);
            }
        }

        public static async Task ClickElementAsync(this ILocator locator, int delayInMilliseconds = 1000)
        {
            locator.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await locator.ScrollIntoViewIfNeededAsync();
            await locator.ClickAsync(new() { Force = true });

            if (delayInMilliseconds > 0)
            {
                await Task.Delay(delayInMilliseconds);
            }
        }

        public static async Task<string> GetElementAttributeAsync(this ILocator locator, string attributeName, int delayInMilliseconds = 1000)
        {
            locator.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await locator.ScrollIntoViewIfNeededAsync();
            var value = await locator.GetAttributeAsync($"{attributeName}");

            if (delayInMilliseconds > 0)
            {
                await Task.Delay(delayInMilliseconds);
            }

            return value;
        }

        public static async Task<string> GetElementTextAsync(this ILocator locator, int delayInMilliseconds = 1000)
        {
            locator.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await locator.ScrollIntoViewIfNeededAsync();
            var value = await locator.InnerTextAsync();

            if (delayInMilliseconds > 0)
            {
                await Task.Delay(delayInMilliseconds);
            }

            return value;
        }
    }
}
