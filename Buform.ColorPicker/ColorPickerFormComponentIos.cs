namespace Buform
{
    [FormComponent]
    // ReSharper disable once UnusedType.Global
    public class ColorPickerFormComponent : IFormComponent
    {
        public void Register()
        {
            Buform.RegisterItemClass<ColorPickerFormItem, ColorPickerFormCell>();
        }
    }
}