using System;
using JetBrains.Annotations;

namespace CodeBase.Framework.Attributes
{
  [MeansImplicitUse(ImplicitUseKindFlags.Assign)]
  [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
  public class InjectAttribute : Attribute
  {
  }
}