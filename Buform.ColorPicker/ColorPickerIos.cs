namespace Buform
{
    [Component]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ColorPicker : IFormComponent
    {
        public void Register()
        {
            Buform.RegisterClass<ColorPickerFormItem, ColorPickerFormCell>();
        }
    }
}