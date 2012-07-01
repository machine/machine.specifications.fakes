using Machine.Fakes.Examples.SampleCode;
using Machine.Specifications;

namespace Machine.Fakes.Examples.UsingMFakesApi
{
    [Subject(typeof(VIPChecker)), Tags("Examples")]
    public class A_person_with_nick_ScottGu : WithFakes
    {
        static VIPChecker _vipChecker;
        static bool _isVip;

        Establish context = () =>
        {
            var specification = An<ISpecification<Person>>();
            _vipChecker = new VIPChecker(specification);

            specification
                .WhenToldTo(x => x.IsSatisfiedBy(Param<Person>.Matches(p => p.NickName == "ScottGu")))
                .Return(true);
        };

        Because of = () => { _isVip = _vipChecker.IsVip(new Person { NickName = "ScottGu" }); };

        It should_be_vip = () => _isVip.ShouldBeTrue();
    }
}