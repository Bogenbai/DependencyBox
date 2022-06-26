using System;

namespace CodeBase.Runtime.DependencyBox.Framework
{
  public class ResolveException : Exception
  {
    public ResolveException(string message) : base(message)
    {
    }
  }
}