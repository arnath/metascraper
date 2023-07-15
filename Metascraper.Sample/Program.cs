using Metascraper.Core;

namespace Metascraper.Sample;

public class Program
{
    public static async Task Main()
    {
        await using Scraper scraper = await Scraper.CreateNewAsync();
        string html = await scraper.ScrapeMetadataAsync(new Uri("https://www.google.com"));
        Console.WriteLine(html);
    }
}
