using Foundation;

namespace Buform
{
    [Preserve(AllMembers = true)]
    [FormComponent]
    // ReSharper disable once UnusedType.Global
    public sealed class ButtonFormComponent : IFormComponent
    {
        public void Register()
        {
            Buform.RegisterItemClass<ButtonFormItem, ButtonFormCell>();
        }
    }
}