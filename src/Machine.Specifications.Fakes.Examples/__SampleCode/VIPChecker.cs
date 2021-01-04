namespace Machine.Fakes.Examples.SampleCode
{
    public class VIPChecker
    {
        readonly ISpecification<Person> _vipChecker;

        public VIPChecker(ISpecification<Person> vipChecker)
        {
            _vipChecker = vipChecker;
        }

        public bool IsVip(Person person)
        {
            return _vipChecker.IsSatisfiedBy(person);
        }
    }
}