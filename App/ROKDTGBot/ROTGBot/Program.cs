using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Serilog;
using Microsoft.Extensions.Configuration;

namespace ROTGBot
{
    public class Program
    {
        private const string _logDirectory = "Logs";
        private const string _logFileName = "log-startup.txt";
        private const string _appSettingsFileName = "appsettings.json";
        private const string _startUpInfoMessage = "App starts with arguments: {0}";

        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            string _startUpLogPath = Path.Combine(_logDirectory, _logFileName);
            var loggerConfig = new LoggerConfiguration()
               .WriteTo.Console()
               .WriteTo.File(_startUpLogPath)
               .MinimumLevel.Verbose();

            using var logger = loggerConfig.CreateLogger();
            logger.Information(string.Format(_startUpInfoMessage, string.Join(", ", args)));

            GetWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create IWebHostBuilder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected static IWebHostBuilder GetWebHostBuilder(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(GetConfiguration())
                .ConfigureAppConfiguration((hostingContext, config) => ConfigureApp(args, config))
                .ConfigureLogging((hostingContext, logging) => CreateLogger(hostingContext, logging))
                .UseKestrel()
                .UseStartup<Startup>();

            return builder;
        }

        /// <summary>
        /// Create Logger method
        /// </summary>
        /// <param name="hostingContext"></param>
        /// <param name="logging"></param>
        private static void CreateLogger(WebHostBuilderContext hostingContext, ILoggingBuilder logging)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(hostingContext.Configuration)
                .CreateLogger();
            logging.AddSerilog(Log.Logger);           
        }

        /// <summary>
        /// Configure App method
        /// </summary>
        /// <param name="args"></param>
        /// <param name="config"></param>
        private static void ConfigureApp(string[] args, IConfigurationBuilder config)
        {
            if (args != null) config.AddCommandLine(args);

        }

        /// <summary>
        /// Build app Configuration
        /// </summary>
        /// <returns></returns>
        private static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile(_appSettingsFileName, optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
                                .AddDbConfiguration()
                                .Build();
        }
    }
}
