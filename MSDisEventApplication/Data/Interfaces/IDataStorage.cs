using MSDisEventApplication.Models;

namespace MSDisEventApplication.Data.Interfaces;

public interface IDataStorage
{
    void SaveEvent(UserEvent userEvent);
}
