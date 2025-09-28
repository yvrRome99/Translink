using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Translink.Tests.Schedule;

namespace Translink
{
    [TestClass]
    public class Regression : PageTest
    {
        public static ExtentReports Report { get; set; }
        public static ExtentTest Test { get; set; }
        public static ExtentTest Node { get; set; }

        private const string Schedule = @"TestData\Schedule.csv";

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // This method is called once for the test assembly, before any tests are run.
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            // This method is called once for the test assembly, after all tests are run.
        }

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            // This method is called once for the test class, after all tests of the class are run.
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
            //Page.GotoAsync(TestContext.Properties["SiteURL"].ToString());
            await Tests.Schedule.SearchForSchedule.RunTest(browserName, TestContext, Schedule);
        }
    }
}
