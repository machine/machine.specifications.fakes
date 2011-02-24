using System;
using Machine.Fakes.Internal;
using Machine.Fakes.Sdk;
using Machine.Fakes.Specs.TestClasses;
using Machine.Specifications;

namespace Machine.Fakes.Specs
{
    [Subject(typeof(SpecificationController<>))]
    public class Given_that_the_specification_subject_is_created_and_configured_by_hand_when_accessing_the_subject_property
    {
        Establish context = () =>
        {
            var dummyFakeEngine = new DummyFakeEngine();
            _specController = new SpecificationController<object>(dummyFakeEngine);
            _configuredSubject = new object();
            _specController.Subject = _configuredSubject;
        };

        Because of = () => _subject = _specController.Subject;

        It Should_return_the_hand_rolled_subject = () => _subject.ShouldBeTheSameAs(_configuredSubject);

        static object _configuredSubject;
        static object _subject;
        static SpecificationController<object> _specController;
    }

    [Subject(typeof(SpecificationController<>))]
    public class When_accessing_the_subject_property_for_the_first_time_without_configuring_it
    {
        Establish context = () =>
        {
            var dummyFakeEngine = new DummyFakeEngine();
            _specController = new SpecificationController<object>(dummyFakeEngine);
        };

        Because of = () => _subject = _specController.Subject;

        It Should_automatically_create_the_subject = () => _subject.ShouldNotBeNull();

        static object _subject;
        static SpecificationController<object> _specController;
    }

    [Subject(typeof(SpecificationController<>))]
    public class When_the_subjects_contstructor_contains_an_interface_dependency
    {
        Establish context = () =>
        {
            _fakeEngine = new DummyFakeEngine
            {
                CreatedFake = new Car()
            };

            _specController = new SpecificationController<Garage>(_fakeEngine);
        };

        Because of = () => _subject = _specController.Subject;

        It Should_be_able_to_build_the_subject = () => _subject.ShouldNotBeNull();

        It Should_reach_out_to_the_fake_engine_and_create_an_implementation_for_that_dependency =
            () => _fakeEngine.RequestedFakeType.ShouldEqual(typeof(ICar));

        static Garage _subject;
        static SpecificationController<Garage> _specController;
        static DummyFakeEngine _fakeEngine;
    }

    [Subject(typeof(SpecificationController<>))]
    public class When_the_subject_class_contains_more_than_one_constructor
    {
        Establish context = () =>
        {
            _fakeEngine = new DummyFakeEngine
            {
                CreatedFake = new Car()
            };

            _specController = new SpecificationController<WithMultipleConstructors>(_fakeEngine);
        };

        Because of = () => _subject = _specController.Subject;

        It Should_be_able_to_build_the_subject = () => _subject.ShouldNotBeNull();

        It Should_build_the_subject_using_the_greediest_constructor =
            () => _fakeEngine.RequestedFakeType.ShouldEqual(typeof(ICar));

        static WithMultipleConstructors _subject;
        static SpecificationController<WithMultipleConstructors> _specController;
        static DummyFakeEngine _fakeEngine;
    }

    [Subject(typeof(SpecificationController<>))]
    public class When_automatically_creating_the_subject_fails_due_to_an_exception_in_the_subjects_constructor
    {
        Establish context = () =>
        {
            _specController = new SpecificationController<Bumsdi, DummyFakeEngine>();
        };

        Because of = () => _exception = Catch.Exception(() => _subject = _specController.Subject);

        It Should_throw_a__SubjectCreationException__ =
            () => _exception.ShouldBeOfType<SubjectCreationException>();

        It Should_indicate_that_it_was_unable_to_create_the_subject =
            () => _exception.Message.ShouldStartWith("Unable to create an instance of the target type Bumsdi");

        It Should_indicate_that_an_exception_was_thrown_in_the_constructor =
            () => _exception.Message.ShouldEndWith("The constructor threw an exception.");

        static SpecificationController<Bumsdi, DummyFakeEngine> _specController;
        static Bumsdi _subject;
        static Exception _exception;
    }

    [Subject(typeof(SpecificationController<>))]
    public class When_automatically_creating_the_subject_fails_due_to_a_missing_public_constructor
    {
        Establish context = () =>
        {
            _specController = new SpecificationController<WithoutPublicConstructor, DummyFakeEngine>();
        };

        Because of = () => _exception = Catch.Exception(() => _subject = _specController.Subject);

        It Should_throw_a__SubjectCreationException__ =
            () => _exception.ShouldBeOfType<SubjectCreationException>();

        It Should_indicate_that_it_was_unable_to_create_the_subject =
            () => _exception.Message.ShouldStartWith("Unable to create an instance of the target type WithoutPublicConstructor");

        It Should_indicate_that_no_public_constructor_is_available =
            () => _exception.Message.ShouldEndWith("Please check that the type has at least a single public constructor!");

        static SpecificationController<WithoutPublicConstructor, DummyFakeEngine> _specController;
        static WithoutPublicConstructor _subject;
        static Exception _exception;
    }
}