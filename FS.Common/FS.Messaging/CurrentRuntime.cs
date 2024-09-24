using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FS.Messaging.Rabbit;
using Newtonsoft.Json.Linq;

namespace FS.Messaging
{
    public static class CurrentRuntime
    {

        public enum MessageQueues
        {
            Lazy_Object_Crud,
            Dead_Queue,
            Object_Event,
            Object_Mirror,
            Email,
            Unit_Testing,
            Report_Writer,
            Object_Audit,
            Api_Log,
            Api_Post
        }

        public static void Initialize()
        {
            if (FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("EnableMessaging", "false").ToLower() == "true")
            {
                //Exchange.Create(Exchange.DEFAULT_EXCHANGE_NAME, Exchange.HMExchangeType.Topic);
                //Queue.Create(Queue.DEFAULT_QUEUE_NAME);
                //Queue.BindToExchange(Queue.DEFAULT_QUEUE_NAME, Exchange.DEFAULT_EXCHANGE_NAME, "general");

                //Queue.Create(Queue.EMAIL_QUEUE_NAME);
                //Queue.BindToExchange(Queue.EMAIL_QUEUE_NAME, Exchange.DEFAULT_EXCHANGE_NAME, "email");

                //Queue.Create(Queue.OBJECT_EVENT_QUEUE_NAME);
                //Queue.BindToExchange(Queue.OBJECT_EVENT_QUEUE_NAME, Exchange.DEFAULT_EXCHANGE_NAME, "objectEvent");

                //Queue.Create(Queue.DEAD_QUEUE_NAME);
                //Queue.BindToExchange(Queue.DEAD_QUEUE_NAME, Exchange.DEFAULT_EXCHANGE_NAME, "dead");

                //Queue.Create(Queue.OBJECT_MIRROR_QUEUE_NAME);
                //Queue.BindToExchange(Queue.OBJECT_MIRROR_QUEUE_NAME, Exchange.DEFAULT_EXCHANGE_NAME, "objectMirror");
            }
        }

        //public static void SendMessage2(string body)
        //{
        //    if (
        //        FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("EnableMessaging", "false").ToLower() ==
        //        "true")
        //    {
        //        Exchange.SendMessage(Exchange.DEFAULT_EXCHANGE_NAME, "", body);
        //    }
        //}
        //public static void SendMessageToEmailQueue(string body)
        //{
        //    if (
        //        FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("EnableMessaging", "false").ToLower() ==
        //        "true")
        //    {
        //        Exchange.SendMessage(Exchange.DEFAULT_EXCHANGE_NAME, "email", body);
        //    }
        //}

        //public static Message GetNextMessage()
        //{
        //    string json = Queue.GetNextMessage(Queue.DEFAULT_QUEUE_NAME);
        //    Message message = new Message(json);
        //    return message;
        //}

        public static Message GetNextMessageObject(MessageQueues sourceMessageQueue)
        {
            throw new System.Exception("Use Azure Data Provider");
            string json = string.Empty;
            switch (sourceMessageQueue)
            {
                case MessageQueues.Dead_Queue:
                    json = Queue.GetNextMessage(Queue.DEAD_QUEUE_NAME);
                    break;
                case MessageQueues.Email:
                    json = Queue.GetNextMessage(Queue.EMAIL_QUEUE_NAME);
                    break;
                case MessageQueues.Lazy_Object_Crud:
                    json = Queue.GetNextMessage(Queue.DEFAULT_QUEUE_NAME);
                    break;
                case MessageQueues.Object_Event:
                    json = Queue.GetNextMessage(Queue.OBJECT_EVENT_QUEUE_NAME);
                    break;
                case MessageQueues.Object_Mirror:
                    json = Queue.GetNextMessage(Queue.OBJECT_MIRROR_QUEUE_NAME);
                    break;
                case MessageQueues.Unit_Testing:
                    json = Queue.GetNextMessage(Queue.UNIT_TESTING_QUEUE_NAME);
                    break;
                default:
                    break;
            }
            if (json.Length == 0)
                return null;
            Message message = new Message(json);
            return message;
        }


