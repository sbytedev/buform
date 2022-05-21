namespace Buform
{
    public interface IListFormGroup : IFormGroup
    {
        string? HeaderLabel { get; }
        string? FooterLabel { get; }
    }
}