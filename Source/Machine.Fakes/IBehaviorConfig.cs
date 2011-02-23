namespace Machine.Fakes
{
    /// <summary>
    /// A BehaviorConfig is a simple way to modularize the configuration
    /// of fakes in an <see cref="WithSubject{TSubject, TFakeEngine}"/>. Implementing this
    /// interface enables you to participate in the setup and cleanup stages
    /// of a specification without having to derive from it.
    /// </summary>
    public interface IBehaviorConfig
    {
        /// <summary>
        /// Is called when the specification is establishing the context.
        /// </summary>
        /// <param name="fakeAccessor">
        /// Gives access to creating, accessing and configuring fakes inside
        /// the specifications subject.
        /// </param>
        void EstablishContext(IFakeAccessor fakeAccessor);

        /// <summary>
        /// Is called after the specification has executed and can be used for further cleanup.
        /// </summary>
        /// <param name="subject">
        /// The subject that was used in the spec.
        /// </param>
        void CleanUp(object subject);
    }
}