using System.Collections;

namespace Map_Editor
{
  public class AlphanumComparatorFast : IComparer
  {
    public int Compare(object x, object y)
    {
      if (!(x is string str1) || !(y is string str2))
        return 0;
      int length1 = str1.Length;
      int length2 = str2.Length;
      int index1 = 0;
      int index2 = 0;
      while (index1 < length1 && index2 < length2)
      {
        char c1 = str1[index1];
        char c2 = str2[index2];
        char[] chArray1 = new char[length1];
        int num1 = 0;
        char[] chArray2 = new char[length2];
        int num2 = 0;
        do
        {
          chArray1[num1++] = c1;
          ++index1;
          if (index1 < length1)
            c1 = str1[index1];
          else
            break;
        }
        while (char.IsDigit(c1) == char.IsDigit(chArray1[0]));
        do
        {
          chArray2[num2++] = c2;
          ++index2;
          if (index2 < length2)
            c2 = str2[index2];
          else
            break;
        }
        while (char.IsDigit(c2) == char.IsDigit(chArray2[0]));
        string s = new string(chArray1);
        string str3 = new string(chArray2);
        int num3 = !char.IsDigit(chArray1[0]) || !char.IsDigit(chArray2[0]) ? s.CompareTo(str3) : int.Parse(s).CompareTo(int.Parse(str3));
        if ((uint) num3 > 0U)
          return num3;
      }
      return length1 - length2;
    }
  }
}
