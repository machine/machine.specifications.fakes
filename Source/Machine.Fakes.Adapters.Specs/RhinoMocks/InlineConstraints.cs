using Machine.Fakes.Adapters.Rhinomocks;
using Machine.Fakes.Adapters.Specs.SampleCode;
using Machine.Fakes.Internal;
using Machine.Specifications;

namespace Machine.Fakes.Adapters.Specs.RhinoMocks
{
    [Subject(typeof(RhinoFakeEngine))] 
    [Tags("Inline constraints", "Rhinomocks")]
    public class When_matching_any_parameter_value_and_passing_null : WithCurrentEngine<RhinoFakeEngine>
    {
        static IView _view;
        static bool _configuredBehaviorWasTriggered;

        Establish context = () =>
        {
            _view = FakeEngineGateway.Fake<IView>();

            _view.WhenToldTo(v => v.TryLogin(Param.IsAny<string>(), Param.IsAny<string>()))
                .Return(true);
        };

        Because of = () => { _configuredBehaviorWasTriggered = _view.TryLogin(null, null); };

        It should_have_trigged_the_configured_behavior = () => _configuredBehaviorWasTriggered.ShouldBeTrue();
    }

    [Subject(typeof(RhinoFakeEngine))] 
    [Tags("Inline constraints", "Rhinomocks")]
    public class When_matching_any_parameter_value_and_passing_a_non_null_value : WithCurrentEngine<RhinoFakeEngine>
    {
        static IView _view;
        static bool _configuredBehaviorWasTriggered;

        Establish context = () =>
        {
            _view = FakeEngineGateway.Fake<IView>();

            _view.WhenToldTo(v => v.TryLogin(Param.IsAny<string>(), Param.IsAny<string>()))
                .Return(true);
        };

        Because of = () => { _configuredBehaviorWasTriggered = _view.TryLogin("NON_NULL", "ALSO_NON_NULL"); };

        It should_have_trigged_the_configured_behavior = () => _configuredBehaviorWasTriggered.ShouldBeTrue();
    }

    [Subject(typeof(RhinoFakeEngine))]
    [Tags("Inline constraints", "Rhinomocks")]
    public class When_matching_only_non_null_parameter_values_and_passing_in_null : WithCurrentEngine<RhinoFakeEngine>
    {
        static IView _view;
        static bool _configuredBehaviorWasTriggered;

        Establish context = () =>
        {
            _view = FakeEngineGateway.Fake<IView>();

            _view.WhenToldTo(v => v.TryLogin(Param<string>.IsNotNull, Param<string>.IsNotNull))
                .Return(true);
        };

        Because of = () => { _configuredBehaviorWasTriggered = _view.TryLogin(null, null); };

        It should_not_have_trigged_the_configured_behavior = () => _configuredBehaviorWasTriggered.ShouldBeFalse();
    }

    [Subject(typeof(RhinoFakeEngine))]
    [Tags("Inline constraints", "Rhinomocks")]
    public class When_matching_only_non_null_parameter_values_and_passing_in_non_null_values : WithCurrentEngine<RhinoFakeEngine>
    {
        static IView _view;
        static bool _configuredBehaviorWasTriggered;

        Establish context = () =>
        {
            _view = FakeEngineGateway.Fake<IView>();

            _view.WhenToldTo(v => v.TryLogin(Param<string>.IsNotNull, Param<string>.IsNotNull))
                .Return(true);
        };

        Because of = () => { _configuredBehaviorWasTriggered = _view.TryLogin("NON_NULL", "ALSO_NON_NULL"); };

        It should_have_trigged_the_configured_behavior = () => _configuredBehaviorWasTriggered.ShouldBeTrue();
    }

    [Subject(typeof(RhinoFakeEngine))]
    [Tags("Inline constraints", "Rhinomocks")]
    public class When_matching_only_null_parameter_values_and_passing_in_null : WithCurrentEngine<RhinoFakeEngine>
    {
        static IView _view;
        static bool _configuredBehaviorWasTriggered;

        Establish context = () =>
        {
            _view = FakeEngineGateway.Fake<IView>();

            _view.WhenToldTo(v => v.TryLogin(Param<string>.IsNull, Param<string>.IsNull))
                .Return(true);
        };

        Because of = () => { _configuredBehaviorWasTriggered = _view.TryLogin(null, null); };

        It should_have_trigged_the_configured_behavior = () => _configuredBehaviorWasTriggered.ShouldBeTrue();
    }

    [Subject(typeof(RhinoFakeEngine))]
    [Tags("Inline constraints", "Rhinomocks")]
    public class When_matching_only_null_parameter_values_and_passing_in_non_null_values : WithCurrentEngine<RhinoFakeEngine>
    {
        static IView _view;
        static bool _configuredBehaviorWasTriggered;

        Establish context = () =>
        {
            _view = FakeEngineGateway.Fake<IView>();

            _view.WhenToldTo(v => v.TryLogin(Param<string>.IsNull, Param<string>.IsNull))
                .Return(true);
        };

        Because of = () => { _configuredBehaviorWasTriggered = _view.TryLogin("NON_NULL", "ALSO_NON_NULL"); };

        It should_not_have_triggered_the_configured_behavior = () => _configuredBehaviorWasTriggered.ShouldBeFalse();
    }

