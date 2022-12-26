using System.Runtime.Serialization.Json;
using System.Text;

namespace NewsParser.Infrastructure.Parsing.Services;

public class CustomJsonSerializer<T> : ICustomJsonSerializer<T> where T : class
{
    public T? Deserialize(string json)
    {
        using (var stream = new MemoryStream(Encoding.Default.GetBytes(json)))
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            return serializer.ReadObject(stream) as T;
        }
    }
    
    public string Serialize(T instance)
    {
        var serializer = new DataContractJsonSerializer(typeof(T));

        using (var stream = new MemoryStream())
        {
            serializer.WriteObject(stream, instance);
            return Encoding.Default.GetString(stream.ToArray());
        }
    }
}