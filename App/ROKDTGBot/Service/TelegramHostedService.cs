using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ROTGBot.Service
{
    public class TelegramHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        private readonly bool isRunning = true;
        private readonly CancellationTokenSource _tokenSource;
        private int offset = 0;

        public TelegramHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = _serviceProvider.GetRequiredService<ILogger<TelegramHostedService>>();
            _tokenSource = new CancellationTokenSource();

        }

        public async Task Run(CancellationToken _cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var scopeProvider = scope.ServiceProvider;
            var _mainService = scopeProvider.GetRequiredService<ITelegramMainService>();
            await _mainService.SetCommands();

            while (isRunning && !_cancellationToken.IsCancellationRequested)
            {     
                try
                {                                        
                    offset = await _mainService.Execute(offset);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error in TelegramHostedService: Run: {Message} {StackTrace}", ex.Message, ex.StackTrace);
                }
                await Task.Delay(1000, _cancellationToken);
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Factory.StartNew(() => Run(_tokenSource.Token), cancellationToken,
                TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            _tokenSource.Cancel();
        }
    }
}
