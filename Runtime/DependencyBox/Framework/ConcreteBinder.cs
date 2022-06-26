namespace CodeBase.Runtime.DependencyBox.Framework
{
	public class ConcreteBinder<TContract>
	{
		private readonly Binding _binding;

		public ConcreteBinder(Binding binding)
		{
			_binding = binding;
		}

		public void FromInstance(TContract instance) => _binding.Instance = instance;
	}
}