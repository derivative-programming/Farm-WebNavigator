using System;
using System.Collections.Generic;
using System.Text;

namespace FS.Common.Arrays
{
    public class Functions
    {

        public static Int32[] DeDupArray(Int32[] resultList)
        {
            Int32[] resultList2;
            int j = 0;
            resultList2 = new Int32[0];
            Array.Sort(resultList);
            Int32 lastValue = -1;
            Int32 count = 0;

            for (int i = 0; i <= resultList.GetUpperBound(0); i++)
            {
                if ((resultList[i] != lastValue) && resultList[i] != -1)
                {
                    count += 1;
                }
                lastValue = resultList[i];
            }
            lastValue = -1;
            j = 0;
            if (count > 0)
            {
                resultList2 = new Int32[count];
                for (int i = 0; i <= resultList.GetUpperBound(0); i++)
                {
                    if ((resultList[i] != lastValue) && resultList[i] != -1)
                    {
                        resultList2[j] = resultList[i];
                        j = j + 1;
                    }
                    lastValue = resultList[i];
                }
            }

            return resultList2;

        }

        public static string[] DeDupArray(string[] resultList)
        {
            string[] resultList2;
            int j = 0;
            resultList2 = new string[0];
            Array.Sort(resultList);
            string lastValue = string.Empty ;
            Int32 count = 0;

            for (int i = 0; i <= resultList.GetUpperBound(0); i++)
            {
                if ((resultList[i] != lastValue) && resultList[i] != string.Empty)
                {
                    count += 1;
                }
                lastValue = resultList[i];
            }
            lastValue = string.Empty;
            j = 0;
            if (count > 0)
            {
                resultList2 = new string[count];
                for (int i = 0; i <= resultList.GetUpperBound(0); i++)
                {
                    if ((resultList[i] != lastValue) && resultList[i] != string.Empty)
                    {
                        resultList2[j] = resultList[i];
                        j = j + 1;
                    }
                    lastValue = resultList[i];
                }
            }

            return resultList2;

        }
        public static List<string> ToList(string[] array)
        {
            List<string> results = new List<string>();
            for (int i = 0; i < array.Length; i++)
            {
                results.Add(array[i]);
            }
            return results;
        }
        public static List<int> ToList(int[] array)
        {
            List<int> results = new List<int>();
            for (int i = 0; i < array.Length; i++)
            {
                results.Add(array[i]);
            }
            return results;
        }

    }
}
