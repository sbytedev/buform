using MvvmCross.Forms.Presenters.Attributes;
using Xamarin.Forms.Xaml;

namespace Buform.Example.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxContentPagePresentation(WrapInNavigationPage = true)]
    public partial class MenuPage
    {
        public MenuPage()
        {
            InitializeComponent();
        }
    }
}