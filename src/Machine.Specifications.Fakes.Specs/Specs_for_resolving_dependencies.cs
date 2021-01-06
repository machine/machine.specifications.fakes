﻿using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications.Fakes.Internal;
using Machine.Specifications.Fakes.Specs.TestClasses;

namespace Machine.Specifications.Fakes.Specs
{
    [Subject(typeof(AutoFakeContainer))]
    public class When_the_subjects_contstructor_contains_an_interface_dependency
    {
        Establish context = () =>
            autoFakeContainer = new AutoFakeContainer(fakeEngine = new DummyFakeEngine());

        Because of = () => subject = autoFakeContainer.CreateSubject<Garage>();

        It should_be_able_to_build_the_subject = () => subject.ShouldNotBeEmpty();

        It should_reach_out_to_the_fake_engine_and_create_an_implementation_for_that_dependency =
            () => fakeEngine.RequestedFakeType.ShouldEqual(typeof(ICar));

        static Garage subject;
        static DummyFakeEngine fakeEngine;
        static AutoFakeContainer autoFakeContainer;
    }

    [Subject(typeof(AutoFakeContainer))]
    public class When_the_subject_class_has_multiple_constructors
    {
        Establish context = () =>
            autoFakeContainer = new AutoFakeContainer(fakeEngine = new DummyFakeEngine());

        Because of = () => subject = autoFakeContainer.CreateSubject<WithMultipleConstructors>();

        It should_be_able_to_build_the_subject = () => subject.ShouldNotBeNull();

        It should_build_the_subject_using_the_greediest_constructor = () =>
            fakeEngine.RequestedFakeType.ShouldEqual(typeof(ICar));

        static WithMultipleConstructors subject;
        static DummyFakeEngine fakeEngine;
        static AutoFakeContainer autoFakeContainer;
    }

    [Subject(typeof(AutoFakeContainer))]
    public class When_the_subject_class_has_multiple_contructors_with_the_same_number_of_parameters
    {
        Establish context = () =>
        {
            autoFakeContainer = new AutoFakeContainer(new DummyFakeEngine());
            autoFakeContainer.Register(new TypeMapping(typeof(ICar), typeof(Car)));
        };

        Because of = () => subject = autoFakeContainer.CreateSubject<WithMultipleContructorsOfSameParameterNumber>();

        It should_use_the_constructor_whose_parameter_type_has_been_set_up = () =>
        {
            subject.Car.ShouldNotBeNull();
            subject.Driver.ShouldBeNull();
            subject.Garage.ShouldBeNull();
        };

        static WithMultipleContructorsOfSameParameterNumber subject;
        static AutoFakeContainer autoFakeContainer;
    }

    [Subject(typeof(AutoFakeContainer))]
    public class When_automatically_creating_the_subject_fails_due_to_an_exception_in_the_subjects_constructor
    {
        Establish context = () =>
            autoFakeContainer = new AutoFakeContainer(new DummyFakeEngine());

        Because of = () => _exception = Catch.Exception(() => subject = autoFakeContainer.CreateSubject<Bumsdi>());

        It should_throw_an_Exception =
            () => _exception.ShouldBeOfExactType<InstanceCreationException>();

        It should_indicate_that_it_was_unable_to_create_the_subject =
            () => _exception.Message.ShouldStartWith("Unable to create an instance of type Bumsdi");

        It should_indicate_that_an_exception_was_thrown_in_the_constructor =
            () => _exception.Message.ShouldEndWith("The constructor threw an exception.");

        It should_bubble_up_the_exception = () =>
            _exception.InnerException.Message.ShouldContain("Bumsdi");

        static Bumsdi subject;
        static Exception _exception;
        static AutoFakeContainer autoFakeContainer;
    }

    [Subject(typeof(AutoFakeContainer))]
    public class When_automatically_creating_the_subject_fails_due_to_a_missing_public_constructor
    {
        Establish context = () =>
            autoFakeContainer = new AutoFakeContainer(new DummyFakeEngine());

        Because of = () => _exception = Catch.Exception(() =>
            subject = autoFakeContainer.CreateSubject<WithoutPublicConstructor>());

        It should_throw_an_Exception =
            () => _exception.ShouldBeOfExactType<InstanceCreationException>();

        It should_indicate_that_it_was_unable_to_create_the_subject =
            () => _exception.Message.ShouldStartWith("Unable to create an instance of type WithoutPublicConstructor");

        It should_indicate_that_no_public_constructor_is_available =
            () => _exception.Message.ShouldEndWith("Please check that the type has at least a single public constructor.");

        static WithoutPublicConstructor subject;
        static Exception _exception;
        static AutoFakeContainer autoFakeContainer;
    }

