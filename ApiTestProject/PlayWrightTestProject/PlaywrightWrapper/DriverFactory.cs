using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayWrightTestProject.PlaywrightWrapper
{
    public static class DriverFactory
    {
        // Default launch options; override via parameters if needed
        public static BrowserTypeLaunchOptions DefaultLaunchOptions => new()
        {
            Headless = false,
            SlowMo = 0
        };

        public static BrowserNewContextOptions DefaultContextOptions => new()
        {
            ViewportSize = new ViewportSize { Width = 1280, Height = 800 },
            Locale = "en-US",
            // add additional context defaults here (userAgent, timezone, etc.)
        };

        public static Task<Driver> CreateAsync(BrowserKind kind) =>
            CreateAsync(kind, DefaultLaunchOptions, DefaultContextOptions);

        public static Task<Driver> CreateAsync(BrowserKind kind, BrowserTypeLaunchOptions launchOptions, BrowserNewContextOptions? contextOptions = null) =>
            Driver.CreateAsync(kind, launchOptions, contextOptions);
    }

}
