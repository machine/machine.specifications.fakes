namespace Machine.Fakes.Internal
{
    interface IContainer
    {
        void Register(TypeMapping typeMapping);
        void Register<T>(FactoryMapping<T> factoryMapping);
        void Register(ObjectMapping objectMapping);
    }
}