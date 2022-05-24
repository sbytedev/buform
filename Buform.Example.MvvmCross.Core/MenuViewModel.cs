using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Buform.Example.MvvmCross.Core
{
    public sealed class MenuViewModel : MvxNavigationViewModel
    {
        public string Title { get; }

        public Form Form { get; }

        public MenuViewModel(ILoggerFactory logFactory, IMvxNavigationService navigationService)
            : base(logFactory, navigationService)
        {
            Title = "Menu";

            Form = new Form(this)
            {
                new HeaderFormGroup(),
                new TextFormGroup("Gallery")
                {
                    new ButtonFormItem(new MvxAsyncCommand(ShowControlsAsync))
                    {
                        Label = "Show All Components",
                        InputType = ButtonInputType.Done
                    }
                },
                new TextFormGroup("Examples", "Contains some real-life examples.")
                {
                    new ButtonFormItem(new MvxAsyncCommand(CreateConnectionAsync))
                    {
                        Label = "Setup New Connection",
                        InputType = ButtonInputType.Done
                    },
                    new ButtonFormItem(new MvxAsyncCommand(CreateEventAsync))
                    {
                        Label = "Create New Event",
                        InputType = ButtonInputType.Done
                    }
                }
            };
        }

        private Task ShowControlsAsync(CancellationToken cancellationToken)
        {
            return NavigationService.Navigate<ComponentsViewModel>(cancellationToken: cancellationToken);
        }

        private Task CreateConnectionAsync(CancellationToken cancellationToken)
        {
            return NavigationService.Navigate<CreateConnectionViewModel>(cancellationToken: cancellationToken);
        }

        private Task CreateEventAsync(CancellationToken cancellationToken)
        {
            return NavigationService.Navigate<CreateEventViewModel>(cancellationToken: cancellationToken);
        }
    }
}