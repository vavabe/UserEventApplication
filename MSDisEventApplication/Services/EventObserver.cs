using MSDisEventApplication.Data;
using MSDisEventApplication.Models;
using MSDisEventApplication.Services.Interfaces;
using System.Diagnostics;

namespace MSDisEventApplication.Services
{
    public class EventObserver : IEventObserver
    {
        private readonly ILogger<EventObserver> _logger;
        private readonly IDataStorage _dataStorage;

        public EventObserver(IDataStorage dataStorage, ILogger<EventObserver> logger)
        {
            _dataStorage = dataStorage;
            _logger = logger;
        }

        public void OnNext(UserEvent value)
        {
            try
            {
               _dataStorage.SaveEvent(value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on handling user event for UserId={UserId}, EventType={EventType}", 
                    value?.UserId, 
                    value?.EventType);
            }
        }

        public void OnCompleted()
        {
            _logger.LogInformation("EventObserver completed");
        }

        public void OnError(Exception error)
        {
            _logger.LogError(error, $"EventObserver error: {error.Message}");
        }
    }
}
