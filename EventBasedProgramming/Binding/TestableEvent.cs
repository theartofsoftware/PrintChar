using System;
using System.Collections.Generic;
using System.Linq;

namespace EventBasedProgramming.Binding
{
	public class TestableEvent<TArg1>
	{
		private readonly List<Action<TArg1>> _handlers = new List<Action<TArg1>>();

		public void BindTo(Action<TArg1> handler)
		{
			_handlers.Add(handler);
		}

		public void Call(TArg1 arg1)
		{
			_handlers.Each(h => h(arg1));
		}

		public bool IsBoundTo(Action<TArg1> handler)
		{
			return _handlers.Contains(handler);
		}

		public void UnbindFrom(Action<TArg1> handler)
		{
			_handlers.Remove(handler);
		}
	}
}