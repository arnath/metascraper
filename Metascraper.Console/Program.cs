using Metascraper.Core;

namespace Metascraper.Console;

public class Program
{
    public static async Task Main()
    {
        await using Scraper scraper = new Scraper();

        await scraper.InitializeAsync();
        Thread.Sleep(5000);
    }
}
