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
        Cleanup after = () => _controller.Dispose();

        Establish context = () => { _controller = new SpecificationController<object, DummyFakeEngine>(); };

        Because of = () => { _exception = Catch.Exception(() => _controller.With<EmptyBehaviorConfig>()); };

        It should_not_throw_any_exceptions = () => _exception.ShouldBeNull();
    }

    public class BehaviorConfigWithNonInitializedDelegates
    {
        OnEstablish context;
        OnCleanup subject;
    }

    [Subject(typeof (SpecificationController<>))]
    [Tags("BehaviorConfigs")]
    public class When_executing_a_behavior_config_that_contains_non_initialized_delegate_fields
    {
        static SpecificationController<object, DummyFakeEngine> _controller;
        static Exception _exception;
        Cleanup after = () => _controller.Dispose();

        Establish context = () => { _controller = new SpecificationController<object, DummyFakeEngine>(); };

        Because of = () => { _exception = Catch.Exception(() => _controller.With<BehaviorConfigWithNonInitializedDelegates>()); };

        It should_not_throw_any_exceptions = () => _exception.ShouldBeNull();
    }

    public class SimpleBehaviorConfig
    {
        public OnEstablish ContextDelegate;
        public OnCleanup SubjectDelegate;
    }

    [Subject(typeof (SpecificationController<>))]
    [Tags("BehaviorConfigs")]
    public class When_adding_a_behavior_config
    {
        static SpecificationController<object> _controller;
        static SimpleBehaviorConfig _simpleBehaviorConfig;
        static IFakeAccessor _fakeAccessor;
        static object _subject;
        Cleanup after = () => _controller.Dispose();

        Establish context = () =>
        {
            _controller = new SpecificationController<object, DummyFakeEngine>();
            _simpleBehaviorConfig = new SimpleBehaviorConfig
            {
                ContextDelegate = fa => _fakeAccessor = fa,
                SubjectDelegate = subject => _subject = subject
            };
        };

        Because of = () => _controller.With(_simpleBehaviorConfig);

        It should_initialize_the_context_imediately = () => _fakeAccessor.ShouldNotBeNull();

        It should_not_cleanup_anything = () => _subject.ShouldBeNull();
    }

    [Subject(typeof (SpecificationController<>))]
    [Tags("BehaviorConfigs")]
    public class When_cleaning_up_a_specification_controller
    {
        static SpecificationController<object> _controller;
        static SimpleBehaviorConfig _simpleBehaviorConfig;
        static object _subject;

        Establish context = () =>
        {
            _controller = new SpecificationController<object, DummyFakeEngine>();
            _simpleBehaviorConfig = new SimpleBehaviorConfig
            {
                SubjectDelegate = subject => _subject = subject
            };
            _controller.With(_simpleBehaviorConfig);
        };

        Because of = () => _controller.Dispose();

        It should_cleanup_behavior_configs =
            () => _subject.ShouldNotBeNull();

        It should_reset_all_fields_in_the_behavior_configs =
            () => _simpleBehaviorConfig.SubjectDelegate.ShouldBeNull();
    }

    public class DerivedBehaviorConfig : SimpleBehaviorConfig
    {
        public OnEstablish DerivedContextDelegate;
        public OnCleanup DerivedSubjectDelegate;
    }

    [Subject(typeof (SpecificationController<>))]
    [Tags("BehaviorConfigs")]
    public class When_adding_a_derived_behavior_config
    {
        static SpecificationController<object> _controller;
        static SimpleBehaviorConfig _derivedBehaviorConfig;
        static IFakeAccessor _fakeAccessor;
        static object _subject;
        static IFakeAccessor _fakeAccessorDerived;
        static object _subjectDerived;
        
        Establish context = () =>
        {
            _controller = new SpecificationController<object, DummyFakeEngine>();

            _derivedBehaviorConfig = new DerivedBehaviorConfig
            {
                ContextDelegate = fa => _fakeAccessor = fa,
                SubjectDelegate = subject => _subject = subject,
                DerivedContextDelegate = fa => _fakeAccessorDerived = fa,
                DerivedSubjectDelegate = subject => _subjectDerived = subject
            };
        };

        Because of = () => _controller.With(_derivedBehaviorConfig);

        It should_execute_the_configuration_in_the_base_class = 
            () => _fakeAccessor.ShouldNotBeNull();

        It should_execute_the_configuration_in_the_derived_class = 
            () => _fakeAccessorDerived.ShouldNotBeNull();

        It should_not_cleanup_in_the_base_class = 
            () => _subject.ShouldBeNull();

        It should_not_cleanup_in_the_derived_class = 
            () => _subjectDerived.ShouldBeNull();

        Cleanup after = () => _controller.Dispose();
    }

    [Subject(typeof (SpecificationController<>))]
    [Tags("BehaviorConfigs")]
    public class When_cleaning_up_a_specification_controller_and_a_derived_behavior_config_has_been_configured
    {
        static SpecificationController<object> _controller;
        static IFakeAccessor _fakeAccessor;
        static object _subject;
        static IFakeAccessor _fakeAccessorDerived;
        static object _subjectDerived;

        Establish context = () =>
        {
            _controller = new SpecificationController<object, DummyFakeEngine>();

            _controller.With(new DerivedBehaviorConfig
            {
                ContextDelegate = fa => _fakeAccessor = fa,
                SubjectDelegate = subject => _subject = subject,
                DerivedContextDelegate = fa => _fakeAccessorDerived = fa,
                DerivedSubjectDelegate = subject => _subjectDerived = subject
            });
        };

        Because of = () => _controller.Dispose();

        It should_execute_the_cleanup_logic_from_the_base_class = 
            () => _subject.ShouldNotBeNull();

        It should_execute_the_cleanup_logic_from_the_derived_class = 
            () => _subjectDerived.ShouldNotBeNull();
    }
}