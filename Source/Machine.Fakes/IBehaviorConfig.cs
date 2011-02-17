namespace Machine.Fakes
{
    public interface IBehaviorConfig
    {
        void EstablishContext(IFakeAccessor fakeAccessor);
        void CleanUp(object subject);
    }
}