        public static Message GetNextMessageObject(MessageQueues sourceMessageQueue, int millisecondsTimeout)
        {
            throw new System.Exception("Use Azure Data Provider");
            string json = string.Empty;;
            switch (sourceMessageQueue)
            {
                case MessageQueues.Dead_Queue:
                    json = Queue.GetNextMessage(Queue.DEAD_QUEUE_NAME, millisecondsTimeout);
                    break;
                case MessageQueues.Email:
                    json = Queue.GetNextMessage(Queue.EMAIL_QUEUE_NAME, millisecondsTimeout);
                    break;
                case MessageQueues.Lazy_Object_Crud:
                    json = Queue.GetNextMessage(Queue.DEFAULT_QUEUE_NAME, millisecondsTimeout);
                    break;
                case MessageQueues.Object_Event:
                    json = Queue.GetNextMessage(Queue.OBJECT_EVENT_QUEUE_NAME, millisecondsTimeout);
                    break;
                case MessageQueues.Object_Mirror:
                    json = Queue.GetNextMessage(Queue.OBJECT_MIRROR_QUEUE_NAME, millisecondsTimeout);
                    break;
                case MessageQueues.Unit_Testing:
                    json = Queue.GetNextMessage(Queue.UNIT_TESTING_QUEUE_NAME, millisecondsTimeout);
                    break;
                default:
                    break;
            }
            if (json.Length == 0)
                return null;
            Message message = new Message(json);
            return message;
        }
         

        public static string GetNextMessage(MessageQueues sourceMessageQueue)
        {
            throw new System.Exception("Use Azure Data Provider");
            string data = string.Empty;
            switch (sourceMessageQueue)
            {
                case MessageQueues.Dead_Queue:
                    data = Queue.GetNextMessage(Queue.DEAD_QUEUE_NAME);
                    break;
                case MessageQueues.Email:
                    data = Queue.GetNextMessage(Queue.EMAIL_QUEUE_NAME);
                    break;
                case MessageQueues.Lazy_Object_Crud:
                    data = Queue.GetNextMessage(Queue.DEFAULT_QUEUE_NAME);
                    break;
                case MessageQueues.Object_Event:
                    data = Queue.GetNextMessage(Queue.OBJECT_EVENT_QUEUE_NAME);
                    break;
                case MessageQueues.Object_Mirror:
                    data = Queue.GetNextMessage(Queue.OBJECT_MIRROR_QUEUE_NAME);
                    break;
                case MessageQueues.Unit_Testing:
                    data = Queue.GetNextMessage(Queue.UNIT_TESTING_QUEUE_NAME);
                    break;
                default:
                    break;
            }
            return data;
        }

        public static string GetRoutingKey(MessageType messageType)
        {
            string result = String.Empty;
            switch (messageType)
            {
                case MessageType.Email:
                    result = "email";
                    break;
                case MessageType.ObjectDelete:
                    result = "general";
                    break;
                case MessageType.ObjectUpdate:
                    result = "general";
                    break;
                case MessageType.ObjectInsert:
                    result = "general";
                    break;
                case MessageType.ObjectFlow:
                    result = "general";
                    break;
                case MessageType.ObjectDeleteEvent:
                    result = "objectEvent";
                    break;
                case MessageType.ObjectUpdateEvent:
                    result = "objectEvent";
                    break;
                case MessageType.ObjectInsertEvent:
                    result = "objectEvent";
                    break;
                case MessageType.ObjectMirrorDelete:
                    result = "objectMirror";
                    break;
                case MessageType.ObjectMirrorUpdate:
                    result = "objectMirror";
                    break;
                case MessageType.ObjectMirrorInsert:
                    result = "objectMirror";
                    break;
                case MessageType.ReportWriterInsert:
                    result = "reportWriter";
                    break;
                case MessageType.ApiPostInsert:
                    result = "apiPost";
                    break;
                case MessageType.ObjectAuditInsert:
                    result = "objectAudit";
                    break;
                case MessageType.ApiLogInsert:
                    result = "apiLog";
                    break;
                case MessageType.DeadQueueInsert:
                    result = "deadQueue";
                    break;
                default:
                    break;

            }
            return result;// objectNamespace + "::" + objectName + "::" + actionName;
        }


