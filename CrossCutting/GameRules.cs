using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;

namespace cs2_rockthevote
{
    public class GameRules : IPluginDependency<Plugin, Config>
    {
        CCSGameRules? _gameRules = null;
        private Plugin? _plugin = null;

        public void SetGameRules() => _gameRules = Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules").FirstOrDefault()?.GameRules;

        public void SetGameRulesAsync()
        {
            _gameRules = null;
            _plugin.Logger.LogInformation("SetGameRulesAsync() Start...");
            new CounterStrikeSharp.API.Modules.Timers.Timer(1.0F, () =>
            {
                _plugin.Logger.LogInformation("SetGameRulesAsync Timer Start...");
                SetGameRules();
                _plugin.Logger.LogInformation("SetGameRulesAsync Timer End...");
            });
        }

        public void OnLoad(Plugin plugin)
        {
            _plugin = plugin;
            SetGameRulesAsync();
            plugin.RegisterEventHandler<EventRoundStart>(OnRoundStart);
            plugin.RegisterEventHandler<EventRoundAnnounceWarmup>(OnAnnounceWarmup);
        }

        public float GameStartTime => _gameRules?.GameStartTime ?? 0;

        public void OnMapStart(string map)
        {
            SetGameRulesAsync();
        }


        public HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
        {
            SetGameRules();
            return HookResult.Continue;
        }

        public HookResult OnAnnounceWarmup(EventRoundAnnounceWarmup @event, GameEventInfo info)
        {
            SetGameRules();
            return HookResult.Continue;
        }

        public bool WarmupRunning => _gameRules?.WarmupPeriod ?? false;

        public int TotalRoundsPlayed => _gameRules?.TotalRoundsPlayed ?? 0;
    }
}
