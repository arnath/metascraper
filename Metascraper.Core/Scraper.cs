using HtmlAgilityPack;
using Microsoft.Playwright;

namespace Metascraper.Core;

public class Scraper : IAsyncDisposable
{
    private IPlaywright playwright;

    private IBrowser browser;

    private int disposed = 0;

    private Scraper(IPlaywright playwright, IBrowser browser)
    {
        this.playwright = playwright;
        this.browser = browser;
    }

    public static async Task<Scraper> CreateNewAsync()
    {
        IPlaywright playwright = await Playwright.CreateAsync();
        IBrowser browser = await playwright.Chromium.LaunchAsync();
        Scraper scraper = new Scraper(playwright, browser);

        return scraper;
    }

    public async Task<string> ScrapeHtmlAsync(Uri url)
    {
        IBrowserContext context = await this.browser!.NewContextAsync();
        try
        {
            IPage page = await context.NewPageAsync();
            await page.GotoAsync(url.ToString(), new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded });
            string html = await page.ContentAsync();

            return html;
        }
        finally
        {
            await context.DisposeAsync();
        }
    }

    public async ValueTask DisposeAsync()
    {
        await this.DisposeAsyncCore(true);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore(bool disposing)
    {
        if (Interlocked.Exchange(ref this.disposed, 1) == 0)
        {
            if (disposing)
            {
                if (this.browser != null)
                {
                    await this.browser.DisposeAsync();
                    this.browser = null;
                }

                if (this.playwright != null)
                {
                    this.playwright.Dispose();
                    this.playwright = null;
                }
            }
        }
    }
}
