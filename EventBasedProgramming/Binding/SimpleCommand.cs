using System;
using System.Windows.Input;
using EventBasedProgramming.Binding.Impl;

namespace EventBasedProgramming.Binding
{
	public class SimpleCommand : ICommand
	{
		private readonly Func<bool> _canExecute;
		private readonly Action _execute;

		public SimpleCommand(Func<bool> canExecute, Action execute)
		{
			_canExecute = canExecute;
			_execute = execute;
		}

		public void Execute(object parameter)
		{
			_execute();
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute();
		}

		public event EventHandler CanExecuteChanged;

		public BindingInfo MethodHandlingCanExecute
		{
			get { return new BindingInfoForFunc(_canExecute.Method, _canExecute.Target); }
		}

		public BindingInfo MethodHandlingExecute
		{
			get { return new BindingInfoForFunc(_execute.Method, _execute.Target); }
		}

		public void NotifyThatCanExecuteChanged()
		{
			var handler = CanExecuteChanged;
			if (handler != null)
				handler(this, new EventArgs());
		}
	}
}
