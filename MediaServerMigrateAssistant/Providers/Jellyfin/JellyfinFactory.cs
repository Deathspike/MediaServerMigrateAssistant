using MediaServerMigrateAssistant.Extensions;

namespace MediaServerMigrateAssistant.Providers.Jellyfin
{
    internal class JellyfinFactory : IFactory
    {
        #region Implementation of IFactory

        public async Task<IMediaServer> CreateAsync()
        {
            while (true)
            {
                try
                {
                    var baseUrl  = this.ReadString("[Jellyfin] What is your server URL?");
                    var username = this.ReadString("[Jellyfin] What is your username?");
                    var password = this.ReadString("[Jellyfin] What is your password?");
                    return await JellyfinMediaServer.CreateAsync(baseUrl, username, password);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: {ex.Message}");
                }
            }
        }

        #endregion
    }
}