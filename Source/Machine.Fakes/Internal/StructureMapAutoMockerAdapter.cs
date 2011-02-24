using Machine.Fakes.Sdk;
using StructureMap.AutoMocking;

namespace Machine.Fakes.Internal
{
	internal class StructureMapAutoMockerAdapter<TTargetClass> : AutoMocker<TTargetClass> where TTargetClass : class
	{
		public StructureMapAutoMockerAdapter(ServiceLocator locator)
		{
			Guard.AgainstArgumentNull(locator, "locator");

			_serviceLocator = locator;
			_container = new AutoMockedContainer(locator);
		}
	}
}