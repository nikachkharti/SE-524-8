namespace Lecture29.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MyRequiredAttribute : MyValidationAttribute
    {
        public override void Validate(object value, object instance, string propertyName)
        {
            if (value is null)
            {
                throw new ArgumentNullException($"{propertyName} is required");
            }
        }
    }
}
