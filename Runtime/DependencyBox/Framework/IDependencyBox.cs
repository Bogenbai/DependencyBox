using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace CodeBase.Runtime.DependencyBox.Framework
{
  [PublicAPI]
  public interface IDependencyBox
  {
    Dictionary<Type, Binding> Container { get; }
    DependencyBox CreateSubContainer();
  }
}