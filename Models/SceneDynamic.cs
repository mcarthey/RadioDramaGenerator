class SceneDynamic
{
    public string SceneName { get; set; } // "Market Square at Sunset"
    public string Description { get; set; } // The rich scene description
    public Dictionary<string, string> States { get; set; } // E.g., "TimeOfDay" -> "Sunset"
    public List<string> Challenges { get; set; } // Dynamic challenges added to the scene
    public List<string> NotableLocations { get; set; } // E.g., "A blacksmith's forge..."
    public List<string> BackgroundSounds { get; set; } // E.g., "Soft chatter of merchants..."
    public bool IsComplete { get; set; }

    public SceneDynamic()
    {
        States = new Dictionary<string, string>();
        Challenges = new List<string>();
        NotableLocations = new List<string>();
        BackgroundSounds = new List<string>();
        IsComplete = false;
    }
}
