using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Humanizer;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Buform.Example.Core
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
                        Label = "All-day",
                        ValueChangedCallback = (form, value) =>
                        {
                            var inputType = value ? DateTimeInputType.Date : DateTimeInputType.DateTime;

                            form.GetItem<DateTimeFormItem>(() => model.StartsAt)!.InputType = inputType;
                            form.GetItem<DateTimeFormItem>(() => model.EndsAt)!.InputType = inputType;

                            form.GetItem(() => model.TravelTime)!.IsVisible = !value;
                        }
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
                    },
                    new PickerFormItem<CreateEventModel.RepeatType>(() => model.Repeat)
                    {
                        Label = "Repeat",
                        Source = Enum.GetValues(typeof(CreateEventModel.RepeatType)).OfType<CreateEventModel.RepeatType>(),
                        Formatter = item => item.Humanize(LetterCasing.Title)
                    },
                    new PickerFormItem<CreateEventModel.TravelTimeType>(() => model.TravelTime)
                    {
                        Label = "Travel Time",
                        Source = Enum.GetValues(typeof(CreateEventModel.TravelTimeType)).OfType<CreateEventModel.TravelTimeType>(),
                        Formatter = item => item.Humanize(LetterCasing.Title)
                    }
                },
                new TextFormGroup
                {
                    new ButtonFormItem(new MvxCommand(AddAttachment))
                    {
                        Label = "Add attachment..."
                    }
                },
                new TextFormGroup
                {
                    new TextFormItem(() => model.Url)
                    {
                        Placeholder = "URL",
                        InputType = TextInputType.Url
                    },
                    new MultilineTextFormItem(() => model.Notes)
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

        private void AddAttachment()
        {
        }
    }
}