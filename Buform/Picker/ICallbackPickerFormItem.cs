namespace Buform
{
    public interface ICallbackPickerFormItem : IValidatableFormItem
    {
        string? Label { get; }

        string? FormattedValue { get; }

        void ExecuteCallback();
    }
}