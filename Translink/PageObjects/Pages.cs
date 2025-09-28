using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink.PageObjects
{
    public static class Pages
    {
        private static readonly AsyncLocal<Dictionary<Type, object>> _pageCache = new();
        private static IPage _currentPage;
        private static TestContext _currentTestContext;

        public static void Init(IPage page, TestContext testContext)
        {
            _currentPage = page;
            _currentTestContext = testContext;
            Clear();
        }

        private static Dictionary<Type, object> Cache => _pageCache.Value ??= new Dictionary<Type, object>();

        private static T Get<T>(bool refresh = false) where T : class
        {
            var type = typeof(T);

            if (refresh || !Cache.TryGetValue(type, out var instance))
            {
                instance = Activator.CreateInstance(type, _currentPage, _currentTestContext)!;
                Cache[type] = instance;
            }

            return (T)instance;
        }

        public static void Clear() => Cache.Clear();

        // Page object shortcuts
        public static WelcomeToTransLinkPage WelcomeToTransLink(bool refresh = false) => Get<WelcomeToTransLinkPage>(refresh);
        public static BusSchedulesPage BusSchedule(bool refresh = false) => Get<BusSchedulesPage>(refresh);
        public static RoutePage Route(bool refresh = false) => Get<RoutePage>(refresh);
        public static StopPage Stop(bool refresh = false) => Get<StopPage>(refresh);
        public static MyFavouritesPage MyFavourite(bool refresh = false) => Get<MyFavouritesPage>(refresh);
    }
}
