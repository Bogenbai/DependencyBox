using CodeBase.Framework.Attributes;
using UnityEngine;

namespace CodeBase.Tests.TestingTools.Stubs
{
  public class StubMonoBehaviourWithDependencies : MonoBehaviour
  {
    private IStub _stub;

    [Inject]
    private void SetDependencies(IStub stub)
    {
      _stub = stub;
    }

    public IStub GetStub() => _stub;
  }
}