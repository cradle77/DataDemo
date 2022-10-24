using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace SampleApp.Client
{
    public class NotificationsManager : IAsyncDisposable
    {
        private NavigationManager _navigation;
        private HubConnection _hubConnection;
        private string _baseUrl;

        public event Func<string, Task> NotificationReceivedAsync;

        public NotificationsManager(IConfiguration configuration)
        {
            _baseUrl = configuration["SignalRBaseUrl"];
        }

        public async Task InitializeAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_baseUrl)
                .Build();

            _hubConnection.On<string>("statusUpdated", async (message) =>
            {
                await this.NotificationReceivedAsync?.Invoke(message);
            });

            await _hubConnection.StartAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _hubConnection.DisposeAsync();
        }
    }
}
