using Microsoft.Playwright;

namespace PlayWrightTestProject.PlaywrightWrapper
{
    public class Driver : IAsyncDisposable
    {
        private readonly IPlaywright _playwright;
        private readonly IBrowser _browser;
        private readonly IBrowserContext _context;
        private readonly IPage _page;

        // Expose page and optionally context/browser for advanced uses
        public IPage Page => _page;
        public IBrowserContext Context => _context;
        public IBrowser Browser => _browser;

        private Driver(IPlaywright playwright, IBrowser browser, IBrowserContext context, IPage page)
        {
            _playwright = playwright;
            _browser = browser;
            _context = context;
            _page = page;
        }


        // Factory method used by DriverFactory
        internal static async Task<Driver> CreateAsync(BrowserKind kind, BrowserTypeLaunchOptions launchOptions, BrowserNewContextOptions? contextOptions = null)
        {
            var playwright = await Playwright.CreateAsync();

            IBrowser browser = kind switch
            {
                BrowserKind.Firefox => await playwright.Firefox.LaunchAsync(launchOptions),
                BrowserKind.WebKit => await playwright.Webkit.LaunchAsync(launchOptions),
                _ => await playwright.Chromium.LaunchAsync(launchOptions)
            };

            var context = await browser.NewContextAsync(contextOptions ?? new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize { Width = 1280, Height = 800 }
            });

            var page = await context.NewPageAsync();
            return new Driver(playwright, browser, context, page);
        }

        public async ValueTask DisposeAsync()
        {
            // Dispose in correct order: page/context/browser/playwright
            try
            {
                await _context.CloseAsync();
            }
            catch { /* ignore */ }

            try
            {
                await _browser.CloseAsync();
            }
            catch { /* ignore */ }

            _playwright.Dispose();
        }

    }
}
