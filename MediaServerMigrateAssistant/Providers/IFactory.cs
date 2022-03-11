namespace MediaServerMigrateAssistant.Providers
{
    internal interface IFactory
    {
        Task<IMediaServer> CreateAsync();
    }
}