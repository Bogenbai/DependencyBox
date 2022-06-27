using System;
using System.Collections.Generic;

namespace CodeBase.Framework
{
  public class Binder : IBinder
  {
    private readonly Dictionary<Type, Binding> _container;

    public Binder(Dictionary<Type, Binding> container)
    {
      _container = container;
    }

    public ContractBinder<TContract> Bind<TContract>()
    {
      EnsureThatDependencyNotRegistered<TContract>();

      var binding = new Binding();

      if (typeof(TContract).IsClass)
        binding.ConcreteType = typeof(TContract);

      _container.Add(typeof(TContract), binding);

      return new ContractBinder<TContract>(binding);
    }
    
    public void Unbind<TContract>() => _container.Remove(typeof(TContract));
    
    private void EnsureThatDependencyNotRegistered<TContract>()
    {
      if (_container.ContainsKey(typeof(TContract)))
        throw new BindingException($"Dependency of type {typeof(TContract)} is already registered as single.");
    }
  }
}