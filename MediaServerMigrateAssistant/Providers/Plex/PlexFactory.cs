using MediaServerMigrateAssistant.Extensions;

namespace MediaServerMigrateAssistant.Providers.Plex
{
    internal class PlexFactory : IFactory
    {
        #region Implementation of IFactory

        public async Task<IMediaServer> CreateAsync()
        {
            while (true)
            {
                try
                {
                    var token = this.ReadString("[Plex] What is your token?");
                    return await PlexMediaServer.CreateAsync(token);
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