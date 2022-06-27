namespace CodeBase.Framework
{
	public class ContractBinder<TContract>
	{
		private readonly Binding _binding;

		public ContractBinder(Binding binding)
		{
			_binding = binding;
		}

		public ConcreteBinder<TContract> To<TConcrete>() where TConcrete : TContract
		{
			_binding.ConcreteType = typeof(TConcrete);
			return new ConcreteBinder<TContract>(_binding);
		}

		public TContract FromInstance<TConcrete>(TConcrete concrete) where TConcrete : TContract
		{
			_binding.Instance = concrete;
			_binding.ConcreteType = typeof(TConcrete);

			return concrete;
		}
	}
}