using System;

using FakeItEasy.Creation;
using Machine.Fakes.Adapters.FakeItEasy;
using Machine.Fakes.Adapters.Specs.SampleCode;
using Machine.Specifications;

namespace Machine.Fakes.Adapters.Specs.FakeItEasy
{
    [Tags("FakeItEasy")]
    [Subject(typeof(FakeItEasyHelper))]
    public class When_Helper_is_requested_to_create_a_type_with_parameters
    {
        static Delegate action;

        Because of = () => 
            action = FakeItEasyHelper.CreateForType(typeof(DummyNoDefaultCtorClass), new object[] { 100 });

        It must_return_a_non_null_delegate = () => 
            action.ShouldNotBeNull();

        It must_return_an_action_with_fake_options_delegate = () => 
            action.ShouldBeOfType<Action<IFakeOptionsBuilder<DummyNoDefaultCtorClass>>>();
    }
}