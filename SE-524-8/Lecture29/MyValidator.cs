using Lecture29.Attributes;
using System.Reflection;

namespace Lecture29
{
    public static class MyValidator
    {
        public static void Validate(object saxeli)
        {
            Type saxelitype = saxeli.GetType();
            var allProps = saxelitype.GetProperties();
            foreach (var prop in allProps)
            {
                var value = prop.GetValue(saxeli);
                var validationAttributes = prop.GetCustomAttributes<MyValidationAttribute>();
                foreach (var a in validationAttributes) a.Validate(value, saxeli, prop.Name);
            }
        }
    }
}
