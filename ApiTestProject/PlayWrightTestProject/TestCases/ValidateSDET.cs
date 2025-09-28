using FluentAssertions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PlayWrightTestProject.Google;
using PlayWrightTestProject.Pages;
using PlayWrightTestProject.PlaywrightWrapper;

namespace PlayWrightTestProject.TestCases
{
    public class Tests
    {
        public Driver Driver;
        [SetUp]
        public void Setup()
        {
            Driver = new Driver();
        }

        [TearDown]
        public void Teardown()
        {
            Driver.Dispose();
        }

        [Test]
        public async Task VerifySDETOpening()
        {
            var googleSearchPage = new SearchPage(Driver.Page);
            await googleSearchPage.GoToAsync();
            await googleSearchPage.SearchForAsync("Prometheus Group");
            var actualFirstLink = await googleSearchPage.GetFirstResultHrefAsync();
            actualFirstLink.Should().Be("https://www.prometheusgroup.com/");
            await googleSearchPage.ClickLinkAsync("Career", "https://www.prometheusgroup.com/company/careers");
            var prometheusCareerPage = new PrometheusCareerPage(Driver.Page);
            
            var countMenuItem = await prometheusCareerPage.MenuBarItems.CountAsync();
            countMenuItem.Should().BeGreaterThan(4, because: "there should be atleast 4 menu item with accordians");
            
            var success = 0;
            for (int i = 0; i < countMenuItem && success < 4; i++)
            {
                var trigger = prometheusCareerPage.MenuBarItems.Nth(i);
                var hovering = await prometheusCareerPage.HoverOverMenuItem(trigger);
                if (!hovering)
                    continue; // menu item is most likely hidden
                var submenu = trigger.Locator(".dropdown-container");
                var opened = await prometheusCareerPage.IsOpenAsync(trigger, submenu);
                if (opened)
                {
                    var subMenuText = await submenu.InnerTextAsync();
                    subMenuText.Should().NotBeEmpty();
                    success++;
                }
            }
            success.Should().Be(4);

            var SeniorSDETLink = prometheusCareerPage.SeniorSDET;
            var countSeniorSDETLinks = await SeniorSDETLink.CountAsync();
            countSeniorSDETLinks.Should().BeGreaterThan(0, because: "the career link should exist on the page");

        }
    }
}
