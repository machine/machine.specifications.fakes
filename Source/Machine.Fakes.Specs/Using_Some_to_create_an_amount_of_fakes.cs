using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

using Machine.Fakes.Sdk;
using Machine.Fakes.Specs.TestClasses;
using Machine.Specifications;

namespace Machine.Fakes.Specs
{
    [Subject(typeof(SpecificationController<,>))]
    public class When_specifying_a_negative_amount_of_fakes_to_the_Some_method
    {
        const int NEGATIVE_AMOUNT = -1;

        Establish context = () => _specController = new SpecificationController<object, DummyFakeEngine>();

        Because of = () => _exception = Catch.Exception(() => _specController.Some<IServiceContainer>(NEGATIVE_AMOUNT));

        It Should_throw_an_exception = () => _exception.ShouldNotBeNull();

        It Should_throw_an_ArgumentOutOfRangeException = () => _exception.ShouldBeOfExactType<ArgumentOutOfRangeException>();

        static SpecificationController<object, DummyFakeEngine> _specController;
        static Exception _exception;
    }

    [Subject(typeof(SpecificationController<,>))]
    public class When_using_the_Some_method_and_specifying_the_amount_of_fakes_to_create
    {
        const int CONFIGURED_AMOUNT = 10;
        
        Establish context = () => _specController = new SpecificationController<object, DummyFakeEngine>();

        Because of = () => _fakes = _specController.Some<IServiceContainer>(CONFIGURED_AMOUNT);

        It Should_not_return_null = () => _fakes.ShouldNotBeNull();

        It Should_return_a_list_whose_legth_matches_the_configured_amount_of_fakes = () => _fakes.Count.ShouldEqual(CONFIGURED_AMOUNT);

        static SpecificationController<object, DummyFakeEngine> _specController;
        static IList<IServiceContainer> _fakes;
    }

    [Subject(typeof(SpecificationController<,>))]
    public class When_using_the_Some_method
    {
        Establish context = () => _specController = new SpecificationController<object, DummyFakeEngine>();

        Because of = () => _fakes = _specController.Some<IServiceContainer>();

        It Should_not_return_null = () => _fakes.ShouldNotBeNull();

        It Should_return_a_list_of_three_fakes = () => _fakes.Count.ShouldEqual(3);

        static SpecificationController<object, DummyFakeEngine> _specController;
        static IList<IServiceContainer> _fakes;
    }
}