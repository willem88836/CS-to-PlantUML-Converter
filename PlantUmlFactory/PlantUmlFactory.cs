using Newtonsoft.Json;
using PlantUml.Net;
using System;
using System.Diagnostics;
using System.IO;

namespace PlantUmlBuilder
{
    public class PlantUmlFactory
    {
        private readonly Settings settings;

        public PlantUmlFactory()
        {
            settings = LoadSettings();

            if (!File.Exists(settings.AssemblyPath))
            {
                Console.WriteLine("Could not find assembly path. Terminating PlantUMLBuilder..");
                return;
            }

            Resources resources = LoadResources(settings.ResourcePath);

            PlantUmlBuilder builder = new PlantUmlBuilder(settings.AssemblyPath, resources);
            string plantCode = builder.CompilePlantUMLCode();

            Save(plantCode, settings.OutputDirectory);

            MakeImage(plantCode);

            Console.WriteLine("Completed PlantUML Code!");
        }

        private Settings LoadSettings()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string settingsPath = Path.Combine(currentDirectory, "config/settings.json");
            string json = File.ReadAllText(settingsPath);
            Settings settings = JsonConvert.DeserializeObject<Settings>(json);

            if (settings.OutputDirectory.StartsWith('\\'))
                settings.OutputDirectory = Path.Combine(currentDirectory, settings.OutputDirectory.Substring(1));
            if (settings.ResourcePath.StartsWith('\\'))
                settings.ResourcePath = Path.Combine(currentDirectory, settings.ResourcePath.Substring(1));

            Console.WriteLine($"Loaded settings file: {settingsPath}.");
            return settings;
        }

        private Resources LoadResources(string path)
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

        private void Save(string code, string path)
        {
            if (!Directory.Exists(this.settings.OutputDirectory))
                Directory.CreateDirectory(this.settings.OutputDirectory);

            string outputPath = Path.Combine(path, "plant-code.wsd");
            File.WriteAllText(outputPath, code);
            Console.WriteLine($"Saved UML Code at: {outputPath}.");
        }

        private void MakeImage(string code)
        {
            if (settings.OutputFormats.Length == 0)
                return;

            RendererFactory factory = new RendererFactory();
            PlantUmlSettings plantSettings = new PlantUmlSettings();
            plantSettings.RenderingMode = RenderingMode.Local;

            IPlantUmlRenderer renderer = factory.CreateRenderer(plantSettings);
            foreach (OutputFormat format in settings.OutputFormats)
            {
                string formatName = Enum.GetName(typeof(OutputFormat), format);
                Console.WriteLine($"Rendering: {formatName}");
                byte[] bytes = renderer.Render(code, format);
                string writePath = Path.Combine(settings.OutputDirectory, $"diagram-{formatName}.{formatName.ToLower()}");
                File.WriteAllBytes(writePath, bytes);
                Console.WriteLine($"Saved {formatName} render at: {writePath}");
            }
        }
    }
}
