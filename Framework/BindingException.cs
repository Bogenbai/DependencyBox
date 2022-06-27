using System;

namespace CodeBase.Framework
{
  public class BindingException : Exception
  {
    public BindingException(string message = null) : base(message)
    {
    }
  }
}