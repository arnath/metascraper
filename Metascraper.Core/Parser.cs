using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace Metascraper.Core;

public partial class Parser
{
    public void ParseMetadata(string html, params MetaProperties[] properties)
    {
        if (html == null)
        {
            throw new ArgumentException("HTML string must be non-empty.", nameof(html));
        }

        if (properties == null || properties.Length == 0)
        {
            throw new ArgumentException("At least one property to parse must be specified.", nameof(properties));
        }

        HtmlDocument hd = new HtmlDocument();
        hd.LoadHtml(html);
        HtmlNode root = hd.DocumentNode;

        foreach (MetaProperties property in properties)
        {
            
        }
    }

    

    private string? TryParseValue(HtmlNode root, string querySelector, Func<HtmlNode, string> extractContent)
    {
        HtmlNode? node = root.QuerySelectorAll(querySelector).FirstOrDefault();
        if (node == null)
        {
            return null;
        }

        return extractContent.Invoke(node);
    }
}