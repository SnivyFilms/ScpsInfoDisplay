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
                    foreach (var player in Player.List.Where(p => p != null && (ScpsInfoDisplay.Singleton.Config.DisplayStrings.ContainsKey(p.Role.Type) || ScpsInfoDisplay.Singleton.Config.CustomRolesIntegrations.Keys.Any(key => CustomRole.Registered.Any(r => r.Name == key)))))
                    {
                        var builder = StringBuilderPool.Shared.Rent($"<align={ScpsInfoDisplay.Singleton.Config.TextAlignment.ToString().ToLower()}>");

                        foreach (var integration in ScpsInfoDisplay.Singleton.Config.CustomRolesIntegrations)
                        {
                            builder.Append(Player.List.Where(p => p?.SessionVariables.ContainsKey(integration.Key) == true).Aggregate(builder.ToString(), (current, any) => current + (player == any ? ScpsInfoDisplay.Singleton.Config.PlayersMarker : "") + ProcessStringVariables(integration.Value, player, any))).Append('\n');
                        }

                        foreach (var scp in Player.List.Where(p => p?.Role.Team == Team.SCPs && ScpsInfoDisplay.Singleton.Config.DisplayStrings.ContainsKey(p.Role.Type)))
                        {
                            builder.Append((scp == player ? ScpsInfoDisplay.Singleton.Config.PlayersMarker : "") + ProcessStringVariables(ScpsInfoDisplay.Singleton.Config.DisplayStrings[scp.Role.Type], player, scp)).Append('\n');
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
            .Replace("%096state%", target.Role.Is(out Exiled.API.Features.Roles.Scp096Role scp096) ? (scp096.RageState == Scp096RageState.Docile ? "Calm" : (scp096.RageState == Scp096RageState.Distressed ? "Enraging" : (scp096.RageState == Scp096RageState.Enraged ? "Enraged" : "Calming"))) : "")
            .Replace("%096targets%", target.Role.Is(out Exiled.API.Features.Roles.Scp096Role _) ? scp096.Targets.Count.ToString() : "")
            .Replace("%173stared%", target.Role.Is(out Scp173Role scp173) ? (scp173.IsObserved ? "ⓞ" : "-") : "")
            .Replace("%playername%", target.Nickname);

        private string SkeletonDisguiseNames(RoleTypeId disguise)
        {
            if (ScpsInfoDisplay.Singleton.Config.Scp3114DisguiseDisplay.TryGetValue(disguise, out var displayName))
            {
                return displayName;
            }

            return disguise.ToString();
        }
    }
}