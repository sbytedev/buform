namespace Buform
{
    public interface IMultilineTextFormItem : IValidatableFormItem
    {
        TextInputType InputType { get; }

        string? FormattedValue { get; }

        void SetValue(string? value);
    }
}