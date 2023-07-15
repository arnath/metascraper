using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Metascraper.Core;

public class Scraper : IAsyncDisposable
{
    private IPlaywright? playwright;

    private IBrowser? browser;

    private int disposed;

    public async Task InitializeAsync()
    {
        this.playwright = await Playwright.CreateAsync();
        this.browser = await this.playwright.Chromium.LaunchAsync();
    }

    public async Task ScrapeMetadataAsync(Uri url)
    {
        this.VerifyInitialized();

        IBrowserContext context = await this.browser!.NewContextAsync();
        try
        {
            IPage page = await context.NewPageAsync();
            await page.GotoAsync(url.ToString());
            string html = await page.ContentAsync();

            Console.WriteLine(html);
        }
        finally
        {
            await context.DisposeAsync();
        }
    }

    private void VerifyInitialized()
    {
        if (this.playwright == null || this.browser == null)
        {
            throw new InvalidOperationException("Must call InitializeAsync on the Metascraper instance before using it.");
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
                if (this.playwright != null)
                {
                    this.playwright.Dispose();
                    this.playwright = null;
                }

                if (this.browser != null)
                {
                    await this.browser.DisposeAsync();
                    this.browser = null;
                }
            }
        }
    }
}
