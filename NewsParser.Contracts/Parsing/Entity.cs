using System.Runtime.Serialization;

namespace NewsParser.Contracts.Parsing;

[DataContract]
public record Entity
(
    [property: DataMember(Name = "posts")] IEnumerable<Post>? Posts
);