using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace FS.Messaging.Rabbit
{
    public static  class Exchange
    {
         
        public const string DEFAULT_EXCHANGE_NAME = "HM_Message_Exchange";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="exchangeType">RabbitMQ.Client.ExchangeType</param>
        public static void Create(string name, HMExchangeType exchangeType)
        {
            using (var connection = GetConnection())
            using (var model = connection.CreateModel())
            {
                model.ExchangeDeclare(name, exchangeType.ToString("g").ToLower(), true);
            } 
        }

        public static IConnection GetConnection()
        {  
            ConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("RabbitMQ_HostName", "localhost");
            connectionFactory.UserName = FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("RabbitMQ_Username", "guest");
            connectionFactory.Password = FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("RabbitMQ_Password", "guest");
            connectionFactory.Port = AmqpTcpEndpoint.UseDefaultPort;
            //connectionFactory.Protocol = Protocols.DefaultProtocol;
            connectionFactory.VirtualHost = "/";
             
            return connectionFactory.CreateConnection();
        }

        public static void BindToExchange(string sourceExchangeName1, string destinationExchangeName2, string routingKey)
        { 
            using (var connection = GetConnection())
            using (var model = connection.CreateModel())
            {
                model.ExchangeBind(destinationExchangeName2, sourceExchangeName1, routingKey);
            } 
        }

        public static void SendMessage(string exchangeName, string routingKey, string body)
        {
            using (var connection = GetConnection())
            using (var model = connection.CreateModel())
            {
                IBasicProperties basicProperties = model.CreateBasicProperties();
                basicProperties.Persistent = true;
                byte[] payload = Encoding.UTF8.GetBytes(body);
                model.BasicPublish(exchangeName, routingKey, basicProperties, payload);
            }  
        }

        public enum HMExchangeType
        {
            Direct,
            Fanout,
            Headers,
            Topic
        }
    }
}
