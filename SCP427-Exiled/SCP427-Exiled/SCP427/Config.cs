using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Loader;

using SCP427.Configs;

namespace SCP427
{
    public class Config : IConfig
    {
        public Configs.Items ItemConfigs;


        [Description("Whether or not this plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Whether or not debug messages should be displayed in the server console.")]
        public bool IsDebugEnabled { get; set; } = false;

        public string ItemConfigFolder { get; set; } = Path.Combine(Paths.Configs, "CustomItems");

        public string ItemConfigFile { get; set; } = "global.yml";

        public void LoadItems()
        {
            if (!Directory.Exists(ItemConfigFolder))
                Directory.CreateDirectory(ItemConfigFolder);

            string filePath = Path.Combine(ItemConfigFolder, ItemConfigFile);
            Log.Info($"{filePath}");
            if (!File.Exists(filePath))
            {
                ItemConfigs = new Configs.Items();
                File.WriteAllText(filePath, Loader.Serializer.Serialize(ItemConfigs));
            }
            else
            {
                ItemConfigs = Loader.Deserializer.Deserialize<Configs.Items>(File.ReadAllText(filePath));
                File.WriteAllText(filePath, Loader.Serializer.Serialize(ItemConfigs));

            }
        }
    }

}
