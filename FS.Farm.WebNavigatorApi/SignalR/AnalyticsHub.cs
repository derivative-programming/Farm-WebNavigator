using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace FS.Farm.FSFarmAPI.SignalR
{
    public class AnalyticsHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public static Dictionary<string, string> connections { get; set; } = new Dictionary<string, string>();

        public async Task SendMessage(string user, string message)
        {
            connections.TryGetValue(user, out string? connectionId);
            if (!string.IsNullOrEmpty(connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", "", message);
            }
        }

        public async Task SendAllMessage()
        {
            while (true)
            {
                await Task.Delay(60 * 1000);
                await Clients.All.SendAsync("ReceiveMessage", "", "Connection Alive");
            }
        }

        public void SetConnectionId(string customConnectionId)
        {
            connections.TryGetValue(customConnectionId, out string? connectionId);

            if (connectionId == null)
            {
                connections.TryAdd(customConnectionId, Context.ConnectionId);
            }
        }
        public void CollectDataFromClient(string data)
        {
            bool test = true;
            test = false;
            FS.Common.Diagnostics.Loggers.Manager.LogMessage(data);

        }

        public async Task SendMessageFromHub(string connectionId)
        {
            await SendMessage(connectionId, "Receive Message from Hub");
            SendAllMessage();

        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connection = connections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (connection != null)
            {
                connections.Remove(connection);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
