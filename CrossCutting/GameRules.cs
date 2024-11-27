using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using StarCore.Module.TimerModule;
using Timer = CounterStrikeSharp.API.Modules.Timers.Timer;

namespace cs2_rockthevote
{
    public class GameRules : IPluginDependency<Plugin, Config>
    {
        CCSGameRules? _gameRules = null;
        private Plugin _parentPlugin = null!;

        public void SetGameRules() => _gameRules = Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules").FirstOrDefault()?.GameRules;

        public void SetGameRulesAsync()
        {
            _gameRules = null;
            StarTimerManager.AddTimer(_parentPlugin, 1.0F, () =>
            {
                SetGameRules();
            }, TimerLifeState.Once);
        }

        public void OnLoad(Plugin plugin)
        {
            _parentPlugin = plugin;
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
