namespace Dottor.MyWorkerService
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private readonly IServiceScopeFactory _scopeFactory;

		public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
		{
			_logger = logger;
			_scopeFactory = scopeFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));

			while (await timer.WaitForNextTickAsync(stoppingToken))
			{
				using var scope = _scopeFactory.CreateScope();
				try
				{
					var dummy = scope.ServiceProvider.GetService<ILoggerFactory>();

					_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
				}
				finally
				{
					if (scope is IAsyncDisposable disposeAsync)
						await disposeAsync.DisposeAsync();
					else
						scope?.Dispose();
				}

			}


			//while (!stoppingToken.IsCancellationRequested)
			//{
			//    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
			//    await Task.Delay(10000, stoppingToken);
			//}
		}
	}
}