        public static string GetQueueName(MessageType messageType)
        {
            string result = String.Empty;
            switch (messageType)
            {
                case MessageType.Email:
                    result = Azure.Queue.EMAIL_QUEUE_NAME;
                    break;
                case MessageType.ObjectDelete:
                    result = Azure.Queue.DEFAULT_QUEUE_NAME;
                    break;
                case MessageType.ObjectUpdate:
                    result = Azure.Queue.DEFAULT_QUEUE_NAME;
                    break;
                case MessageType.ObjectInsert:
                    result = Azure.Queue.DEFAULT_QUEUE_NAME;
                    break;
                case MessageType.ObjectFlow:
                    result = Azure.Queue.DEFAULT_QUEUE_NAME;
                    break;
                case MessageType.ObjectDeleteEvent:
                    result = Azure.Queue.OBJECT_EVENT_QUEUE_NAME;
                    break;
                case MessageType.ObjectUpdateEvent:
                    result = Azure.Queue.OBJECT_EVENT_QUEUE_NAME;
                    break;
                case MessageType.ObjectInsertEvent:
                    result = Azure.Queue.OBJECT_EVENT_QUEUE_NAME;
                    break;
                case MessageType.ObjectMirrorDelete:
                    result = Azure.Queue.OBJECT_MIRROR_QUEUE_NAME;
                    break;
                case MessageType.ObjectMirrorUpdate:
                    result = Azure.Queue.OBJECT_MIRROR_QUEUE_NAME;
                    break;
                case MessageType.ObjectMirrorInsert:
                    result = Azure.Queue.OBJECT_MIRROR_QUEUE_NAME;
                    break;
                case MessageType.ReportWriterInsert:
                    result = Azure.Queue.REPORT_WRITER_QUEUE_NAME;
                    break;
                case MessageType.ApiPostInsert:
                    result = Azure.Queue.API_POST_QUEUE_NAME;
                    break;
                case MessageType.ObjectAuditInsert:
                    result = Azure.Queue.OBJECT_AUDIT_QUEUE_NAME;
                    break;
                case MessageType.ApiLogInsert:
                    result = Azure.Queue.API_LOG_QUEUE_NAME;
                    break;
                case MessageType.DeadQueueInsert:
                    result = Azure.Queue.DEAD_QUEUE_NAME;
                    break;
                default:
                    break;

            }
            return result;// objectNamespace + "::" + objectName + "::" + actionName;
        }

        public static bool IsMessageTooLong(string objectNamespace, string objectName, string actionName, JObject data, MessageType messageType)
        {
            bool result = false;
            string routingKey = GetRoutingKey(messageType);
            Message message = new Message(objectNamespace, objectName, actionName, data, routingKey, messageType); 
            if(System.Text.ASCIIEncoding.ASCII.GetByteCount(message.ToJSON()) > 250000)
            {
                result = true;
            }    
            return result; 
        }

        public static bool IsMessagingActive()
        {
            bool result = false;
            if (FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("EnableMessaging", "false").ToLower() == "true")
            {
                result = true;
            }
            return result;
        }

        public static void SendMessage(string objectNamespace, string objectName, string actionName, JObject data, MessageType messageType)
        {
            if (FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("EnableMessaging", "false").ToLower() == "true")
            {
                string routingKey = GetRoutingKey(messageType);
                Message message = new Message(objectNamespace, objectName, actionName, data, routingKey, messageType);
                //Exchange.SendMessage(Exchange.DEFAULT_EXCHANGE_NAME,
                //    routingKey,
                //    message.ToJSON());
                Azure.Queue.SendMessage(GetQueueName(messageType), message.ToJSON());
            }
        }

        public static async Task SendMessageAsync(string objectNamespace, string objectName, string actionName, JObject data, MessageType messageType)
        {
            if (FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("EnableMessaging", "false").ToLower() == "true")
            {
                //await Task.FromResult(0);
                string routingKey = GetRoutingKey(messageType);
                Message message = new Message(objectNamespace, objectName, actionName, data, routingKey, messageType);
                //Exchange.SendMessage(Exchange.DEFAULT_EXCHANGE_NAME,
                //    routingKey,
                //    message.ToJSON());
                await Azure.Queue.SendMessageAsync(GetQueueName(messageType), message.ToJSON());
            }
        }


        public static void SendToDeadQueue(Message message, Exception ex)
        {
            message.LastError = ex.ToString();
            //Exchange.SendMessage(Exchange.DEFAULT_EXCHANGE_NAME,"dead",message.ToJSON());
            Azure.Queue.SendMessage(Azure.Queue.DEAD_QUEUE_NAME, message.ToJSON());
        }

        public static async Task SendToDeadQueueAsync(Message message, Exception ex)
        {
            message.LastError = ex.ToString();
            //Exchange.SendMessage(Exchange.DEFAULT_EXCHANGE_NAME,"dead",message.ToJSON());
            await Azure.Queue.SendMessageAsync(Azure.Queue.DEAD_QUEUE_NAME, message.ToJSON());
        }
    }
}
