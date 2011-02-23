using System.Collections.Generic;
using Machine.Fakes.Internal;
using Machine.Specifications;

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
        private static SpecificationController<object, TFakeEngine> _specificationController;
        
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
        /// <returns>
        ///   An newly created fake implementing <typeparamref name = "TInterfaceType" />.
        /// </returns>
        public static TInterfaceType An<TInterfaceType>() where TInterfaceType : class
        {
            return _specificationController.An<TInterfaceType>();
        }

        /// <summary>
        ///   Creates a list containing 3 fake instances of the type specified
        ///   via <typeparamref name = "TInterfaceType" />.
        /// </summary>
        /// <typeparam name = "TInterfaceType">Specifies the item type of the list. This should be an interface or an abstract class.</typeparam>
        /// <returns>An <see cref = "IList{T}" />.</returns>
        public static IList<TInterfaceType> Some<TInterfaceType>() where TInterfaceType : class
        {
            return _specificationController.Some<TInterfaceType>();
        }

        Cleanup after = () => _specificationController.Dispose();
    }
}