    [Subject(typeof(RhinoFakeEngine))]
    [Tags("Inline constraints", "Rhinomocks")]
    public class When_matching_only_on_a_particular_value_and_passing_in_that_value : WithCurrentEngine<RhinoFakeEngine>
    {
        static IView _view;
        static bool _configuredBehaviorWasTriggered;

        Establish context = () =>
        {
            _view = FakeEngineGateway.Fake<IView>();

            _view.WhenToldTo(v => v.TryLogin(Param.Is("John"), Param.Is("Doe")))
                .Return(true);
        };

        Because of = () => { _configuredBehaviorWasTriggered = _view.TryLogin("John", "Doe"); };

        It should_have_triggered_the_configured_behavior = () => _configuredBehaviorWasTriggered.ShouldBeTrue();
    }
    
    [Subject(typeof(RhinoFakeEngine))]
    [Tags("Inline constraints", "Rhinomocks")]
    public class When_matching_only_on_a_particular_value_and_passing_different_values : WithCurrentEngine<RhinoFakeEngine>
    {
        static IView _view;
        static bool _configuredBehaviorWasTriggered;

        Establish context = () =>
        {
            _view = FakeEngineGateway.Fake<IView>();

            _view.WhenToldTo(v => v.TryLogin(Param.Is("John"), Param.Is("Doe")))
                .Return(true);
        };

        Because of = () => { _configuredBehaviorWasTriggered = _view.TryLogin("NOT_John", "NOT_Doe"); };

        It should_not_have_triggered_the_configured_behavior = () => _configuredBehaviorWasTriggered.ShouldBeFalse();
    }

    [Subject(typeof(RhinoFakeEngine))]
    [Tags("Inline constraints", "Rhinomocks")]
    public class When_matching_using_expressions_and_passing_in_a_value_that_matches_the_expression : WithCurrentEngine<RhinoFakeEngine>
    {
        static IView _view;
        static bool _configuredBehaviorWasTriggered;

        Establish context = () =>
        {
            _view = FakeEngineGateway.Fake<IView>();

            _view.WhenToldTo(v => v.TryLogin(
                    Param<string>.Matches(p1 => p1.StartsWith("J")), 
                    Param<string>.Matches(p2 => p2.StartsWith("D"))))
                .Return(true);
        };

        Because of = () => { _configuredBehaviorWasTriggered = _view.TryLogin("John", "Doe"); };

        It should_have_triggered_the_configured_behavior = () => _configuredBehaviorWasTriggered.ShouldBeTrue();
    }
    
    [Subject(typeof(RhinoFakeEngine))]
    [Tags("Inline constraints", "Rhinomocks")]
    public class When_matching_using_expressions_and_passing_in_a_value_that_does_not_match_the_expression : WithCurrentEngine<RhinoFakeEngine>
    {
        static IView _view;
        static bool _configuredBehaviorWasTriggered;

        Establish context = () =>
        {
            _view = FakeEngineGateway.Fake<IView>();

            _view.WhenToldTo(v => v.TryLogin(
                    Param<string>.Matches(p1 => p1.StartsWith("J")),
                    Param<string>.Matches(p2 => p2.StartsWith("D"))))
                .Return(true);
        };

        Because of = () => { _configuredBehaviorWasTriggered = _view.TryLogin("NOT_John", "NOT_Doe"); };

        It should_not_have_triggered_the_configured_behavior = () => _configuredBehaviorWasTriggered.ShouldBeFalse();
    }

    [Subject(typeof(RhinoFakeEngine))]
    [Tags("Inline constraints", "Rhinomocks")]
    public class When_matching_only_when_a_type_implements_an_interface_and_the_parameter_value_implements_that_interface : WithCurrentEngine<RhinoFakeEngine>
    {
        static IFlashVerifier _flashVerifier;
        static bool _configuredBehaviorWasTriggered;

        Establish context = () =>
        {
            _flashVerifier = FakeEngineGateway.Fake<IFlashVerifier>();

            _flashVerifier
                .WhenToldTo(v => v.CanPlayFlash(Param<ITablet>.IsA<ICanPlayFlash>()))
                .Return(true);
        };

        Because of = () =>
        {
            _configuredBehaviorWasTriggered = _flashVerifier.CanPlayFlash(new Xoom());
        };

        It should_have_triggered_the_configured_behavior = () => _configuredBehaviorWasTriggered.ShouldBeTrue();
    }

    [Subject(typeof(RhinoFakeEngine))]
    [Tags("Inline constraints", "Rhinomocks")]
    public class When_matching_only_when_a_type_implements_an_interface_and_the_parameter_value_does_not_implement_that_interface : WithCurrentEngine<RhinoFakeEngine>
    {
        static IFlashVerifier _flashVerifier;
        static bool _configuredBehaviorWasTriggered;

        Establish context = () =>
        {
            _flashVerifier = FakeEngineGateway.Fake<IFlashVerifier>();

            _flashVerifier
                .WhenToldTo(v => v.CanPlayFlash(Param<ITablet>.IsA<ICanPlayFlash>()))
                .Return(true);
        };

        Because of = () =>
        {
            _configuredBehaviorWasTriggered = _flashVerifier.CanPlayFlash(new IPad());
        };

        It should_not_have_triggered_the_configured_behavior = () => _configuredBehaviorWasTriggered.ShouldBeFalse();
    }
}