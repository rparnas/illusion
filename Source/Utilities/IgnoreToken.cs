namespace Illusion.Utilities;

internal class IgnoreToken
{
  int Count;

  public bool Ignore => Count > 0;

  public void Do(Action action)
  {
    Count++;

    try
    {
      action.Invoke();
    }
    finally
    {
      Count--;
    }
  }
}
