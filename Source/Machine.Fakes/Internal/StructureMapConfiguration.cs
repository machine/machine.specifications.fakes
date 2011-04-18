using StructureMap;

namespace Machine.Fakes.Internal
{
    class StructureMapConfiguration : IContainer
    {
        readonly ConfigurationExpression _config;

        public StructureMapConfiguration(ConfigurationExpression config)
        {
            _config = config;
        }

        public void Register(TypeMapping typeMapping)
        {
            _config.For(typeMapping.InterfaceType).Use(typeMapping.ImplementationType);
        }

        public void Register<T>(FactoryMapping<T> factoryMapping)
        {
            _config.For<T>().Use(factoryMapping.Factory);
        }

        public void Register(ObjectMapping objectMapping)
        {
            _config.For(objectMapping.InterfaceType).Use(objectMapping.Implementation);
        }
    }
}