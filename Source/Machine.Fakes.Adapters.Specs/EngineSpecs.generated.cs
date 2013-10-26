using System;
using System.ComponentModel.Design;
using Machine.Fakes.Adapters.FakeItEasy;
using Machine.Fakes.Adapters.Moq;
using Machine.Fakes.Adapters.NSubstitute;
using Machine.Fakes.Adapters.Rhinomocks;
using Machine.Fakes.Adapters.Specs.SampleCode;
using Machine.Fakes.Internal;
using Machine.Specifications;

namespace Machine.Fakes.Adapters.Specs.RhinoMocks
{
    [Subject(typeof(RhinoFakeEngine))]
    public class AfterInitializingANewFakeCurrentEngine : WithCurrentEngine<RhinoFakeEngine>
    {
        static IServiceContainer _fake;

        Because of = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        It should_be_able_to_create_an_instance = () => _fake.ShouldNotBeNull();
    }

    [Subject(typeof(RhinoFakeEngine))]
    public class Given_that_a_call_was_not_expected_to_happen_but_happened_when_verifying : WithCurrentEngine<RhinoFakeEngine>
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

    [Subject(typeof(RhinoFakeEngine))]
    public class Given_that_a_call_was_not_expected_to_happen_and_did_not_happened_when_verifying : WithCurrentEngine<RhinoFakeEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _exception = Catch.Exception(() => _fake.WasNotToldTo(f => f.RemoveService(null)));

        It should_not_have_thrown_an_exception = () => _exception.ShouldBeNull();
    }

    [Subject(typeof(RhinoFakeEngine))]
    public class Now_we_can_initialize_a_class_with_no_default_ctor : WithCurrentEngine<RhinoFakeEngine>
    {
        static DummyNoDefaultCtorClass _fake;
        static object[] _args = new object[] { 1 };

        Because of = () => _fake = FakeEngineGateway.Fake<DummyNoDefaultCtorClass>(_args);

        It should_be_able_to_create_an_instance = () => _fake.ShouldNotBeNull();

        It should_use_the_given_arguments = () => _fake.Value.ShouldEqual(1);
    }

    [Subject(typeof(RhinoFakeEngine))]
    public class When_using_an_abstract_base_class_as_a_fake_and_a_constructor_parameter_is_unfakable : WithCurrentEngine<RhinoFakeEngine>
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
            () => _fake.ReceivedConstructorArgument.ShouldEqual(_unfakableCtoParameter);

        It should_be_able_to_fake_virtual_methods_on_the_abstract_base_class = () =>
        {
            _fake.WhenToldTo(x => x.VirtualMethod()).Return("Faked result");

            var result = _fake.VirtualMethod();

            result.ShouldEqual("Faked result");
        };
    }

    [Subject(typeof(RhinoFakeEngine))]
    public class When_faking_an_interface_with_a_property : WithCurrentEngine<RhinoFakeEngine>
    {
        static ITypeWithProperty _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<ITypeWithProperty>();

        Because of = () => _fake.Property = "new property value";

        It should_track_property_changes = () =>
            _fake.Property.ShouldEqual("new property value");
    }

    [Subject(typeof(RhinoFakeEngine))]
    public class When_faking_a_delegate : WithCurrentEngine<RhinoFakeEngine>
    {
        static MyDelegate _fake;
        public delegate void MyDelegate();

        Because of = () => _fake = FakeEngineGateway.Fake<MyDelegate>();

        It should_be_able_to_fake_the_delegate_without_throwing_an_exception = () =>
            _fake.ShouldNotBeNull();
    }
}

namespace Machine.Fakes.Adapters.Specs.NSubstitute
{
    [Subject(typeof(NSubstituteEngine))]
    public class AfterInitializingANewFakeCurrentEngine : WithCurrentEngine<NSubstituteEngine>
    {
        static IServiceContainer _fake;

        Because of = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        It should_be_able_to_create_an_instance = () => _fake.ShouldNotBeNull();
    }

    [Subject(typeof(NSubstituteEngine))]
    public class Given_that_a_call_was_not_expected_to_happen_but_happened_when_verifying : WithCurrentEngine<NSubstituteEngine>
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

