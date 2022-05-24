using Foundation;

namespace Buform
{
    [Preserve(AllMembers = true)]
    [FormComponent]
    // ReSharper disable once UnusedType.Global
    public sealed class DateTimeFormComponent : IFormComponent
    {
        public void Register()
        {
            Buform.RegisterItemClass<DateTimeFormItem, DateTimeFormCell>();
        }
    }
}