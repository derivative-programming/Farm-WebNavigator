using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
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
            pageView.AvailableCommands.Add(new AvailableCommand { CommandText = "MM", Description = "Go To Main Menu" });
            pageView.AvailableCommands.Add(new AvailableCommand { CommandText = "RP", Description = "Refresh the current page" });

            return pageView;
        }

        protected PagePointer ProcessDefaultCommands(string commandText, Guid contextCode)
        {
            PagePointer result = null;

            if (commandText.Equals("MM",StringComparison.OrdinalIgnoreCase))
            {
                result = new PagePointer("MainMenu", Guid.Empty);
            }

            if (commandText.Equals("RP",StringComparison.OrdinalIgnoreCase))
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
            var sourceDictionary = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonSource);
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
        protected string ToQueryString(object obj)
        {
            // Serialize the object to a JSON string with custom settings (camelCase, etc.)
            var json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(), // Adjust case here (optional)
                NullValueHandling = NullValueHandling.Ignore // Ignore null values
            });

            // Deserialize the JSON into a dictionary
            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            // Convert the dictionary to query string
            var queryString = string.Join("&", dict.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value?.ToString())}"));

            return queryString;
        }

        public PageView BuildAvailableCommandForReportButton(
            PageView pageView,
            string name,
            string destinationPageName,
            string codeName,
            bool isVisible,
            bool isEnabled,
            string buttonText,
            bool conditionallyVisible = true)
        {
            if (!isVisible)
                return pageView;

            if (!isEnabled)
                return pageView;

            if (!conditionallyVisible)
                return pageView;

            pageView.AvailableCommands.Add(
                new AvailableCommand { CommandText = name,  Description = buttonText }
                );

            return pageView;
        }

        public PageView BuildAvailableCommandForSortOnColumn(
            PageView pageView,
            string name, 
            bool isVisible, 
            string headerText)
        {
            if (!isVisible)
                return pageView; 

            pageView.AvailableCommands.Add(
                new AvailableCommand { CommandText = "sortOnColumn:" + name, Description = "Sort on column '" + headerText + "'"}
                );

            return pageView;
        }

        public PageView BuildTableHeader(
            PageView pageView,
            string name,
            bool isVisible,
            string headerText)
        {
            if (!isVisible)
                return pageView;

            pageView.TableHeaders.Add(name, headerText);

            return pageView;
        }
        public Dictionary<string, string> BuildTableDataCellValue(
            Dictionary<string, string> rowData,
            string name,
            string value,
            bool isVisible,
            bool conditionallyVisible = true
            )
        {
            if (!isVisible)
                value = string.Empty;

            if (!conditionallyVisible)
                value = string.Empty;

            rowData.Add(name, value);

            return rowData;
        }
    }
}
