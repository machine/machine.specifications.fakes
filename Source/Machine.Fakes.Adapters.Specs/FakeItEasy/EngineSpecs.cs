using System;
using System.ComponentModel.Design;
using Machine.Fakes.Adapters.FakeItEasy;
using Machine.Fakes.Adapters.Specs.SampleCode;
using Machine.Fakes.Internal;
using Machine.Specifications;
using Machine.Fakes;

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
    public class When_using_a_abstract_base_classes_as_a_fakes_and_the_constructor_parameter_is_unfakable : WithCurrentEngine<FakeItEasyEngine>
    {
        static ClassWithUnfakableParameter _fake;
        static string _unfakableCtoParameter;
        static string _recievedValue;

        Establish context = () =>
        {
            _unfakableCtoParameter = "Look at me! I'm unfakable!!!";
        };

        Because of = () => _fake = FakeEngineGateway.Fake<ClassWithUnfakableParameter>(_unfakableCtoParameter);

        It should_able_to_construct_the_instance_when_ctor_parameters_are_supplied = 
            () => _fake.RecievedCtorArgument.ShouldEqual(_unfakableCtoParameter);

        It should_be_able_to_fake_virtual_methods_on_the_abstract_base_class = () =>
        {
            _fake.WhenToldTo(x => x.VirtualMethod()).Return("Faked result");   

            var result = _fake.VirtualMethod();

            result.ShouldEqual("Faked result");
        };
    }
}