using MediaServerMigrateAssistant.Providers;

namespace MediaServerMigrateAssistant.Extensions
{
    internal static class FactoryExtensions
    {
        #region Statics

        public static string ReadString(this IFactory _, string value)
        {
            Console.WriteLine(value);
            Console.Write("> ");
            return Console.ReadLine() ?? string.Empty;
        }

        #endregion
    }
}