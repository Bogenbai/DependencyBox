using JetBrains.Annotations;
using UnityEngine;

namespace Tests.TestingTools.Stubs
{
  public class StubMonoBehaviourWithDependencies : MonoBehaviour
  {
    private IStub _stub;

    [UsedImplicitly]
    public void SetDependencies(IStub stub)
    {
      _stub = stub;
    }

    public IStub GetStub() => _stub;
  }
}