namespace MiniBank.Repository.Attributes
{
    public class CustomToUpperAttribute : BaseValidationAttribute
    {
        public override bool IsValid(object value, out string errorMessage)
        {
            errorMessage = null;
            return true;
        }

        public string Transform(string value) => value?.ToUpper();
    }
}
