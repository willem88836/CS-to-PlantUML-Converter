using System;
using System.IO;
using Newtonsoft.Json;
using PlantUMLBuilder;

namespace myApp
{
    public class Progam
    {
        private static Settings settings;

        static void Main(string[] args)
        {
            settings = LoadSettings();

            if (!File.Exists(settings.AssemblyPath))
            {
                Console.WriteLine("Could not find assembly path. Terminating PlantUMLBuilder..");
                return;
            }

            Resources resources = LoadResources(settings.ResourcePath);

            PlantUMLBuilder builder = new PlantUMLBuilder(settings.AssemblyPath, resources);
            string plantCode = builder.CompilePlantUMLCode();

            Save(plantCode, settings.SavePath);
            Console.WriteLine("Completed PlantUML Code!");
        }

        private static Settings LoadSettings()
        {
            string settingsPath = Path.Combine(Directory.GetCurrentDirectory(), "settings.json");
            Settings settings;

            // Creates a new settings file if none exists.
            if (!File.Exists(settingsPath))
            {
                Console.WriteLine("Could not find settings file. Making a new one...");
                settings = new Settings();
                settings.AssemblyPath = null;
                settings.ResourcePath = Path.Combine(Directory.GetCurrentDirectory(), "resources.json");
                settings.SavePath = Path.Combine(Directory.GetCurrentDirectory(), "out.wsd");
                string jsonOut = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(settingsPath, jsonOut);
                return settings;
            }

            // Loads the settings file.
            string json = File.ReadAllText(settingsPath);
            settings = JsonConvert.DeserializeObject<Settings>(json);
            Console.WriteLine($"Loaded settings file: {settingsPath}.");
            return settings;
        }

        private static Resources LoadResources(string path)
        {
            Resources resources;

            if (!File.Exists(path))
            {
                Console.WriteLine("Could not find resources file. Making a new one...");
                resources = new Resources();
                string jsonOut = JsonConvert.SerializeObject(resources, Formatting.Indented);
                File.WriteAllText(path, jsonOut);
                return resources;
            }

            // loads and returns 
            string json = File.ReadAllText(path);
            resources = JsonConvert.DeserializeObject<Resources>(json);
            Console.WriteLine($"Loaded resources file: {path}.");
            return resources;
        }

        private static void Save(string code, string path)
        {
            File.WriteAllText(path, code);
            Console.WriteLine($"Saved UML Code at: {path}.");
        }
    }
}
