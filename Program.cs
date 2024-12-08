using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DotNetEnv;
using OpenAI.Chat;

namespace RadioDramaGenerator
{
    class Program
    {
        private const string CharactersDirectory = "Characters";
        private const string ScenesDirectory = "Scenes";

        static async Task Main(string[] args)
        {
            // Load .env file
            Env.Load("key.env");

            // Retrieve API key from environment variables
            string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                ?? throw new Exception("API key not set in environment variables.");

            // Initialize OpenAI Chat client
            var client = new ChatClient(model: "gpt-4", apiKey: apiKey);

            // Load characters
            Character? kai = Character.LoadStaticData(GetCharacterFilePath("kai-static.json"));
            Character? elena = Character.LoadStaticData(GetCharacterFilePath("elena-static.json"));

            // Load dialogue history
            kai.LoadDialogueHistory(GetCharacterFilePath("kai-dialogue.json"));
            elena.LoadDialogueHistory(GetCharacterFilePath("elena-dialogue.json"));

            // Initialize scene
            SceneDynamic scene = LoadScene("market-square.json");

            // Main loop
            while (!scene.IsComplete)
            {
                Console.WriteLine($"\nScene: {scene.Description}");
                DisplaySceneStates(scene);

                // Generate character responses
                string kaiResponse = await GenerateEmotionallyDrivenResponse(client, kai, elena, scene);
                string elenaResponse = await GenerateEmotionallyDrivenResponse(client, elena, kai, scene);

                Console.WriteLine($"\nKai: {kaiResponse}");
                Console.WriteLine($"Elena: {elenaResponse}");

                // Save dialogue history
                kai.AddDialogue(kaiResponse);
                elena.AddDialogue(elenaResponse);
                kai.SaveDialogueHistory(GetCharacterFilePath("kai-dialogue.json"));
                elena.SaveDialogueHistory(GetCharacterFilePath("elena-dialogue.json"));

                // Narrator menu
                Console.WriteLine("\nWhat would you like to do next?");
                Console.WriteLine("1. Add a manual scene directive.");
                Console.WriteLine("2. Generate a dynamic challenge.");
                Console.WriteLine("3. Continue the scene.");
                Console.WriteLine("4. End the scene.");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddNarratorDirective(scene);
                        break;
                    case "2":
                        await GenerateDynamicChallenge(client, scene);
                        break;
                    case "3":
                        Console.WriteLine("The scene progresses...");
                        break;
                    case "4":
                        scene.IsComplete = true;
                        Console.WriteLine("Ending the scene.");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Continuing the scene.");
                        break;
                }
            }

            Console.WriteLine("Scene complete.");
        }

        static SceneDynamic LoadScene(string sceneFile)
        {
            string path = Path.Combine(ScenesDirectory, sceneFile);

            if (!File.Exists(path))
            {
                return new SceneDynamic
                {
                    SceneName = "Default Scene",
                    Description = "A generic scene unfolds...",
                    States = new Dictionary<string, string>
                    {
                        { "TimeOfDay", "Unknown" },
                        { "Weather", "Unknown" }
                    },
                    Challenges = new List<string>(),
                    NotableLocations = new List<string>(),
                    BackgroundSounds = new List<string>()
                };
            }

            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<SceneDynamic>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new SceneDynamic();
        }


        static async Task<string> GenerateEmotionallyDrivenResponse(ChatClient client, Character character, Character otherCharacter, SceneDynamic scene)
        {
            // Build the chat prompt
            string prompt = BuildPromptContext(character, otherCharacter, scene);

            // Safely retrieve emotion and motivation as strings
            string emotion = character.Traits.GetValueOrDefault("emotion", "neutral")?.ToString() ?? "neutral";
            string motivation = character.Traits.GetValueOrDefault("motivation", "no specific motivation")?.ToString() ?? "no specific motivation";


            // Add emotional context
            prompt += $"\n{character.Name} is feeling {emotion} and is motivated by {motivation}.";
            prompt += $" Respond in a way that reflects this emotional state.";

            // Send the prompt and get the chat completion
            var result = await client.CompleteChatAsync(prompt);
            var chatCompletion = result.Value;

            // Extract and concatenate the content parts
            if (chatCompletion?.Content is null || !chatCompletion.Content.Any())
            {
                return "No response generated.";
            }

            return string.Join(" ", chatCompletion.Content.Select(part => part.Text)).Trim();
        }

