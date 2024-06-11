using System.Linq.Expressions;
using System.Reflection;

namespace Enterprise.Reporting.Utils;

public class PropertyAccessor<T>
{
    private static readonly PropertyInfo[] Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToArray();

    private static Func<T, string, object> compiledPropertyAccessor;

    public static readonly PropertyAccessor<T> Instance = new PropertyAccessor<T>();

    public object Get(T obj, string key) => compiledPropertyAccessor(obj, key);

    private PropertyAccessor()
    {
        compiledPropertyAccessor = CreatePropertyAccessor();
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
            typeof(PropertyAccessor<T>).GetMethod(nameof(Concatenate))!, 
            Expression.NewArrayInit(typeof(object), propertyAccessExpressions));

        var lambda = Expression.Lambda<Func<T, string, object>>(concatenated, inputObject, propertyName);

        return lambda.Compile();
    }

    public static object? Concatenate(IEnumerable<object> input)
    {
        if (input == null)
            return null;

        return input.FirstOrDefault(x => x is not null)?? null;
    }
}
