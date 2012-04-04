using System;
using System.Linq.Expressions;

namespace Machine.Fakes
{
    /// <summary>
    /// Interface to a fake framework. 
    /// </summary>
    public interface IFakeEngine
    {
        /// <summary>
        /// Creates a fake of the type specified via <paramref name="interfaceType"/> using a non default constructor.
        /// </summary>
        /// <param name="interfaceType">
        /// Specifies the interface type to create a fake for.
        /// </param>
        /// <param name="args">
        /// Specifies parameters for non default constructor.
        /// </param>
        /// <returns>
        /// The created fake instance.
        /// </returns>
        object CreateFake(Type interfaceType, params object[] args);

        /// <summary>
        /// Creates a partial mock.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the type of the partial mock. This needs to be 
        /// an abstract base class.
        /// </typeparam>
        /// <param name="args">
        /// Specifies the constructor parameters.
        /// </param>
        /// <returns>
        /// The created instance.
        /// </returns>
        T PartialMock<T>(params  object[] args) where T : class;

        /// <summary>
        ///   Configures the behavior of the fake specified by <paramref name = "fake" />.
        /// </summary>
        /// <typeparam name = "TFake">
        ///   Specifies the type of the fake.
        /// </typeparam>
        /// <typeparam name = "TReturnValue">
        ///   Specifies the type of the return value.
        /// </typeparam>
        /// <param name = "fake">
        ///   The fake to configure behavior on.
        /// </param>
        /// <param name = "func">
        ///   Expression to set up the behavior.
        /// </param>
        /// <returns>
        ///   A <see cref = "IQueryOptions{TReturn}" /> for further configuration.
        /// </returns>
        IQueryOptions<TReturnValue> SetUpQueryBehaviorFor<TFake, TReturnValue>(
            TFake fake, 
            Expression<Func<TFake, TReturnValue>> func) where TFake : class;

        /// <summary>
        ///   Configures the behavior of the fake specified by <paramref name = "fake" />.
        /// </summary>
        /// <typeparam name = "TFake">
        ///   Specifies the type of the fake.
        /// </typeparam>
        /// <param name = "fake">
        ///   The fake to configure behavior on.
        /// </param>
        /// <param name = "func">
        ///   Configures the behavior. This must be a void method.
        /// </param>
        /// <returns>
        ///   A <see cref = "ICommandOptions" /> for further configuration.
        /// </returns>
        /// <remarks>
        ///   This method is used for command, e.g. methods returning void.
        /// </remarks>
        ICommandOptions SetUpCommandBehaviorFor<TFake>(
            TFake fake,
            Expression<Action<TFake>> func) where TFake : class;

        /// <summary>
        /// Verifies that the behavior specified by <paramref name="func"/>
        /// was not executed on the fake specified by <paramref name="fake"/>.
        /// </summary>
        /// <typeparam name="TFake">
        /// Specifies the type of the fake.
        /// </typeparam>
        /// <param name="fake">
        /// Specifies the fake instance.
        /// </param>
        /// <param name="func">
        /// Specifies the behavior that was not supposed to happen.
        /// </param>
        void VerifyBehaviorWasNotExecuted<TFake>(
            TFake fake,
            Expression<Action<TFake>> func) where TFake : class;

        /// <summary>
        /// Verifies that the behavior specified by <paramref name="func"/>
        /// was executed on the fake specified by <paramref name="fake"/>.
        /// </summary>
        /// <typeparam name="TFake">
        /// Specifies the type of the fake.
        /// </typeparam>
        /// <param name="fake">
        /// Specifies the fake instance.
        /// </param>
        /// <param name="func">
        /// Specifies the behavior that was supposed to happen.
        /// </param>
        /// <returns>
        /// A <see cref="IMethodCallOccurance"/> which can be used
        /// to narrow down the expectations to a particular amount of times.
        /// </returns>
        IMethodCallOccurance VerifyBehaviorWasExecuted<TFake>(
            TFake fake, 
            Expression<Action<TFake>> func) where TFake : class ;
    }
}