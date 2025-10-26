using MSDisEventApplication.Data.Interfaces;
using MSDisEventApplication.Models;
using MSDisEventApplication.Services.Interfaces;

namespace MSDisEventApplication.Services;

public class EventObserver : IEventObserver
{
    private readonly ILogger<EventObserver> _logger;
    private readonly IDataStorage _dataStorage;
    private readonly int _minuteIntervals = 10;

    public EventObserver(IDataStorage dataStorage, ILogger<EventObserver> logger)
    {
        _dataStorage = dataStorage;
        _logger = logger;
    }

    public void OnNext(UserEvent value)
    {
        try
        {
            value.Timestamp = GetIntervalByMinutesStart(value.Timestamp, _minuteIntervals);
           _dataStorage.SaveEvent(value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка обработки события EventType={EventType} для UserId={UserId}", 
                value?.EventType,
                value?.UserId);
        }
    }

    public void OnCompleted()
    {
        _logger.LogInformation("EventObserver completed");
    }

    public void OnError(Exception error)
    {
        _logger.LogError(error, $"EventObserver возникла ошибка: {error.Message}");
    }

    private DateTime GetIntervalByMinutesStart(DateTime timestamp, int minuteInterval)
    {
        var minutes = timestamp.Minute;
        var roundedMinutes = (minutes / minuteInterval) * minuteInterval;

        return new DateTime(
            timestamp.Year,
            timestamp.Month,
            timestamp.Day,
            timestamp.Hour,
            roundedMinutes,
            0,
            DateTimeKind.Utc
        );
    }
}
