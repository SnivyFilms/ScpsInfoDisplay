using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.CustomRoles.API.Features;
using MEC;
using NorthwoodLib.Pools;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp096;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Player = Exiled.API.Features.Player;

namespace ScpsInfoDisplay
{
    internal class EventHandlers
    {
        private CoroutineHandle _displayCoroutine;
        private static Config _config = ScpsInfoDisplay.Singleton.Config;
        internal void OnRoundStarted()
        {
            if (_displayCoroutine.IsRunning)
                Timing.KillCoroutines(_displayCoroutine);

            _displayCoroutine = Timing.RunCoroutine(ShowDisplay());
        }

        private IEnumerator<float> ShowDisplay()
        {
            while (Round.InProgress)
            {
                yield return Timing.WaitForSeconds(1f);
                try
                {
                    foreach (var player in Player.List.Where(p => p != null && ShouldDisplayForPlayer(p)))
                    {
                        var builder = StringBuilderPool.Shared.Rent($"<align={ScpsInfoDisplay.Singleton.Config.TextAlignment.ToString().ToLower()}>");

                        // Display SCPs
                        foreach (var scp in Player.List.Where(p => p?.Role.Team == Team.SCPs && ShouldDisplayForPlayer(p)))
                        {
                            if (ScpsInfoDisplay.Singleton.Config.DisplayStrings.ContainsKey(scp.Role.Type))
                            {
                                builder.Append((scp == player ? ScpsInfoDisplay.Singleton.Config.PlayersMarker : "")
                                               + ProcessStringVariables(ScpsInfoDisplay.Singleton.Config.DisplayStrings[scp.Role.Type], player, scp)).Append('\n');
                            }
                        }

                        // Display Custom Roles, but only the ones defined in CustomRolesIntegrations
                        foreach (var customRole in CustomRole.Registered)
                        {
                            if (ScpsInfoDisplay.Singleton.Config.CustomRolesIntegrations.ContainsKey(customRole.Name))
                            {
                                foreach (var customPlayer in customRole.TrackedPlayers)
                                {
                                    builder.Append((customPlayer == player ? ScpsInfoDisplay.Singleton.Config.PlayersMarker : "")
                                                   + ProcessCustomRoleVariables(customRole, customPlayer)).Append('\n');
                                }
                            }
                        }

                        builder.Append($"<voffset={ScpsInfoDisplay.Singleton.Config.TextPositionOffset}em> </voffset></align>");
                        player.ShowHint(StringBuilderPool.Shared.ToStringReturn(builder), 1.25f);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
        }
        private bool ShouldDisplayForPlayer(Player player)
        {
            return ScpsInfoDisplay.Singleton.Config.DisplayStrings.ContainsKey(player.Role.Type) ||
                   CustomRole.Registered.Any(customRole => customRole.TrackedPlayers.Contains(player) &&
                                                           ScpsInfoDisplay.Singleton.Config.CustomRolesIntegrations.ContainsKey(customRole.Name));
        }

        private string ProcessStringVariables(string raw, Player observer, Player target) => raw
            .Replace("%arhealth%", Math.Floor(target.HumeShield) >= 0 ? Math.Floor(target.HumeShield).ToString() : "")
            .Replace("%healthpercent%", Math.Floor(target.Health / target.MaxHealth * 100).ToString())
            .Replace("%health%", Math.Floor(target.Health).ToString())
            .Replace("%generators%", Generator.List.Count(gen => gen.IsEngaged).ToString())
            .Replace("%engaging%", Generator.List.Count(gen => gen.IsActivating) > 0 ? $" (+{Generator.List.Count(gen => gen.IsActivating)})" : "")
            .Replace("%distance%", target != observer ? Math.Floor(Vector3.Distance(observer.Position, target.Position)) + "m" : "")
            .Replace("%zombies%", Player.List.Count(p => p.Role.Type == RoleTypeId.Scp0492).ToString())
            .Replace("%079level%", target.Role.Is(out Scp079Role scp079) ? scp079.Level.ToString() : "")
            .Replace("%079energy%", target.Role.Is(out Scp079Role _) ? Math.Floor(scp079.Energy).ToString() : "")
            .Replace("%079experience%", target.Role.Is(out Scp079Role _) ? Math.Floor((double)scp079.Experience).ToString() : "")
            .Replace("%106vigor%", target.Role.Is(out Scp106Role scp106) ? Math.Floor(scp106.Vigor * 100).ToString() : "")
            .Replace("%3114disguisetype%", target.Role.Is(out Scp3114Role scp3114) ? (scp3114.DisguiseStatus.ToString() != "None" ? SkeletonDisguiseNames(scp3114.StolenRole) : "None") : "")
            .Replace("%096state%", target.Role.Is(out Exiled.API.Features.Roles.Scp096Role scp096) ? (_config.Scp096StateIndicator.TryGetValue(scp096.RageState, out var stateIcon) ? stateIcon : "Unknown") : "")
            .Replace("%096targets%", target.Role.Is(out Exiled.API.Features.Roles.Scp096Role _) ? scp096.Targets.Count.ToString() : "")
            .Replace("%173stared%", target.Role.Is(out Scp173Role scp173) ? (_config.Scp173ObservationIndicators.TryGetValue(scp173.IsObserved ? "Observed" : "Unobserved", out var icon) ? icon : "-") : "")
            .Replace("%playername%", target.Nickname);
        
        private string ProcessCustomRoleVariables(CustomRole customRole, Player observer)
        {
            var raw = ScpsInfoDisplay.Singleton.Config.CustomRolesIntegrations.TryGetValue(customRole.Name, out var value) ? value : "";

            return raw
                .Replace("%customrole%", customRole.Name)
                .Replace("%playername%", observer.Nickname)
                .Replace("%health%", Math.Floor(observer.Health).ToString())
                .Replace("%healthpercent%",
                    Math.Floor(observer.ArtificialHealth) >= 0 ? Math.Floor(observer.ArtificialHealth).ToString() : "");
        }

        private string SkeletonDisguiseNames(RoleTypeId disguise)
        {
            return _config.Scp3114DisguiseDisplay.TryGetValue(disguise, out var displayName) ? displayName : disguise.ToString();
        }
    }
}