    [Subject(typeof(AutoFakeContainer))]
    public class When_the_subject_has_a_value_type_constructor_parameter
    {
        Establish context = () =>
            autoFakeContainer = new AutoFakeContainer(new DummyFakeEngine());

        Because of = () => subject = autoFakeContainer.CreateSubject<WithBoolInConstructor>();

        It should_use_the_default_value = () => subject.Yes.ShouldEqual(default(bool));

        static WithBoolInConstructor subject;
        static AutoFakeContainer autoFakeContainer;
    }

    [Subject(typeof(AutoFakeContainer))]
    public class When_the_subject_has_a_string_constructor_parameter
    {
        Establish context = () =>
            autoFakeContainer = new AutoFakeContainer(new DummyFakeEngine());

        Because of = () => subject = autoFakeContainer.CreateSubject<WithStringInConstructor>();

        It should_use_the_empty_string = () => subject.Content.ShouldEqual("");

        static WithStringInConstructor subject;
        static AutoFakeContainer autoFakeContainer;
    }

    [Subject(typeof(AutoFakeContainer))]
    public class When_the_subject_has_a_value_type_constructor_parameter_and_a_value_to_use_has_been_configured
    {
        Establish context = () =>
        {
            autoFakeContainer = new AutoFakeContainer(new DummyFakeEngine());
            autoFakeContainer.Register(new ObjectMapping(typeof(bool), true));
        };

        Because of = () => subject = autoFakeContainer.CreateSubject<WithBoolInConstructor>();

        It should_use_the_configured_value = () => subject.Yes.ShouldBeTrue();

        static WithBoolInConstructor subject;
        static AutoFakeContainer autoFakeContainer;
    }

    [Subject(typeof(AutoFakeContainer))]
    public class When_the_subject_has_an__IEnumerable__of_an_interface_type_as_constructor_parameter
    {
        Establish context = () =>
            autoFakeContainer = new AutoFakeContainer(new DummyFakeEngine());

        Because of = () => subject = autoFakeContainer.CreateSubject<WithEnumerableInterfaceInConstructor>();

        It should_create_an_empty_enumerable = () =>
        {
            subject.Cars.ShouldNotBeNull();
            subject.Cars.ShouldBeEmpty();
        };

        static WithEnumerableInterfaceInConstructor subject;
        static AutoFakeContainer autoFakeContainer;
    }

    [Subject(typeof(AutoFakeContainer))]
    public class When_the_subject_has_an__IEnumerable__of_a_concrete_type_as_constructor_parameter
    {
        Establish context = () =>
            autoFakeContainer = new AutoFakeContainer(new DummyFakeEngine());

        Because of = () => subject = autoFakeContainer.CreateSubject<WithEnumerableInConstructor>();

        It should_create_an_empty_enumerable = () =>
        {
            subject.Cars.ShouldNotBeNull();
            subject.Cars.ShouldBeEmpty();
        };

        static WithEnumerableInConstructor subject;
        static AutoFakeContainer autoFakeContainer;
    }

    [Subject(typeof(AutoFakeContainer))]
    public class When_the_subject_has_an__IEnumerable__of_an_interface_type_as_constructor_parameter_and_an_implementation_has_been_configured_for_the_interface
    {
        Establish context = () =>
        {
            autoFakeContainer = new AutoFakeContainer(new DummyFakeEngine());
            autoFakeContainer.Register(new TypeMapping(typeof(ICar), typeof(Car)));
        };

        Because of = () => subject = autoFakeContainer.CreateSubject<WithEnumerableInterfaceInConstructor>();

        It should_create_an_enumerable = () => subject.Cars.ShouldNotBeNull();

        It should_add_one_fake_instance_to_the_enumerable = () => subject.Cars.Count().ShouldEqual(1);

        static WithEnumerableInterfaceInConstructor subject;
        static AutoFakeContainer autoFakeContainer;
    }

    [Subject(typeof(AutoFakeContainer))]
    public class When_the_subject_has_an__IEnumerable__of_an_interface_type_as_constructor_parameter_and_two_implementations_have_been_configured_for_the_interface
    {
        Establish context = () =>
        {
            autoFakeContainer = new AutoFakeContainer(new DummyFakeEngine());
            autoFakeContainer.Register(new TypeMapping(typeof(ICar), typeof(Car)));
            autoFakeContainer.Register(new TypeMapping(typeof(ICar), typeof(Car2)));
        };

        Because of = () => cars = autoFakeContainer.CreateSubject<WithEnumerableInterfaceInConstructor>().Cars;

        Behaves_like<AnEnumerableTypeWithTwoDifferentInstances> creating_an_enumerable_with_two_instances;

        protected static IEnumerable<ICar> cars;
        static AutoFakeContainer autoFakeContainer;
    }

