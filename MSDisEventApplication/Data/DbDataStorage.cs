using Microsoft.Extensions.Options;
using MSDisEventApplication.Data.Interfaces;
using MSDisEventApplication.Models;
using MSDisEventApplication.Options;
using Npgsql;

namespace MSDisEventApplication.Data;

public class DbDataStorage : IDataStorage
{
    private readonly DbOptions _dbOptions;
    private readonly ILogger<DbDataStorage> _logger;

    public DbDataStorage(IOptions<DbOptions> dbOptions, ILogger<DbDataStorage> logger)
    {
        _dbOptions = dbOptions.Value;
        _logger = logger;
    }

    public void SaveEvent(UserEvent userEvent)
    {
        try
        {
            using var conn = new NpgsqlConnection(_dbOptions.ConnectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                    "INSERT INTO user_event_stats (user_id, event_type, count) " +
                    "VALUES (@userId, @eventType, 1) " +
                    "ON CONFLICT (user_id, event_type) " +
                    "DO UPDATE SET count = user_event_stats.count + 1;",
                    conn);
            cmd.Parameters.AddWithValue("userId", userEvent.UserId);
            cmd.Parameters.AddWithValue("eventType", userEvent.EventType);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка при сохранении в базу данных: {ex.Message}");
        }
    }
}
