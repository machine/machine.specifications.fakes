






using System;
using Machine.Fakes.Adapters.Specs.SampleCode;
using Machine.Fakes.Internal;
using Machine.Specifications;


#if !NETSTANDARD



namespace Machine.Fakes.Adapters.Specs.Rhinomocks
{

	using Machine.Fakes.Adapters.Rhinomocks;
    
	[Subject(typeof(RhinoFakeEngine))]
    public class Given_a_method_was_not_configured_on_a_Fake_when_verifying_whether_it_was_accessed : WithCurrentEngine<RhinoFakeEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.GetService(null)));

        It should_throw_an_exception = () => _exception.ShouldNotBeNull();
    }

    [Subject(typeof(RhinoFakeEngine))]
    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed : WithCurrentEngine<RhinoFakeEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.RemoveService(null);
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(null)));

        It should_not_throw_an_exception = () => _exception.ShouldBeNull();
    }

    [Subject(typeof(RhinoFakeEngine))]
    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed_twice : WithCurrentEngine<RhinoFakeEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.RemoveService(null);
            _fake.RemoveService(null);
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(null)).Twice());

        It should_not_throw_an_exception = () => _exception.ShouldBeNull();
    }

    [Subject(typeof(RhinoFakeEngine))]
    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed_twice_but_was_only_executed_once : WithCurrentEngine<RhinoFakeEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.RemoveService(null);
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(null)).Twice());

        It should_throw_an_exception = () => _exception.ShouldNotBeNull();
    }

    [Subject(typeof(RhinoFakeEngine))]
    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed_only_once_but_was_excuted_twice : WithCurrentEngine<RhinoFakeEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.RemoveService(null);
            _fake.RemoveService(null);
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(null)).OnlyOnce());

        It should_throw_an_exception = () => _exception.ShouldNotBeNull();
    }

    [Subject(typeof(RhinoFakeEngine))]
    public class Given_a_query_was_configured_on_a_fake_when_verifying_whether_it_was_executed : WithCurrentEngine<RhinoFakeEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.GetService(null);
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.GetService(null)));

        It should_not_throw_an_exception = () => _exception.ShouldBeNull();
    }

    [Subject(typeof(RhinoFakeEngine))]
    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed_only_once_with_a_given_parameter_but_was_executed_twice_in_total : WithCurrentEngine<RhinoFakeEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.RemoveService(typeof(Object));
            _fake.RemoveService(typeof(String));
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(typeof(String))).OnlyOnce());

        It should_not_throw_an_exception = () => _exception.ShouldBeNull();
    }
}

#endif



#if !NETSTANDARD



namespace Machine.Fakes.Adapters.Specs.NSubstitute
{

	using Machine.Fakes.Adapters.NSubstitute;
    
	[Subject(typeof(NSubstituteEngine))]
    public class Given_a_method_was_not_configured_on_a_Fake_when_verifying_whether_it_was_accessed : WithCurrentEngine<NSubstituteEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.GetService(null)));

        It should_throw_an_exception = () => _exception.ShouldNotBeNull();
    }

    [Subject(typeof(NSubstituteEngine))]
    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed : WithCurrentEngine<NSubstituteEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.RemoveService(null);
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(null)));

        It should_not_throw_an_exception = () => _exception.ShouldBeNull();
    }

    [Subject(typeof(NSubstituteEngine))]
    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed_twice : WithCurrentEngine<NSubstituteEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.RemoveService(null);
            _fake.RemoveService(null);
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(null)).Twice());

        It should_not_throw_an_exception = () => _exception.ShouldBeNull();
    }

    [Subject(typeof(NSubstituteEngine))]
    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed_twice_but_was_only_executed_once : WithCurrentEngine<NSubstituteEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.RemoveService(null);
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(null)).Twice());

        It should_throw_an_exception = () => _exception.ShouldNotBeNull();
    }

    [Subject(typeof(NSubstituteEngine))]
    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed_only_once_but_was_excuted_twice : WithCurrentEngine<NSubstituteEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.RemoveService(null);
            _fake.RemoveService(null);
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(null)).OnlyOnce());

        It should_throw_an_exception = () => _exception.ShouldNotBeNull();
    }

    [Subject(typeof(NSubstituteEngine))]
    public class Given_a_query_was_configured_on_a_fake_when_verifying_whether_it_was_executed : WithCurrentEngine<NSubstituteEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.GetService(null);
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.GetService(null)));

        It should_not_throw_an_exception = () => _exception.ShouldBeNull();
    }

    [Subject(typeof(NSubstituteEngine))]
    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed_only_once_with_a_given_parameter_but_was_executed_twice_in_total : WithCurrentEngine<NSubstituteEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.RemoveService(typeof(Object));
            _fake.RemoveService(typeof(String));
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(typeof(String))).OnlyOnce());

        It should_not_throw_an_exception = () => _exception.ShouldBeNull();
    }
}

