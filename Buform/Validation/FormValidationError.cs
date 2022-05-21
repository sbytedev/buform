namespace Buform
{
    public sealed class FormValidationError
    {
        public string TargetPropertyName { get; }

        public string ErrorMessage { get; }

        public FormValidationError(string targetPropertyName, string errorMessage)
        {
            TargetPropertyName = targetPropertyName;
            ErrorMessage = errorMessage;
        }
    }
}