using System;
using Moq;
using Machine.Fakes.Utils;

namespace Machine.Fakes.Adapters.Moq
{
    class MoqEventRaiser<TFake> : IEventRaiser where TFake : class
    {
        private readonly Action<TFake> _assignement;
        private readonly Mock<TFake> _mock;

        public MoqEventRaiser(Mock<TFake> mock, Action<TFake> assignement)
        {
            Guard.AgainstArgumentNull(mock, "assignement");
            Guard.AgainstArgumentNull(assignement, "assignement");

            _mock = mock;
            _assignement = assignement;
        }

        #region IEventRaiser Members

        public void Raise(object sender, EventArgs e)
        {
            _mock.Raise(_assignement, e);
        }

        public void Raise(params object[] args)
        {
            _mock.Raise(_assignement, args);
        }

        #endregion
    }
}