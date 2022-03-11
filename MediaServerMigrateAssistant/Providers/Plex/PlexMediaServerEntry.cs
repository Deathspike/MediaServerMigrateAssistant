using System.Text.Json.Serialization;
using Plex.ServerApi.PlexModels.Media;

namespace MediaServerMigrateAssistant.Providers.Plex
{
    internal class PlexMediaServerEntry : IMediaServerEntry
    {
        #region Constructor

        private PlexMediaServerEntry(Metadata metadata, MediaPart part)
        {
            File = part.File;
            IsWatched = metadata.ViewCount > 0;
            RatingKey = metadata.RatingKey;
        }

        public static PlexMediaServerEntry Create(Metadata metadata, MediaPart part)
        {
            return new PlexMediaServerEntry(metadata, part);
        }

        #endregion

        #region Properties

        [JsonPropertyName("file")]
        public string File { get; }

        [JsonPropertyName("isWatched")]
        public bool IsWatched { get; }

        [JsonPropertyName("ratingKey")]
        public string RatingKey { get; }

        #endregion
    }
}