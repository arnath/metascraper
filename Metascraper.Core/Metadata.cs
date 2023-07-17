namespace Metascraper.Core;

public class Metadata
{
    public Metadata(IDictionary<MetaProperties, string?> fields)
    {
        foreach (var kvp in fields)
        {
            if (kvp.Value == null)
            {
                continue;
            }

            switch (kvp.Key)
            {
                case MetaProperties.Title:
                    this.Title = kvp.Value;
                    break;

                case MetaProperties.Description:
                    this.Description = kvp.Value;
                    break;

                case MetaProperties.Image:
                    this.Image = kvp.Value;
                    break;

                case MetaProperties.Url:
                    this.Url = kvp.Value;
                    break;

                default:
                    throw new NotImplementedException($"The specified meta property {kvp.Key.ToString()} is not yet implemented.");
            }
        }
    }

    public string? Title { get; }

    public string? Description { get; }

    public string? Image { get; }

    public string? Url { get; }
}