using Microsoft.Playwright;

namespace PlayWrightTestProject.Google
{
    public class SearchPage
    {
        private readonly IPage _page;
        public SearchPage(IPage page) => _page = page;

        // Locators
        public ILocator SearchInput => _page.Locator("textarea[name=\"q\"]");
        public ILocator FirstResultLink => _page.Locator("div#search a").First;
        
        // Navigation and helpers
        public async Task GoToAsync()
        {
            await _page.GotoAsync("https://www.google.com");
            await SearchInput.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5000 });
        }

        public async Task SearchForAsync(string term)
        {
            await SearchInput.FillAsync(term);
            await _page.Keyboard.PressAsync("Enter");
            await WaitForResultsAsync();
        }

        public async Task WaitForResultsAsync()
        {
            await _page.Locator("#search").WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 10000 });
            await FirstResultLink.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 10000 });
        }

        public async Task ClickLinkAsync(string linkText, string url)
        {
            await Task.WhenAll(
                _page.ClickAsync($"a:has-text(\"{linkText}\")"),
                _page.WaitForURLAsync(url, new PageWaitForURLOptions { WaitUntil = WaitUntilState.Load, Timeout = 15000 })
            );
        }

        public async Task<string?> GetFirstResultHrefAsync()
        {
            return await FirstResultLink.GetAttributeAsync("href");
        }
    }
}

