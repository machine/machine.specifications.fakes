using System;
using Machine.Fakes.Sdk;
using Machine.Fakes.Specs.TestClasses;
using Machine.Specifications;

namespace Machine.Fakes.Specs
{
    public class EmptyBehaviorConfig
    {
    }

    [Subject(typeof (SpecificationController<>))]
    [Tags("BehaviorConfigs")]
    public class When_executing_a_behavior_config_that_does_not_contain_the_relevant_delegate_definitions
    {
        static SpecificationController<object, DummyFakeEngine> _controller;
        static Exception _exception;

        Establish context = () => { _controller = new SpecificationController<object, DummyFakeEngine>(); };

        Because of = () => { _exception = Catch.Exception(() => _controller.With<EmptyBehaviorConfig>()); };

        It should_not_throw_any_exceptions = () => _exception.ShouldBeNull();
    }

    public class BehaviorConfigWithNonInitializedDelegates
    {
        OnEstablish context;
        OnCleanUp subject;
    }

    [Subject(typeof(SpecificationController<>))]
    [Tags("BehaviorConfigs")]
    public class When_executing_a_behavior_config_that_contains_non_initialized_delegate_fields
    {
        static SpecificationController<object, DummyFakeEngine> _controller;
        static Exception _exception;

        Establish context = () => { _controller = new SpecificationController<object, DummyFakeEngine>(); };

        Because of = () => { _exception = Catch.Exception(() => _controller.With<BehaviorConfigWithNonInitializedDelegates>()); };

        It should_not_throw_any_exceptions = () => _exception.ShouldBeNull();
    }

    public class SimpleBehaviorConfig
    {
        public static IFakeAccessor FakeAccessor;
        public static object Subject;

        OnEstablish context = fakeAccessor => FakeAccessor = fakeAccessor;

        OnCleanUp subject = subject => Subject = subject;
    }

    [Subject(typeof(SpecificationController<>))]
    [Tags("BehaviorConfigs")]
    public class When_adding_a_behavior_config
    {
        static SpecificationController<object> _controller;

        Establish context = () =>
        {
            _controller = new SpecificationController<object, DummyFakeEngine>();
        };

        Because of = () => _controller.With<SimpleBehaviorConfig>();

        It should_initialize_the_context_imediately = () => SimpleBehaviorConfig.FakeAccessor.ShouldNotBeNull();

        It should_not_cleanup_anything = () => SimpleBehaviorConfig.Subject.ShouldBeNull();
    }

    [Subject(typeof(SpecificationController<>))]
    [Tags("BehaviorConfigs")]
    public class When_cleaning_up_a_specification_controller
    {
        static SpecificationController<object> _controller;

        Establish context = () =>
        {
            _controller = new SpecificationController<object, DummyFakeEngine>();
            _controller.With<SimpleBehaviorConfig>();
        };

        Because of = () => _controller.Dispose();

        It should_cleanup_behavior_configs = () => SimpleBehaviorConfig.Subject.ShouldNotBeNull();
    }

    public class DerivedBehaviorConfig : SimpleBehaviorConfig
    {
        public static IFakeAccessor FakeAccessorDerived;
        public static object SubjectDerived;

        OnEstablish context = fakeAccessor => FakeAccessorDerived = fakeAccessor;

        OnCleanUp subject = subject => SubjectDerived = subject;
    }

    [Subject(typeof(SpecificationController<>))]
    [Tags("BehaviorConfigs")]
    public class When_adding_a_derived_behavior_config
    {
        static SpecificationController<object> _controller;

        Establish context = () =>
        {
            _controller = new SpecificationController<object, DummyFakeEngine>();
        };

        Because of = () => _controller.With<DerivedBehaviorConfig>();

        It should_execute_the_configuration_in_the_base_class = () => SimpleBehaviorConfig.FakeAccessor.ShouldNotBeNull();

        It should_execute_the_configuration_in_the_derived_class = () => DerivedBehaviorConfig.FakeAccessorDerived.ShouldNotBeNull();

        It should_not_cleanup_in_the_base_class = () => SimpleBehaviorConfig.Subject.ShouldBeNull();
        
        It should_not_cleanup_in_the_derived_class = () => DerivedBehaviorConfig.SubjectDerived.ShouldBeNull();
    }

    [Subject(typeof(SpecificationController<>))]
    [Tags("BehaviorConfigs")]
    public class When_cleaning_up_a_specification_controller_and_a_derived_behavior_config_has_been_configured
    {
        static SpecificationController<object> _controller;

        Establish context = () =>
        {
            _controller = new SpecificationController<object, DummyFakeEngine>();
            _controller.With<DerivedBehaviorConfig>();
        };

        Because of = () => _controller.Dispose();

        It should_execute_the_cleanup_logic_from_the_base_class = () => SimpleBehaviorConfig.Subject.ShouldNotBeNull();

        It should_execute_the_cleanup_logic_from_the_derived_class= () => DerivedBehaviorConfig.SubjectDerived.ShouldNotBeNull();
    }
}