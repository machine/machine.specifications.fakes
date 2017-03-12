






using Machine.Fakes.Adapters.Specs.SampleCode;
using Machine.Fakes.Internal;
using Machine.Specifications;



#if !NETSTANDARD


namespace Machine.Fakes.Adapters.Specs.Rhinomocks
{

	using Machine.Fakes.Adapters.Rhinomocks;

    [Subject(typeof(RhinoFakeEngine))]
    public class When_setting_a_simple_property_to_value_2 : WithCurrentEngine<RhinoFakeEngine>
    {
        Establish context = () => _fake = FakeEngineGateway.Fake<IProperties>();

        Because of = () => _fake.Prop = 2;

        It should_have_value_2 = () => _fake.Prop.ShouldEqual(2);
        
        static IProperties _fake;
    }

    [Subject(typeof(RhinoFakeEngine))]
    public class When_setting_a_behavior_on_a_function : WithCurrentEngine<RhinoFakeEngine>
    {
        Establish context = () => _fake = FakeEngineGateway.Fake<IProperties>();

        Because of = () =>
        {
            _fake.WhenToldTo(f => f.TheFunction()).Return(true);
            _fake.Prop = 3;
        };

        It should_not_remove_the_property_behavior = () => _fake.Prop.ShouldEqual(3);
        
        static IProperties _fake;
    }

    [Subject(typeof(RhinoFakeEngine))]
    public class When_setting_a_simple_property_to_value_2_and_then_to_3 : WithCurrentEngine<RhinoFakeEngine>
    {
        Establish context = () => _fake = FakeEngineGateway.Fake<IProperties>();

        Because of = () =>
        {
            _fake.Prop = 2;
            _fake.Prop = 3;
        };

        It should_have_value_3 = () => _fake.Prop.ShouldEqual(3);

        static IProperties _fake;
    }

    [Subject(typeof(RhinoFakeEngine))]
    public class When_setting_up_a_return_value_for_a_property_with_getter_and_setter : WithCurrentEngine<RhinoFakeEngine>
    {
        Establish context = () => _fake = FakeEngineGateway.Fake<IProperties>();

        Because of = () => _fake.WhenToldTo(a => a.Prop).Return(2);

        It should_return_the_value = () => _fake.Prop.ShouldEqual(2);

        static IProperties _fake;
    }
}

#endif





namespace Machine.Fakes.Adapters.Specs.NSubstitute
{

	using Machine.Fakes.Adapters.NSubstitute;

    [Subject(typeof(NSubstituteEngine))]
    public class When_setting_a_simple_property_to_value_2 : WithCurrentEngine<NSubstituteEngine>
    {
        Establish context = () => _fake = FakeEngineGateway.Fake<IProperties>();

        Because of = () => _fake.Prop = 2;

        It should_have_value_2 = () => _fake.Prop.ShouldEqual(2);
        
        static IProperties _fake;
    }

    [Subject(typeof(NSubstituteEngine))]
    public class When_setting_a_behavior_on_a_function : WithCurrentEngine<NSubstituteEngine>
    {
        Establish context = () => _fake = FakeEngineGateway.Fake<IProperties>();

        Because of = () =>
        {
            _fake.WhenToldTo(f => f.TheFunction()).Return(true);
            _fake.Prop = 3;
        };

        It should_not_remove_the_property_behavior = () => _fake.Prop.ShouldEqual(3);
        
        static IProperties _fake;
    }

    [Subject(typeof(NSubstituteEngine))]
    public class When_setting_a_simple_property_to_value_2_and_then_to_3 : WithCurrentEngine<NSubstituteEngine>
    {
        Establish context = () => _fake = FakeEngineGateway.Fake<IProperties>();

        Because of = () =>
        {
            _fake.Prop = 2;
            _fake.Prop = 3;
        };

        It should_have_value_3 = () => _fake.Prop.ShouldEqual(3);

        static IProperties _fake;
    }

    [Subject(typeof(NSubstituteEngine))]
    public class When_setting_up_a_return_value_for_a_property_with_getter_and_setter : WithCurrentEngine<NSubstituteEngine>
    {
        Establish context = () => _fake = FakeEngineGateway.Fake<IProperties>();

        Because of = () => _fake.WhenToldTo(a => a.Prop).Return(2);

