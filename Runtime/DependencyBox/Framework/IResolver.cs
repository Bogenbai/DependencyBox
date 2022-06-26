using System;
using JetBrains.Annotations;

namespace CodeBase.Runtime.DependencyBox.Framework
{
  [PublicAPI]
  public interface IResolver
  {
    TContract Resolve<TContract>();
    object Resolve(Type contractType);
  }
}