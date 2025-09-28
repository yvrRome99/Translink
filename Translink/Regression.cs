using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Translink.Tests.Schedule;

namespace Translink
{
    [TestClass]
    public class Regression : PageTest
    {
        public static ExtentReports Report { get; set; }
        private static ExtentSparkReporter HtmlReporter { get; set; }
        public static ExtentTest Test { get; set; }
        public static ExtentTest Node { get; set; }

        private const string Schedule = @"TestData\Schedule.csv";

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            
        }

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            
        }

        [TestInitialize]
        public void TestInit()
        {
            Report = new ExtentReports();
            var htmlReporter = new ExtentSparkReporter(Path.Combine(TestContext.TestRunResultsDirectory, TestContext.ManagedMethod, "Results.html"));
            Report.AttachReporter(htmlReporter);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Report.Flush();
            TestContext.AddResultFile(Path.Combine(TestContext.TestRunResultsDirectory, TestContext.ManagedMethod, "Results.html"));
        }

        [TestMethod]
        [TestCategory("Schedule")]
        [DataRow("chromium")]
        public async Task SearchForSchedule(string browserName)
        {
            await Tests.Schedule.SearchForSchedule.RunTest(browserName, TestContext, Schedule);
        }
    }
}
