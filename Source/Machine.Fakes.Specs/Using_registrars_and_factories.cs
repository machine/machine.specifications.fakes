using Machine.Fakes.Sdk;
using Machine.Fakes.Specs.TestClasses;
using Machine.Specifications;

namespace Machine.Fakes.Adapters.Specs.RegistrarSpecs
{
    [Subject(typeof(SpecificationController<>))]
    public class When_a_factory_is_specified_in_a_registrar
    {
        static SpecificationController<Configuration, DummyFakeEngine> specificationController;
        Establish context = () =>
        {
            specificationController = new SpecificationController<Configuration, DummyFakeEngine>();
            specificationController.Configure(c => c.For<IConfigurationStore>().Use(() => new ConfigurationStore()));
        };

        Because of = () => { };

        It should_use_the_factory_to_create_the_dependency = () =>
            specificationController.Subject.ConfigurationStore.ShouldBeOfType<ConfigurationStore>();
    }

    public class Configuration
    {
        public readonly IConfigurationStore ConfigurationStore;

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