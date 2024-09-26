using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace FS.Farm.WebNavigator.Page.Reports.Init
{
    public class LandPlantListInitReport
    {
        public LandPlantListInitReport()
        {
        }

        public async Task<LandPlantListGetInitResponse> GetInitResponse(APIClient aPIClient, Guid contextCode)
        {
            string url = $"/land-plant-list/{contextCode.ToString()}/init";

            LandPlantListGetInitResponse result = await aPIClient.GetAsync<LandPlantListGetInitResponse>(url);

            return result;
        }

        public List<PageHeader> GetPageHeaders(LandPlantListGetInitResponse apiResponse)
        {
            List<PageHeader> result = new List<PageHeader>();

            var landNameHeaderIsVisible = true;
            var currentDateHeaderValHeaderIsVisible = true;
            var currentDateTimeHeaderValHeaderIsVisible = true;

            if (landNameHeaderIsVisible) //landName
                result.Add(new PageHeader("Land Name", apiResponse.LandName));

            if (currentDateHeaderValHeaderIsVisible) //currentDateHeaderVal
                result.Add(new PageHeader("Current Date", apiResponse.CurrentDateHeaderVal));

            if (currentDateTimeHeaderValHeaderIsVisible) //currentDateTimeHeaderVal
                result.Add(new PageHeader("Current Date/Time", apiResponse.CurrentDateTimeHeaderVal));

            return result;
        }

        public class LandPlantListGetInitResponse
        {
            [Newtonsoft.Json.JsonProperty("success", Required = Newtonsoft.Json.Required.Always)]
            public bool Success { get; set; }

            [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Message { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterIntVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public int SomeFilterIntVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterBigIntVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public long SomeFilterBigIntVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterBitVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public bool SomeFilterBitVal { get; set; }

            [Newtonsoft.Json.JsonProperty("isFilterEditAllowed", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public bool IsFilterEditAllowed { get; set; }

            [Newtonsoft.Json.JsonProperty("isFilterDeleteAllowed", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public bool IsFilterDeleteAllowed { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterFloatVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterFloatVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterDecimalVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterDecimalVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someMinUTCDateTimeVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public DateTime SomeMinUTCDateTimeVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someMinDateVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public DateTime SomeMinDateVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterMoneyVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterMoneyVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterNVarCharVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterNVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterVarCharVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterTextVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterTextVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterPhoneNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterPhoneNumber { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterEmailAddress", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterEmailAddress { get; set; }

            [Newtonsoft.Json.JsonProperty("flavorFilterCode", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Guid FlavorFilterCode { get; set; }

            [Newtonsoft.Json.JsonProperty("landCode", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Guid LandCode { get; set; }

            [Newtonsoft.Json.JsonProperty("tacCode", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Guid TacCode { get; set; }

            [Newtonsoft.Json.JsonProperty("landName", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string LandName { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterUniqueIdentifier", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Guid SomeFilterUniqueIdentifier { get; set; }

            [Newtonsoft.Json.JsonProperty("currentDateHeaderVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string CurrentDateHeaderVal { get; set; }

            [Newtonsoft.Json.JsonProperty("currentDateTimeHeaderVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string CurrentDateTimeHeaderVal { get; set; }

            [Newtonsoft.Json.JsonProperty("validationError", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Collections.Generic.ICollection<ValidationError> ValidationError { get; set; }

        }


        private class LandPlantListGetInitModel
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
