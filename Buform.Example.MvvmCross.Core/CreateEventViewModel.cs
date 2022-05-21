using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Buform.Example.MvvmCross.Core
{
    public sealed class CreateEventViewModel : MvxNavigationViewModel
    {
        public string Title { get; }

        public ICommand CancelCommand { get; }
        public ICommand CreateCommand { get; }

        public FluentValidationForm<CreateEventModel> Form { get; }

        public CreateEventViewModel(ILoggerFactory logFactory, IMvxNavigationService navigationService)
            : base(logFactory, navigationService)
        {
            var model = new CreateEventModel();

            Title = "New Event";

            CancelCommand = new MvxAsyncCommand(CancelAsync);
            CreateCommand = new MvxCommand(Create, CanCreate);

            Form = new FluentValidationForm<CreateEventModel>(model, new CreateEventModelValidator())
            {
                new TextFormGroup
                {
                    new TextFormItem(() => model.Title)
                    {
                        Placeholder = "Title"
                    },
                    new TextFormItem(() => model.Location)
                    {
                        Placeholder = "Location or Video Call"
                    }
                },
                new TextFormGroup
                {
                    new SwitchFormItem(() => model.IsAllDay)
                    {
                        Label = "All-day"
                    },
                    new DateTimeFormItem(() => model.StartsAt)
                    {
                        Label = "Starts",
                        InputType = DateTimeInputType.DateTime
                    },
                    new DateTimeFormItem(() => model.EndsAt)
                    {
                        Label = "Ends",
                        InputType = DateTimeInputType.DateTime
                    }
                },
                new TextFormGroup
                {
                    new TextFormItem(() => model.Url)
                    {
                        Placeholder = "URL",
                        InputType = TextInputType.Url
                    },
                    new TextFormItem(() => model.Notes)
                    {
                        Placeholder = "Notes"
                    }
                },
            };
        }

        private Task CancelAsync(CancellationToken cancellationToken)
        {
            return NavigationService.Close(this, cancellationToken);
        }

        private bool CanCreate()
        {
            // ReSharper disable once ConstantConditionalAccessQualifier
            return Form?.IsValid ?? false;
        }

        private void Create()
        {
        }
    }
}