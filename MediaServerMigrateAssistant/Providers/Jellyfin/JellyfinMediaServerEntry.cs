using System.Text.Json.Serialization;
using Jellyfin.Sdk;

namespace MediaServerMigrateAssistant.Providers.Jellyfin
{
    internal class JellyfinMediaServerEntry : IMediaServerEntry
    {
        #region Constructor

        private JellyfinMediaServerEntry(BaseItemDto item)
        {
            Id = item.Id;
            IsWatched = item.UserData.Played;
            Path = item.Path;
        }

        public static JellyfinMediaServerEntry Create(BaseItemDto item)
        {
            return new JellyfinMediaServerEntry(item);
        }

        #endregion

        #region Properties

        [JsonPropertyName("id")]
        public Guid Id { get; }

        [JsonPropertyName("isWatched")]
        public bool IsWatched { get; }

        [JsonPropertyName("path")]
        public string Path { get; }

        #endregion
    }
}