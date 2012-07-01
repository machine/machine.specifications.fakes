using Machine.Fakes.Adapters.Rhinomocks;
using Machine.Fakes.Sdk;
using Machine.Specifications;

namespace Machine.Fakes.Adapters.Specs.RegistrarSpecs
{
    [Subject(typeof(SpecificationController<>))]
    public class When_a_factory_is_specified_in_a_registrar : WithSubject<Configuration, RhinoFakeEngine>
    {
        Establish context = () =>
        {
            Configure(c => c.For<IConfigurationStore>().Use(() => new ConfigurationStore()));
        };

        Because of = () => { };

        It should_use_the_factory_to_create_the_dependency = 
            () => Subject.ConfigurationStore.ShouldBeOfType<ConfigurationStore>();
    }

    public class Configuration
    {
        public IConfigurationStore ConfigurationStore;

        public Configuration(IConfigurationStore configurationStore)
        {
            ConfigurationStore = configurationStore;
        }
    }

    public interface IConfigurationStore
    {
    }

    class ConfigurationStore : IConfigurationStore
    {
    }
}