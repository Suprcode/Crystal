using System;
using System.Collections;

namespace Map_Editor
{
  public class FilesNameComparerClass : IComparer
  {
    int IComparer.Compare(object x, object y)
    {
      string str1 = x != null && y != null ? x as string : throw new ArgumentException("Parameters can't be null");
      string str2 = y as string;
      char[] charArray1 = str1.ToCharArray();
      char[] charArray2 = str2.ToCharArray();
      int index1 = 0;
      int index2 = 0;
      while (index1 < charArray1.Length && index2 < charArray2.Length)
      {
        if (char.IsDigit(charArray1[index1]) && char.IsDigit(charArray2[index2]))
        {
          string s1 = "";
          string s2 = "";
          for (; index1 < charArray1.Length && char.IsDigit(charArray1[index1]); ++index1)
            s1 += charArray1[index1].ToString();
          for (; index2 < charArray2.Length && char.IsDigit(charArray2[index2]); ++index2)
            s2 += charArray2[index2].ToString();
          if (int.Parse(s1) > int.Parse(s2))
            return 1;
          if (int.Parse(s1) < int.Parse(s2))
            return -1;
        }
        else
        {
          if ((int) charArray1[index1] > (int) charArray2[index2])
            return 1;
          if ((int) charArray1[index1] < (int) charArray2[index2])
            return -1;
          ++index1;
          ++index2;
        }
      }
      return charArray1.Length == charArray2.Length ? 0 : (charArray1.Length > charArray2.Length ? 1 : -1);
    }
  }
}
