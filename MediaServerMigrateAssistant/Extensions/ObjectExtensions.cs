using System.Text.Json;

namespace MediaServerMigrateAssistant.Extensions
{
    internal static class ObjectExtensions
    {
        #region Statics

        public static async Task<string> WriteJsonAsync<T>(this T value, string fileName)
        {
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), fileName);
            var serializerOptions = new JsonSerializerOptions {WriteIndented = true};
            await File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(value, serializerOptions));
            return filePath;
        }

        #endregion
    }
}