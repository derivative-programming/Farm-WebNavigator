﻿using FS.Farm.WebNavigator.Page.Reports.Init;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FS.Farm.WebNavigator.Page.Forms.Init;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using FS.Farm.WebNavigator.Page;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http.Internal;
using System.Globalization;

namespace FS.Farm.WebNavigator.Page.Forms.Models
{ 

    public class LandAddPlantPostResponse
    {
        [Newtonsoft.Json.JsonProperty("success", Required = Newtonsoft.Json.Required.Always)]
        public bool Success { get; set; }

        [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Message { get; set; }

        [Newtonsoft.Json.JsonProperty("outputFlavorCode", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Guid OutputFlavorCode { get; set; }

        [Newtonsoft.Json.JsonProperty("outputOtherFlavor", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string OutputOtherFlavor { get; set; }

        [Newtonsoft.Json.JsonProperty("outputSomeIntVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int OutputSomeIntVal { get; set; }

        [Newtonsoft.Json.JsonProperty("outputSomeBigIntVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public long OutputSomeBigIntVal { get; set; }

        [Newtonsoft.Json.JsonProperty("outputSomeBitVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool OutputSomeBitVal { get; set; }

        [Newtonsoft.Json.JsonProperty("outputIsEditAllowed", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool OutputIsEditAllowed { get; set; }

        [Newtonsoft.Json.JsonProperty("outputIsDeleteAllowed", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool OutputIsDeleteAllowed { get; set; }

        [Newtonsoft.Json.JsonProperty("outputSomeFloatVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string OutputSomeFloatVal { get; set; }

        [Newtonsoft.Json.JsonProperty("outputSomeDecimalVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string OutputSomeDecimalVal { get; set; }

        [Newtonsoft.Json.JsonProperty("outputSomeUTCDateTimeVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public DateTime OutputSomeUTCDateTimeVal { get; set; }

        [Newtonsoft.Json.JsonProperty("outputSomeDateVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public DateTime OutputSomeDateVal { get; set; }

        [Newtonsoft.Json.JsonProperty("outputSomeMoneyVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string OutputSomeMoneyVal { get; set; }

        [Newtonsoft.Json.JsonProperty("outputSomeNVarCharVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string OutputSomeNVarCharVal { get; set; }

        [Newtonsoft.Json.JsonProperty("outputSomeVarCharVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string OutputSomeVarCharVal { get; set; }

        [Newtonsoft.Json.JsonProperty("outputSomePhoneNumber", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string OutputSomePhoneNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("landCode", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Guid LandCode { get; set; }

        [Newtonsoft.Json.JsonProperty("outputSomeTextVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string OutputSomeTextVal { get; set; }

        [Newtonsoft.Json.JsonProperty("plantCode", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Guid PlantCode { get; set; }


        [Newtonsoft.Json.JsonProperty("outputSomeEmailAddress", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string OutputSomeEmailAddress { get; set; } //outputSomeEmailAddress


        [Newtonsoft.Json.JsonProperty("validationErrors", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<ValidationError> ValidationErrors { get; set; } 

    }
         
    public class LandAddPlantPostModel
    {
        [Newtonsoft.Json.JsonProperty("requestFlavorCode", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.Guid RequestFlavorCode { get; set; }

        [Newtonsoft.Json.JsonProperty("requestOtherFlavor", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string RequestOtherFlavor { get; set; }

        [Newtonsoft.Json.JsonProperty("requestSomeIntVal", Required = Newtonsoft.Json.Required.Always)]
        public int RequestSomeIntVal { get; set; }

        [Newtonsoft.Json.JsonProperty("requestSomeBigIntVal", Required = Newtonsoft.Json.Required.Always)]
        public long RequestSomeBigIntVal { get; set; }

        [Newtonsoft.Json.JsonProperty("requestSomeBitVal", Required = Newtonsoft.Json.Required.Always)]
        public bool RequestSomeBitVal { get; set; }

        [Newtonsoft.Json.JsonProperty("requestIsEditAllowed", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool RequestIsEditAllowed { get; set; }

        [Newtonsoft.Json.JsonProperty("requestIsDeleteAllowed", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool RequestIsDeleteAllowed { get; set; }

        [Newtonsoft.Json.JsonProperty("requestSomeFloatVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string RequestSomeFloatVal { get; set; }

        [Newtonsoft.Json.JsonProperty("requestSomeDecimalVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string RequestSomeDecimalVal { get; set; }

        [Newtonsoft.Json.JsonProperty("requestSomeUTCDateTimeVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public DateTime RequestSomeUTCDateTimeVal { get; set; }

        [Newtonsoft.Json.JsonProperty("requestSomeDateVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public DateTime RequestSomeDateVal { get; set; }

        [Newtonsoft.Json.JsonProperty("requestSomeMoneyVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string RequestSomeMoneyVal { get; set; }

        [Newtonsoft.Json.JsonProperty("requestSomeNVarCharVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string RequestSomeNVarCharVal { get; set; }

        [Newtonsoft.Json.JsonProperty("requestSomeVarCharVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string RequestSomeVarCharVal { get; set; }

        [Newtonsoft.Json.JsonProperty("requestSomeTextVal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string RequestSomeTextVal { get; set; }

        [Newtonsoft.Json.JsonProperty("requestSomePhoneNumber", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string RequestSomePhoneNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("requestSomeEmailAddress", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string RequestSomeEmailAddress { get; set; }

        [Newtonsoft.Json.JsonProperty("requestSampleImageUploadFile", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string RequestSampleImageUploadFile { get; set; }

        [Newtonsoft.Json.JsonProperty("someImageUrlVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string SomeImageUrlVal { get; set; }

        [Newtonsoft.Json.JsonProperty("requestSomeLongNVarCharVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string RequestSomeLongNVarCharVal { get; set; } //requestSomeLongNVarCharVal


        [Newtonsoft.Json.JsonProperty("requestSomeLongVarCharVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string RequestSomeLongVarCharVal { get; set; } 


    }  
}
