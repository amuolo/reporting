using System.Linq.Expressions;

namespace Enterprise.Reporting.Utils;

public class DimensionExtractor<T>
{
    private static readonly System.Reflection.PropertyInfo[] Properties = typeof(T).GetProperties().ToArray();

    private static Func<string, string> compiledExtractingFunction;

    public static readonly DimensionExtractor<T> Instance = new DimensionExtractor<T>();

    public string Get(string key) => compiledExtractingFunction(key);

    private DimensionExtractor()
    {
        compiledExtractingFunction = GetExtractingFunction();
    }

    private static Func<string, string> GetExtractingFunction()
    {
        // TODO
        return a => a;
    }
}
