using Jellyfin.Sdk;

namespace MediaServerMigrateAssistant.Providers.Jellyfin
{
    internal class JellyfinMediaServerSettings : SdkClientSettings
    {
        #region Constructor

        public JellyfinMediaServerSettings(string baseUrl)
        {
            BaseUrl = baseUrl;
            ClientName = nameof(MediaServerMigrateAssistant);
            ClientVersion = "1.0";
            DeviceName = ClientName;
            DeviceId = DeviceName;
        }

        #endregion
    }
}