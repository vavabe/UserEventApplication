namespace MSDisEventApplication.Models;

public class UserEvent
{
    public int UserId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
}
