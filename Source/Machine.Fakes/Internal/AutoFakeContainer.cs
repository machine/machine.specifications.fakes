using System;
using System.Linq.Expressions;
using Machine.Fakes.Utils;
using StructureMap;
using StructureMap.AutoMocking;

namespace Machine.Fakes.Internal
{
	sealed class AutoFakeContainer<TTargetClass> :
		ServiceLocator,
		IFakeEngine where TTargetClass : class
	{
		private readonly IFakeEngine _fakeEngine;
		private readonly StructureMapAutoMockerAdapter<TTargetClass> _autoMocker;
	   
		public AutoFakeContainer(Type specType)
		{
			Guard.AgainstArgumentNull(specType, "specType");

			_fakeEngine = FakeEngineInstaller.InstallFor(specType);
			_autoMocker = new StructureMapAutoMockerAdapter<TTargetClass>(this);
		}

		public TTargetClass CreateTarget()
		{
			try
			{
				return _autoMocker.ClassUnderTest;
			}
			catch (StructureMapException ex)
			{
				throw new TargetCreationException(typeof(TTargetClass), ex);
			}
		}

		public TFakeSingleton Get<TFakeSingleton>() where TFakeSingleton : class
		{
			return _autoMocker.Get<TFakeSingleton>();
		}

		public void Inject(Type contract, object implementation)
		{
			_autoMocker.Inject(contract, implementation);
		}

		#region IFakeEngine Members

		public T PartialMock<T>(params object[] args) where T : class
		{
			return _fakeEngine.PartialMock<T>(args);
		}


		public object CreateFake(Type interfaceType)
		{
			return _fakeEngine.CreateFake(interfaceType);
		}

		public IQueryOptions<TReturnValue> SetUpQueryBehaviorFor<TFake, TReturnValue>(
			TFake fake,
			Expression<Func<TFake, TReturnValue>> func) where TFake : class
		{
			return _fakeEngine.SetUpQueryBehaviorFor(fake, func);
		}

		public ICommandOptions SetUpCommandBehaviorFor<TFake>(
			TFake fake,
			Expression<Action<TFake>> func) where TFake : class
		{
			return _fakeEngine.SetUpCommandBehaviorFor(fake, func);
		}

		public void VerifyBehaviorWasNotExecuted<TFake>(
			TFake fake,
			Expression<Action<TFake>> func) where TFake : class
		{
			_fakeEngine.VerifyBehaviorWasNotExecuted(fake, func);
		}

		public IMethodCallOccurance VerifyBehaviorWasExecuted<TFake>(
			TFake fake,
			Expression<Action<TFake>> func) where TFake : class
		{
			return _fakeEngine.VerifyBehaviorWasExecuted(fake, func);
		}

		public IEventRaiser CreateEventRaiser<TFake>(TFake fake, Action<TFake> assignement) where TFake : class
		{
			return _fakeEngine.CreateEventRaiser(fake, assignement);
		}

		#endregion

		#region ServiceLocator Members

		T ServiceLocator.Service<T>()
		{
			return _fakeEngine.Stub<T>();
		}

		object ServiceLocator.Service(Type serviceType)
		{
			return _fakeEngine.CreateFake(serviceType);
		}

		#endregion
	}
}