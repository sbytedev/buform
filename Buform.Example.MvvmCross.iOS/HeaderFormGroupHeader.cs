using System;
using Buform.Example.MvvmCross.Core;
using Foundation;

namespace Buform.Example.MvvmCross.iOS
{
    [Preserve(AllMembers = true)]
    [Register(nameof(HeaderFormGroupHeader))]
    public sealed class HeaderFormGroupHeader : FormHeaderFooter<HeaderFormGroup>
    {
        public HeaderFormGroupHeader()
        {
            /* Required constructor */
        }

        public HeaderFormGroupHeader(IntPtr handle) : base(handle)
        {
            /* Required constructor */
        }
    }
}
