﻿using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RainBorg
{
    class Config
    {
        public static async Task Load()
        {
            // Check if config file exists and create it if it doesn't
            if (!File.Exists(Constants.Config))
            {
                File.Create(Constants.Config).Close();
                await Save();
            }
            else
            {
                // Load values
                JObject Config = JObject.Parse(File.ReadAllText(Constants.Config));
                RainBorg.tipFee = (double)Config["tipFee"];
                RainBorg.tipMin = (double)Config["tipMin"];
                RainBorg.tipMax = (double)Config["tipMax"];
                RainBorg.userMin = (int)Config["userMin"];
                RainBorg.userMax = (int)Config["userMax"];
                RainBorg.waitMin = (int)Config["waitMin"];
                RainBorg.waitMax = (int)Config["waitMax"];
                RainBorg.logLevel = (int)Config["logLevel"];
                RainBorg.Operators = Config["operators"].ToObject<List<ulong>>();
                RainBorg.Blacklist = Config["blacklist"].ToObject<List<ulong>>();
                RainBorg.ChannelWeight = Config["channelWeight"].ToObject<List<ulong>>();
                RainBorg.StatusChannel = Config["statusChannel"].ToObject<List<ulong>>();
                foreach (ulong Id in RainBorg.ChannelWeight)
                    if (!RainBorg.UserPools.ContainsKey(Id))
                        RainBorg.UserPools.Add(Id, new List<ulong>());
            }
        }

        public static Task Save()
        {
            // Store values
            JObject Config = new JObject
            {
                ["tipFee"] = RainBorg.tipFee,
                ["tipMin"] = RainBorg.tipMin,
                ["tipMax"] = RainBorg.tipMax,
                ["userMin"] = RainBorg.userMin,
                ["userMax"] = RainBorg.userMax,
                ["waitMin"] = RainBorg.waitMin,
                ["waitMax"] = RainBorg.waitMax,
                ["logLevel"] = RainBorg.logLevel,
                ["operators"] = JToken.FromObject(RainBorg.Operators),
                ["blacklist"] = JToken.FromObject(RainBorg.Blacklist),
                ["channelWeight"] = JToken.FromObject(RainBorg.ChannelWeight),
                ["statusChannel"] = JToken.FromObject(RainBorg.StatusChannel)
            };

            // Flush to file
            File.WriteAllText(Constants.Config, Config.ToString());

            // Completed
            return Task.CompletedTask;
        }
    }
}