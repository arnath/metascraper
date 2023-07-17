using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace Metascraper.Core;

public partial class Parser
{
    private static readonly ContentExtractor[] TitleExtractors = new[]
    {
        new ContentExtractor("meta[property=\"og:title\"]", (node) => node.GetAttributeValue("content", null)),
        new ContentExtractor("meta[name=\"twitter:title\"]", (node) => node.GetAttributeValue("content", null)),
        new ContentExtractor("meta[property=\"twitter:title\"]", (node) => node.GetAttributeValue("content", null)),
        new ContentExtractor("title", (node) => node.GetDirectInnerText()),
        new ContentExtractor(".post-title", (node) => node.GetDirectInnerText()),
        new ContentExtractor(".entry-title", (node) => node.GetDirectInnerText()),
        new ContentExtractor("h1[class*=\"title\" i] a", (node) => node.GetDirectInnerText()),
        new ContentExtractor("h1[class*=\"title\" i]", (node) => node.GetDirectInnerText()),
    };

    private static readonly ContentExtractor[] DescriptionExtractors = new[]
    {
        new ContentExtractor("meta[property=\"og:description\"]", (node) => node.GetAttributeValue("content", null)),
        new ContentExtractor("meta[name=\"twitter:description\"]", (node) => node.GetAttributeValue("content", null)),
        new ContentExtractor("meta[property=\"twitter:description\"]", (node) => node.GetAttributeValue("content", null)),
        new ContentExtractor("meta[name=\"description\"]", (node) => node.GetAttributeValue("content", null)),
        new ContentExtractor("meta[itemprop=\"description\"]", (node) => node.GetAttributeValue("content", null)),
    };

    private static readonly ContentExtractor[] ImageExtractors = new[]
    {
        new ContentExtractor("meta[property=\"og:image:secure_url\"]", (node) => node.GetAttributeValue("content", null)),
        new ContentExtractor("meta[property=\"og:image:url\"]", (node) => node.GetAttributeValue("content", null)),
        new ContentExtractor("meta[property=\"og:image\"]", (node) => node.GetAttributeValue("content", null)),
        new ContentExtractor("meta[name=\"twitter:image:src\"]", (node) => node.GetAttributeValue("content", null)),
        new ContentExtractor("meta[property=\"twitter:image:src\"]", (node) => node.GetAttributeValue("content", null)),
        new ContentExtractor("meta[name=\"twitter:image\"]", (node) => node.GetAttributeValue("content", null)),
        new ContentExtractor("meta[property=\"twitter:image\"]", (node) => node.GetAttributeValue("content", null)),
    };

    private static readonly ContentExtractor[] UrlExtractors = new[]
    {
        new ContentExtractor("meta[property=\"og:url\"]", (node) => node.GetAttributeValue("content", null)),
        new ContentExtractor("meta[name=\"twitter:url\"]", (node) => node.GetAttributeValue("content", null)),
        new ContentExtractor("meta[property=\"twitter:url\"]", (node) => node.GetAttributeValue("content", null)),
        new ContentExtractor("link[rel=\"canonical\"]", (node) => node.GetAttributeValue("href", null)),
        new ContentExtractor("link[rel=\"alternate\"][hreflang=\"x-default\"]", (node) => node.GetAttributeValue("href", null)),
    };

    private string? TryParseContent(HtmlNode root, MetaProperties property)
    {
        ContentExtractor[] extractors;
        switch (property)
        {
            case MetaProperties.Title:
                extractors = TitleExtractors;
                break;

            case MetaProperties.Description:
                extractors = DescriptionExtractors;
                break;

            case MetaProperties.Image:
                extractors = ImageExtractors;
                break;

            case MetaProperties.Url:
                extractors = UrlExtractors;
                break;

            default:
                throw new NotImplementedException($"The specified meta property {property.ToString()} is not yet implemented.");
        }

        foreach (ContentExtractor extractor in extractors)
        {
            HtmlNode? node = root.QuerySelectorAll(extractor.Selector).FirstOrDefault();
            if (node == null)
            {
                continue;
            }

            string result = extractor.Extractor.Invoke(node);
            if (!string.IsNullOrEmpty(result))
            {
                return result;
            }
        }

        return null;
    }

    private struct ContentExtractor
    {
        public ContentExtractor(string selector, Func<HtmlNode, string> extractor)
        {
            this.Selector = selector;
            this.Extractor = extractor;
        }
        
        public string Selector { get; set; }

        public Func<HtmlNode, string> Extractor { get; set; }
    }
}