    [Subject(typeof(AutoFakeContainer))]
    public class When_the_subject_has_an__Array__of_an_interface_type_as_constructor_parameter
    {
        Establish context = () =>
            autoFakeContainer = new AutoFakeContainer(new DummyFakeEngine());

        Because of = () => subject = autoFakeContainer.CreateSubject<WithInterfaceArrayInConstructor>();

        It should_create_an_empty_array = () =>
        {
            subject.Cars.ShouldNotBeNull();
            subject.Cars.ShouldBeEmpty();
        };

        static WithInterfaceArrayInConstructor subject;
        static AutoFakeContainer autoFakeContainer;
    }

    [Subject(typeof(AutoFakeContainer))]
    public class When_the_subject_has_an__Array__of_a_concrete_type_as_constructor_parameter
    {
        Establish context = () =>
            autoFakeContainer = new AutoFakeContainer(new DummyFakeEngine());

        Because of = () => subject = autoFakeContainer.CreateSubject<WithArrayInConstructor>();

        It should_create_an_empty_array = () =>
        {
            subject.Cars.ShouldNotBeNull();
            subject.Cars.ShouldBeEmpty();
        };

        static WithArrayInConstructor subject;
        static AutoFakeContainer autoFakeContainer;
    }

    [Subject(typeof(AutoFakeContainer))]
    public class When_the_subject_has_an__Array__of_an_interface_type_as_constructor_parameter_and_an_implementation_has_been_configured_for_the_interface
    {
        Establish context = () =>
        {
            autoFakeContainer = new AutoFakeContainer(new DummyFakeEngine());
            autoFakeContainer.Register(new TypeMapping(typeof(ICar), typeof(Car)));
        };

        Because of = () => subject = autoFakeContainer.CreateSubject<WithInterfaceArrayInConstructor>();

        It should_create_an_array = () => subject.Cars.ShouldNotBeNull();

        It should_add_one_fake_instance_to_the_array = () => subject.Cars.Count().ShouldEqual(1);

        static WithInterfaceArrayInConstructor subject;
        static AutoFakeContainer autoFakeContainer;
    }

    [Subject(typeof(AutoFakeContainer))]
    public class When_the_subject_has_an__Array__of_an_interface_type_as_constructor_parameter_and_two_implementations_have_been_configured_for_the_interface
    {
        Establish context = () =>
        {
            autoFakeContainer = new AutoFakeContainer(new DummyFakeEngine());
            autoFakeContainer.Register(new TypeMapping(typeof(ICar), typeof(Car)));
            autoFakeContainer.Register(new FactoryMapping<ICar>(() => new Car2()));
        };

        Because of = () => cars = autoFakeContainer.CreateSubject<WithInterfaceArrayInConstructor>().Cars;

        Behaves_like<AnEnumerableTypeWithTwoDifferentInstances> creating_an_array_with_two_instances;

        protected static IEnumerable<ICar> cars;
        static AutoFakeContainer autoFakeContainer;
    }

    [Behaviors]
    internal class AnEnumerableTypeWithTwoDifferentInstances
    {
        protected static IEnumerable<ICar> cars = Enumerable.Empty<ICar>();

        It should_create_an_enumerable = () => cars.ShouldNotBeNull();

        It should_add_one_fake_instance_of_each_configured_type_to_the_enumerable = () =>
        {
            cars.Count().ShouldEqual(2);
            cars.ShouldContain(x => x.GetType() == typeof(Car));
            cars.ShouldContain(x => x.GetType() == typeof(Car2));
        };
    }

    [Subject(typeof(AutoFakeContainer))]
    public class When_the_subject_has_an__Enumerable__as_a_constructor_parameter_and_an__Enumerable__implementation_has_been_configured
    {
        Establish context = () =>
        {
            autoFakeContainer = new AutoFakeContainer(new DummyFakeEngine());

            // should use IEnumerable implementation even if ICar is configured
            autoFakeContainer.Register(new TypeMapping(typeof(ICar), typeof(Car)));
            autoFakeContainer.Register(new ObjectMapping(typeof(IEnumerable<ICar>), cars));
        };

        Because of = () => subject = autoFakeContainer.CreateSubject<WithEnumerableInterfaceInConstructor>();

        It should_use_the_configured_implementation = () => subject.Cars.ShouldBeTheSameAs(cars);

        static WithEnumerableInterfaceInConstructor subject;
        static ICar[] cars = new ICar[] { new Car2() };
        static AutoFakeContainer autoFakeContainer;
    }

    [Subject(typeof(AutoFakeContainer))]
    public class When_the_subject_has_an__Array__as_a_constructor_parameter_and_an__Array__implementation_has_been_configured
    {
        Establish context = () =>
        {
            autoFakeContainer = new AutoFakeContainer(new DummyFakeEngine());

            // should use array implementation even if ICar is configured
            autoFakeContainer.Register(new TypeMapping(typeof(ICar), typeof(Car)));
            autoFakeContainer.Register(new ObjectMapping(typeof(ICar[]), cars));
        };

