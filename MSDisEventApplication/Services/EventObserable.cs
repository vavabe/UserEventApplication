using MSDisEventApplication.Models;
using MSDisEventApplication.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace MSDisEventApplication.Services;

public class EventObservable : IEventObservable<UserEvent>, IDisposable
{
    private readonly Subject<UserEvent> _subject = new Subject<UserEvent>();
    private bool _disposed;

    public IDisposable Subscribe(IObserver<UserEvent> observer)
    {
        return _subject.Subscribe(observer);
    }

    public void PublishEvent(UserEvent userEvent)
    {
        if (!_disposed)
            _subject.OnNext(userEvent);
    }

    public void OnError(Exception error)
    {
        _subject.OnError(error);
    }

    public void OnCompleted()
    {
        _subject.OnCompleted();
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _subject.OnCompleted();
            _subject.Dispose();
            _disposed = true;
        }
    }
}
