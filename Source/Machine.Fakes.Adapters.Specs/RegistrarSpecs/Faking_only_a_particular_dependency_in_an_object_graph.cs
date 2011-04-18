using System.Collections.Generic;
using System.Linq;
using Machine.Fakes.Adapters.Rhinomocks;
using Machine.Fakes.Sdk;
using Machine.Specifications;
using Machine.Specifications.Utility;

namespace Machine.Fakes.Adapters.Specs.RegistrarSpecs
{
    [Subject(typeof (SpecificationController<>))]
    public class When_using_an_registrar_expression_to_configure_the_dependency_graph_of_a_subject_and_leaving_one_dependency_out : WithSubject<ServiceFacade, RhinoFakeEngine>
    {
        Establish context = () =>
        {
            Configure(config =>
            {
                config.For<ICommandBus>().Use<CommandBus>();
                config.For<ICommand>().Use<TestCommand>();
            });
        };

        Because of = () => Subject.DoIt("TestMessage");

        It should_replace_the_missing_dependency_with_a_fake = 
            () => The<IIncomingCommandObjects>().WasToldTo(x => x.Store("TestMessage"));
    }

    [Subject(typeof(SpecificationController<>))]
    public class When_using_a_registrar_to_configure_the_dependency_graph_of_a_subject_and_leaving_one_dependency_out : WithSubject<ServiceFacade, RhinoFakeEngine>
    {
        Establish context = () => Configure<MyRegistrar>();

        Because of = () => Subject.DoIt("TestMessage");

        It should_replace_the_missing_dependency_with_a_fake =
            () => The<IIncomingCommandObjects>().WasToldTo(x => x.Store("TestMessage"));
    }

    public class MyRegistrar : Registrar
    {
        public MyRegistrar()
        {
            For<ICommandBus>().Use<CommandBus>();
            For<ICommand>().Use<TestCommand>();
        }
    }

    #region Example classes  

    public class ServiceFacade
    {
        readonly ICommandBus _commandBus;

        public ServiceFacade(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        public void DoIt(string message)
        {
            _commandBus.Execute(message);
        }
    }

    public interface ICommandBus
    {
        void Execute<T>(T commandObj);
    }

    public interface ICommand<in TCommandObj> : ICommand
    {
        void Execute(TCommandObj command);
    }

    public interface ICommand
    {
    }

    public class CommandBus : ICommandBus
    {
        readonly IEnumerable<ICommand> _commands;

        public CommandBus(IEnumerable<ICommand> commands)
        {
            _commands = commands;
        }

        public void Execute<T>(T commandObj)
        {
            _commands.Where(cmd => cmd is ICommand<T>)
                .Select(cmd => cmd as ICommand<T>)
                .Each(cmd => cmd.Execute(commandObj));
        }
    }

    public class TestCommand : ICommand<string>
    {
        readonly IIncomingCommandObjects _store;

        public TestCommand(IIncomingCommandObjects store)
        {
            _store = store;
        }

        public void Execute(string command)
        {
            _store.Store(command);    
        }
    }

    public interface IIncomingCommandObjects
    {
        void Store(string commandObject);
    }

    #endregion

}