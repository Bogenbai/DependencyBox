using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeBase.Framework.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace CodeBase.Framework
{
  public class Instantiator : IInstantiator
  {
    private readonly IResolver _resolver;

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

    public void InjectToSceneGameObjects()
    {
      var sceneGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

      foreach (GameObject sceneGameObject in sceneGameObjects)
      {
        var components = sceneGameObject.GetComponentsInChildren<MonoBehaviour>();
        InjectInto(components);
      }
    }

    private void InjectInto(IEnumerable<MonoBehaviour> components)
    {
      foreach (MonoBehaviour component in components)
      {
        var injectMethods = component
          .GetType()
          .GetMethods()
          .Where(HasInjectMethods)
          .ToList();

        foreach (MethodInfo injectMethod in injectMethods)
        {
          var args = injectMethod.GetParameters();
          var argsToInject = new object[args.Length];

          for (var i = 0; i < args.Length; i++)
            argsToInject[i] = _resolver.Resolve(args[i].ParameterType);

          injectMethod.Invoke(component, argsToInject);
        }
      }
    }

    private object InstantiateMonoBehaviour(Type concreteType)
    {
      var gameObject = new GameObject(concreteType.Name.AddSpacesBetweenCapital());
      Component component = gameObject.AddComponent(concreteType);

      InjectInto(new List<MonoBehaviour> {component as MonoBehaviour});
      return component;
    }

    private static bool HasInjectMethods(MethodInfo methodInfo) =>
      methodInfo.GetCustomAttributes(typeof(InjectAttribute), false).Length > 0;
  }
}