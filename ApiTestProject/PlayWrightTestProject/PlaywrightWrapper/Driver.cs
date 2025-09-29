using Microsoft.Playwright;

namespace PlayWrightTestProject.PlaywrightWrapper
{
    public class Driver : IDisposable
    {
        private readonly Task<IPage> _page;
        private IBrowser? _browser;
        public Driver()
        {
            _page = InitializePlaywright();
        }

        public IPage Page => _page.Result;

        private async Task<IPage> InitializePlaywright()
        {
            IPlaywright playwright = await Playwright.CreateAsync();

            _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                SlowMo = 800,
                Headless = false
            });

            return await _browser.NewPageAsync();
        }

        public void Dispose()
        {
            _browser?.CloseAsync();
        }
    }
}
