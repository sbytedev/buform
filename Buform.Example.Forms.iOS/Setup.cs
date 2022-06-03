using Buform.Example.Core;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Forms.Platforms.Ios.Core;
using MvvmCross.IoC;
using Serilog;
using Serilog.Extensions.Logging;

namespace Buform.Example.Forms.iOS
{
    [Preserve(AllMembers = true)]
    public sealed class Setup : MvxFormsIosSetup<Core.Application, Application>
    {
        protected override ILoggerProvider CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        protected override ILoggerFactory CreateLogFactory()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.NSLog()
                .Enrich.WithDisplayMetrics()
                .Enrich.WithSystemVersion()
                .Enrich.WithDeviceModel()
                .Enrich.WithDeviceId()
                .Enrich.WithDeviceName()
                .Enrich.WithPackageName()
                .Enrich.WithPackageVersionName()
                .Enrich.WithPackageVersionCode()
                .CreateLogger();

            return new SerilogLoggerFactory();
        }

        protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
            base.InitializeFirstChance(iocProvider);

            iocProvider.LazyConstructAndRegisterSingleton<IMvxCommandHelper, MvxStrongCommandHelper>();
        }

        protected override void InitializeLastChance(IMvxIoCProvider iocProvider)
        {
            base.InitializeLastChance(iocProvider);

            BuformForms.RegisterGroupHeaderClass<HeaderFormGroup, HeaderFormGroupView>();
        }
    }
}