        Because of = () => subject = autoFakeContainer.CreateSubject<WithInterfaceArrayInConstructor>();

        It should_use_the_configured_implementation = () => subject.Cars.ShouldBeTheSameAs(cars);

        static WithInterfaceArrayInConstructor subject;
        static ICar[] cars = new ICar[] { new Car2() };
        static AutoFakeContainer autoFakeContainer;
    }

    [Subject(typeof(AutoFakeContainer))]
    public class When_the_subject_has_a__Func__of_an_implementation_type_as_constructor_parameter
    {
        Establish context = () => autoFakeContainer = new AutoFakeContainer(new DummyFakeEngine());

        Because of = () => subject = autoFakeContainer.CreateSubject<WithFuncOfImplementationInConstructor>();

        It should_inject_a_factory_that_returns_the_instance_into_the_subject = () => subject.Car.ShouldBeOfExactType<Car>();

        static WithFuncOfImplementationInConstructor subject;
        static AutoFakeContainer autoFakeContainer;
    }

    [Subject(typeof(AutoFakeContainer))]
    public class When_the_subject_has_a__Func__of_an_interface_type_as_constructor_parameter
    {
        Establish context = () =>
        {
            createdFake = new Car();
            autoFakeContainer = new AutoFakeContainer(fakeEngine = new DummyFakeEngine { CreatedFake = createdFake });
        };

        Because of = () => car = autoFakeContainer.CreateSubject<WithFuncOfInterfaceInConstructor>().Car;

        It should_inject_a_factory_that_returns_the_fake_into_the_subject = () => car.ShouldBeTheSameAs(createdFake);

        It should_request_a_fake_for_that_interface_from_the_fake_engine = () =>
            fakeEngine.RequestedFakeType.ShouldEqual(typeof(ICar));

        static DummyFakeEngine fakeEngine;
        static ICar car;
        static Car createdFake;
        static AutoFakeContainer autoFakeContainer;
    }

    [Subject(typeof(AutoFakeContainer))]
    public class When_the_subject_has_a__Func__of_an_interface_type_as_constructor_parameter_and_an_implementation_has_been_configured
    {
        Establish context = () =>
        {
            autoFakeContainer = new AutoFakeContainer(new DummyFakeEngine());
            autoFakeContainer.Register(new ObjectMapping(typeof(ICar), car = new Car()));
        };

        Because of = () => subject = autoFakeContainer.CreateSubject<WithFuncOfInterfaceInConstructor>();

        It should_inject_a_factory_that_returns_the_implementation_into_the_subject = () =>
            subject.Car.ShouldBeTheSameAs(car);

        static ICar car;
        static AutoFakeContainer autoFakeContainer;
        static WithFuncOfInterfaceInConstructor subject;
    }

    [Subject(typeof(AutoFakeContainer))]
    public class When_the_subject_has_a__Lazy__of_an_interface_type_as_constructor_parameter
    {
        Establish context = () =>
        {
            createdFake = new Car();
            autoFakeContainer = new AutoFakeContainer(fakeEngine = new DummyFakeEngine { CreatedFake = createdFake });
        };

        Because of = () => subject = autoFakeContainer.CreateSubject<WithLazyInConstructor>();

        It should_inject_the__lazy__fake_into_the_subject = () => subject.Car.ShouldBeTheSameAs(createdFake);

        It should_request_a_fake_for_that_interface_from_the_fake_engine = () =>
            fakeEngine.RequestedFakeType.ShouldEqual(typeof(ICar));

        static DummyFakeEngine fakeEngine;
        static WithLazyInConstructor subject;
        static Car createdFake;
        static AutoFakeContainer autoFakeContainer;
    }

    [Subject(typeof(AutoFakeContainer))]
    public class When_the_dependency_graph_of_a_subject_is_configured_but_one_dependency_is_left_out
    {
        static AutoFakeContainer autoFakeContainer;
        static DummyFakeEngine dummyFakeEngine;

        Establish context = () =>
        {
            dummyFakeEngine = new DummyFakeEngine { CreatedFake = new FakeIncomingCommandObjects() };
            autoFakeContainer = new AutoFakeContainer(dummyFakeEngine);
            autoFakeContainer.Register(new TypeMapping(typeof(ICommandBus), typeof(CommandBus)));
            autoFakeContainer.Register(new TypeMapping(typeof(ICommand), typeof(TestCommand)));
        };

        Because of = () => autoFakeContainer.CreateSubject<ServiceFacade>().DoIt("a");

        It should_replace_the_missing_dependency_with_a_fake = () =>
            dummyFakeEngine.RequestedFakeType.ShouldEqual(typeof(IIncomingCommandObjects));
    }
}
