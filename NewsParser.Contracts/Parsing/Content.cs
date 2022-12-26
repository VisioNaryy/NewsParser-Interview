using System.Runtime.Serialization;

namespace NewsParser.Contracts.Parsing;

[DataContract]
public record Content
{
    [DataMember(Name = "rendered")] 
    public string? Rendered { get; set; }
}