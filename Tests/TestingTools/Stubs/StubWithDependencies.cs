using JetBrains.Annotations;

namespace CodeBase.Tests.TestingTools.Stubs
{
  [UsedImplicitly]
  public class StubWithDependencies : IStub
  {
    private readonly int _value;

    public StubWithDependencies(int value)
    {
      _value = value;
    }

    public int GetValue() => _value;
  }
}