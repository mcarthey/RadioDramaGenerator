using RadioDramaGenerator;
using System.Text.Json;

class Character
{
    public string Name { get; set; }
    public string Backstory { get; set; }
    public string Description { get; set; }
    public Dictionary<string, object> Traits { get; set; } // Changed from <string, int> to <string, object>
    public List<string> Equipment { get; set; }
    public List<DialogueEntry> DialogueHistory { get; private set; }

    public Character()
    {
        DialogueHistory = new List<DialogueEntry>();
        Traits = new Dictionary<string, object>(); // Ensure initialization
    }

    public static Character? LoadStaticData(string filePath)
    {
        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Character>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public void LoadDialogueHistory(string filePath)
    {
        var json = File.ReadAllText(filePath);
        var history = JsonSerializer.Deserialize<DialogueHistory>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        DialogueHistory = history?.Dialogues ?? new List<DialogueEntry>();
    }

    public void AddDialogue(string line)
    {
        DialogueHistory.Add(new DialogueEntry
        {
            Timestamp = DateTime.UtcNow,
            Line = line
        });
    }

    public void SaveDialogueHistory(string filePath)
    {
        var history = new DialogueHistory { Dialogues = DialogueHistory };
        var json = JsonSerializer.Serialize(history, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }
}
