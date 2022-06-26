using System;

namespace CodeBase.Runtime.DependencyBox.Framework
{
  public class BindingException : Exception
  {
    public BindingException(string message = null) : base(message)
    {
    }
  }
}