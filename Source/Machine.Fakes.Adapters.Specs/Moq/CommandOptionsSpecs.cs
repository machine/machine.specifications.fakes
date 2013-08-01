using System;
using Machine.Fakes.Adapters.Moq;
using Machine.Fakes.Adapters.Specs.SampleCode;
using Machine.Fakes.Internal;
using Machine.Specifications;

namespace Machine.Fakes.Adapters.Specs.Moq
{
    [Subject(typeof(MoqFakeEngine))]
    public class When_an__out__or__ref__parameter_is_configured : WithCurrentEngine<MoqFakeEngine>
    {
        static ILookupService _fake;
        static Exception exception;

        Establish context = () => _fake = FakeEngineGateway.Fake<ILookupService>();

        Because of = () =>
        {
            string value;
            exception = Catch.Exception(() => _fake.WhenToldTo(x => x.TryLookup("a", out value)).AssignOutAndRefParameters("b"));
        };

        It should_throw = () => exception.ShouldBeOfType<NotSupportedException>();
    }
}