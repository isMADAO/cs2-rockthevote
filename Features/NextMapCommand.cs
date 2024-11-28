﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;

namespace cs2_rockthevote.Features
{
    public class NextMapCommand : IPluginDependency<Plugin, Config>
    {
        private ChangeMapManager _changeMapManager;
        private StringLocalizer _stringLocalizer;
        private NextmapConfig _config = new();

        public NextMapCommand(ChangeMapManager changeMapManager, StringLocalizer stringLocalizer)
        {
            _changeMapManager = changeMapManager;
            _stringLocalizer = stringLocalizer;
        }

        public void CommandHandler(CCSPlayerController? player)
        {
            string text;
            if (_changeMapManager.NextMap is not null)
            {
                text = _stringLocalizer.LocalizeWithPrefix("nextmap", _changeMapManager.NextMap);
            }
            else
                text = _stringLocalizer.LocalizeWithPrefix("nextmap.decided-by-vote");

            if (_config.ShowToAll)
                Server.PrintToChatAll(text);
            else if (player is not null)
                player.PrintToChat(text);
            else
                Server.PrintToConsole(text);
        }

        public void OnLoad(Plugin plugin)
        {

            plugin.AddCommand("nextmap", "Shows nextmap when defined", (player, info) =>
            {
                string playerName = "Console";
                if (player is not null && player.IsValid)
                {
                    playerName = player.PlayerName;
                }
                plugin.Logger.LogInformation($"执行指令[nextmap], 发起人[{playerName}]");
                CommandHandler(player);
            });
        }

        public void OnConfigParsed(Config config)
        {
            _config = config.Nextmap ?? new();
        }
    }
}
