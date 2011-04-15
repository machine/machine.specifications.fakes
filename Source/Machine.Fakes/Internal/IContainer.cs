namespace Machine.Fakes.Internal
{
    interface IContainer
    {
        void Register(TypeMapping objectMapping);
        void Register<T>(FactoryMapping<T> factoryMapping);
        void Register(ObjectMapping objectMapping);
    }
}