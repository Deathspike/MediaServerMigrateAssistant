using MediaServerMigrateAssistant.Extensions;
using MediaServerMigrateAssistant.Providers;

namespace MediaServerMigrateAssistant
{
    internal class Program
    {
        private readonly Context _context;
        private readonly Factory _factory;

        #region Constructor

        private Program()
        {
            _context = new Context();
            _factory = new Factory();
        }

        #endregion

        #region Methods

        public async Task MainAsync()
        {
            // Initialize the source and destination.
            Console.WriteLine("[App] Creating source connection");
            var source = await _factory.CreateAsync();
            Console.WriteLine("[App] Creating destination connection");
            var destination = await _factory.CreateAsync();
            Console.WriteLine("[App] Fetching source states");
            var sourceFiles = await source.CollectAsync();
            Console.WriteLine("[App] Fetching destination states");
            var destinationFiles = await destination.CollectAsync();

            // Initialize the processing context.
            Console.WriteLine("[App] Updating processing context");
            _context.MapFiles(sourceFiles, destinationFiles);
            Console.WriteLine("[App] Flushing processing context");
            Console.WriteLine("- {0}", await _context.WriteJsonAsync("Context.json"));

            // Initialize the processing summary.
            Console.WriteLine("[App] Fetching processing summary");
            Console.WriteLine("- Mark Played         : {0}", _context.MarkPlayed.Count);
            Console.WriteLine("- Mark Unplayed       : {0}", _context.MarkUnplayed.Count);
            Console.WriteLine("- Missing Destination : {0}", _context.MissingDestination.Count);
            Console.WriteLine("- Missing Source      : {0}", _context.MissingSource.Count);
            Console.WriteLine("- No Change           : {0}", _context.NoChange.Count);

            // Initialize the user confirmation.
            Console.WriteLine("[App] Checking user confirmation");
            Console.WriteLine("- Press {ENTER} to continue");
            Console.ReadLine();

            // Process the changes.
            Console.WriteLine("[App] Updating destination states");
            await RunAsync(destination, destinationFiles);
            Console.WriteLine("[App] Done!");
        }

        private async Task RunAsync(IMediaServer destination, IReadOnlyDictionary<string, IMediaServerEntry> destinationFiles)
        {
            foreach (var key in _context.MarkPlayed)
            {
                if (!destinationFiles.TryGetValue(key, out var destinationEntry)) continue;
                await destination.UpdateAsync(destinationEntry, true);
            }

            foreach (var key in _context.MarkUnplayed)
            {
                if (!destinationFiles.TryGetValue(key, out var destinationEntry)) continue;
                await destination.UpdateAsync(destinationEntry, false);
            }
        }

        #endregion

        #region Statics

        public static void Main()
        {
            new Program().MainAsync().Wait();
        }

        #endregion
    }
}