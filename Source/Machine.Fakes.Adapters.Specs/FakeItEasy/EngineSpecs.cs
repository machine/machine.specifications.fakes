using System;
using System.ComponentModel.Design;
using Machine.Fakes.Adapters.FakeItEasy;
using Machine.Fakes.Adapters.Specs.SampleCode;
using Machine.Fakes.Internal;
using Machine.Specifications;

namespace Machine.Fakes.Adapters.Specs.FakeItEasy
{
    [Subject(typeof(FakeItEasyEngine))]
    [Tags("FakeItEasy")]
    public class AfterInitializingANewFakeCurrentEngine : WithCurrentEngine<FakeItEasyEngine>
    {
        static IServiceContainer _fake;

        Because of = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        It should_be_able_to_create_an_instance = () => _fake.ShouldNotBeNull();
    }

    [Subject(typeof(FakeItEasyEngine))]
    [Tags("Verifying a mock (without inline constaints)", "FakeItEasy")]
    public class Given_that_a_call_was_not_expected_to_happen_but_happened_when_verifying : WithCurrentEngine<FakeItEasyEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.RemoveService(null);
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasNotToldTo(f => f.RemoveService(null)));

        It should_have_thrown_an_exception = () => _exception.ShouldNotBeNull();
    }

    [Subject(typeof(FakeItEasyEngine))]
    [Tags("Verifying a mock (without inline constaints)", "FakeItEasy")]
    public class Given_that_a_call_was_not_expected_to_happen_and_did_not_happened_when_verifying :
        WithCurrentEngine<FakeItEasyEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _exception = Catch.Exception(() => _fake.WasNotToldTo(f => f.RemoveService(null)));

        It should_not_have_thrown_an_exception = () => _exception.ShouldBeNull();
    }

    [Subject(typeof(FakeItEasyEngine))]
    [Tags("FakeItEasy", "Constructing an instance")]
    public class Now_we_can_initialize_a_class_with_no_default_ctor :
        WithCurrentEngine<FakeItEasyEngine>
    {
        static DummyNoDefaultCtorClass _fake;
        static object[] _args = new object[] { 1 };

        Because of = () => _fake = FakeEngineGateway.Fake<DummyNoDefaultCtorClass>(_args);

        It should_be_able_to_create_an_instance = () => _fake.ShouldNotBeNull();
    }

    [Subject(typeof(FakeItEasyEngine))]
    [Tags("FakeItEasy", "Constructing an instance")]
    public class When_using_a_non_default_ctor_an_executing_fake : WithCurrentEngine<FakeItEasyEngine>
    {
        static MyInternal _inner;
        static Sample _fake;

        Establish context = () =>
        {
            _inner = new MyInternal();
            _fake = FakeEngineGateway.Fake<Sample>(_inner);
        };

        Because of = () => _fake.DoCall();

        It should_call_inner_instance = () => _inner.Used.ShouldBeTrue();
    }

    public class Sample
    {
        readonly MyInternal inner;

        public Sample(MyInternal inner)
        {
            this.inner = inner;
        }

        public void DoCall()
        {
            inner.UseIt();
        }
    }

    public class MyInternal
    {
        public bool Used { get; private set; }

        public MyInternal()
        {
            Used = false;
        }

        public void UseIt()
        {
            Used = true;
        }
    }
}