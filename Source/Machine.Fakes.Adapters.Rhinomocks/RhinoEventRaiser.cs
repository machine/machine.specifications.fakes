using System;

namespace Machine.Fakes.Adapters.Rhinomocks
{
    class RhinoEventRaiser : IEventRaiser
    {
        private readonly Rhino.Mocks.Interfaces.IEventRaiser _rhinoEventRaiser;

        public RhinoEventRaiser(Rhino.Mocks.Interfaces.IEventRaiser rhinoEventRaiser)
        {
            _rhinoEventRaiser = rhinoEventRaiser;
        }

        #region IEventRaiser Members

        public void Raise(object sender, EventArgs e)
        {
            _rhinoEventRaiser.Raise(sender, e);
        }

        public void Raise(params object[] args)
        {
            _rhinoEventRaiser.Raise(args);
        }

        #endregion
    }
}