#endif






namespace Machine.Fakes.Adapters.Specs.Moq
{

	using Machine.Fakes.Adapters.Moq;
    
	[Subject(typeof(MoqFakeEngine))]
    public class Given_a_method_was_not_configured_on_a_Fake_when_verifying_whether_it_was_accessed : WithCurrentEngine<MoqFakeEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.GetService(null)));

        It should_throw_an_exception = () => _exception.ShouldNotBeNull();
    }

    [Subject(typeof(MoqFakeEngine))]
    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed : WithCurrentEngine<MoqFakeEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.RemoveService(null);
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(null)));

        It should_not_throw_an_exception = () => _exception.ShouldBeNull();
    }

    [Subject(typeof(MoqFakeEngine))]
    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed_twice : WithCurrentEngine<MoqFakeEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.RemoveService(null);
            _fake.RemoveService(null);
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(null)).Twice());

        It should_not_throw_an_exception = () => _exception.ShouldBeNull();
    }

    [Subject(typeof(MoqFakeEngine))]
    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed_twice_but_was_only_executed_once : WithCurrentEngine<MoqFakeEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.RemoveService(null);
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(null)).Twice());

        It should_throw_an_exception = () => _exception.ShouldNotBeNull();
    }

    [Subject(typeof(MoqFakeEngine))]
    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed_only_once_but_was_excuted_twice : WithCurrentEngine<MoqFakeEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.RemoveService(null);
            _fake.RemoveService(null);
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(null)).OnlyOnce());

        It should_throw_an_exception = () => _exception.ShouldNotBeNull();
    }

    [Subject(typeof(MoqFakeEngine))]
    public class Given_a_query_was_configured_on_a_fake_when_verifying_whether_it_was_executed : WithCurrentEngine<MoqFakeEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.GetService(null);
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.GetService(null)));

        It should_not_throw_an_exception = () => _exception.ShouldBeNull();
    }

    [Subject(typeof(MoqFakeEngine))]
    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed_only_once_with_a_given_parameter_but_was_executed_twice_in_total : WithCurrentEngine<MoqFakeEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.RemoveService(typeof(Object));
            _fake.RemoveService(typeof(String));
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(typeof(String))).OnlyOnce());

        It should_not_throw_an_exception = () => _exception.ShouldBeNull();
    }
}




#if !NETSTANDARD



namespace Machine.Fakes.Adapters.Specs.FakeItEasy
{

	using Machine.Fakes.Adapters.FakeItEasy;
    
	[Subject(typeof(FakeItEasyEngine))]
    public class Given_a_method_was_not_configured_on_a_Fake_when_verifying_whether_it_was_accessed : WithCurrentEngine<FakeItEasyEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.GetService(null)));

        It should_throw_an_exception = () => _exception.ShouldNotBeNull();
    }

    [Subject(typeof(FakeItEasyEngine))]
    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed : WithCurrentEngine<FakeItEasyEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.RemoveService(null);
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(null)));

        It should_not_throw_an_exception = () => _exception.ShouldBeNull();
    }

    [Subject(typeof(FakeItEasyEngine))]
    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed_twice : WithCurrentEngine<FakeItEasyEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.RemoveService(null);
            _fake.RemoveService(null);
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(null)).Twice());

        It should_not_throw_an_exception = () => _exception.ShouldBeNull();
    }

    [Subject(typeof(FakeItEasyEngine))]
    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed_twice_but_was_only_executed_once : WithCurrentEngine<FakeItEasyEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.RemoveService(null);
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(null)).Twice());

        It should_throw_an_exception = () => _exception.ShouldNotBeNull();
    }

    [Subject(typeof(FakeItEasyEngine))]
    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed_only_once_but_was_excuted_twice : WithCurrentEngine<FakeItEasyEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.RemoveService(null);
            _fake.RemoveService(null);
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(null)).OnlyOnce());

        It should_throw_an_exception = () => _exception.ShouldNotBeNull();
    }

    [Subject(typeof(FakeItEasyEngine))]
    public class Given_a_query_was_configured_on_a_fake_when_verifying_whether_it_was_executed : WithCurrentEngine<FakeItEasyEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.GetService(null);
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.GetService(null)));

        It should_not_throw_an_exception = () => _exception.ShouldBeNull();
    }

    [Subject(typeof(FakeItEasyEngine))]
    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed_only_once_with_a_given_parameter_but_was_executed_twice_in_total : WithCurrentEngine<FakeItEasyEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.RemoveService(typeof(Object));
            _fake.RemoveService(typeof(String));
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(typeof(String))).OnlyOnce());

        It should_not_throw_an_exception = () => _exception.ShouldBeNull();
    }
}

#endif

