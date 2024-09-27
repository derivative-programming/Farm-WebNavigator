using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator.Services
{
    public class PacUserTestAsyncFileDownload
    {
        public PacUserTestAsyncFileDownload()
        {
        }

        public async static Task<PacUserTestAsyncFileDownloadResponse> GetResponse(APIClient aPIClient, Guid pacCode)
        {
            string url = $"/pac-user-test-async-file-download/{pacCode.ToString()}";

            PacUserTestAsyncFileDownloadModel model = new PacUserTestAsyncFileDownloadModel();

            PacUserTestAsyncFileDownloadResponse result = await aPIClient.PostAsync<PacUserTestAsyncFileDownloadModel, PacUserTestAsyncFileDownloadResponse>(url, model);

            return result;
        }

        public class PacUserTestAsyncFileDownloadResponse
        {
            [Newtonsoft.Json.JsonProperty("success", Required = Newtonsoft.Json.Required.Always)]
            public bool Success { get; set; }

            [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Message { get; set; }

            [Newtonsoft.Json.JsonProperty("validationErrors", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public ICollection<ValidationError> ValidationErrors { get; set; }

        }

        private class PacUserTestAsyncFileDownloadModel
        {

        }
    }
}

