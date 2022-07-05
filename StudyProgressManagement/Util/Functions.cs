using Microsoft.AspNet.SignalR;
using SignalRProgressBarSimpleExample.Hubs;

namespace SignalRProgressBarSimpleExample.Util
{
    public class Functions
    {
        public static void SendProgress(string progressMessage, int progressCount, int totalItems)
        {
            // In order to invoke signalr functionality directly from server side we must use this
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();

            // Calculating percentage based on the parameters sent
            var percentage = (progressCount * 100) / totalItems;

            // Pushing data to all clients
            hubContext.Clients.All.AddProgress(progressMessage, percentage + "%");
        }
    }
}