        static string BuildPromptContext(Character character, Character otherCharacter, SceneDynamic scene)
        {
            return $"Scene:\n{scene.Description}\n\n" +
                   $"{character.Name}'s perspective:\n" +
                   $"Backstory: {character.Backstory}\n" +
                   $"Description: {character.Description}\n" +
                   $"Traits: Intelligence: {character.Traits["intelligence"]}, Dexterity: {character.Traits.GetValueOrDefault("dexterity", 0)}, Charisma: {character.Traits.GetValueOrDefault("charisma", 0)}\n" +
                   $"Equipment: {string.Join(", ", character.Equipment)}\n\n" +
                   $"{character.Name} is currently interacting with {otherCharacter.Name}.\n" +
                   $"Recent dialogue history:\n" +
                   $"{GetRecentDialogue(character)}\n" +
                   $"{GetRecentDialogue(otherCharacter)}\n\n" +
                   $"{character.Name}'s next response:";
        }

        static void AddNarratorDirective(SceneDynamic scene)
        {
            Console.WriteLine("Enter your directive:");
            string directive = Console.ReadLine();

            scene.Challenges.Add(directive);
            Console.WriteLine($"Directive added: {directive}");
        }

        static async Task GenerateDynamicChallenge(ChatClient client, SceneDynamic scene)
        {
            string challengePrompt = "Generate an unexpected challenge for the current scene involving a market and a relic.";
            var result = await client.CompleteChatAsync(challengePrompt);
            var chatCompletion = result.Value;

            string challenge = string.Join(" ", chatCompletion.Content.Select(part => part.Text)).Trim();
            scene.Challenges.Add(challenge);

            Console.WriteLine($"Generated challenge: {challenge}");
        }

        static void DisplaySceneStates(SceneDynamic scene)
        {
            Console.WriteLine($"Scene: {scene.SceneName}");
            Console.WriteLine(scene.Description);

            if (scene.States.Any())
            {
                Console.WriteLine("\nCurrent Scene States:");
                foreach (var state in scene.States)
                {
                    Console.WriteLine($" - {state.Key}: {state.Value}");
                }
            }

            if (scene.NotableLocations.Any())
            {
                Console.WriteLine("\nNotable Locations:");
                foreach (var location in scene.NotableLocations)
                {
                    Console.WriteLine($" - {location}");
                }
            }

            if (scene.BackgroundSounds.Any())
            {
                Console.WriteLine("\nBackground Sounds:");
                foreach (var sound in scene.BackgroundSounds)
                {
                    Console.WriteLine($" - {sound}");
                }
            }

            if (scene.Challenges.Any())
            {
                Console.WriteLine("\nActive Challenges:");
                foreach (var challenge in scene.Challenges)
                {
                    Console.WriteLine($" - {challenge}");
                }
            }
        }


        static string GetCharacterFilePath(string fileName)
        {
            string? currentDirectory = AppContext.BaseDirectory;

            while (currentDirectory != null && !Directory.Exists(Path.Combine(currentDirectory, "Characters")))
            {
                currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
            }

            if (currentDirectory != null)
            {
                return Path.Combine(currentDirectory, "Characters", fileName);
            }

            throw new DirectoryNotFoundException("Characters directory not found. Please ensure it exists in the project root.");
        }

        static string GetRecentDialogue(Character character)
        {
            int maxEntries = 3;
            var recentDialogues = character.DialogueHistory.TakeLast(maxEntries);

            return recentDialogues.Any()
                ? string.Join("\n", recentDialogues.Select(d => $"[{d.Timestamp:yyyy-MM-dd HH:mm}] {d.Line}"))
                : "No recent dialogue available.";
        }
    }


}
