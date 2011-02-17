namespace Machine.Fakes
{
    public class BehaviorConfigBase : IBehaviorConfig
    {
        public virtual void EstablishContext(IFakeAccessor fakeAccessor)
        {
        }

        public virtual void CleanUp(object subject)
        {
        }
    }
}