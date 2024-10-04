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
    public class PacUserTriStateFilterListListModel
    {
        [Newtonsoft.Json.JsonProperty("pageNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int PageNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("items", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<PacUserTriStateFilterListListModelItem> Items { get; set; }

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
        public PacUserTriStateFilterListListRequest Request { get; set; }

    }

    public class PacUserTriStateFilterListListModelItem
    {
        [Newtonsoft.Json.JsonProperty("triStateFilterCode", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //triStateFilterCode
        public System.Guid TriStateFilterCode { get; set; }
        [Newtonsoft.Json.JsonProperty("triStateFilterDescription", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //triStateFilterDescription
        public string TriStateFilterDescription { get; set; }
        [Newtonsoft.Json.JsonProperty("triStateFilterDisplayOrder", Required = Newtonsoft.Json.Required.Always)]
        public int TriStateFilterDisplayOrder { get; set; }
        [Newtonsoft.Json.JsonProperty("triStateFilterIsActive", Required = Newtonsoft.Json.Required.Always)]
        public bool TriStateFilterIsActive { get; set; }
        [Newtonsoft.Json.JsonProperty("triStateFilterLookupEnumName", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //triStateFilterLookupEnumName
        public string TriStateFilterLookupEnumName { get; set; }
        [Newtonsoft.Json.JsonProperty("triStateFilterName", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //triStateFilterName
        public string TriStateFilterName { get; set; }
        [Newtonsoft.Json.JsonProperty("triStateFilterStateIntValue", Required = Newtonsoft.Json.Required.Always)]
        public int TriStateFilterStateIntValue { get; set; }

    }

    public class PacUserTriStateFilterListListRequest
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

