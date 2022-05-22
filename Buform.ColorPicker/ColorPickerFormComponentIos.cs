namespace Buform
{
    [FormComponent]
    // ReSharper disable once UnusedType.Global
    public class ColorPickerFormComponent : IFormComponent
    {
        public void Register()
        {
            Buform.RegisterClass<ColorPickerFormItem, ColorPickerFormCell>();
        }
    }
}