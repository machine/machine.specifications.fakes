using Machine.Specifications.Fakes.Sdk;
using Machine.Specifications.Fakes.Specs.TestClasses;

namespace Machine.Specifications.Fakes.Specs
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
}
