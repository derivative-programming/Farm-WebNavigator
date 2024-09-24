using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FS.Common.Lists
{
    public class Functions
    {

        public static List<string> DeDupList(List<string> list)
        {
            string[] tmparray = FS.Common.Arrays.Functions.DeDupArray(list.ToArray());
            return FS.Common.Arrays.Functions.ToList(tmparray);
        }
        public static List<int> DeDupList(List<int> list)
        {
            int[] tmparray = FS.Common.Arrays.Functions.DeDupArray(list.ToArray());
            return FS.Common.Arrays.Functions.ToList(tmparray);
        }

        public static string ToDelimitedString(List<string> list, string delimiter)
        {
            string result = string.Empty;
            for (int i = 0; i < list.Count; i++)
            {
                result += delimiter + list[i];
            }
            return result.TrimStart(delimiter.ToCharArray()).TrimEnd(delimiter.ToCharArray());
        }


        public static List<string> DeDupList(ref List<string> list, int requiredNumberOfDuplicates)
        {
            List<string> resultList = new List<string>();
            string lastValue = string.Empty;
            int lastValueCount = 0;
            list.Sort();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Equals(lastValue))
                        lastValueCount = lastValueCount + 1;
                    if ((!list[i].Equals(lastValue)) && list[i].Trim().Length > 0)
                        lastValueCount = 0;
                    else if (list[i].Equals(lastValue) && list[i].Trim().Length > 0 &&
                        (lastValueCount + 1) == requiredNumberOfDuplicates)
                        resultList.Add(list[i]);
                    lastValue = list[i];
                }
            }
            return resultList;
        }

        public static List<int> DeDupList(ref List<int> list, int requiredNumberOfEachValue)
        {
            List<Int32> resultList = new List<int>();
            Int32 lastValue = -1;
            int itemCount = 0;

            if (list.Count >= requiredNumberOfEachValue)
            {
                list.Sort();
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    int val = list[i];
                    if (val == lastValue)
                        itemCount = itemCount + 1;
                    else //if (list[i] != lastValue)
                    {
                        itemCount = 1;
                        if ((i + requiredNumberOfEachValue - 1) >= count)
                            break;
                    }
                    if (itemCount == requiredNumberOfEachValue)
                        resultList.Add(val);
                    lastValue = val;
                }
            }
            return resultList;
        }

        public static System.Data.DataTable GetDataTableNative<TObject>(List<TObject> list)
            where TObject : class, new()
        {

            System.Data.DataTable resultDataTable = new System.Data.DataTable("results");
            System.Data.DataRow resultDataRow = null;

            //Meta Data. 
            TObject demo = new TObject();
            //System.Reflection.PropertyInfo[]    itemProperties = list[0].GetType().GetProperties();
            System.Reflection.PropertyInfo[] itemProperties = demo.GetType().GetProperties();

            //itemProperties =
            //list[0].GetType().GetProperties();
            itemProperties = demo.GetType().GetProperties();
            foreach (System.Reflection.PropertyInfo p in itemProperties)
            {
                resultDataTable.Columns.Add(p.Name, p.GetGetMethod().ReturnType);
            }

            //Data
            foreach (TObject item in list)
            {
                //' Get the data from this item into a DataRow
                //' then add the DataRow to the  DataTable.
                //' Eeach items property becomes a colunm.

                itemProperties = item.GetType().GetProperties();
                resultDataRow = resultDataTable.NewRow();
                foreach (System.Reflection.PropertyInfo p in itemProperties)
                {
                    resultDataRow[p.Name] = p.GetValue(item, null);
                }

                resultDataTable.Rows.Add(resultDataRow);

            }
            return resultDataTable;
        }

        public static List<string> GetIntersection(List<string> list1, List<string> list2)
        {

            return list1.Select(i => i.ToString()).Intersect(list2).ToList();
        }
    }
}
