using Jellyfin.Sdk;

namespace MediaServerMigrateAssistant.Providers.Jellyfin.Extensions
{
    internal static class UserClientExtensions
    {
        #region Statics

        public static Task<AuthenticationResult> AuthenticateAsync(this UserClient userClient, string username, string password)
        {
            var body = new AuthenticateUserByName {Username = username, Pw = password};
            return userClient.AuthenticateUserByNameAsync(body);
        }

        #endregion
    }
}