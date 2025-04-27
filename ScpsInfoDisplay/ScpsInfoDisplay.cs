using Exiled.API.Features;
using System;
using Server = Exiled.Events.Handlers.Server;

namespace ScpsInfoDisplay
{
    internal class ScpsInfoDisplay : Plugin<Config>
    {
        public override string Prefix => "scpsinfodisplay";
        public override string Name => "ScpsInfoDisplay";
        public override string Author => "Bladuk and Vicious Vikki";
        public override Version Version { get; } = new Version(2, 3, 0);
        public override Version RequiredExiledVersion { get; } = new Version(9, 6, 0);
        public static ScpsInfoDisplay Instance;
        public EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            Instance = this;
            EventHandlers = new EventHandlers();

            RegisterEvents();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnregisterEvents();
            EventHandlers = null;
            Instance = null;

            base.OnDisabled();
        }

        private void RegisterEvents()
        {
            Server.RoundStarted += EventHandlers.OnRoundStarted;
        }

        private void UnregisterEvents()
        {
            Server.RoundStarted -= EventHandlers.OnRoundStarted;
        }
    }
}