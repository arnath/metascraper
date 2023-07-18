using Metascraper.Core;

namespace Metascraper.Sample;

public class Program
{
    public static async Task Main()
    {
        await using Scraper scraper = await Scraper.CreateNewAsync();
        string html = await scraper.ScrapeHtmlAsync(new Uri("https://www.washingtonpost.com/national-security/2023/07/13/zelensky-ukraine-nato-invitation/"));
        Metadata metadata = Parser.ParseMetadata(html, MetaProperties.Title, MetaProperties.Description, MetaProperties.Image, MetaProperties.Url);
        Console.WriteLine(metadata);
    }
}
