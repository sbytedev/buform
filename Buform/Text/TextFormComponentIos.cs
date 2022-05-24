using Foundation;

namespace Buform
{
    [Preserve(AllMembers = true)]
    [FormComponent]
    // ReSharper disable once UnusedType.Global
    public sealed class TextFormComponent : IFormComponent
    {
        public void Register()
        {
            Buform.RegisterGroupHeaderClass<TextFormGroup, TextFormGroupHeader>();
            Buform.RegisterGroupFooterClass<TextFormGroup, TextFormGroupFooter>();

            Buform.RegisterItemClass<IMultilineTextFormItem, MultilineTextFormCell>();
            Buform.RegisterItemClass<ITextFormItem, TextFormCell>();
        }
    }
}