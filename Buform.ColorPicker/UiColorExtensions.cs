using System.Drawing;
using UIKit;

namespace Buform
{
    public static class UiColorExtensions
    {
        public static Color ToColor(this UIColor color)
        {
            color.GetRGBA(out var red, out var green, out var blue, out var alpha);

            return Color.FromArgb((int)(alpha * 255), (int)(red * 255), (int)(green * 255), (int)(blue * 255));
        }
    }
}