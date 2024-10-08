﻿using Newtonsoft.Json.Serialization;
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
        public bool IsAutoSubmit { get; set; } 

        protected PageView AddDefaultAvailableCommands(PageView pageView)
        {
            if(_pageName != "MainMenu")
            {
                pageView.AvailableCommands.Add(new AvailableCommand { CommandText = "MM", Description = "Go To Main Menu" });
            }
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
                    if (targetProp.Name.Equals(sourceProp.Key,StringComparison.OrdinalIgnoreCase) && targetProp.CanWrite)
                    {
                        if(targetProp.PropertyType.Name.Equals("Int32",StringComparison.OrdinalIgnoreCase))
                        {
                            int testVal = 0;
                            if(int.TryParse(sourceProp.Value.ToString(),out testVal))
                            {
                                targetProp.SetValue(target, testVal);
                            }
                            else
                            {
                                targetProp.SetValue(target, null);
                            }
                        }
                        else if (targetProp.PropertyType.Name.Equals("Boolean", StringComparison.OrdinalIgnoreCase))
                        {
                            bool testVal = false;
                            if (bool.TryParse(sourceProp.Value.ToString(), out testVal))
                            {
                                targetProp.SetValue(target, testVal);
                            }
                            else
                            {
                                targetProp.SetValue(target, null);
                            }
                        }
                        else if (targetProp.PropertyType.Name.Equals("DateTime", StringComparison.OrdinalIgnoreCase))
                        {
                            DateTime testVal = DateTime.UtcNow;
                            if (DateTime.TryParse(sourceProp.Value.ToString(), out testVal))
                            {
                                targetProp.SetValue(target, testVal);
                            }
                            else
                            {
                                targetProp.SetValue(target, null);
                            }
                        }
                        else
                        {
                            // Convert the JsonElement to the target property type and set the value
                            object? value = sourceProp.Value.Deserialize(targetProp.PropertyType);
                            targetProp.SetValue(target, value);

                        }
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

        public PageView BuildAvailableCommandForObjWFButton(
            PageView pageView,
            string name,
            string destinationPageName,
            string codeName,
            bool isVisible,
            bool isEnabled,
            string buttonText)
        {
            if (!isVisible)
                return pageView;

            if (!isEnabled)
                return pageView;

            pageView.AvailableCommands.Add(
                new AvailableCommand { CommandText = name, Description = buttonText }
                );

            return pageView;
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

            pageView.PageTable.TableHeaders.Add(name, headerText);

            return pageView;
        }
        public PageView BuildTableAvailableFilter(
            PageView pageView,
            string name,
            bool isVisible,
            string labelText,
            string dataType)
        {
            if (!isVisible)
                return pageView;

            pageView.PageTable.tableAvailableFilters.Add(
                new TableAvailableFilter()
                {
                    DataType = dataType,
                    Label = labelText,
                    Name = name
                });

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
