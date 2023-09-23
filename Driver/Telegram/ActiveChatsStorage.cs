using System.Collections;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Driver.Telegram;

public sealed class ActiveChatsStorage : IEnumerable<long>
{
    private readonly ILogger<ActiveChatsStorage> _logger;
    private readonly HashSet<long> _activeChats;
    
    private const string Path = "chats.json";

    public ActiveChatsStorage(ILogger<ActiveChatsStorage> logger)
    {
        _logger = logger;
        _activeChats = Load();
    }
    
    public void Add(long chatId)
    {
        _activeChats.Add(chatId);
        Save();
    }

    public void Remove(long chatId)
    {
        _activeChats.Remove(chatId);
        Save();
    }
    
    public IEnumerator<long> GetEnumerator()
    {
        return _activeChats.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void Save()
    {
        var json = JsonSerializer.Serialize(_activeChats);
        File.WriteAllText(Path, json);
    }
    
    private HashSet<long> Load()
    {
        try
        {
            var jsonString = File.ReadAllText(Path);
            return JsonSerializer.Deserialize<HashSet<long>>(jsonString);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while loading chats data {ex.Message}", ex);
            return new HashSet<long>();
        }
    }
}