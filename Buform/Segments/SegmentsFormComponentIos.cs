using Foundation;

namespace Buform
{
    [Preserve(AllMembers = true)]
    [FormComponent]
    // ReSharper disable once UnusedType.Global
    public sealed class SegmentsFormComponent : IFormComponent
    {
        public void Register()
        {
            Buform.RegisterItemClass<ISegmentsFormItem, SegmentsFormCell>();
        }
    }
}