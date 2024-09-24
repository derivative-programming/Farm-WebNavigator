using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FS.Farm.FSFarmAPI.Models.API.V1_0
{

    public class LandAddPlantPostModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select a Flavor")]

        public Guid requestFlavorCode { get; set; }
        public String requestOtherFlavor { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Some Int Val")]

        public Int32 requestSomeIntVal { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Some Big Int Val")]

        public Int64 requestSomeBigIntVal { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Some Bit Val")]

        public Boolean requestSomeBitVal { get; set; }
        public Boolean requestIsEditAllowed { get; set; }
        public Boolean requestIsDeleteAllowed { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Some Float Val")]

        public Double requestSomeFloatVal { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Some Decimal Val")]

        public Decimal requestSomeDecimalVal { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Some UTC Date Time Val")]

        [DataType(DataType.Date)]

        public DateTime requestSomeUTCDateTimeVal { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Some UTC Date Time Val")]

        [DataType(DataType.Time)]
        public DateTime requestSomeUTCDateTimeValTimeExtension { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Some Date Val")]

        [DataType(DataType.Date)]

        public DateTime requestSomeDateVal { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Some Money Val")]

        public Decimal requestSomeMoneyVal { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Some N Var Char Val")]

        public String requestSomeNVarCharVal { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Some Secure Var Char Val")]

        [DataType(DataType.Password)]

        public String requestSomeVarCharVal { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Some Text Val")]

        public String requestSomeTextVal { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Some Phone Number")]

        [DataType(DataType.PhoneNumber)]

        public String requestSomePhoneNumber { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Some Email Address")]

        [DataType(DataType.Text)]

        public String requestSomeEmailAddress { get; set; }
        public String requestSampleImageUploadFile { get; set; }
        public String someImageUrlVal { get; set; }
        public String requestSomeLongNVarCharVal { get; set; }
        public String requestSomeLongVarCharVal { get; set; }

        public Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("RequestFlavorCode", this.requestFlavorCode.ToString());
            result.Add("RequestOtherFlavor", this.requestOtherFlavor.ToString());
            result.Add("RequestSomeIntVal", this.requestSomeIntVal.ToString());
            result.Add("RequestSomeBigIntVal", this.requestSomeBigIntVal.ToString());
            result.Add("RequestSomeBitVal", this.requestSomeBitVal.ToString());
            result.Add("RequestIsEditAllowed", this.requestIsEditAllowed.ToString());
            result.Add("RequestIsDeleteAllowed", this.requestIsDeleteAllowed.ToString());
            result.Add("RequestSomeFloatVal", this.requestSomeFloatVal.ToString());
            result.Add("RequestSomeDecimalVal", this.requestSomeDecimalVal.ToString());
            result.Add("RequestSomeUTCDateTimeVal", this.requestSomeUTCDateTimeVal.ToString());
            result.Add("RequestSomeDateVal", this.requestSomeDateVal.ToString());
            result.Add("RequestSomeMoneyVal", this.requestSomeMoneyVal.ToString());
            result.Add("RequestSomeNVarCharVal", this.requestSomeNVarCharVal.ToString());
            result.Add("RequestSomeVarCharVal", this.requestSomeVarCharVal.ToString());
            result.Add("RequestSomeTextVal", this.requestSomeTextVal.ToString());
            result.Add("RequestSomePhoneNumber", this.requestSomePhoneNumber.ToString());
            result.Add("RequestSomeEmailAddress", this.requestSomeEmailAddress.ToString());
            result.Add("RequestSampleImageUploadFile", this.requestSampleImageUploadFile.ToString());
            result.Add("SomeImageUrlVal", this.someImageUrlVal.ToString());
            result.Add("RequestSomeLongNVarCharVal", this.requestSomeLongNVarCharVal.ToString());
            result.Add("RequestSomeLongVarCharVal", this.requestSomeLongVarCharVal.ToString());
            return result;
        }
    }

    public class LandAddPlantPostResponse : PostResponse
    {
        public Guid outputFlavorCode { get; set; }
        public String outputOtherFlavor { get; set; }
        public Int32 outputSomeIntVal { get; set; }
        public Int64 outputSomeBigIntVal { get; set; }
        public Boolean outputSomeBitVal { get; set; }
        public Boolean outputIsEditAllowed { get; set; }
        public Boolean outputIsDeleteAllowed { get; set; }
        public Double outputSomeFloatVal { get; set; }
        public Decimal outputSomeDecimalVal { get; set; }
        public DateTime outputSomeUTCDateTimeVal { get; set; }
        public DateTime outputSomeDateVal { get; set; }
        public Decimal outputSomeMoneyVal { get; set; }
        public String outputSomeNVarCharVal { get; set; }
        public String outputSomeVarCharVal { get; set; }
        public String outputSomeTextVal { get; set; }
        public String outputSomePhoneNumber { get; set; }
        public String outputSomeEmailAddress { get; set; }
        public Guid landCode { get; set; }
        public Guid plantCode { get; set; }
    }
     

}
