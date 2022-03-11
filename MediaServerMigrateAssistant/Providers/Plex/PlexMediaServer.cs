using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Plex.Library.ApiModels.Libraries;
using Plex.Library.ApiModels.Servers;
using Plex.ServerApi;
using Plex.ServerApi.Api;
using Plex.ServerApi.Clients;
using Plex.ServerApi.PlexModels.Media;

namespace MediaServerMigrateAssistant.Providers.Plex
{
    internal class PlexMediaServer : IMediaServer
    {
        private readonly IEnumerable<Server> _servers;

        #region Abstracts

        private static void Process(IDictionary<string, IMediaServerEntry> entries, MediaContainer container)
        {
            foreach (var metadata in container.Media)
            {
                foreach (var media in metadata.Media)
                {
                    foreach (var part in media.Part)
                    {
                        var current = PlexMediaServerEntry.Create(metadata, part);

                        if (entries.TryGetValue(current.File, out var previous) && current.IsWatched != previous.IsWatched)
                        {
                            throw new Exception($"Conflict on ${current.File}");
                        }

                        entries[current.File] = current;
                    }
                }
            }
        }

        #endregion

        #region Constructor

        private PlexMediaServer(IEnumerable<Server>? servers = null)
        {
            _servers = servers ?? Enumerable.Empty<Server>();
        }

        public static async Task<IMediaServer> CreateAsync(string token)
        {
            var service = new ApiService(
                new PlexRequestsHttpClient(),
                new Logger<ApiService>(new NullLoggerFactory()));
            var factory = new global::Plex.Library.Factories.PlexFactory(
                new PlexServerClient(new ClientOptions(), service),
                new PlexAccountClient(new ClientOptions(), service),
                new PlexLibraryClient(new ClientOptions(), service));
            var servers = await factory
                .GetPlexAccount(token)
                .Servers();
            return new PlexMediaServer(servers);
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
            foreach (var server in _servers.Take(1))
            {
                foreach (var library in await server.Libraries())
                {
                    if (library is MovieLibrary movieLibrary)
                    {
                        Process(entries, await movieLibrary.AllMovies(string.Empty, 0, int.MaxValue));
                    }
                    else if (library is ShowLibrary showLibrary)
                    {
                        Process(entries, await showLibrary.AllEpisodes(string.Empty, 0, int.MaxValue));
                    }
                }
            }
        }

        public async Task UpdateAsync(IMediaServerEntry entry, bool isWatched)
        {
            if (entry is not PlexMediaServerEntry source)
                throw new Exception();
            foreach (var server in _servers.Take(1))
                await (isWatched
                    ? server.ScrobbleItem(source.RatingKey)
                    : server.UnScrobbleItem(source.RatingKey));
        }

        #endregion
    }
}