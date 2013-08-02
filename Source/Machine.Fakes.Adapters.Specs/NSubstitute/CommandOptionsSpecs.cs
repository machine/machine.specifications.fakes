using System;

using Machine.Fakes.Adapters.NSubstitute;
using Machine.Fakes.Adapters.Specs.SampleCode;
using Machine.Fakes.Internal;
using Machine.Specifications;

namespace Machine.Fakes.Adapters.Specs.NSubstitute
{
    [Subject(typeof(NSubstituteEngine))]
    public class When_an__out__parameter_is_configured : WithCurrentEngine<NSubstituteEngine>
    {
        static ILookupService _fake;
        static string initial;

        Establish context = () => _fake = FakeEngineGateway.Fake<ILookupService>();

        Because of = () => _fake.WhenToldTo(x => x.TryLookup("a", out initial))
                                .AssignOutAndRefParameters("b");

        It should_set_the__out__parameter = () =>
        {
            string result;
            _fake.TryLookup("a", out result);
            result.ShouldEqual("b");
        };
    }

    [Subject(typeof(NSubstituteEngine))]
    public class When_an__out__parameter_is_configured_with_null : WithCurrentEngine<NSubstituteEngine>
    {
        static ILookupService _fake;
        static string initial;

        Establish context = () => _fake = FakeEngineGateway.Fake<ILookupService>();

        Because of = () => _fake.WhenToldTo(x => x.TryLookup("a", out initial))
                                .AssignOutAndRefParameters(new object[] { null });

        It should_set_the__out__parameter = () =>
        {
            string result;
            _fake.TryLookup("a", out result);
            result.ShouldBeNull();
        };
    }

    [Subject(typeof(NSubstituteEngine))]
    public class When_a__ref__parameter_is_configured : WithCurrentEngine<NSubstituteEngine>
    {
        static IWorkOnReferences _fake;
        static object initial;
        static object setupResult = new object();

        Establish context = () => _fake = FakeEngineGateway.Fake<IWorkOnReferences>();

        Because of = () => _fake.WhenToldTo(x => x.Work(ref initial))
                                .AssignOutAndRefParameters(setupResult);

        It should_set_the__ref__parameter = () =>
        {
            object actualResult = null;
            _fake.Work(ref actualResult);
            actualResult.ShouldBeTheSameAs(setupResult);
        };
    }

    [Subject(typeof(NSubstituteEngine))]
    public class When_an__out__and_a__ref__parameter_are_configured : WithCurrentEngine<NSubstituteEngine>
    {
        static IReturnOutAndRef _fake;
        static string initial1;
        static object initial2;
        static object setupResult = new object();

        Establish context = () => _fake = FakeEngineGateway.Fake<IReturnOutAndRef>();

        Because of = () => _fake.WhenToldTo(x => x.GetTwoValues("a", out initial1, ref initial2))
            .AssignOutAndRefParameters("b", setupResult);

        It should_set_both_parameters = () =>
        {
            string actualResult1;
            object actualResult2 = null;
            _fake.GetTwoValues("a", out actualResult1, ref actualResult2);
            actualResult1.ShouldEqual("b");
            actualResult2.ShouldBeTheSameAs(setupResult);
        };
    }

    [Subject(typeof(NSubstituteEngine))]
    public class When_more_out_and_ref_parameters_are_configured_than_the_method_has : WithCurrentEngine<NSubstituteEngine>
    {
        static IReturnOutAndRef _fake;
        static string initial1;
        static object initial2;

        Establish context = () => _fake = FakeEngineGateway.Fake<IReturnOutAndRef>();

        Because of = () => _fake.WhenToldTo(x => x.GetTwoValues("a", out initial1, ref initial2))
            .AssignOutAndRefParameters("b", new object(), new object());

        It should_throw = () =>
        {
            string actualResult1;
            object actualResult2 = null;
            Catch.Exception(() => _fake.GetTwoValues("a", out actualResult1, ref actualResult2))
                .ShouldBeOfType<InvalidOperationException>();
        };
    }

    [Subject(typeof(NSubstituteEngine))]
    public class When_less_out_and_ref_parameters_are_configured_than_the_method_has : WithCurrentEngine<NSubstituteEngine>
    {
        static IReturnOutAndRef _fake;
        static string initial1;
        static object initial2;

        Establish context = () => _fake = FakeEngineGateway.Fake<IReturnOutAndRef>();

        Because of = () => _fake.WhenToldTo(x => x.GetTwoValues("a", out initial1, ref initial2))
            .AssignOutAndRefParameters("b");

        It should_throw = () =>
        {
            string actualResult1;
            object actualResult2 = null;
            Catch.Exception(() => _fake.GetTwoValues("a", out actualResult1, ref actualResult2))
                .ShouldBeOfType<InvalidOperationException>();
        };
    }
}