namespace Buform
{
    public interface IMultilineTextFormItem : IValidatableFormItem
    {
        string? Placeholder { get; }

        TextInputType InputType { get; }

        string? FormattedValue { get; }

        void SetValue(string? value);
    }
}