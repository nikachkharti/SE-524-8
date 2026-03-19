using MiniBank.Repository.Attributes;
using System.Reflection;

namespace MiniBank.Repository.Validators
{
    public static class Validator
    {
        public static List<string> Validate(object obj)
        {
            var errors = new List<string>();

            var properties = obj.GetType().GetProperties();

            foreach (var prop in properties)
            {
                var value = prop.GetValue(obj);

                var attributes = prop.GetCustomAttributes<BaseValidationAttribute>();

                foreach (var attr in attributes)
                {
                    if (!attr.IsValid(value, out string error))
                    {
                        errors.Add($"{prop.Name}: {error}");
                    }

                    // Transformation (მაგ: ToUpper)
                    if (attr is CustomToUpperAttribute upper && value is string str)
                    {
                        var newValue = upper.Transform(str);
                        prop.SetValue(obj, newValue);
                    }
                }
            }

            return errors;
        }
    }
}
