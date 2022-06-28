using Buform.Example.Core;
using Foundation;

namespace Buform.Example.MvvmCross.iOS
{
    [Preserve(AllMembers = true)]
    [FormComponent]
    // ReSharper disable once UnusedType.Global
    public sealed class DigitGenerationComponent : IFormComponent
    {
        public void Register()
        {
            Buform.RegisterItemClass<DigitGenerationItem, DigitGenerationCell>();
        }
    }
}