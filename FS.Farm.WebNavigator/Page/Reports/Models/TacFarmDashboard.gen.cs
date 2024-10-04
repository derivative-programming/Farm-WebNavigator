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
    public class TacFarmDashboardListModel
    {
        [Newtonsoft.Json.JsonProperty("pageNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int PageNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("items", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<TacFarmDashboardListModelItem> Items { get; set; }

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
        public TacFarmDashboardListRequest Request { get; set; }

    }

    public class TacFarmDashboardListModelItem
    {
        [Newtonsoft.Json.JsonProperty("fieldOnePlantListLinkLandCode", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //fieldOnePlantListLinkLandCode
        public System.Guid FieldOnePlantListLinkLandCode { get; set; }
        [Newtonsoft.Json.JsonProperty("conditionalBtnExampleLinkLandCode", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //conditionalBtnExampleLinkLandCode
        public System.Guid ConditionalBtnExampleLinkLandCode { get; set; }
        [Newtonsoft.Json.JsonProperty("isConditionalBtnAvailable", Required = Newtonsoft.Json.Required.Always)]
        public bool IsConditionalBtnAvailable { get; set; }
        [Newtonsoft.Json.JsonProperty("testFileDownloadLinkPacCode", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //testFileDownloadLinkPacCode
        public System.Guid TestFileDownloadLinkPacCode { get; set; }
        [Newtonsoft.Json.JsonProperty("testConditionalFileDownloadLinkPacCode", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //testConditionalFileDownloadLinkPacCode
        public System.Guid TestConditionalFileDownloadLinkPacCode { get; set; }
        [Newtonsoft.Json.JsonProperty("testAsyncFlowReqLinkPacCode", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //testAsyncFlowReqLinkPacCode
        public System.Guid TestAsyncFlowReqLinkPacCode { get; set; }
        [Newtonsoft.Json.JsonProperty("testConditionalAsyncFlowReqLinkPacCode", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //testConditionalAsyncFlowReqLinkPacCode
        public System.Guid TestConditionalAsyncFlowReqLinkPacCode { get; set; }

    }

    public class TacFarmDashboardListRequest
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

