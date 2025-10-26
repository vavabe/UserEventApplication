
using MSDisEventApplication.Models;
using MSDisEventApplication.Services.Interfaces;

namespace MSDisEventApplication.Services;

public class EventBackgroundService : IHostedService
{
    private IDisposable? _observerSubscription;
    private readonly ILogger<EventBackgroundService> _logger;
    private readonly IEventObservable<UserEvent> _eventObservable;
    private readonly IEventObserver _eventObserver;

    public EventBackgroundService(ILogger<EventBackgroundService> logger, 
        IEventObservable<UserEvent> eventObservable, 
        IEventObserver eventObserver)
    {
        _logger = logger;
        _eventObservable = eventObservable;
        _eventObserver = eventObserver;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Event processing service starting...");

        _observerSubscription = _eventObservable.Subscribe(_eventObserver);

        _logger.LogInformation("Event processing service started.");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Event processing service stopping...");

        _observerSubscription?.Dispose();

        _logger.LogInformation("Event processing service stopped.");

        return Task.CompletedTask;
    }
}
