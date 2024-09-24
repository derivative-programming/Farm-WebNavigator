using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace FS.Common.Factory
{
    public static class Build
    {

        #region Assembly Type Enumerators

        public enum AssemblyNameTypes : short
        {
            Type2 = 2,
            Type3
        }

        #endregion
        #region Constructors

        //N/A

        #endregion

        #region Public Member Types

        static public object Create(string memberAndClass)
        {
            return FS.Common.Factory.Build.Create(null, memberAndClass);


        }
        static public object Create(object[] args, string memberAndClass)
        {

            // Define variables
            string path = string.Empty;
            string className = string.Empty;

            object bufferType = null;

            try
            {

                // Verify argument list
                if (args != null && args.Length == 0)
                {
                    throw new ArgumentException("Invalid argument list. Value can not be null or must have one or more items.");
                }

                //GetPathAndType(memberAndClass, ref path, ref className);

                //bufferType = AppDomain.CurrentDomain.CreateInstanceAndUnwrap(path, className, true, BindingFlags.Default, null, args, null, null, null);
                if (Type.GetType(memberAndClass) != null)
                {
                    bufferType = System.Activator.CreateInstance(Type.GetType(memberAndClass), args);
                }
                if (bufferType == null)
                {
                    bufferType = FS.Common.Factory.Build.Create(args, memberAndClass, AssemblyNameTypes.Type2);
                }
                if (bufferType == null)
                {
                    bufferType = FS.Common.Factory.Build.Create(args, memberAndClass, AssemblyNameTypes.Type3);
                }
                 


                if (bufferType == null)
                {
                    throw new Exception(string.Format("Factory build failed for {0}. Verify application configuration settings for this component creation.", memberAndClass));
                }

                return bufferType;

            } 
            catch (System.Exception ex)
            {
                throw new Exception(string.Format("Building class - {0} - caused the following: {1}", className, ex.Message));
            }

        }

        static private object Create(object[] args, string memberAndClass, AssemblyNameTypes assemblyType)
        {

            // Define variables
            string path = string.Empty;
            string className = string.Empty;
            string[] memberSplit;

            System.Int16 assemblySections;

            object bufferType = null;


            // Verify member type
            if (memberAndClass.Length.Equals(0))
            {
                throw new System.ArgumentException("Invalid parameter for create. Member and class type was not found. Verify configuration settings for the member type settings.");
            }

            // Set assemby name type
            assemblySections = (System.Int16)assemblyType;

            // Load member type and class name. 
            memberSplit = memberAndClass.Split('.');
            for (Int16 i = 0; i < assemblySections; i++)
            {
                path += memberSplit[i] + ".";
            }
            path = path.TrimEnd('.');

            className = memberAndClass;

            try
            {
                bufferType = Activator.CreateInstance(Type.GetType(className), args);
                //bufferType = AppDomain.CurrentDomain.CreateInstanceAndUnwrap(path, className, true, BindingFlags.Default, null, args, null, null, null);
            }
            catch (System.Exception)
            {
            }


            return bufferType;

        }


        #endregion

        #region Helper Functions

        private static void GetPathAndType(string pathAndType, ref string path, ref string fullType)
        {

            string className = string.Empty;
            string[] memberSplit;

            // Verify member type
            if (pathAndType.Length.Equals(0))
            {
                throw new System.ArgumentException("Invalid parameter for create. Member and class type was not found. Verify configuration settings for the member type settings.");
            }

            memberSplit = pathAndType.Split('.');
            for (Int16 i = 0; i < 2; i++)
            {
                path += memberSplit[i] + ".";
            }
            path = path.TrimEnd('.');

            fullType = pathAndType;

        }

        #endregion

    }

}
