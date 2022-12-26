namespace NewsParser.Infrastructure.Extensions;

public static class DataReaderExtensions
{
    public static T GetData<T>(this IDataReader dr, string name, T returnValue = default)
    {
        var value = dr[name];
        if (!value.Equals(DBNull.Value))
        {
            returnValue = (T) value;
        }

        return returnValue;
    }
}