using UIKit;

namespace Buform
{
    public sealed class ColorPickerViewControllerDelegate : UIColorPickerViewControllerDelegate
    {
        private ColorPickerFormItem? _item;

        public ColorPickerViewControllerDelegate(ColorPickerFormItem item)
        {
            _item = item;
        }

        public override void DidSelectColor(UIColorPickerViewController viewController, UIColor color, bool continuously)
        {
            if (continuously)
            {
                return;
            }

            if (_item == null)
            {
                return;
            }

            _item.Value = color.ToColor();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _item = null;
            }

            base.Dispose(disposing);
        }
    }
}