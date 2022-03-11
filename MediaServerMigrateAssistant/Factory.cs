using System.Reflection;
using MediaServerMigrateAssistant.Providers;

namespace MediaServerMigrateAssistant
{
    internal class Factory : IFactory
    {
        private readonly IReadOnlyDictionary<string, IFactory> _providers;

        #region Constructor

        public Factory()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => x.Name != nameof(Factory))
                .Where(x => x.IsClass && typeof(IFactory).IsAssignableFrom(x));
            var instances = types.ToDictionary(
                x => x.Name.Replace(nameof(Factory), string.Empty),
                x => (IFactory) Activator.CreateInstance(x)!);
            _providers = new SortedDictionary<string, IFactory>(instances, StringComparer.InvariantCultureIgnoreCase);
        }

        #endregion

        #region Implementation of IFactory

        public async Task<IMediaServer> CreateAsync()
        {
            while (true)
            {
                foreach (var key in _providers.Keys)
                {
                    Console.WriteLine("- {0}", key);
                }

                Console.Write("> ");

                if (_providers.TryGetValue(Console.ReadLine() ?? string.Empty, out var factory))
                {
                    return await factory.CreateAsync();
                }
            }
        }

        #endregion
    }
}