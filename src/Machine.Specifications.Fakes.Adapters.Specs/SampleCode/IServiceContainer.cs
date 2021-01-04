
using System;

namespace Machine.Fakes.Adapters.Specs.SampleCode
{
    public interface IServiceProvider
	{
		object GetService(Type serviceType);
	}

    public interface IServiceContainer : IServiceProvider
    {
		void RemoveService(Type serviceType);
    }
}
