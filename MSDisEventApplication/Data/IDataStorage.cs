using MSDisEventApplication.Models;

namespace MSDisEventApplication.Data;

public interface IDataStorage
{
    void SaveEvent(UserEvent userEvent);
}
