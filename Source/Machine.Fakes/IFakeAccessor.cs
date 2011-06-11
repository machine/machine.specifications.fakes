using System;
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
        /// <param name="args">
        /// The ctor parameters for the newly created entity
        /// </param>
        /// <returns>
        /// An newly created fake implementing <typeparamref name="TInterfaceType"/>.
        /// </returns>
        TInterfaceType An<TInterfaceType>(params object[] args) where TInterfaceType : class;

        /// <summary>
        /// Creates a fake of the type specified by <paramref name="interfaceType"/>.
        /// </summary>
        /// The type to create a fake for. (Should be an interface or an abstract class)
        /// <param name="interfaceType">
        /// Specifies the type of item to fake.
        /// </param>
        /// <param name="args">
        /// The ctor parameters for the newly created entity
        /// </param>
        /// <returns>
        /// An newly created fake implementing <paramref name="interfaceType"/>.
        /// </returns>
        object An(Type interfaceType, params object[] args);

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
        /// Applies the configuration embedded in the registar to the underlying container.
        /// </summary>
        /// <param name="registrar">
        /// Specifies the registrar.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the supplied registrar is <c>null</c>.
        /// </exception>
        void Configure(Registrar registrar);
    }
}