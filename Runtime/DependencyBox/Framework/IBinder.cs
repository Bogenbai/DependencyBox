using JetBrains.Annotations;

namespace CodeBase.Runtime.DependencyBox.Framework
{
  
  [PublicAPI]
  public interface IBinder
  {
    ContractBinder<TContract> Bind<TContract>();
    void Unbind<TContract>();
  }
}