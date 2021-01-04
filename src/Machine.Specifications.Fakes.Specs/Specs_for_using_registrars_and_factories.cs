using Machine.Fakes.Sdk;
using Machine.Fakes.Specs.TestClasses;
using Machine.Specifications;

namespace Machine.Fakes.Specs
{
    [Subject(typeof(SpecificationController<>))]
    public class When_a_factory_is_used_to_configure_a_dependency
    {
        static SpecificationController<Configuration, DummyFakeEngine> specificationController;
        
        Establish context = () =>
        {
            specificationController = new SpecificationController<Configuration, DummyFakeEngine>();
            specificationController.Configure(c => c.For<IConfigurationStore>().Use(() => new ConfigurationStore()));
        };

        Because of = () => { };

        It should_use_the_factory_to_create_the_dependency = () =>
            specificationController.Subject.ConfigurationStore.ShouldBeOfExactType<ConfigurationStore>();
    }

    [Subject(typeof(SpecificationController<>))]
    public class When_a_registrar_expression_is_used_to_configure_two_dependencies
    {
        static SpecificationController<object, DummyFakeEngine> specificationController;

        Establish context = () =>
            specificationController = new SpecificationController<object, DummyFakeEngine>();

        Because of = () => specificationController.Configure(config =>
        {
            config.For<ICar>().Use<Car>();
            config.For<IGarage>().Use<Garage>();
        });

        It should_return_both_configured_dependencies = () =>
        {
            specificationController.The<ICar>().ShouldBeOfExactType<Car>();
            specificationController.The<IGarage>().ShouldBeOfExactType<Garage>();
        };
    }

    [Subject(typeof(SpecificationController<>))]
    public class When_a_registrar_is_used_to_configure_two_dependencies
    {
        static SpecificationController<object, DummyFakeEngine> specificationController;

        Establish context = () =>
            specificationController = new SpecificationController<object, DummyFakeEngine>();

        Because of = () => specificationController.Configure(new CarRegistrar());

        It should_return_both_configured_dependencies = () =>
        {
            specificationController.The<ICar>().ShouldBeOfExactType<Car>();
            specificationController.The<IGarage>().ShouldBeOfExactType<Garage>();
        };
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

    public class CarRegistrar : Registrar
    {
        public CarRegistrar()
        {
            For<ICar>().Use<Car>();
            For<IGarage>().Use<Garage>();
        }
    }
}