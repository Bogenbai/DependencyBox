using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Framework
{
  public class Instantiator : IInstantiator
  {
    private readonly IResolver _resolver;
    private const string InjectMethodName = "SetDependencies";

    public Instantiator(IResolver resolver)
    {
      _resolver = resolver;
    }

    public TConcrete Instantiate<TConcrete>() => (TConcrete) Instantiate(typeof(TConcrete));

    public object Instantiate(Type concreteType) => concreteType.IsSubclassOf(typeof(MonoBehaviour))
      ? InstantiateMonoBehaviour(concreteType)
      : InstantiateNonMonoBehaviour(concreteType);

    public GameObject InstantiatePrefab(GameObject original)
    {
      GameObject clone = Object.Instantiate(original);
      var components = clone.GetComponents<MonoBehaviour>();

      InjectInto(components);

      return clone;
    }

    private object InstantiateNonMonoBehaviour(Type concreteType)
    {
      var constructors = concreteType.GetConstructors();

      if (constructors.Length <= 0)
        return Activator.CreateInstance(concreteType);

      var args = constructors[0].GetParameters();
      var argsToInject = new object[args.Length];

      for (var i = 0; i < args.Length; i++)
        argsToInject[i] = _resolver.Resolve(args[i].ParameterType);

      return Activator.CreateInstance(concreteType, argsToInject);
    }

    private void InjectInto(IEnumerable<MonoBehaviour> components)
    {
      foreach (MonoBehaviour component in components)
      {
        MethodInfo injectMethod = component.GetType().GetMethod(
          InjectMethodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

        if (injectMethod == null) continue;

        var args = injectMethod.GetParameters();
        var argsToInject = new object[args.Length];

        for (var i = 0; i < args.Length; i++)
          argsToInject[i] = _resolver.Resolve(args[i].ParameterType);

        injectMethod.Invoke(component, argsToInject);
      }
    }

    private object InstantiateMonoBehaviour(Type concreteType)
    {
      var gameObject = new GameObject(concreteType.Name.AddSpacesBetweenCapital());
      Component component = gameObject.AddComponent(concreteType);

      InjectInto(new List<MonoBehaviour> {component as MonoBehaviour});
      return component;
    }
  }
}