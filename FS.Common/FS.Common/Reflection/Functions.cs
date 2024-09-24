using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection; 

namespace FS.Common.Reflection
{
    public class Functions
    {
        public static object GetObject(object baseObject, string path)
        {
            object currentObject = baseObject;
            string[] pathInfo = path.Split('.');
            for (int i = 1; i < pathInfo.Length; i++)
            {
                string cmd = pathInfo[i];
                if(cmd.Contains("()"))
                {
                    string methodname = cmd.Substring(0,(cmd.IndexOf('(')));
                    currentObject = CallMethod(currentObject, methodname);
                }
                else if (cmd.Contains("(") && cmd.Contains(")"))
                {
                    string methodname = cmd.Substring(0, (cmd.IndexOf('(')));
                    int val1 = cmd.IndexOf('(');
                    int val2 = cmd.IndexOf(')');
                    string argData = cmd.Substring(val1+1, val2 - val1-1);
                    currentObject = CallMethod(currentObject, methodname, argData.Split(','));
                }
                else
                {
                    string propertyName = cmd;
                    currentObject = GetPropertyValueAsObject(currentObject, propertyName);
                }
            }

            return currentObject;
        }

        public static bool PropertyExists(object obj, string propertyName)
        {
            bool found = false;
            PropertyInfo[] props = obj.GetType().GetProperties();
            foreach (PropertyInfo p in props)
            {
                if (p.Name.ToLower() != propertyName.ToLower())
                    continue;
                found = true;
                break;
            } 

            return found;
        }
        public static string GetPropertyValue(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null).ToString();
        }

        public static Type GetPropertyDataType(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).PropertyType;
        }

        public static object CreateObject(string assemblyName, string objectClassName)
        {
            Assembly asm = Assembly.LoadWithPartialName(assemblyName);
            object returnObj = asm.CreateInstance(objectClassName);
            return returnObj;
        }

        public static string[] GetProperties(object obj)
        {
            PropertyInfo[] propertyInfo = obj.GetType().GetProperties();
            string[] propertyList = new string[propertyInfo.Length];
            for(int i=0; i < propertyInfo.Length; i++) {
                propertyList[i] = propertyInfo[i].Name;
            }
            return propertyList;
        }

        public static object GetPropertyValueAsObject(object obj, string propertyName)
        { 
            PropertyInfo[] props = obj.GetType().GetProperties();
            foreach (PropertyInfo p in props)
            {
                if (p.Name.ToLower() != propertyName.ToLower())
                    continue;
                //found = true;
                return p.GetValue(obj,null);
            }
            return null;// obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }
        public static void SetPropertyValue(ref object obj, string propertyName, string propertyValue)
        {
            Type type = obj.GetType();
            System.Reflection.PropertyInfo propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo.PropertyType == Guid.NewGuid().GetType())
            {
                propertyInfo.SetValue(obj, Guid.Parse(propertyValue), null);

            }
            else
                propertyInfo.SetValue(obj, System.Convert.ChangeType(propertyValue, propertyInfo.PropertyType), null);
        }

        public static bool MethodExists(object obj, string methodName)
        {
            Type t = obj.GetType(); 

            MethodInfo[] mi = t.GetMethods();

            bool found = false;
            foreach (MethodInfo m in mi)
            {
                if (m.Name.ToLower() != methodName.ToLower())
                    continue; 
                found = true;
                break;
            }

            return found;
        }
        public static object CallMethod(object obj, string methodName)
        {
            Type t = obj.GetType();
            object result = null;

            MethodInfo[] mi = t.GetMethods();

            bool found = false; 
            foreach (MethodInfo m in mi)
            {
                if(m.Name.ToLower() != methodName.ToLower())
                    continue;

                result = m.Invoke(obj, null);
                found = true;
                break; 
            }
            if (!found)
                throw new System.Exception("Method not found: " + methodName);
            return result;
        }
        public static object CallMethod(object obj, string methodName, string[] paramVals)
        {
            Type t = obj.GetType();
            object result = null;

            MethodInfo[] mi = t.GetMethods();

            bool found = false; 

            foreach (MethodInfo m in mi)
            {
                if (m.Name.ToLower() != methodName.ToLower())
                    continue;
                 
                found = true;

                // Get the parameters 
                ParameterInfo[] pi = m.GetParameters();

                if (pi.Length != paramVals.Length)
                {
                    found = false;
                    continue;
                }
                    //throw new System.Exception("incorrect number of parameters");

                if (pi.Length == 0)
                    result = m.Invoke(obj, null);
                else
                {
                    object[] args = new object[pi.Length];
                    for (int i = 0; i < pi.Length; i++)
                    {
                        args[i] = System.Convert.ChangeType(paramVals[i], pi[i].ParameterType);
                    }
                    result = m.Invoke(obj, args);
                } 
                 
                break;

            }
            if (!found)
                throw new System.Exception("Method not found: " + methodName);
            return result;
        }
    }
}
