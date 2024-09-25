using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator.Page
{
    public class PageBase
    {
        protected string _pageName = string.Empty;

        protected PageView AddDefaultAvailableCommands(PageView pageView)
        {
            pageView.AvailableCommands.Add(new AvailableCommand { CommandText = "MM", CommandTitle = "Main Menu", CommandDescription = "Go To Main Menu" });
            pageView.AvailableCommands.Add(new AvailableCommand { CommandText = "RP", CommandTitle = "Refresh Page", CommandDescription = "Refresh the current page" });

            return pageView;
        }

        protected PagePointer ProcessDefaultCommands(string commandText, Guid contextCode)
        {
            PagePointer result = null;

            if (commandText == "MM")
            {
                result = new PagePointer("MainMenu", Guid.Empty);
            }

            if (commandText == "RP")
            {
                result = new PagePointer(this._pageName, contextCode);
            }

            return result;
        } 
        public static void MergeProperties<T, U>(T target, U source)
        {
            PropertyInfo[] sourceProperties = typeof(U).GetProperties();
            PropertyInfo[] targetProperties = typeof(T).GetProperties();

            foreach (var sourceProp in sourceProperties)
            {
                foreach (var targetProp in targetProperties)
                {
                    if (targetProp.Name == sourceProp.Name && targetProp.PropertyType == sourceProp.PropertyType && targetProp.CanWrite)
                    {
                        targetProp.SetValue(target, sourceProp.GetValue(source));
                        break;
                    }
                }
            }
        }


        public static void MergeProperties<T>(T target, string jsonSource)
        {
            if (string.IsNullOrWhiteSpace(jsonSource))
            {
                // If the source string is empty or null, do nothing
                return;
            }

            // Deserialize the JSON string to a dictionary with property names and values
            var sourceDictionary = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonSource);
            if (sourceDictionary == null) return;

            // Use reflection to set the values in the target object
            PropertyInfo[] targetProperties = typeof(T).GetProperties();

            foreach (var sourceProp in sourceDictionary)
            {
                foreach (var targetProp in targetProperties)
                {
                    if (targetProp.Name == sourceProp.Key && targetProp.CanWrite)
                    {
                        // Convert the JsonElement to the target property type and set the value
                        object? value = sourceProp.Value.Deserialize(targetProp.PropertyType);
                        targetProp.SetValue(target, value);
                        break;
                    }
                }
            }
        }
    }
}
