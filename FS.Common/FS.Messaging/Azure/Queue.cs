using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography; 
using System.Threading; 
using System.Web; 
using Newtonsoft.Json;
using System.Globalization;

namespace FS.Messaging.Azure
{
    public static class Queue
    {
        public const string DEFAULT_QUEUE_NAME = "FS_Message_Queue";
        public const string EMAIL_QUEUE_NAME = "FS_Email_Queue";
        public const string OBJECT_EVENT_QUEUE_NAME = "FS_Object_Event_Queue";
        public const string DEAD_QUEUE_NAME = "FS_Dead_Queue";
        public const string OBJECT_MIRROR_QUEUE_NAME = "FS_Object_Mirror_Queue";
        public const string UNIT_TESTING_QUEUE_NAME = "FS_Unit_Testing_Queue";
        public const string REPORT_WRITER_QUEUE_NAME = "FS_Report_Writer_Queue";
        public const string API_POST_QUEUE_NAME = "FS_Api_Post_Queue";
        public const string OBJECT_AUDIT_QUEUE_NAME = "FS_Object_Audit_Queue";
        public const string API_LOG_QUEUE_NAME = "FS_Api_Log_Queue";




        public static void SendMessage(string queueName, string jsonMessage)
        {
              RESTAPI_SendMessage(
                FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("azure.BusQueue.uri"),
                queueName,
                FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("azure.BusQueue.keyName"),
                FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("azure.BusQueue.key"),
                jsonMessage);
        }
        public static async Task SendMessageAsync(string queueName, string jsonMessage)
        {
            await RESTAPI_SendMessageAsync(
               FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("azure.BusQueue.uri"),
               queueName,
               FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("azure.BusQueue.keyName"),
               FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("azure.BusQueue.key"),
               jsonMessage);
        }


        private static async Task<bool> RESTAPI_SendMessageAsync(string uri, string queueName, string keyName, string key, string jsonMessage)
        {
            try
            {
                string baseUri = uri;
                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    client.BaseAddress = new Uri(baseUri);
                    client.DefaultRequestHeaders.Accept.Clear();

                    TimeSpan timeToLive = TimeSpan.FromDays(1);
                    string queueUrl = uri + queueName;
                    string token = createToken(queueUrl, keyName, key);

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SharedAccessSignature", token);
                     
                    HttpContent content = new StringContent(jsonMessage, Encoding.UTF8);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    string path = queueName + "/messages";

                    HttpResponseMessage response = await client.PostAsync(path, content); 
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static bool RESTAPI_SendMessage(string uri, string queueName, string keyName, string key, string jsonMessage)
        {
            try
            {
                string baseUri = uri;
                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    client.BaseAddress = new Uri(baseUri);
                    client.DefaultRequestHeaders.Accept.Clear();

                    TimeSpan timeToLive = TimeSpan.FromDays(1);
                    string queueUrl = uri + queueName;
                    string token = createToken(queueUrl, keyName, key);

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SharedAccessSignature", token);
                     
                    HttpContent content = new StringContent(jsonMessage, Encoding.UTF8);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    string path = queueName + "/messages";

                    HttpResponseMessage response =  client.PostAsync(path, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string createToken(string resourceUri, string keyName, string key)
        {
            TimeSpan sinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var week = 60 * 60 * 24 * 7;
            var expiry = Convert.ToString((int)sinceEpoch.TotalSeconds + week);
            string stringToSign = HttpUtility.UrlEncode(resourceUri) + "\n" + expiry;
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
            var sasToken = String.Format(CultureInfo.InvariantCulture, "sr={0}&sig={1}&se={2}&skn={3}", HttpUtility.UrlEncode(resourceUri), HttpUtility.UrlEncode(signature), expiry, keyName);
            return sasToken;
        }
    }
}
