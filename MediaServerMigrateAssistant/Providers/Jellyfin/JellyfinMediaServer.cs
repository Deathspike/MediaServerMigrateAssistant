using Jellyfin.Sdk;
using MediaServerMigrateAssistant.Providers.Jellyfin.Extensions;

namespace MediaServerMigrateAssistant.Providers.Jellyfin
{
    internal class JellyfinMediaServer : IMediaServer
    {
        private readonly HttpClient _http;
        private readonly SdkClientSettings _settings;
        private readonly Guid _userId;

        #region Abstracts

        private static void Process(IDictionary<string, IMediaServerEntry> entries, BaseItemDtoQueryResult queryResult)
        {
            foreach (var item in queryResult.Items)
            {
                var current = JellyfinMediaServerEntry.Create(item);

                if (entries.TryGetValue(current.Path, out var previous) && current.IsWatched != previous.IsWatched)
                {
                    throw new Exception($"Conflict on ${current.Path}");
                }
                
                entries[current.Path] = current;
            }
        }

        #endregion

        #region Constructor

        public JellyfinMediaServer(HttpClient http, SdkClientSettings settings, Guid userId)
        {
            _http = http;
            _settings = settings;
            _userId = userId;
        }

        public static async Task<IMediaServer> CreateAsync(string baseUrl, string username, string password)
        {
            var http = new HttpClient();
            var settings = new JellyfinMediaServerSettings(baseUrl);
            var userResult = await new UserClient(settings, http).AuthenticateAsync(username, password);
            settings.AccessToken = userResult.AccessToken;
            return new JellyfinMediaServer(http, settings, userResult.User.Id);
        }

        #endregion

        #region Implementation of IMediaServer

        public async Task<IReadOnlyDictionary<string, IMediaServerEntry>> CollectAsync()
        {
            var entries = new Dictionary<string, IMediaServerEntry>();
            await CollectAsync(entries);
            return entries;
        }

        public async Task CollectAsync(IDictionary<string, IMediaServerEntry> entries)
        {
            Process(entries, await new ItemsClient(_settings, _http).GetItemsAsync(_userId,
                fields: new[] {ItemFields.Path},
                includeItemTypes: new[] {"Episode", "Movie"},
                recursive: true));
        }

        public async Task UpdateAsync(IMediaServerEntry entry, bool isWatched)
        {
            if (entry is not JellyfinMediaServerEntry source)
                throw new Exception();
            await (isWatched
                ? new PlaystateClient(_settings, _http).MarkPlayedItemAsync(_userId, source.Id)
                : new PlaystateClient(_settings, _http).MarkUnplayedItemAsync(_userId, source.Id));
        }

        #endregion
    }
}