using Foundation;

namespace Buform
{
    [Preserve(AllMembers = true)]
    [FormComponent]
    // ReSharper disable once UnusedType.Global
    public sealed class PickerFormComponent : IFormComponent
    {
        public void Register()
        {
            Buform.RegisterItemClass<IAsyncPickerFormItem, AsyncPickerFormCell>();
            Buform.RegisterItemClass<ICallbackPickerFormItem, CallbackPickerFormCell>();
            Buform.RegisterItemClass<IMultiValuePickerFormItem, MultiValuePickerFormCell>();
            Buform.RegisterItemClass<IPickerFormItem, PickerFormCell>();
        }

        public static class Texts
        {
            public static string Clear = "Clear";
            public static string Cancel = "Cancel";
        }
    }
}