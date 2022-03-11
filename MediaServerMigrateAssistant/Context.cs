using System.Text.Json.Serialization;

namespace MediaServerMigrateAssistant
{
    internal class Context
    {
        #region Constructor

        public Context()
        {
            MarkPlayed = new HashSet<string>();
            MarkUnplayed = new HashSet<string>();
            MissingDestination = new HashSet<string>();
            MissingSource = new HashSet<string>();
            NoChange = new HashSet<string>();
        }

        #endregion

        #region Properties

        [JsonPropertyName("markPlayed")]
        public HashSet<string> MarkPlayed { get; }

        [JsonPropertyName("markUnplayed")]
        public HashSet<string> MarkUnplayed { get; }

        [JsonPropertyName("missingDestination")]
        public HashSet<string> MissingDestination { get; }

        [JsonPropertyName("missingSource")]
        public HashSet<string> MissingSource { get; }

        [JsonPropertyName("noChange")]
        public HashSet<string> NoChange { get; }

        #endregion
    }
}