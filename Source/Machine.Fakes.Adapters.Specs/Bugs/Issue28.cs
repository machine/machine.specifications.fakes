using System;
using Machine.Fakes.Adapters.Rhinomocks;
using Machine.Specifications;
using Rhino.Mocks;

namespace Machine.Fakes.Adapters.Specs.Bugs
{
    [Subject("Issue 28")]
    [Tags("Bug", "Issue 28")]
    public class When_the_subject_is_only_used_indirectly_in_a_spec : WithSubject<ViewModel, RhinoFakeEngine>
    {
        Because of = () => The<IObservable>()
            .GetEventRaiser(s => s.SomethingHappened += null)
            .Raise(null, EventArgs.Empty);

        It should_be_automatically_created_before_the_Because_method_has_been_called = () =>
            Subject.SomethingHappened.ShouldBeTrue();
    }

    public class ViewModel
    {
        readonly IObservable _observable;

        public bool SomethingHappened;

        public ViewModel(IObservable observable)
        {
            _observable = observable;
            _observable.SomethingHappened += (s, e) => SomethingHappened = true;
        }
    }

    public interface IObservable
    {
        event EventHandler SomethingHappened;
    }
}