using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FS.Messaging.Rabbit
{
    public static class Queue
    {
        public const string DEFAULT_QUEUE_NAME = "HM_Message_Queue";
        public const string EMAIL_QUEUE_NAME = "HM_Email_Queue";
        public const string OBJECT_EVENT_QUEUE_NAME = "HM_Object_Event_Queue";
        public const string DEAD_QUEUE_NAME = "HM_Dead_Queue";
        public const string OBJECT_MIRROR_QUEUE_NAME = "HM_Object_Mirror_Queue";
        public const string UNIT_TESTING_QUEUE_NAME = "HM_Unit_Testing_Queue";

        public static void Create(string name)
        { 
            using (var connection = Exchange.GetConnection())
            using (var model = connection.CreateModel())
            {
                model.QueueDeclare(name, true, false, false, null);
            } 
            
        }

        public static void BindToExchange(string queueName, string exchangeName, string routingKey)
        { 
            using (var connection = Exchange.GetConnection())
            using (var model = connection.CreateModel())
            {
                model.QueueBind(queueName, exchangeName, routingKey);
            } 
        }

        public static void ReceiveMessages(string queueName)
        {
            using (var connection = Exchange.GetConnection())
            using (var model = connection.CreateModel())
            {
                //model.BasicQos(0, 1, false); //basic quality of service 
                //QueueingBasicConsumer consumer = new QueueingBasicConsumer(model);
                //model.BasicConsume(queueName, false, consumer);
                //while (true)
                //{ 
                //    BasicDeliverEventArgs deliveryArguments = consumer.Queue.Dequeue() as BasicDeliverEventArgs;
                //    String message = Encoding.UTF8.GetString(deliveryArguments.Body);
                //    Console.WriteLine("Message received: {0}", message);
                //    model.BasicAck(deliveryArguments.DeliveryTag, false);
                //}
            }  
        }

        public static string GetNextMessage(string queueName)
        {
            String message = string.Empty;
            using (var connection = Exchange.GetConnection())
            using (var model = connection.CreateModel())
            { 
                //model.BasicQos(0, 1, false); //basic quality of service
                //QueueingBasicConsumer consumer = new QueueingBasicConsumer(model);
                //model.BasicConsume(queueName, false, consumer);
                //BasicDeliverEventArgs deliveryArguments = consumer.Queue.Dequeue() as BasicDeliverEventArgs;
                //message = Encoding.UTF8.GetString(deliveryArguments.Body);
                //Console.WriteLine("Message received: {0}", message);
                //model.BasicAck(deliveryArguments.DeliveryTag, false);
            }  
            return message;
        }
        public static string GetNextMessage(string queueName, int millisecondsTimeout)
        {
            String message = string.Empty;
            using (var connection = Exchange.GetConnection())
            using (var model = connection.CreateModel())
            {
                //model.BasicQos(0, 1, false); //basic quality of service
                //QueueingBasicConsumer consumer = new QueueingBasicConsumer(model);
                //string tag = model.BasicConsume(queueName, false, consumer); 
                //BasicDeliverEventArgs deliveryArguments = null;// consumer.Queue.Dequeue() as BasicDeliverEventArgs;
                //consumer.Queue.Dequeue(millisecondsTimeout, out deliveryArguments);
                //if (deliveryArguments != null)
                //{
                //    message = Encoding.UTF8.GetString(deliveryArguments.Body);
                //    Console.WriteLine("Message received: {0}", message);
                //    model.BasicAck(deliveryArguments.DeliveryTag, false);
                //}
            }
            return message;
        }
    }
}
