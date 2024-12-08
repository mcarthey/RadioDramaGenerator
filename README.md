# Radio Drama Generator 🎙️

The **Radio Drama Generator** is an immersive storytelling tool that brings scenes and characters to life through AI-powered dialogue and dynamic scene descriptions. It’s perfect for writers, RPG enthusiasts, or anyone who wants to explore interactive, character-driven narratives.

## Features

- **Dynamic Scenes**:
  - Rich, detailed descriptions with evolving states like weather, time of day, and crowd activity.
  - Challenges and events that add complexity to each scene.

- **Emotionally Responsive Characters**:
  - Characters are driven by their emotional state and motivations.
  - Dialogue adapts to the context, their traits, and relationships.

- **Narrator Integration**:
  - Jump into the story to manually guide or disrupt the flow.
  - Seamlessly add challenges, directives, or let the story progress naturally.

- **AI-Powered Dialogue**:
  - Uses OpenAI’s GPT-4 to generate contextually aware and personality-rich character responses.
  - Easily customizable prompts and interactions.

## How It Works

1. **Characters**:
   - Defined using JSON files, each character has unique traits, motivations, and equipment.
   - Emotional states influence how they respond to scenes and interact with others.

2. **Scenes**:
   - Scenes are loaded from JSON files and include descriptions, notable locations, ambient sounds, and active challenges.
   - States like weather and time of day evolve dynamically as the story progresses.

3. **Narration and Interaction**:
   - You can act as the narrator, directing the story by adding events or challenges manually.
   - AI-generated twists can be triggered to keep the story unpredictable.

## Example Output

```plaintext
Scene: Market Square at Sunset
The sun sets over the bustling market square, casting long shadows across the cobblestoned streets. Stalls are being packed up by weary merchants, their goods reduced to a few scattered items...

Current Scene States:
 - timeOfDay: Sunset
 - weather: Mild, with a gentle breeze

Notable Locations:
 - A blacksmith's forge with glowing embers visible through the open door
 - A bakery with its shutters half-closed, the smell of fresh bread lingering

Kai: "Absolutely, let's uncover the magic hiding in this thing!"
Elena: "Let’s take this relic to the blacksmith's forge and see what secrets it holds."
```

## The Vision

This tool is designed to make storytelling interactive and vibrant, merging the creativity of tabletop RPGs with the power of AI. Whether you're crafting a collaborative story or immersing yourself in a narrative world, the Radio Drama Generator adapts to your storytelling style.
