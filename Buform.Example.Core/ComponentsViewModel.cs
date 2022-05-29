using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Humanizer;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using NLipsum.Core;

namespace Buform.Example.Core
{
    public sealed class ComponentsViewModel : MvxNavigationViewModel
    {
        public enum Enum
        {
            First,
            Second,
            Third
        }

        public ICommand CloseCommand { get; }
        public ICommand ToggleReadOnlyModeCommand { get; }
        public Color Color { get; set; }
        public ObservableCollection<int> List { get; }
        public Enum Segments { get; set; }
        public string? Text { get; set; }
        public string? MultilineText { get; set; }
        public float Slider { get; set; }
        public int Stepper { get; set; }
        public DateTime DateTime { get; set; }
        public bool Switch { get; set; }
        public string? HiddenText { get; set; }
        public string? CallbackPicker { get; set; }
        public int? Picker { get; set; }
        public int? AsyncPicker { get; set; }
        public int[]? MultiValuePicker { get; set; }

        public ICommand Command { get; }

        public string Title { get; }

        public Form Form { get; }

        public ComponentsViewModel(ILoggerFactory logFactory, IMvxNavigationService navigationService)
            : base(logFactory, navigationService)
        {
            List = new ObservableCollection<int>(Enumerable.Range(1, 10));
            Color = Color.Gold;
            MultilineText = new LipsumGenerator().GenerateLipsum(1);
            DateTime = DateTime.UtcNow;

            CloseCommand = new MvxAsyncCommand(CloseAsync);
            ToggleReadOnlyModeCommand = new MvxCommand(() => Form!.IsReadOnly = !Form.IsReadOnly);

            Command = new MvxCommand(Execute);

            Title = "Components";

            Form = new Form(this)
            {
                new TextFormGroup("Color Pickers")
                {
                    new ColorPickerFormItem(() => Color)
                    {
                        Label = "Color Picker"
                    }
                },
                new ListFormGroup<int>("List")
                {
                    Formatter = item => item.ToWords(),
                    Source = List,
                    RemoveCommand = new MvxCommand<int>(item => List.Remove(item)),
                    MoveCommand = new MvxCommand<(int, int)>(item => List.Move(item.Item1, item.Item2))
                },
                new TextFormGroup("Pickers")
                {
                    new MultiValuePickerFormItem<int>(() => MultiValuePicker)
                    {
                        Label = "Multi-value Picker",
                        Message = "Please select a number.",
                        CanBeCleared = true,
                        InputType = PickerInputType.Default,
                        ItemFormatter = item => item.ToWords(),
                        ValueFormatter = value => value?.Humanize() ?? "None",
                        Source = Enumerable.Range(1, 10).ToArray()
                    },
                    new MultiValuePickerFormItem<int>(() => MultiValuePicker)
                    {
                        Label = "Dialog Multi-value Picker",
                        Message = "Please select a roman number.",
                        CanBeCleared = true,
                        InputType = PickerInputType.Dialog,
                        ItemFormatter = item => item.ToRoman(),
                        ValueFormatter = value => value?.Humanize() ?? "None",
                        Source = Enumerable.Range(1, 10).ToArray()
                    },
                    new CallbackPickerFormItem(() => CallbackPicker)
                    {
                        Label = "Callback Picker",
                        Formatter = item => item ?? "None",
                        Callback = () => Task.FromResult(new LipsumGenerator().GenerateWords(1)[0])!
                    },
                    new PickerFormItem<int?>(() => Picker)
                    {
                        Label = "Picker",
                        Message = "Please select a number.",
                        CanBeCleared = true,
                        InputType = PickerInputType.Default,
                        Formatter = item => item?.ToWords() ?? "None",
                        Source = Enumerable.Range(1, 10).OfType<int?>().ToArray()
                    },
                    new PickerFormItem<int?>(() => Picker)
                    {
                        Label = "Dialog Picker",
                        Message = "Please select a metric number.",
                        CanBeCleared = true,
                        InputType = PickerInputType.Dialog,
                        Formatter = item => item?.ToMetric() ?? "None",
                        Source = Enumerable.Range(1, 10).OfType<int?>().ToArray()
                    },
                    new PickerFormItem<int?>(() => Picker)
                    {
                        Label = "Pop-up Picker",
                        Message = "Please select a roman number.",
                        CanBeCleared = true,
                        InputType = PickerInputType.PopUp,
                        Formatter = item => item?.ToRoman() ?? "None",
                        Source = Enumerable.Range(1, 10).OfType<int?>().ToArray()
                    },
                    new AsyncPickerFormItem<int?>(() => AsyncPicker)
                    {
                        Label = "Async Picker",
                        Message = "Please select a number.",
                        CanBeCleared = true,
                        InputType = PickerInputType.Default,
                        Formatter = item => item?.ToWords() ?? "None",
                        SourceFactory = async cancellationToken =>
                        {
                            await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken).ConfigureAwait(false);
                            return Enumerable.Range(1, 10).OfType<int?>().ToArray();
                        }
                    },
                    new AsyncPickerFormItem<int?>(() => AsyncPicker)
                    {
                        Label = "Dialog Async Picker",
                        Message = "Please select a roman number.",
                        CanBeCleared = true,
                        InputType = PickerInputType.Dialog,
                        Formatter = item => item?.ToRoman() ?? "None",
                        SourceFactory = async cancellationToken =>
                        {
                            await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken).ConfigureAwait(false);
                            return Enumerable.Range(1, 10).OfType<int?>().ToArray();
                        }
                    }
                },
                new TextFormGroup("Buttons")
                {
                    new ButtonFormItem(Command)
                    {
                        Label = "Default Button",
                        InputType = ButtonInputType.Default
                    },
                    new ButtonFormItem(Command)
                    {
                        Label = "Destructive Button",
                        InputType = ButtonInputType.Destructive
                    },
                    new ButtonFormItem(Command)
                    {
                        Label = "Done Button",
                        InputType = ButtonInputType.Done
                    }
                },
                new TextFormGroup("Segments")
                {
                    new SegmentsFormItem<Enum>(() => Segments)
                    {
                        Label = "Segments",
                        Source = System.Enum.GetValues(typeof(Enum)).OfType<Enum>()
                    }
                },
                new TextFormGroup("Texts")
                {
                    new TextFormItem(() => Text)
                    {
                        Placeholder = "Default text",
                        InputType = TextInputType.Default
                    },
                    new TextFormItem(() => Text)
                    {
                        Label = "Number & Punctuation Text",
                        InputType = TextInputType.NumberAndPunctuation
                    },
                    new TextFormItem(() => Text)
                    {
                        Label = "Number Text",
                        InputType = TextInputType.Number
                    },
                    new TextFormItem(() => Text)
                    {
                        Label = "Decimal Text",
                        InputType = TextInputType.Decimal
                    },
                    new TextFormItem(() => Text)
                    {
                        Label = "Phone Text",
                        InputType = TextInputType.Phone
                    },
                    new TextFormItem(() => Text)
                    {
                        Label = "Url Text",
                        InputType = TextInputType.Url
                    },
                    new TextFormItem(() => Text)
                    {
                        Label = "Email Address Text",
                        InputType = TextInputType.EmailAddress
                    },
                    new TextFormItem(() => Text)
                    {
                        Label = "Password Text",
                        IsSecured = true
                    },
                    new MultilineTextFormItem(() => MultilineText)
                    {
                        Placeholder = "Multiline text"
                    }
                },
                new TextFormGroup("Sliders")
                {
                    new TextFormItem<float>(
                        () => Slider,
                        @string => float.TryParse(@string, out var value) ? value : 0
                    )
                    {
                        Label = "Slider value",
                        InputType = TextInputType.Decimal
                    },
                    new SliderFormItem(() => Slider)
                    {
                        MinValue = 0,
                        MaxValue = 10
                    }
                },
                new TextFormGroup("Switches")
                {
                    new SwitchFormItem(() => Switch)
                    {
                        Label = "Switch",
                        ValueChangedCallback = (form, value) => form.GetItem(() => HiddenText)!.IsVisible = value
                    },
                    new TextFormItem(
                        () => HiddenText
                    )
                    {
                        Label = "Hidden text",
                        IsVisible = false
                    }
                },
                new TextFormGroup("Steppers")
                {
                    new TextFormItem<int>(
                        () => Stepper,
                        @string => int.TryParse(@string, out var value) ? value : 0
                    )
                    {
                        Label = "Stepper value",
                        InputType = TextInputType.Number
                    },
                    new StepperFormItem(() => Stepper)
                    {
                        MinValue = -10,
                        MaxValue = 10,
                        StepAmount = 5,
                        Label = "Stepper"
                    }
                },
                new TextFormGroup("Date & Time")
                {
                    new DateTimeFormItem(() => DateTime)
                    {
                        Label = "Date and time",
                        InputType = DateTimeInputType.DateTime
                    },
                    new DateTimeFormItem(() => DateTime)
                    {
                        Label = "Date",
                        InputType = DateTimeInputType.Date
                    },
                    new DateTimeFormItem(() => DateTime)
                    {
                        Label = "Time",
                        InputType = DateTimeInputType.Time
                    }
                }
            };
        }

        private Task CloseAsync(CancellationToken cancellationToken)
        {
            return NavigationService.Close(this, cancellationToken);
        }

        private static void Execute()
        {
            Console.WriteLine("Command executed");
        }

        // ReSharper disable once UnusedMember.Global
        public void OnPropertyChanged(string propertyName, object before, object after)
        {
            RaisePropertyChanged(propertyName);

            Console.WriteLine($"{propertyName}: {before} -> {after}");
        }
    }
}