using System;
using System.Linq.Expressions;
using Machine.Fakes.Utils;

namespace Machine.Fakes.Internal
{
    /// <summary>
    /// A static facade for the current fake engine.
    /// </summary>
    public class FakeEngineGateway
    {
        private static IFakeEngine _fakeEngine;

        /// <summary>
        /// Hosts the supplied <see cref="IFakeEngine"/>
        /// in the current gateway.
        /// </summary>
        /// <param name="fakeEngine">The engine</param>
        public static void EngineIs(IFakeEngine fakeEngine)
        {
            Guard.AgainstArgumentNull(fakeEngine, "fakeEngine");

            _fakeEngine = fakeEngine;
        }

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
        ///   Configures the behavior. This must be a void method.
        /// </param>
        /// <returns>
        ///   A <see cref = "IQueryOptions{TReturn}" /> for further configuration.
        /// </returns>
        public static IQueryOptions<TReturnValue> SetUpQueryBehaviorFor<TFake, TReturnValue>(
            TFake fake,
            Expression<Func<TFake, TReturnValue>> func) where TFake : class
        {
            Guard.AgainstArgumentNull(fake, "fake");
            Guard.AgainstArgumentNull(func, "func");

            return _fakeEngine.SetUpQueryBehaviorFor(fake, func);
        }

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
        public static ICommandOptions SetUpCommandBehaviorFor<TFake>(
            TFake fake,
            Expression<Action<TFake>> func) where TFake : class
        {
            Guard.AgainstArgumentNull(fake, "fake");
            Guard.AgainstArgumentNull(func, "func");

            return _fakeEngine.SetUpCommandBehaviorFor(fake, func);
        }

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
        public static void VerifyBehaviorWasNotExecuted<TFake>(
            TFake fake,
            Expression<Action<TFake>> func) where TFake : class
        {
            Guard.AgainstArgumentNull(fake, "fake");
            Guard.AgainstArgumentNull(func, "func");

            _fakeEngine.VerifyBehaviorWasNotExecuted(fake, func);
        }

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
        public static IMethodCallOccurance VerifyBehaviorWasExecuted<TFake>(
            TFake fake,
            Expression<Action<TFake>> func) where TFake : class
        {
            Guard.AgainstArgumentNull(fake, "fake");
            Guard.AgainstArgumentNull(func, "func");

            return _fakeEngine.VerifyBehaviorWasExecuted(fake, func);
        }

        /// <summary>
        /// Gets an <see cref="IEventRaiser"/> which can be used to raise an event
        /// on the fake specified via <typeparamref name="TFake"/>.
        /// </summary>
        /// <typeparam name="TFake">
        /// Specifies the type of the fake.
        /// </typeparam>
        /// <param name="fake">
        /// Specifies the fake instance.
        /// </param>
        /// <param name="assignement">
        /// A function specifying the event assignement.
        /// </param>
        /// <returns>
        /// A <see cref="IEventRaiser"/>.
        /// </returns>
        /// <remarks>
        ///     Get a refrence to an <see cref="IEventRaiser"/> like this:
        ///     <code>
        ///         var eventRaiser = fake.Event(x => x.PropertyChanged +=null);
        ///     </code>
        ///     Use it to raise the event.
        ///     <code>
        ///         fake.Event(x => x.PropertyChanged += null)
        ///             .Raise(this, new PropertyChangedArgs("NoProp")); 
        ///     </code>
        /// </remarks>
        public static IEventRaiser CreateEventRaiser<TFake>(
            TFake fake,
            Action<TFake> assignement) where TFake : class
        {
            Guard.AgainstArgumentNull(fake, "fake");
            Guard.AgainstArgumentNull(assignement, "assignement");

            return _fakeEngine.CreateEventRaiser(fake, assignement);
        }

        /// <summary>
        /// Creates a new fake instance of the interface specified by <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The interface you want to fake.
        /// </typeparam>
        /// <returns>
        /// The fake.
        /// </returns>
        public static T Fake<T>()
        {
            return _fakeEngine.Stub<T>();
        }
    }
}