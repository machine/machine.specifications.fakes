namespace Machine.Fakes.Adapters.Specs.SampleCode
{
    public interface ITablet
    {
    }

    public interface ICanPlayFlash
    {
    }

    public class IPad : ITablet
    {
    }

    public class Xoom : ITablet, ICanPlayFlash
    {
    }
}