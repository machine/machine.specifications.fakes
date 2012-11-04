using System;
using Rhino.Mocks;

namespace Machine.Fakes.Adapters.Rhinomocks
{
    /// <summary>
    ///   This class encapsulates the Rhino.Mocks mechanics for specifiing call expectations.
    /// </summary>
    /// <typeparam name = "TDependency">
    ///   Specifies the type of the dependency which is configured via the methods on <see cref = "RhinoMocksExtensions" />.
    /// </typeparam>
    class RhinoMethodCallOccurrence<TDependency> : IMethodCallOccurrence
    {
        private readonly Action<TDependency> _action;
        private readonly TDependency _fake;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "RhinoMethodCallOccurrence{TDependency}" /> class.
        /// </summary>
        /// <param name = "fake">The dependency on which an action is expected.</param>
        /// <param name = "action">The action that should have been called.</param>
        public RhinoMethodCallOccurrence(TDependency fake, Action<TDependency> action)
        {
            _fake = fake;
            _action = action;
            _fake.AssertWasCalled(action, y => y.Repeat.AtLeastOnce());
        }

        /// <summary>
        ///   Specifies that the action on the dependency should be called several times. <paramref name = "numberOfTimesTheMethodShouldHaveBeenCalled" />
        ///   specifies exactly how often.
        /// </summary>
        /// <param name = "numberOfTimesTheMethodShouldHaveBeenCalled">
        ///   The number of times the method should have been called.
        /// </param>
        public void Times(int numberOfTimesTheMethodShouldHaveBeenCalled)
        {
            _fake.AssertWasCalled(_action, y => y.Repeat.Times(numberOfTimesTheMethodShouldHaveBeenCalled));
        }

        /// <summary>
        ///   Specifies that the action on the dependency should only be called once.
        /// </summary>
        public void OnlyOnce()
        {
            Times(1);
        }

        /// <summary>
        ///   Specifies that the action on the dependency should be called twice.
        /// </summary>
        public void Twice()
        {
            Times(2);
        }
    }
}