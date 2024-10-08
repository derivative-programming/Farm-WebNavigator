﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator.Page.Forms.Init
{
    public class LandAddPlantInitObjWF  
    {
        public LandAddPlantInitObjWF()
        { 
        }  

        public async Task<GetInitResponse> RequestGetInitResponse(APIClient aPIClient, Guid contextCode)
        {
            string url = $"/land-add-plant/{contextCode.ToString()}/init";

            GetInitResponse result = await aPIClient.GetAsync<GetInitResponse>(url);

            return result;
        }

        public List<PageHeader> GetPageHeaders(GetInitResponse apiResponse)
        {
            List<PageHeader> result = new List<PageHeader>();

            var landNameHeaderIsVisible = true;
            var currentDateHeaderValHeaderIsVisible = true;
            var currentDateTimeHeaderValHeaderIsVisible = true;

            if(landNameHeaderIsVisible) //landName
                result.Add(new PageHeader("Land Name", apiResponse.LandName));

            if(currentDateHeaderValHeaderIsVisible) //currentDateHeaderVal
                result.Add(new PageHeader("Current Date", apiResponse.CurrentDateHeaderVal.ToString("yyyy-MM-dd")));

            if (currentDateTimeHeaderValHeaderIsVisible) //currentDateTimeHeaderVal
                result.Add(new PageHeader("Current Date/Time", apiResponse.CurrentDateTimeHeaderVal.ToString("yyyy-MM-ddTHH:mm:ss")));

            return result;
        }

        public class GetInitResponse
        {
            [Newtonsoft.Json.JsonProperty("success", Required = Newtonsoft.Json.Required.Always)]
            public bool Success { get; set; }

            [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Message { get; set; }

            [Newtonsoft.Json.JsonProperty("requestFlavorCode", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public Guid RequestFlavorCode { get; set; }

            [Newtonsoft.Json.JsonProperty("requestOtherFlavor", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string RequestOtherFlavor { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeIntVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public int RequestSomeIntVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeBigIntVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public long RequestSomeBigIntVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeBitVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public bool RequestSomeBitVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestIsDeleteAllowed", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public bool RequestIsDeleteAllowed { get; set; }

            [Newtonsoft.Json.JsonProperty("requestIsEditAllowed", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public bool RequestIsEditAllowed { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeFloatVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string RequestSomeFloatVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeDecimalVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string RequestSomeDecimalVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeUTCDateTimeVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public DateTime RequestSomeUTCDateTimeVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeDateVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public DateTime RequestSomeDateVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeMoneyVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string RequestSomeMoneyVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeNVarCharVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string RequestSomeNVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeVarCharVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string RequestSomeVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeTextVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string RequestSomeTextVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomePhoneNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string RequestSomePhoneNumber { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeEmailAddress", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string RequestSomeEmailAddress { get; set; }

            [Newtonsoft.Json.JsonProperty("landName", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string LandName { get; set; }

            [Newtonsoft.Json.JsonProperty("tacCode", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public Guid TacCode { get; set; }

            [Newtonsoft.Json.JsonProperty("someImageUrlVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeImageUrlVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeLongVarCharVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string RequestSomeLongVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeLongNVarCharVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string RequestSomeLongNVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("currentDateHeaderVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public DateTime CurrentDateHeaderVal { get; set; }

            [Newtonsoft.Json.JsonProperty("currentDateTimeHeaderVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public DateTime CurrentDateTimeHeaderVal { get; set; }

            [Newtonsoft.Json.JsonProperty("validationErrors", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public ICollection<ValidationError> ValidationErrors { get; set; }

        }

        private class GetInitModel
        {

        }

        public partial class ValidationError
        {
            [Newtonsoft.Json.JsonProperty("property", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Property { get; set; }

            [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Message { get; set; }

        }
    }
}
