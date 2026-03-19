namespace MiniBank.Repository.Attributes
{
    public class CustomEmailAttribute : BaseValidationAttribute
    {
        public override bool IsValid(object value, out string errorMessage)
        {
            if (value is string str)
            {
                if (!str.Contains('@') || !str.Contains('.'))
                {
                    errorMessage = "Invalid email format";
                    return false;
                }
            }

            errorMessage = null;
            return true;
        }
    }
}
