namespace NewsParser.Application.Parsing.Interfaces.Services;

public interface ICustomJsonSerializer<T> where T : class
{
    T? Deserialize(string json);
    
    string Serialize(T instance);
}