using System.Drawing;
using UIKit;

namespace Buform
{
    public static class ColorExtensions
    {
        public static UIColor ToColor(this Color color)
        {
            return UIColor.FromRGBA(color.R, color.G, color.B, color.A);
        }
    }
}