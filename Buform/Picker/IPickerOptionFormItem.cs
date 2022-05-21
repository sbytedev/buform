namespace Buform
{
    public interface IPickerOptionFormItem : IFormItem
    {
        string? FormattedValue { get; }
    }
}