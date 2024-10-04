using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using FS.Farm.WebNavigator.Page.Reports.Init;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace FS.Farm.WebNavigator.Page.Reports.Models
{
    public class PacUserDateGreaterThanFilterListListModel
    {
        [Newtonsoft.Json.JsonProperty("pageNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int PageNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("items", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<PacUserDateGreaterThanFilterListListModelItem> Items { get; set; }

        [Newtonsoft.Json.JsonProperty("itemCountPerPage", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int ItemCountPerPage { get; set; }

        [Newtonsoft.Json.JsonProperty("orderByColumnName", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string OrderByColumnName { get; set; }

        [Newtonsoft.Json.JsonProperty("orderByDescending", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool OrderByDescending { get; set; }

        [Newtonsoft.Json.JsonProperty("success", Required = Newtonsoft.Json.Required.Always)]
        public bool Success { get; set; }

        [Newtonsoft.Json.JsonProperty("recordsTotal", Required = Newtonsoft.Json.Required.Always)]
        public int RecordsTotal { get; set; }

        [Newtonsoft.Json.JsonProperty("recordsFiltered", Required = Newtonsoft.Json.Required.Always)]
        public int RecordsFiltered { get; set; }

        [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Message { get; set; }

        [Newtonsoft.Json.JsonProperty("appVersion", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string AppVersion { get; set; }

        [Newtonsoft.Json.JsonProperty("request", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public PacUserDateGreaterThanFilterListListRequest Request { get; set; }

    }

    public class PacUserDateGreaterThanFilterListListModelItem
    {
        [Newtonsoft.Json.JsonProperty("dateGreaterThanFilterCode", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //dateGreaterThanFilterCode
        public System.Guid DateGreaterThanFilterCode { get; set; }
        [Newtonsoft.Json.JsonProperty("dateGreaterThanFilterDayCount", Required = Newtonsoft.Json.Required.Always)]
        public int DateGreaterThanFilterDayCount { get; set; }
        [Newtonsoft.Json.JsonProperty("dateGreaterThanFilterDescription", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //dateGreaterThanFilterDescription
        public string DateGreaterThanFilterDescription { get; set; }
        [Newtonsoft.Json.JsonProperty("dateGreaterThanFilterDisplayOrder", Required = Newtonsoft.Json.Required.Always)]
        public int DateGreaterThanFilterDisplayOrder { get; set; }
        [Newtonsoft.Json.JsonProperty("dateGreaterThanFilterIsActive", Required = Newtonsoft.Json.Required.Always)]
        public bool DateGreaterThanFilterIsActive { get; set; }
        [Newtonsoft.Json.JsonProperty("dateGreaterThanFilterLookupEnumName", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //dateGreaterThanFilterLookupEnumName
        public string DateGreaterThanFilterLookupEnumName { get; set; }
        [Newtonsoft.Json.JsonProperty("dateGreaterThanFilterName", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //dateGreaterThanFilterName
        public string DateGreaterThanFilterName { get; set; }

    }

    public class PacUserDateGreaterThanFilterListListRequest
    {

        [Newtonsoft.Json.JsonProperty("pageNumber", Required = Newtonsoft.Json.Required.Always)]
        public int PageNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("itemCountPerPage", Required = Newtonsoft.Json.Required.Always)]
        public int ItemCountPerPage { get; set; }

        [Newtonsoft.Json.JsonProperty("orderByColumnName", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string OrderByColumnName { get; set; }

        [Newtonsoft.Json.JsonProperty("orderByDescending", Required = Newtonsoft.Json.Required.Always)]
        public bool OrderByDescending { get; set; }

        [Newtonsoft.Json.JsonProperty("forceErrorMessage", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ForceErrorMessage { get; set; }

    }
}

