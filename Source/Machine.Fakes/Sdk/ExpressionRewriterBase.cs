using System;
using System.Linq.Expressions;

namespace Machine.Fakes.Sdk
{
    /// <summary>
    /// Derive from this class in order to make inline constraints work.
    /// This is just glue code. The expression writer does does all the job.
    /// </summary>
    public abstract class RewritingFakeEngine : IFakeEngine
    {
        private readonly IExpressionRewriter _rewriter;

        /// <summary>
        /// Creates a new instance of the <see cref="RewritingFakeEngine" /> class.
        /// </summary>
        /// <param name="rewriter">
        /// Specifies the rewriter that translates the expressions or more
        /// specific the generic inline constraints used in expressions
        /// to the inline constraints used in the target fake framework.
        /// </param>
        protected RewritingFakeEngine(IExpressionRewriter rewriter)
        {
            Guard.AgainstArgumentNull(rewriter, "rewriter");

            _rewriter = rewriter;
        }

        /// <summary>
        /// Creates a fake of the type specified via <paramref name="interfaceType"/> with no default constructor.
        /// </summary>
        /// <param name="interfaceType">
        /// Specifies the interface type to create a fake for.
        /// </param>
        /// <param name="args">
        /// Specifies the constructor parameters.
        /// </param>
        /// <returns>
        /// The created fake instance.
        /// </returns>
        public abstract object CreateFake(Type interfaceType, params object[] args);

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
        public abstract T PartialMock<T>(params object[] args) where T : class;

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
        public IQueryOptions<TReturnValue> SetUpQueryBehaviorFor<TFake, TReturnValue>(
            TFake fake, 
            Expression<Func<TFake, TReturnValue>> func) where TFake : class
        {
            return OnSetUpQueryBehaviorFor(fake, Rewrite(func));
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
        public ICommandOptions SetUpCommandBehaviorFor<TFake>(
            TFake fake, 
            Expression<Action<TFake>> func) where TFake : class
        {
            return OnSetUpCommandBehaviorFor(fake, Rewrite(func));
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
        public void VerifyBehaviorWasNotExecuted<TFake>(
            TFake fake, 
            Expression<Action<TFake>> func) where TFake : class
        {
            OnVerifyBehaviorWasNotExecuted(fake, Rewrite(func));
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
        public IMethodCallOccurance VerifyBehaviorWasExecuted<TFake>(
            TFake fake, 
            Expression<Action<TFake>> func) where TFake : class
        {
            return OnVerifyBehaviorWasExecuted(fake, Rewrite(func));
        }

        protected abstract IMethodCallOccurance OnVerifyBehaviorWasExecuted<TFake>(
            TFake fake,
            Expression<Action<TFake>> func) where TFake : class;

        protected abstract IQueryOptions<TReturnValue> OnSetUpQueryBehaviorFor<TFake, TReturnValue>(
            TFake fake, 
            Expression<Func<TFake, TReturnValue>> func) where TFake : class;

        protected abstract void OnVerifyBehaviorWasNotExecuted<TFake>(
            TFake fake, 
            Expression<Action<TFake>> func) where TFake : class;

        protected abstract ICommandOptions OnSetUpCommandBehaviorFor<TFake>(
            TFake fake,
            Expression<Action<TFake>> func) where TFake : class;

        Expression<Func<TType, TValue>> Rewrite<TType, TValue>(
            Expression<Func<TType, TValue>> expression)
        {
            return expression.RewriteUsing(_rewriter); 
        }

        Expression<Action<TType>> Rewrite<TType>(Expression<Action<TType>> expression)
        {
            return expression.RewriteUsing(_rewriter);
        }
    }
}