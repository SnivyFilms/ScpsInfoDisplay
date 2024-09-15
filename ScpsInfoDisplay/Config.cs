using Exiled.API.Interfaces;
using PlayerRoles;
using ScpsInfoDisplay.Enums;
using System.Collections.Generic;
using System.ComponentModel;
using PlayerRoles.PlayableScps.Scp096;

namespace ScpsInfoDisplay
{
    public class Config : IConfig
    {
        [Description("Is the plugin enabled?")]
        public bool IsEnabled { get; set; } = true;
        [Description("Display debug messages in server console?")]
        public bool Debug { get; set; } = false;
        [Description("Display strings. Format: Role, display string.")]
        public Dictionary<RoleTypeId, string> DisplayStrings { get; set; } = new Dictionary<RoleTypeId, string>()
        {
            { RoleTypeId.Scp049, "<color=#FF0000><size=30>SCP-049</color> [<color=#19FF40>%health%</color>, <color=#19B2FF>%arhealth%</color>]\n<color=#FF19FF>Zombies: %zombies%</color></size>" },
            { RoleTypeId.Scp079, "<color=#FF0000><size=30>SCP-079</color> <color=#19FF40>Generators:</color> [<color=#19FF40>%generators%</color><color=#FF0000>%engaging%</color><color=#19FF40>/3</color>]\n <color=#19B2FF>Level: %079level%</color> <color=#FF19FF>Energy: %079energy%</color></size>" },
            { RoleTypeId.Scp096, "<color=#FF0000><size=30>SCP-096</color> [<color=#19FF40>%health%</color>, <color=#19B2FF>%arhealth%</color>] \n<color=#19B2FF>Rage State: %096state%</color>, <color=#FF19FF>Targets: %096targets%</color></size>" },
            { RoleTypeId.Scp106, "<color=#FF0000><size=30>SCP-106</color> [<color=#19FF40>%health%</color>, <color=#19B2FF>%arhealth%</color>] \n<color=#FF19FF>Vigor: %106vigor%% </size></color>" },
            { RoleTypeId.Scp173, "<color=#FF0000><size=30>SCP-173</color> [<color=#19FF40>%health%</color>, <color=#19B2FF>%arhealth%</color>, <color=#FF19FF>%173stared%</color>]</size>"},
            { RoleTypeId.Scp939, "<color=#FF0000><size=30>SCP-939</color> [<color=#19FF40>%health%</color>, <color=#19B2FF>%arhealth%</color>] </size>" },
            { RoleTypeId.Scp3114, "<color=#FF0000><size=30>SCP-3114</color> [<color=#19FF40>%health%</color>, <color=#19B2FF>%arhealth%</color>, <color=#FF19FF>%3114disguisetype%</color>]</size>" }
        };
        /*[Description("Custom roles integrations. Format: SessionVariable that marks that the player belongs to that role, display string.")]
        public Dictionary<string, string> CustomRolesIntegrations { get; set; } = new Dictionary<string, string>();*/
        [Description("Display string alignment. Possible values: Left, Center, Right.")]
        public TextAlignment TextAlignment { get; set; } = TextAlignment.Right;
        [Description("Display text position offset.")]
        public sbyte TextPositionOffset { get; set; } = 30;
        [Description("The player seeing the list will be highlighted with the special marker to the left. Leave it empty if disabled.")]
        public string PlayersMarker { get; set; } = "<color=#D7E607><size=30>You: </size></color>";
        [Description("The disguise text display for SCP-3114.")]
        public Dictionary<RoleTypeId, string> Scp3114DisguiseDisplay { get; set; } = new Dictionary<RoleTypeId, string>()
        {
            { RoleTypeId.ClassD, "D-Class"},
            { RoleTypeId.Scientist, "Scientist"},
            { RoleTypeId.FacilityGuard, "Facility Guard"},
            { RoleTypeId.NtfCaptain, "MTF Captain"},
            { RoleTypeId.NtfSergeant, "MTF Sergeant"},
            { RoleTypeId.NtfSpecialist, "MTF Specialist"},
            { RoleTypeId.NtfPrivate, "MTF Private"},
            { RoleTypeId.ChaosConscript, "Chaos Conscript"},
            { RoleTypeId.ChaosMarauder, "Chaos Marauder"},
            { RoleTypeId.ChaosRepressor, "Chaos Repressor"},
            { RoleTypeId.ChaosRifleman, "Chaos Rifleman"},
            { RoleTypeId.Tutorial, "Tutorial"}
        };

        [Description("Indicators for SCP-173 %173stared%")]
        public Dictionary<string, string> Scp173ObservationIndicators { get; set; } = new Dictionary<string, string>
        {
            { "Observed", "ⓞ" },
            { "Unobserved", "-" }
        };
        [Description("Indicators for SCP-096 %096state%")]
        public Dictionary<Scp096RageState, string> Scp096StateIndicator { get; set; } = new Dictionary<Scp096RageState, string>
        {
            { Scp096RageState.Docile, "Calm"},
            { Scp096RageState.Distressed, "Enraging"},
            { Scp096RageState.Enraged, "Enraged"},
            { Scp096RageState.Calming, "Calming"}
        };
    }
}