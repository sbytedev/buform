using Foundation;

namespace Buform
{
    [Preserve(AllMembers = true)]
    [FormComponent]
    // ReSharper disable once UnusedType.Global
    public sealed class SwitchFormComponent : IFormComponent
    {
        public void Register()
        {
            Buform.RegisterItemClass<SwitchFormItem, SwitchFormCell>();
        }
    }
}