using System;

namespace CodeBase.Framework
{
  public class Resolver : IResolver
  {
    private readonly IDependencyBox _dependencyBox;
    private readonly IInstantiator _instantiator;

    public Resolver(IDependencyBox dependencyBox, IInstantiator instantiator)
    {
      _dependencyBox = dependencyBox;
      _instantiator = instantiator;
    }

    public TContract Resolve<TContract>() => (TContract) Resolve(typeof(TContract));

    public object Resolve(Type contractType)
    {
      var container = _dependencyBox.Container;

      if (!_dependencyBox.Container.ContainsKey(contractType))
        throw new ResolveException($"{contractType} is not binded");

      if (container[contractType].ConcreteType == null)
        throw new ResolveException($"{contractType} is not binded to a concrete type");

      Binding binding = container[contractType];

      if (binding.Instance != null)
        return binding.Instance;

      binding.Instance = _instantiator.Instantiate(binding.ConcreteType);
      return binding.Instance;
    }
  }
}