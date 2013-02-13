using Machine.Fakes.Adapters.Moq;

namespace Machine.Fakes
{
    /// <summary>
    /// Base class for the simpler cases than <see cref="WithSubject{TSubject}"/>. 
    /// This class only contains the shortcuts for creating fakes via "An" and "Some".
    /// </summary>
    public class WithFakes : WithFakes<MoqFakeEngine>
    {
    }
}