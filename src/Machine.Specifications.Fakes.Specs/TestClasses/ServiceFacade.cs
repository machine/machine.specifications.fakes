using System.Collections.Generic;
using System.Linq;
using Machine.Specifications.Utility;

namespace Machine.Specifications.Fakes.Specs.TestClasses
{
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
            _commands.OfType<ICommand<T>>().Each(cmd => cmd.Execute(commandObj));
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
        // ReSharper disable UnusedParameter.Global
        void Store(string commandObject);
        // ReSharper restore UnusedParameter.Global
    }

    public class FakeIncomingCommandObjects : IIncomingCommandObjects
    {
        public void Store(string commandObject)
        {
        }
    }
}
