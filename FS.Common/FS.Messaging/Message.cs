using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq; 

namespace FS.Messaging
{
    public class Message
    {
        public List<JObject> DataList = new List<JObject>();
        public string ObjectNamespace = String.Empty;
        public string ObjectName = string.Empty;
        public string Action = string.Empty;
        public string RoutingKey = string.Empty; 
        public MessageType MessageType = MessageType.Unknown;
        public Guid ThreadCode = Guid.Empty;
        public string LastError = string.Empty;
        public DateTime CreatedUTCDateTime = DateTime.Parse("1/1/1960"); 

        public Message(string objectNamespace, string objectName, string actionName, JObject data,
            string routingKey, MessageType messageType)
        {
            this.ObjectNamespace = objectNamespace;
            this.ObjectName = objectName;
            this.Action = actionName;
            this.RoutingKey = routingKey;
            this.ThreadCode = Guid.NewGuid();
            this.MessageType = messageType;
            this.CreatedUTCDateTime = DateTime.UtcNow;
            this.DataList.Add(data);
        }
        public Message(string objectNamespace, string objectName, string actionName, JObject data,
            string routingKey, MessageType messageType, Guid threadCode)
        {
            this.ObjectNamespace = objectNamespace;
            this.ObjectName = objectName;
            this.Action = actionName;
            this.RoutingKey = routingKey;
            this.ThreadCode = threadCode;
            this.MessageType = messageType;
            this.CreatedUTCDateTime = DateTime.UtcNow;
            this.DataList.Add(data);
        }
        public Message(string messageJSON)
        {
            JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(messageJSON);
            if (jObject["object_namespace"] != null)
                this.ObjectNamespace = jObject["object_namespace"].ToString();
            if (jObject["object_name"] != null)
                this.ObjectName = jObject["object_name"].ToString();
            if (jObject["action"] != null)
                this.Action = jObject["action"].ToString();
            if (jObject["routing_key"] != null)
                this.RoutingKey = jObject["routing_key"].ToString();
            if (jObject["message_type"] != null)
                MessageType.TryParse(jObject["message_type"].ToString(), out this.MessageType);
            if (jObject["thread_code"] != null)
                Guid.TryParse(jObject["thread_code"].ToString(), out this.ThreadCode);
            if (jObject["last_error"] != null)
                this.LastError = jObject["last_error"].ToString();
            if (jObject["created_utc_date_time"] != null)
                this.CreatedUTCDateTime = DateTime.Parse(jObject["created_utc_date_time"].ToString());

            if (jObject["data_list"] != null)
            {
                JArray jArray = (JArray)jObject["data_list"];
                for (int i = 0; i < jArray.Count; i++)
                {
                    this.DataList.Add((JObject)jArray[i]);
                }
            }

        }
        public string ToJSON()
        {
            string result = string.Empty;

            JObject jObject = new JObject();
            jObject["object_namespace"] = this.ObjectNamespace;
            jObject["object_name"] = this.ObjectName;
            jObject["action"] = this.Action;
            jObject["routing_key"] = this.RoutingKey;
            jObject["thread_code"] = this.ThreadCode.ToString();
            jObject["message_type"] = this.MessageType.ToString("g");
            jObject["last_error"] = this.LastError.ToString();
            jObject["created_utc_date_time"] = this.CreatedUTCDateTime.ToString();

             
            JArray dataArray = new JArray();
            for (int i = 0; i < DataList.Count; i++)
            {
                dataArray.Add(DataList[i]);
            }

            jObject["data_list"] = dataArray;

            result = jObject.ToString();

            return result;
        }

        
    }
}
