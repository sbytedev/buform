namespace Buform
{
    [Component]
    // ReSharper disable once UnusedType.Global
    public class ColorPicker : IFormComponent
    {
        public void Register()
        {
            Buform.RegisterClass<ColorPickerFormItem, ColorPickerFormCell>();
        }
    }
}