using System.Collections.Generic;

namespace Machine.Fakes
{
    /// <summary>
    /// Accessor interface for dependencies created and managed by fake framework / auto fake container.
    /// </summary>
    public interface IFakeAccessor
    {
        /// <summary>
        /// Creates a fake of the type specified by <typeparamref name="TInterfaceType"/>.
        /// </summary>
        /// <typeparam name="TInterfaceType">
        /// The type to create a fake for. (Should be an interface or an abstract class)
        /// </typeparam>
        /// <returns>
        /// An newly created fake implementing <typeparamref name="TInterfaceType"/>.
        /// </returns>
        TInterfaceType An<TInterfaceType>() where TInterfaceType : class;

        /// <summary>
        /// Creates a fake of the type specified by <typeparamref name="TInterfaceType"/>.
        /// This method reuses existing instances. If an instance of <typeparamref name="TInterfaceType"/>
        /// was already requested it's returned here. (You can say this is kind of a singleton behavior)
        /// 
        /// Besides that, you can obtain a reference to automatically injected fakes with this 
        /// method.
        /// </summary>
        /// <typeparam name="TInterfaceType">
        /// The type to create a fake for. (Should be an interface or an abstract class)
        /// </typeparam>
        /// <returns>
        /// An instance implementing <typeparamref name="TInterfaceType"/>.
        /// </returns>
        TInterfaceType The<TInterfaceType>() where TInterfaceType : class;

        /// <summary>
        /// Creates a list containing 3 fake instances of the type specified 
        /// via <typeparamref name="TInterfaceType"/>.
        /// </summary>
        /// <typeparam name="TInterfaceType">
        /// Specifies the item type of the list. This should be an interface or an abstract class.
        /// </typeparam>
        /// <returns>
        /// An <see cref="IList{T}"/>.
        /// </returns>
        IList<TInterfaceType> Some<TInterfaceType>() where TInterfaceType : class;

        /// <summary>
        /// Creates a list containing fake instances of the type specified via <typeparamref name = "TInterfaceType" />.
        /// </summary>
        /// <param name="amount">
        /// Specifies the amount of fakes in the list.
        /// </param>
        /// <typeparam name = "TInterfaceType">Specifies the item type of the list. This should be an interface or an abstract class.</typeparam>
        /// <returns>An <see cref = "IList{T}" />.</returns>
        IList<TInterfaceType> Some<TInterfaceType>(int amount) where TInterfaceType : class;

        /// <summary>
        /// Uses the instance supplied by <paramref name="instance"/> during the 
        /// creation of the sut. The specified instance will be injected into the constructor.
        /// </summary>
        /// <typeparam name="TInterfaceType">
        /// Specifies the interface type.
        /// </typeparam>
        /// <param name="instance">
        /// Specifies the instance to be used for the specification.
        /// </param>
        void Use<TInterfaceType>(TInterfaceType instance) where TInterfaceType : class;
    }
}