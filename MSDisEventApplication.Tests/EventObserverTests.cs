using Microsoft.Extensions.Logging;
using Moq;
using MSDisEventApplication.Data;
using MSDisEventApplication.Models;
using MSDisEventApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSDisEventApplication.Tests
{
    public class EventObserverTests
    {
        private readonly EventObserver _eventObserver;
        private readonly Mock<IDataStorage> _dataStorageMock = new Mock<IDataStorage>();
        private readonly Mock<ILogger<EventObserver>> _loggerMock = new Mock<ILogger<EventObserver>>();

        public EventObserverTests()
        {
            _eventObserver = new EventObserver(_dataStorageMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void OnNext_DataStorageCalledOnce()
        {
            var userEvent = new UserEvent { UserId = 123, EventType = "click" };

            _eventObserver.OnNext(userEvent);

            _dataStorageMock.Verify(ds => ds.SaveEvent(userEvent), Times.Once);
        }
    }
}