    [Subject(typeof(NSubstituteEngine))]
    public class Given_that_a_call_was_not_expected_to_happen_and_did_not_happened_when_verifying : WithCurrentEngine<NSubstituteEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _exception = Catch.Exception(() => _fake.WasNotToldTo(f => f.RemoveService(null)));

        It should_not_have_thrown_an_exception = () => _exception.ShouldBeNull();
    }

    [Subject(typeof(NSubstituteEngine))]
    public class Now_we_can_initialize_a_class_with_no_default_ctor : WithCurrentEngine<NSubstituteEngine>
    {
        static DummyNoDefaultCtorClass _fake;
        static object[] _args = new object[] { 1 };

        Because of = () => _fake = FakeEngineGateway.Fake<DummyNoDefaultCtorClass>(_args);

        It should_be_able_to_create_an_instance = () => _fake.ShouldNotBeNull();

        It should_use_the_given_arguments = () => _fake.Value.ShouldEqual(1);
    }

    [Subject(typeof(NSubstituteEngine))]
    public class When_using_an_abstract_base_class_as_a_fake_and_a_constructor_parameter_is_unfakable : WithCurrentEngine<NSubstituteEngine>
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
            () => _fake.ReceivedConstructorArgument.ShouldEqual(_unfakableCtoParameter);

        It should_be_able_to_fake_virtual_methods_on_the_abstract_base_class = () =>
        {
            _fake.WhenToldTo(x => x.VirtualMethod()).Return("Faked result");

            var result = _fake.VirtualMethod();

            result.ShouldEqual("Faked result");
        };
    }

    [Subject(typeof(NSubstituteEngine))]
    public class When_faking_an_interface_with_a_property : WithCurrentEngine<NSubstituteEngine>
    {
        static ITypeWithProperty _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<ITypeWithProperty>();

        Because of = () => _fake.Property = "new property value";

        It should_track_property_changes = () =>
            _fake.Property.ShouldEqual("new property value");
    }

    [Subject(typeof(NSubstituteEngine))]
    public class When_faking_a_delegate : WithCurrentEngine<NSubstituteEngine>
    {
        static MyDelegate _fake;
        public delegate void MyDelegate();

        Because of = () => _fake = FakeEngineGateway.Fake<MyDelegate>();

        It should_be_able_to_fake_the_delegate_without_throwing_an_exception = () =>
            _fake.ShouldNotBeNull();
    }
}

namespace Machine.Fakes.Adapters.Specs.Moq
{
    [Subject(typeof(MoqFakeEngine))]
    public class AfterInitializingANewFakeCurrentEngine : WithCurrentEngine<MoqFakeEngine>
    {
        static IServiceContainer _fake;

        Because of = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        It should_be_able_to_create_an_instance = () => _fake.ShouldNotBeNull();
    }

    [Subject(typeof(MoqFakeEngine))]
    public class Given_that_a_call_was_not_expected_to_happen_but_happened_when_verifying : WithCurrentEngine<MoqFakeEngine>
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

    [Subject(typeof(MoqFakeEngine))]
    public class Given_that_a_call_was_not_expected_to_happen_and_did_not_happened_when_verifying : WithCurrentEngine<MoqFakeEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _exception = Catch.Exception(() => _fake.WasNotToldTo(f => f.RemoveService(null)));

