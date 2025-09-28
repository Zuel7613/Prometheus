using Microsoft.Playwright;

namespace PlayWrightTestProject.Pages
{
    public class PrometheusCareerPage
    {
        private readonly IPage _page;
        public PrometheusCareerPage(IPage page) => _page = page;

        // Locators
        public ILocator MenuBarItems => _page.Locator(".menu-item");
        public ILocator SeniorSDET => _page.Locator("a:has-text(\"Senior Software Developer in Test\")");

        // Navigation and helpers
        public async Task<bool> HoverOverMenuItem(ILocator menuItem)
        {
            try
            {
                await menuItem.HoverAsync(new LocatorHoverOptions { Timeout = 5000 });
                await _page.WaitForTimeoutAsync(250);
            }
            catch (TimeoutException)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsOpenAsync(ILocator trigger, ILocator submenu)
        {
            // 1. aria-expanded on trigger
            var aria = await trigger.GetAttributeAsync("aria-expanded");
            if (!string.IsNullOrEmpty(aria) && (aria == "true" || aria == "1")) return true;

            // 2. visible submenu
            if (await submenu.CountAsync() > 0 && await submenu.IsVisibleAsync()) return true;

            // 3. open class on trigger or parent
            var cls = await trigger.GetAttributeAsync("class") ?? "";
            if (cls.Contains("open") || cls.Contains("is-open") || cls.Contains("active") || cls.Contains("is-active")) return true;

            return false;
        }

    }
}
