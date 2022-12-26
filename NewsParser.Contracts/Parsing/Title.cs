using System.Runtime.Serialization;

namespace NewsParser.Contracts.Parsing;

[DataContract]
public record Title
{
    [DataMember(Name = "rendered")] 
    public string? Rendered { get; set; }
}