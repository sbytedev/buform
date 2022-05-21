using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using SByteDev.MvvmCross.Extensions;

namespace Buform.Example.MvvmCross.Core
{
    public sealed class CreateConnectionViewModel : MvxNavigationViewModel
    {
        public enum ConnectionType
        {
            Ftp,
            FtpSsl,
            WebDav
        }

        private const string AnonymousUsername = "anonymous";

        // ReSharper disable once NotAccessedField.Local
        private readonly IDisposable _connectCommandSubscription;

        public string? Server { get; set; }
        public int? Port { get; set; }
        public ConnectionType Type { get; set; }
        public bool IsAnonymousLogin { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? SshPrivateKey { get; set; }

        public string Title { get; }

        public FluentValidationForm<CreateConnectionViewModel> Form { get; }

        public ICommand CancelCommand { get; }
        public ICommand ConnectCommand { get; }
        public ICommand ResetCommand { get; }

        public CreateConnectionViewModel(ILoggerFactory logFactory, IMvxNavigationService navigationService)
            : base(logFactory, navigationService)
        {
            SetDefaultValues();

            Title = "New Connection";

            CancelCommand = new MvxAsyncCommand(CancelAsync);
            ConnectCommand = new MvxCommand(Connect, CanConnect);
            ResetCommand = new MvxCommand(Reset);

            Form = new FluentValidationForm<CreateConnectionViewModel>(this, new CreateConnectionViewModelValidator())
            {
                new TextFormGroup("Server")
                {
                    new TextFormItem(() => Server)
                    {
                        Placeholder = "Server",
                        InputType = TextInputType.Url
                    },
                    new TextFormItem<int?>(
                        () => Port,
                        @string => int.TryParse(@string, out var port) ? port : null
                    )
                    {
                        Label = "Port",
                        Placeholder = "21",
                        InputType = TextInputType.Number
                    },
                    new SegmentsFormItem<ConnectionType>(() => Type)
                    {
                        Formatter = FormatConnectionType,
                        Source = new []{ ConnectionType.Ftp, ConnectionType.FtpSsl, ConnectionType.WebDav }
                    }
                },
                new TextFormGroup("Authorization", "Anonymous FTP logins are usually the username 'anonymous' with the user's email address as the password.")
                {
                    new SwitchFormItem(() => IsAnonymousLogin)
                    {
                        Label = "Anonymous login",
                        ValueChangedCallback = (form, value) =>
                        {
                            form[() => Username]!.IsReadOnly = value;
                            Username = value ? AnonymousUsername : null;
                            Password = null;
                        }
                    },
                    new TextFormItem(() => Username)
                    {
                        Label = "Username",
                        Placeholder = "Username",
                        InputType = TextInputType.EmailAddress
                    },
                    new TextFormItem(() => Password)
                    {
                        Label = "Password",
                        Placeholder = "Password",
                        IsSecured = true
                    }
                },
                new TextFormGroup
                {
                    new PickerFormItem(() => SshPrivateKey)
                    {
                        Label = "SSH Private Key",
                        InputType = PickerInputType.PopUp,
                        Source = new [] { "Key 1", "Key 2", "Key 3" }
                    }
                },
                new TextFormGroup
                {
                    new ButtonFormItem(ConnectCommand)
                    {
                        Label = "Connect",
                        InputType = ButtonInputType.Done
                    }
                },
                new TextFormGroup
                {
                    new ButtonFormItem(ResetCommand)
                    {
                        Label = "Reset",
                        InputType = ButtonInputType.Destructive
                    }
                }
            };

            _connectCommandSubscription = ConnectCommand.RelayOn(Form, () => Form.IsValid);
        }

        private static string FormatConnectionType(ConnectionType value)
        {
            return value switch
            {
                ConnectionType.Ftp => "FTP",
                ConnectionType.FtpSsl => "FTP-SSL",
                ConnectionType.WebDav => "WebDAW",
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
            };
        }

        private void SetDefaultValues()
        {
            Server = null;
            Port = 21;
            Type = ConnectionType.Ftp;
            IsAnonymousLogin = false;
            Username = null;
            Password = null;
            SshPrivateKey = null;
        }

        private Task CancelAsync(CancellationToken cancellationToken)
        {
            return NavigationService.Close(this, cancellationToken);
        }

        private bool CanConnect()
        {
            // ReSharper disable once ConstantConditionalAccessQualifier
            return Form?.IsValid ?? false;
        }

        private void Connect()
        {
        }

        private void Reset()
        {
            Form.ResetValidation();

            SetDefaultValues();
        }
    }
}