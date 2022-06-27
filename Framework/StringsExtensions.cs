using System.Text;

namespace CodeBase.Framework
{
  public static class StringsExtensions
  {
    public static string AddSpacesBetweenCapital(this string text)
    {
      if (string.IsNullOrWhiteSpace(text))
        return string.Empty;

      var newText = new StringBuilder(text.Length * 2);
      newText.Append(text[0]);

      for (int i = 1; i < text.Length; i++)
      {
        if (char.IsUpper(text[i]))
          if (text[i - 1] != ' ' && !char.IsUpper(text[i - 1]) ||
              char.IsUpper(text[i - 1]) &&
              i < text.Length - 1 && !char.IsUpper(text[i + 1]))
            newText.Append(' ');
        newText.Append(text[i]);
      }

      return newText.ToString();
    }
  }
}