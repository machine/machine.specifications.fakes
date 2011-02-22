namespace Machine.Fakes
{
    /// <summary>
    /// Helper implementation for <see cref="IBehaviorConfig"/> that
    /// implements all methods of the interface as virtual. Override what you need to override. 
    /// </summary>
    public abstract class BehaviorConfigBase : IBehaviorConfig
    {
        /// <summary>
        /// Is called when the specification is establishing the context.
        /// </summary>
        /// <param name="fakeAccessor">
        /// Gives access to creating, accessing and configuring fakes inside
        /// the specifications subject.
        /// </param>
        public virtual void EstablishContext(IFakeAccessor fakeAccessor)
        {
        }

        /// <summary>
        /// Is called after the specification has executed and can be used for further cleanup.
        /// </summary>
        /// <param name="subject">
        /// The subject that was used in the spec.
        /// </param>
        public virtual void CleanUp(object subject)
        {
        }
    }
}