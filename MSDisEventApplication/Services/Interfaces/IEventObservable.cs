using MSDisEventApplication.Models;

namespace MSDisEventApplication.Services.Interfaces
{
    public interface IEventObservable<T> : IObservable<T>
    {
        void PublishEvent(T userEvent);

        void OnError(Exception error);

        void OnCompleted();
    }
}
