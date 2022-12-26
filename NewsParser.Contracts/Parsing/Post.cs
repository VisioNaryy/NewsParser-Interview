using System.Runtime.Serialization;

namespace NewsParser.Contracts.Parsing;

[DataContract]
public record Post
{
    [DataMember(Name = "link")] public string? Link { get; set; }
    [DataMember(Name = "title")] public Title? Title { get; set; }
    [DataMember(Name = "content")] public Content? Content { get; set; }
    [DataMember(Name = "date_gmt")] public string? Date { get; set; }
}