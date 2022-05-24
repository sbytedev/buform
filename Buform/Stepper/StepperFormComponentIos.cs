using Foundation;

namespace Buform
{
    [Preserve(AllMembers = true)]
    [FormComponent]
    // ReSharper disable once UnusedType.Global
    public sealed class StepperFormComponent : IFormComponent
    {
        public void Register()
        {
            Buform.RegisterItemClass<StepperFormItem, StepperFormCell>();
        }
    }
}