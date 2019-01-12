using Machine.Specifications;
using Machine.Specifications.Factories;

namespace Machine.Fakes
{
    /// <summary>
    /// Base class that adds auto mocking (grasp), I mean auto faking capabilities
    /// to Machine.Specifications. 
    /// </summary>
    /// <typeparam name="TSubject">
    /// The subject of the specification. This is the type that is created by the
    /// specification for you.
    /// </typeparam>
    /// <typeparam name="TFakeEngine">
    /// Specifies the concrete fake engine that will be used for creating fake instances.
    /// This must be a class with a parameterless constructor that implements <see cref="IFakeEngine"/>.
    /// </typeparam>
    public abstract class WithSubject<TSubject, TFakeEngine> : WithFakes<TSubject, TFakeEngine>
        where TSubject : class
        where TFakeEngine : IFakeEngine, new()
    {
        /// <summary>
        /// Creates a new instance of the <see cref="WithSubject{TSubject, TFakeEngine}"/> class.
        /// </summary>
        protected WithSubject()
        {
            ContextFactory.ChangeAllowedNumberOfBecauseBlocksTo(2);
        }

        /// <summary>
        /// Gives access to the subject under specification. On first access
        /// the spec tries to create an instance of the subject type by itself.
        /// Override this behavior by manually setting a subject instance.
        /// </summary>
        protected static TSubject Subject
        {
            get
            {
                GuardAgainstStaticContext();

                return _specificationController.Subject;
            }

            set
            {
                GuardAgainstStaticContext();
                
                _specificationController.Subject = value;
            }
        }

        Because of = () => _specificationController.EnsureSubjectCreated();

        Cleanup after = () => ContextFactory.ChangeAllowedNumberOfBecauseBlocksTo(1);
    }
}