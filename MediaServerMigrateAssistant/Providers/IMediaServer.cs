namespace MediaServerMigrateAssistant.Providers
{
    internal interface IMediaServer
    {
        Task<IReadOnlyDictionary<string, IMediaServerEntry>> CollectAsync();

        Task CollectAsync(IDictionary<string, IMediaServerEntry> entries);

        Task UpdateAsync(IMediaServerEntry entry, bool isWatched);
    }
}