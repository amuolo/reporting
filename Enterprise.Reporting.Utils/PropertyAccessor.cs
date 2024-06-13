using System.Linq.Expressions;
using System.Reflection;

namespace Enterprise.Reporting.Utils;

public class PropertyAccessor<T>
{
    private static readonly PropertyInfo[] Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToArray();

    private static readonly PropertyInfo[] StringProperties = Properties.Where(prop => prop.PropertyType == typeof(string)).ToArray();

    private static Func<T, string, object> compiledPropertyAccessor;

    private static Func<T, string> compiledStringAccessor;

    public static readonly PropertyAccessor<T> Instance = new PropertyAccessor<T>();

    public static string[] StringPropertyNames { get; } = StringProperties.Select(prop => prop.Name).ToArray();

    public object Get(T obj, string propertyName) => compiledPropertyAccessor(obj, propertyName);

    public string GetStringProperties(T obj) => compiledStringAccessor(obj);

    private PropertyAccessor()
    {
        compiledPropertyAccessor = CreatePropertyAccessor();
        compiledStringAccessor = CreateStringAccessor();
    }

    public static Func<T, string, object> CreatePropertyAccessor()
    {
        var inputObject = Expression.Parameter(typeof(T));
        var propertyName = Expression.Parameter(typeof(string));

        var propertyAccessExpressions = Properties.Select(prop => Expression.Condition(
            Expression.Equal(propertyName, Expression.Constant(prop.Name)),
            Expression.Convert(Expression.Property(inputObject, prop), typeof(object)),
            Expression.Constant(null, typeof(object))
        ));

        var concatenated = Expression.Call(
            typeof(PropertyAccessor<T>).GetMethod(nameof(TakeFirst))!, 
            Expression.NewArrayInit(typeof(object), propertyAccessExpressions));

        var lambda = Expression.Lambda<Func<T, string, object>>(concatenated, inputObject, propertyName);

        return lambda.Compile();
    }

    public static object? TakeFirst(IEnumerable<object> input)
    {
        if (input == null)
            return null;

        return input.FirstOrDefault(x => x is not null)?? null;
    }

    public static Func<T, string> CreateStringAccessor()
    {
        var inputObject = Expression.Parameter(typeof(T));

        var propertyAccessExpressions = StringProperties.Select(prop => 
            Expression.Convert(Expression.Property(inputObject, prop), typeof(string)) );

        var concatenated = Expression.Call(
            typeof(string).GetMethod(nameof(string.Join), [typeof(string), typeof(string[])])!,
            Expression.Constant("_", typeof(string)),
            Expression.NewArrayInit(typeof(string), propertyAccessExpressions));

        var lambda = Expression.Lambda<Func<T, string>>(concatenated, inputObject);

        return lambda.Compile();
    }
}
