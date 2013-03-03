using System;
using System.Collections.Generic;

using Machine.Fakes.Sdk;
using Machine.Specifications;
using Machine.Specifications.Annotations;

namespace Machine.Fakes
{
    /// <summary>
    /// Base class for the simpler cases than <see cref="WithSubject{TSubject, TFakeEngine}"/>. 
    /// This class only contains the shortcuts for creating fakes via "An" and "Some".
    /// </summary>
    /// <typeparam name="TFakeEngine">
    /// Specifies the concrete fake engine that will be used for creating fake instances.
    /// This must be a class with a parameterless constructor that implements <see cref="IFakeEngine"/>.
    /// </typeparam>
    public abstract class WithFakes<TFakeEngine> where TFakeEngine : IFakeEngine, new()  
    {
        /// <summary>
        /// The specification controller
        /// </summary>
        protected static SpecificationController<object, TFakeEngine> _specificationController;
        
        /// <summary>
        /// Creates a new instance of the <see cref="WithFakes{TFakeEngine}"/> class.
        /// </summary>
        protected WithFakes()
        {
            _specificationController = new SpecificationController<object, TFakeEngine>();
        }

        /// <summary>
        ///   Creates a fake of the type specified by <typeparamref name = "TInterfaceType" />.
        /// </summary>
        /// <typeparam name = "TInterfaceType">The type to create a fake for. (Should be an interface or an abstract class)</typeparam>
        /// <param name="args">
        ///  Optional constructor parameters for abstract base classes as fakes.
        /// </param>
        /// <returns>
        ///   An newly created fake implementing <typeparamref name = "TInterfaceType" />.
        /// </returns>
        public static TInterfaceType An<TInterfaceType>(params object[] args) where TInterfaceType : class
        {
            GuardAgainstStaticContext();

            return _specificationController.An<TInterfaceType>(args);
        }

        /// <summary>
        ///   Creates a list containing 3 fake instances of the type specified
        ///   via <typeparamref name = "TInterfaceType" />.
        /// </summary>
        /// <typeparam name = "TInterfaceType">Specifies the item type of the list. This should be an interface or an abstract class.</typeparam>
        /// <returns>An <see cref = "IList{T}" />.</returns>
        public static IList<TInterfaceType> Some<TInterfaceType>() where TInterfaceType : class
        {
            GuardAgainstStaticContext();

            return _specificationController.Some<TInterfaceType>();
        }

        /// <summary>
        /// Creates a list of fakes.
        /// </summary>
        /// <typeparam name="TInterfaceType">
        /// Specifies the item type of the list. This should be an interface or an abstract class.
        /// </typeparam>
        /// <param name="amount">
        /// Specifies the amount of fakes that have to be created and inserted into the list.
        /// </param>
        /// <returns>
        /// An <see cref="IList{TInterfaceType}"/>.
        /// </returns>
        public static IList<TInterfaceType> Some<TInterfaceType>(int amount) where TInterfaceType : class
        {
            GuardAgainstStaticContext();

            return _specificationController.Some<TInterfaceType>(amount);
        }

        static void GuardAgainstStaticContext()
        {
            if (_specificationController == null)
                throw new InvalidOperationException(
                    "WithFakes has not been initialized yet. Are you calling this from a static initialzer?");
        }

        [UsedImplicitly]
        Cleanup after = () => _specificationController.Dispose();
    }
}