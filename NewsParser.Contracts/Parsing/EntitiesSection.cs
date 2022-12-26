using System.Runtime.Serialization;

namespace NewsParser.Contracts.Parsing;

[DataContract]
public record EntitiesSection
(
    [property: DataMember(Name = "entities")] Entity? Entity
);
