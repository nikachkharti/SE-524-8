namespace Lecture29.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MyStringLengthAttribute : MyValidationAttribute
    {
        private int stringLength;

        public MyStringLengthAttribute(int length)
        {
            stringLength = length;
        }

        public override void Validate(object value, object instance, string propertyName)
        {
            if (propertyName.Length != stringLength)
            {
                throw new ArgumentException();
            }

        }
    }
}
