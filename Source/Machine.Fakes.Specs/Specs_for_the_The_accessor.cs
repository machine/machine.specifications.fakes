using System;
using System.Data;

using Machine.Fakes.Sdk;
using Machine.Fakes.Specs.TestClasses;
using Machine.Specifications;

namespace Machine.Fakes.Specs
{
    public class Specs_for_the_The_accessor
    {
        [Subject(typeof(SpecificationController<>))]
        public class Given_that_a_subject_has_been_automatically_created_when_using_the__The__accessor
        {
            Establish context = () =>
            {
                _injectedFake = new Car();
                _fakeEngine = new DummyFakeEngine { CreatedFake = _injectedFake };
                _specController = new SpecificationController<Garage>(_fakeEngine);
                _subject = _specController.Subject;
            };

            Because of = () => _car = _specController.The<ICar>();

            It Should_be_able_to_get_hold_of_the_injected_dependency_of_the_specified_type =
                () => _car.ShouldBeTheSameAs(_injectedFake);

            static Garage _subject;
            static SpecificationController<Garage> _specController;
            static DummyFakeEngine _fakeEngine;
            static ICar _car;
            static Car _injectedFake;
        }

        [Subject(typeof(WithSubject<,>), "The")]
        public class When_called_from_a_static_initializer : WithSubject<object, DummyFakeEngine>
        {
            static Exception exception;

            static When_called_from_a_static_initializer()
            {
                exception = Catch.Exception(() => The<IDbConnection>());
            }

            It should_throw_the_right_exception = () => exception.ShouldBeOfType<InvalidOperationException>();
        }
    }
}