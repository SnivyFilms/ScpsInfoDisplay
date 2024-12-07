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
        public override Version Version { get; } = new Version(2, 2, 0);
        public override Version RequiredExiledVersion { get; } = new Version(9, 0, 0,4);
        public static ScpsInfoDisplay Singleton = new ScpsInfoDisplay();
        public EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            Singleton = this;
            EventHandlers = new EventHandlers();

            RegisterEvents();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnregisterEvents();
            EventHandlers = null;
            Singleton = null;

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