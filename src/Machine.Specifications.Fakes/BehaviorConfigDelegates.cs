namespace Machine.Fakes
{
    /// <summary>
    /// Delegate used in behavior configs in order
    /// to configure dependencies in the main specification
    /// from outside the specification.
    /// </summary>
    /// <param name="accessor">
    /// Specifies gateway for accessing fakes.
    /// </param>
    public delegate void OnEstablish(IFakeAccessor accessor);

    /// <summary>
    /// Delegate used in behavior configs in order
    /// to cleanup the subject after a specification has been
    /// executed. This can be used for cleaning up transactions
    /// for example.
    /// </summary>
    /// <param name="subject">
    /// Specifies the subject of a context / specification.
    /// </param>
    public delegate void OnCleanup(object subject);
}