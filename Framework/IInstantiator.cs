using System;
using JetBrains.Annotations;
using UnityEngine;

namespace CodeBase.Framework
{
  [PublicAPI]
  public interface IInstantiator
  {
    TConcrete Instantiate<TConcrete>();
    GameObject InstantiatePrefab(GameObject original);
    object Instantiate(Type bindingConcreteType);
  }
}