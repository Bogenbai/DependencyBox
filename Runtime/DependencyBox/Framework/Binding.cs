using System;

namespace CodeBase.Runtime.DependencyBox.Framework
{
  public class Binding
  {
    public Type ConcreteType { get;set; }
    public object Instance { get; set; }
  }
}