        It should_not_have_thrown_an_exception = () => _exception.ShouldBeNull();
    }

    [Subject(typeof(MoqFakeEngine))]
    public class Now_we_can_initialize_a_class_with_no_default_ctor : WithCurrentEngine<MoqFakeEngine>
    {
        static DummyNoDefaultCtorClass _fake;
        static object[] _args = new object[] { 1 };

        Because of = () => _fake = FakeEngineGateway.Fake<DummyNoDefaultCtorClass>(_args);

        It should_be_able_to_create_an_instance = () => _fake.ShouldNotBeNull();

        It should_use_the_given_arguments = () => _fake.Value.ShouldEqual(1);
    }

    [Subject(typeof(MoqFakeEngine))]
    public class When_using_an_abstract_base_class_as_a_fake_and_a_constructor_parameter_is_unfakable : WithCurrentEngine<MoqFakeEngine>
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
            () => _fake.ReceivedConstructorArgument.ShouldEqual(_unfakableCtoParameter);

        It should_be_able_to_fake_virtual_methods_on_the_abstract_base_class = () =>
        {
            _fake.WhenToldTo(x => x.VirtualMethod()).Return("Faked result");

            var result = _fake.VirtualMethod();

            result.ShouldEqual("Faked result");
        };
    }

    [Subject(typeof(MoqFakeEngine))]
    public class When_faking_an_interface_with_a_property : WithCurrentEngine<MoqFakeEngine>
    {
        static ITypeWithProperty _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<ITypeWithProperty>();

        Because of = () => _fake.Property = "new property value";

        It should_track_property_changes = () =>
            _fake.Property.ShouldEqual("new property value");
    }

    [Subject(typeof(MoqFakeEngine))]
    public class When_faking_a_delegate : WithCurrentEngine<MoqFakeEngine>
    {
        static MyDelegate _fake;
        public delegate void MyDelegate();

        Because of = () => _fake = FakeEngineGateway.Fake<MyDelegate>();

        It should_be_able_to_fake_the_delegate_without_throwing_an_exception = () =>
            _fake.ShouldNotBeNull();
    }
}

namespace Machine.Fakes.Adapters.Specs.FakeItEasy
{
    [Subject(typeof(FakeItEasyEngine))]
    public class AfterInitializingANewFakeCurrentEngine : WithCurrentEngine<FakeItEasyEngine>
    {
        static IServiceContainer _fake;

        Because of = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        It should_be_able_to_create_an_instance = () => _fake.ShouldNotBeNull();
    }

    [Subject(typeof(FakeItEasyEngine))]
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
    public class Given_that_a_call_was_not_expected_to_happen_and_did_not_happened_when_verifying : WithCurrentEngine<FakeItEasyEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _exception = Catch.Exception(() => _fake.WasNotToldTo(f => f.RemoveService(null)));

        It should_not_have_thrown_an_exception = () => _exception.ShouldBeNull();
    }

    [Subject(typeof(FakeItEasyEngine))]
    public class Now_we_can_initialize_a_class_with_no_default_ctor : WithCurrentEngine<FakeItEasyEngine>
    {
        static DummyNoDefaultCtorClass _fake;
        static object[] _args = new object[] { 1 };

        Because of = () => _fake = FakeEngineGateway.Fake<DummyNoDefaultCtorClass>(_args);

        It should_be_able_to_create_an_instance = () => _fake.ShouldNotBeNull();

        It should_use_the_given_arguments = () => _fake.Value.ShouldEqual(1);
    }

    [Subject(typeof(FakeItEasyEngine))]
    public class When_using_an_abstract_base_class_as_a_fake_and_a_constructor_parameter_is_unfakable : WithCurrentEngine<FakeItEasyEngine>
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
            () => _fake.ReceivedConstructorArgument.ShouldEqual(_unfakableCtoParameter);

        It should_be_able_to_fake_virtual_methods_on_the_abstract_base_class = () =>
        {
            _fake.WhenToldTo(x => x.VirtualMethod()).Return("Faked result");

            var result = _fake.VirtualMethod();

            result.ShouldEqual("Faked result");
        };
    }

    [Subject(typeof(FakeItEasyEngine))]
    public class When_faking_an_interface_with_a_property : WithCurrentEngine<FakeItEasyEngine>
    {
        static ITypeWithProperty _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<ITypeWithProperty>();

        Because of = () => _fake.Property = "new property value";

        It should_track_property_changes = () =>
            _fake.Property.ShouldEqual("new property value");
    }

    [Subject(typeof(FakeItEasyEngine))]
    public class When_faking_a_delegate : WithCurrentEngine<FakeItEasyEngine>
    {
        static MyDelegate _fake;
        public delegate void MyDelegate();

        Because of = () => _fake = FakeEngineGateway.Fake<MyDelegate>();

        It should_be_able_to_fake_the_delegate_without_throwing_an_exception = () =>
            _fake.ShouldNotBeNull();
    }
}
