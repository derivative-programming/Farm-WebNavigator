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
    public class LandPlantListListModel
    {
        [Newtonsoft.Json.JsonProperty("pageNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int PageNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("items", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<LandPlantListListModelItem> Items { get; set; }

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
        public LandPlantListListRequest Request { get; set; } 

    }


    public class LandPlantListListModelItem
    {
        [Newtonsoft.Json.JsonProperty("plantCode", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //plantCode
        public System.Guid PlantCode { get; set; }

        [Newtonsoft.Json.JsonProperty("someIntVal", Required = Newtonsoft.Json.Required.Always)]
        public int SomeIntVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someConditionalIntVal", Required = Newtonsoft.Json.Required.Always)]
        public int SomeConditionalIntVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someBigIntVal", Required = Newtonsoft.Json.Required.Always)]
        public long SomeBigIntVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someConditionalBigIntVal", Required = Newtonsoft.Json.Required.Always)]
        public long SomeConditionalBigIntVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someBitVal", Required = Newtonsoft.Json.Required.Always)]
        public bool SomeBitVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someConditionalBitVal", Required = Newtonsoft.Json.Required.Always)]
        public bool SomeConditionalBitVal { get; set; }

        [Newtonsoft.Json.JsonProperty("isEditAllowed", Required = Newtonsoft.Json.Required.Always)]
        public bool IsEditAllowed { get; set; }

        [Newtonsoft.Json.JsonProperty("isDeleteAllowed", Required = Newtonsoft.Json.Required.Always)]
        public bool IsDeleteAllowed { get; set; }

        [Newtonsoft.Json.JsonProperty("someFloatVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //someFloatVal
        public string SomeFloatVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someConditionalFloatVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //someConditionalFloatVal
        public string SomeConditionalFloatVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someDecimalVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //someDecimalVal
        public string SomeDecimalVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someConditionalDecimalVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //someConditionalDecimalVal
        public string SomeConditionalDecimalVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someUTCDateTimeVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //someUTCDateTimeVal
        public DateTime SomeUTCDateTimeVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someConditionalUTCDateTimeVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //someConditionalUTCDateTimeVal
        public DateTime SomeConditionalUTCDateTimeVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someDateVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //someDateVal
        public DateTime SomeDateVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someConditionalDateVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //someConditionalDateVal
        public DateTime SomeConditionalDateVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someMoneyVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //someMoneyVal
        public string SomeMoneyVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someConditionalMoneyVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //someConditionalMoneyVal
        public string SomeConditionalMoneyVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someNVarCharVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //someNVarCharVal
        public string SomeNVarCharVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someConditionalNVarCharVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //someConditionalNVarCharVal
        public string SomeConditionalNVarCharVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someVarCharVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //someVarCharVal
        public string SomeVarCharVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someConditionalVarCharVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //someConditionalVarCharVal
        public string SomeConditionalVarCharVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someTextVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //someTextVal
        public string SomeTextVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someConditionalTextVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //someConditionalTextVal
        public string SomeConditionalTextVal { get; set; }

        [Newtonsoft.Json.JsonProperty("somePhoneNumber", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //somePhoneNumber
        public string SomePhoneNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("someConditionalPhoneNumber", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //someConditionalPhoneNumber
        public string SomeConditionalPhoneNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("someEmailAddress", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //someEmailAddress
        public string SomeEmailAddress { get; set; }

        [Newtonsoft.Json.JsonProperty("someConditionalEmailAddress", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //someConditionalEmailAddress
        public string SomeConditionalEmailAddress { get; set; }

        [Newtonsoft.Json.JsonProperty("flavorName", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //flavorName
        public string FlavorName { get; set; }

        [Newtonsoft.Json.JsonProperty("flavorCode", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //flavorCode
        public System.Guid FlavorCode { get; set; }

        [Newtonsoft.Json.JsonProperty("someIntConditionalOnDeletable", Required = Newtonsoft.Json.Required.Always)]
        public int SomeIntConditionalOnDeletable { get; set; }

        [Newtonsoft.Json.JsonProperty("nVarCharAsUrl", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //nVarCharAsUrl
        public string NVarCharAsUrl { get; set; }

        [Newtonsoft.Json.JsonProperty("nVarCharConditionalAsUrl", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //nVarCharConditionalAsUrl
        public string NVarCharConditionalAsUrl { get; set; }

        [Newtonsoft.Json.JsonProperty("updateLinkPlantCode", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //updateLinkPlantCode
        public System.Guid UpdateLinkPlantCode { get; set; }

        [Newtonsoft.Json.JsonProperty("deleteAsyncButtonLinkPlantCode", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //deleteAsyncButtonLinkPlantCode
        public System.Guid DeleteAsyncButtonLinkPlantCode { get; set; }

        [Newtonsoft.Json.JsonProperty("detailsLinkPlantCode", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //detailsLinkPlantCode
        public System.Guid DetailsLinkPlantCode { get; set; }

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

        [Newtonsoft.Json.JsonProperty("someImageUrlVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //someImageUrlVal
        public string SomeImageUrlVal { get; set; } 

        [Newtonsoft.Json.JsonProperty("someConditionalImageUrl", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //someConditionalImageUrl
        public string SomeConditionalImageUrl { get; set; }  //someConditionalImageUrl

        [Newtonsoft.Json.JsonProperty("conditionalBtnExampleLinkPlantCode", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)] //conditionalBtnExampleLinkPlantCode
        public System.Guid ConditionalBtnExampleLinkPlantCode { get; set; }

        [Newtonsoft.Json.JsonProperty("isImageUrlAvailable", Required = Newtonsoft.Json.Required.Always)]
        public bool IsImageUrlAvailable { get; set; }


    }

        
    public class LandPlantListListRequest
    {
        [Newtonsoft.Json.JsonProperty("flavorFilterCode", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Guid FlavorFilterCode { get; set; }

        [Newtonsoft.Json.JsonProperty("someFilterIntVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int SomeFilterIntVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someFilterBigIntVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public long SomeFilterBigIntVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someFilterFloatVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string SomeFilterFloatVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someFilterBitVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool SomeFilterBitVal { get; set; }

        [Newtonsoft.Json.JsonProperty("isFilterEditAllowed", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool IsFilterEditAllowed { get; set; }

        [Newtonsoft.Json.JsonProperty("isFilterDeleteAllowed", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool IsFilterDeleteAllowed { get; set; }

        [Newtonsoft.Json.JsonProperty("someFilterDecimalVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string SomeFilterDecimalVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someMinUTCDateTimeVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public DateTime SomeMinUTCDateTimeVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someMinDateVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public DateTime SomeMinDateVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someFilterMoneyVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string SomeFilterMoneyVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someFilterNVarCharVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string SomeFilterNVarCharVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someFilterVarCharVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string SomeFilterVarCharVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someFilterTextVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string SomeFilterTextVal { get; set; }

        [Newtonsoft.Json.JsonProperty("someFilterPhoneNumber", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string SomeFilterPhoneNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("someFilterEmailAddress", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string SomeFilterEmailAddress { get; set; }


        [Newtonsoft.Json.JsonProperty("someFilterUniqueIdentifier", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Guid SomeFilterUniqueIdentifier { get; set; } //someFilterUniqueIdentifier


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
