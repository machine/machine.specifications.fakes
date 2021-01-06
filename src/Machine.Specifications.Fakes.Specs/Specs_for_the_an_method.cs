using System;
using Machine.Specifications.Fakes.Sdk;
using Machine.Specifications.Fakes.Specs.TestClasses;

namespace Machine.Specifications.Fakes.Specs
{
    [Subject(typeof(SpecificationController<,>))]
    public class When_using_the_an_method_to_create_an_explicit_fake
    {
        static SpecificationController<object> _specController;
        static object result;
        static DummyFakeEngine _fakeEngine;

        Establish context = () =>
        {
            _fakeEngine = new DummyFakeEngine { CreatedFake = new SqlConnection() };
            _specController = new SpecificationController<object>(_fakeEngine);
        };

        Because of = () => result = _specController.An(typeof(IDbConnection));

        It should_return_a_fake_that_implements_the_specified_type_interface =
            () => result.ShouldBeOfExactType<SqlConnection>();
    }

    [Subject(typeof(SpecificationController<,>))]
    public class When_using_the_an_method_with_a_type_with_no_default_constructor
    {
        static SpecificationController<object> _specController;
        static object result;
        static DummyFakeEngine _fakeEngine;
        static string connectionString = "Data Source=(local);Initial Catalog=test;";

        Establish context = () =>
        {
            _fakeEngine = new DummyFakeEngine { CreatedFake = new SqlConnection(connectionString) };
            _specController = new SpecificationController<object>(_fakeEngine);
        };

        Because of = () => result = _specController.An(typeof(IDbConnection), connectionString);

        It should_return_a_fake_that_used_the_ctor_parameter = () =>
            ((IDbConnection)result).ConnectionString.ShouldEqual(connectionString);
    }

    [Subject(typeof(WithFakes<>), "An")]
    public class When_called_from_a_static_initializer : WithFakes<DummyFakeEngine>
    {
        static Exception exception;

        static When_called_from_a_static_initializer()
        {
            exception = Catch.Exception(() => An<IDbConnection>());
        }

        It should_throw_the_right_exception = () => exception.ShouldBeOfExactType<InvalidOperationException>();
    }
}
