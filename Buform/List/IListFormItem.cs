namespace Buform
{
    public interface IListFormItem : IFormItem
    {
        string? FormattedValue { get; }
    }
}