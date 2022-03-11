using MediaServerMigrateAssistant.Providers;

namespace MediaServerMigrateAssistant.Extensions
{
    internal static class ContextExtensions
    {
        #region Statics

        public static void MapFiles(this Context context, IReadOnlyDictionary<string, IMediaServerEntry> source, IReadOnlyDictionary<string, IMediaServerEntry> destination)
        {
            foreach (var key in source.Keys.Where(x => !destination.ContainsKey(x)))
            {
                context.MissingDestination.Add(key);
            }

            foreach (var key in destination.Keys.Where(x => !source.ContainsKey(x)))
            {
                context.MissingSource.Add(key);
            }

            foreach (var (key, value) in source)
            {
                if (destination.TryGetValue(key, out var destinationEntry))
                {
                    if (destinationEntry.IsWatched == value.IsWatched)
                    {
                        context.NoChange.Add(key);
                    }
                    else if (value.IsWatched)
                    {
                        context.MarkPlayed.Add(key);
                    }
                    else
                    {
                        context.MarkUnplayed.Add(key);
                    }
                }
            }
        }

        #endregion
    }
}