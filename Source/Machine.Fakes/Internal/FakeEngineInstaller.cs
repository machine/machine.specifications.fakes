using System;
using System.Linq;
using Machine.Fakes.Utils;

namespace Machine.Fakes.Internal
{
    class FakeEngineInstaller
    {
        public static IFakeEngine InstallFor(Type specificationType)
        {
            var currentSpecAssembly = specificationType.Assembly;

            var configuration = specificationType
                .GetCustomAttributes(typeof(ConfigurationAttribute),false)
                .AlternativeIfNullOrEmpty(
                    () => currentSpecAssembly.GetCustomAttributes(typeof(ConfigurationAttribute), false))
                .AlternativeIfNullOrEmpty(
                    () => typeof(FakeEngineInstaller).Assembly.GetCustomAttributes(typeof(ConfigurationAttribute), false))
                .Cast<ConfigurationAttribute>()
                .FirstOrCustomDefaultValue(new ConfigurationAttribute());

            if (configuration.FakeEngineType == null)
            {
                throw new InvalidOperationException(
                    "Fatal Error: The required fake engine has not been configured." +
                    Environment.NewLine +
                    "Configure it either at spec or assembly level using the ConfigurationAttribute");
            }

            var engine = (IFakeEngine)Activator.CreateInstance(configuration.FakeEngineType);

            FakeEngineGateway.EngineIs(engine);

            return engine;
        }
    }
}