using JetBrains.Annotations;

namespace CodeBase.Framework
{
  
  [PublicAPI]
  public interface IBinder
  {
    ContractBinder<TContract> Bind<TContract>();
    void Unbind<TContract>();
  }
}