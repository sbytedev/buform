using Buform.Example.Core;
using MvvmCross.Forms.Core;

namespace Buform.Example.Forms
{
    public sealed class Application : MvxFormsApplication
    {
        public Application()
        {
            BuformForms.RegisterGroupHeaderClass<HeaderFormGroup, HeaderFormGroupView>();
        }
    }
}