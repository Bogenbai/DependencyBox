using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Framework
{
  public class DependencyBox : IDependencyBox, IInstantiator, IResolver, IDisposable
  {
    public Dictionary<Type, Binding> Container { get; }
    private readonly IInstantiator _instantiator;
    private readonly IResolver _resolver;
    private readonly IBinder _binder;
    private readonly List<DependencyBox> _parents;

    public DependencyBox(List<DependencyBox> parents = null)
    {
      _parents = parents ?? new List<DependencyBox>();
      Container = new Dictionary<Type, Binding>();
      _binder = new Binder(Container);
      _instantiator = new Instantiator(this);
      _resolver = new Resolver(this, this);

      foreach (DependencyBox serviceBox in _parents)
      foreach ((Type contract, Binding binding) in serviceBox.Container)
        Container.Add(contract, binding);

      Unbind<IInstantiator>();
      Unbind<IResolver>();
      Bind<IResolver>().FromInstance(this);
      Bind<IInstantiator>().FromInstance(this);
    }

    public DependencyBox CreateSubContainer() => new(new List<DependencyBox>(_parents) {this});

    public ContractBinder<TContract> Bind<TContract>() => _binder.Bind<TContract>();
    public void Unbind<TContract>() => _binder.Unbind<TContract>();

    public TContract Resolve<TContract>() => _resolver.Resolve<TContract>();
    public object Resolve(Type contractType) => _resolver.Resolve(contractType);

    public TConcrete Instantiate<TConcrete>() => _instantiator.Instantiate<TConcrete>();
    public GameObject InstantiatePrefab(GameObject original) => _instantiator.InstantiatePrefab(original);
    public object Instantiate(Type bindingConcreteType) => _instantiator.Instantiate(bindingConcreteType);

    public void Dispose()
    {
      foreach (var binding in Container)
        (binding.Value.Instance as IDisposable)?.Dispose();
    }
  }
}