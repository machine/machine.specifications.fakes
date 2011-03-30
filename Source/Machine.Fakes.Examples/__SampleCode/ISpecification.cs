namespace Machine.Fakes.Examples.SampleCode
{
    public interface ISpecification<in T>
    {
        bool IsSatisfiedBy(T candidate);
    }
}