        It should_return_the_value = () => _fake.Prop.ShouldEqual(2);

        static IProperties _fake;
    }
}







namespace Machine.Fakes.Adapters.Specs.Moq
{

	using Machine.Fakes.Adapters.Moq;

    [Subject(typeof(MoqFakeEngine))]
    public class When_setting_a_simple_property_to_value_2 : WithCurrentEngine<MoqFakeEngine>
    {
        Establish context = () => _fake = FakeEngineGateway.Fake<IProperties>();

        Because of = () => _fake.Prop = 2;

        It should_have_value_2 = () => _fake.Prop.ShouldEqual(2);
        
        static IProperties _fake;
    }

    [Subject(typeof(MoqFakeEngine))]
    public class When_setting_a_behavior_on_a_function : WithCurrentEngine<MoqFakeEngine>
    {
        Establish context = () => _fake = FakeEngineGateway.Fake<IProperties>();

        Because of = () =>
        {
            _fake.WhenToldTo(f => f.TheFunction()).Return(true);
            _fake.Prop = 3;
        };

        It should_not_remove_the_property_behavior = () => _fake.Prop.ShouldEqual(3);
        
        static IProperties _fake;
    }

    [Subject(typeof(MoqFakeEngine))]
    public class When_setting_a_simple_property_to_value_2_and_then_to_3 : WithCurrentEngine<MoqFakeEngine>
    {
        Establish context = () => _fake = FakeEngineGateway.Fake<IProperties>();

        Because of = () =>
        {
            _fake.Prop = 2;
            _fake.Prop = 3;
        };

        It should_have_value_3 = () => _fake.Prop.ShouldEqual(3);

        static IProperties _fake;
    }

    [Subject(typeof(MoqFakeEngine))]
    public class When_setting_up_a_return_value_for_a_property_with_getter_and_setter : WithCurrentEngine<MoqFakeEngine>
    {
        Establish context = () => _fake = FakeEngineGateway.Fake<IProperties>();

        Because of = () => _fake.WhenToldTo(a => a.Prop).Return(2);

        It should_return_the_value = () => _fake.Prop.ShouldEqual(2);

        static IProperties _fake;
    }
}







namespace Machine.Fakes.Adapters.Specs.FakeItEasy
{

	using Machine.Fakes.Adapters.FakeItEasy;

    [Subject(typeof(FakeItEasyEngine))]
    public class When_setting_a_simple_property_to_value_2 : WithCurrentEngine<FakeItEasyEngine>
    {
        Establish context = () => _fake = FakeEngineGateway.Fake<IProperties>();

        Because of = () => _fake.Prop = 2;

        It should_have_value_2 = () => _fake.Prop.ShouldEqual(2);
        
        static IProperties _fake;
    }

    [Subject(typeof(FakeItEasyEngine))]
    public class When_setting_a_behavior_on_a_function : WithCurrentEngine<FakeItEasyEngine>
    {
        Establish context = () => _fake = FakeEngineGateway.Fake<IProperties>();

        Because of = () =>
        {
            _fake.WhenToldTo(f => f.TheFunction()).Return(true);
            _fake.Prop = 3;
        };

        It should_not_remove_the_property_behavior = () => _fake.Prop.ShouldEqual(3);
        
        static IProperties _fake;
    }

    [Subject(typeof(FakeItEasyEngine))]
    public class When_setting_a_simple_property_to_value_2_and_then_to_3 : WithCurrentEngine<FakeItEasyEngine>
    {
        Establish context = () => _fake = FakeEngineGateway.Fake<IProperties>();

        Because of = () =>
        {
            _fake.Prop = 2;
            _fake.Prop = 3;
        };

        It should_have_value_3 = () => _fake.Prop.ShouldEqual(3);

        static IProperties _fake;
    }

    [Subject(typeof(FakeItEasyEngine))]
    public class When_setting_up_a_return_value_for_a_property_with_getter_and_setter : WithCurrentEngine<FakeItEasyEngine>
    {
        Establish context = () => _fake = FakeEngineGateway.Fake<IProperties>();

        Because of = () => _fake.WhenToldTo(a => a.Prop).Return(2);

        It should_return_the_value = () => _fake.Prop.ShouldEqual(2);

        static IProperties _fake;
    }
}



