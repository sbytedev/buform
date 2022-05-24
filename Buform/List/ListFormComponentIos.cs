using Foundation;

namespace Buform
{
    [Preserve(AllMembers = true)]
    [FormComponent]
    // ReSharper disable once UnusedType.Global
    public sealed class ListFormComponent : IFormComponent
    {
        public void Register()
        {
            Buform.RegisterGroupHeaderClass<IListFormGroup, ListFormGroupHeader>();
            Buform.RegisterGroupFooterClass<IListFormGroup, ListFormGroupFooter>();

            Buform.RegisterItemClass<IListFormItem, ListFormCell>();
        }
    }
}