# ScpsInfoDisplay (EXILED) ![Downloads](https://img.shields.io/github/downloads/SnivyFilms/ScpsInfoDisplay/total.svg)
When SCP spawns, displays information about players who are in the SCP team. Possible integration with custom roles.

## How to install plugin?
Put ScpsInfoDisplay.dll under the release tab into %appdata%\EXILED\Plugins (Windows) or .config/EXILED/Plugins (Linux) folder.

## Default configs
```yaml
scpsinfodisplay:
# Is the plugin enabled?
  is_enabled: true
  # Display strings. Format: Role, display string.
  display_strings:
    Scp049: '<color=#FF0000><size=30>SCP-049</color> [<color=#19FF40>%health%</color>, <color=#19B2FF>%arhealth%</color>]\n<color=#FF19FF>Zombies: %zombies%</color></size>'
    Scp079: '<color=#FF0000><size=30>SCP-079</color> <color=#19FF40>Generators:</color> [<color=#19FF40>%generators%</color><color=#FF0000>%engaging%</color><color=#19FF40>/3</color>]\n <color=#19B2FF>Level: %079level%</color> <color=#FF19FF>Energy: %079energy%</color></size>'
    Scp096: '<color=#FF0000><size=30>SCP-096</color> [<color=#19FF40>%health%</color>, <color=#19B2FF>%arhealth%</color>] <color=#FF19FF>\nRage State: %096state% </size>'
    Scp106: '<color=#FF0000><size=30>SCP-106</color> [<color=#19FF40>%health%</color>, <color=#19B2FF>%arhealth%</color>] <color=#FF19FF>\nVigor: %106vigor%% </size></color>'
    Scp173: <color=#FF0000><size=30>SCP-173</color> [<color=#19FF40>%health%</color>, <color=#19B2FF>%arhealth%</color>, <color=#FF19FF>%173stared%</color>] </size>
    Scp939: <color=#FF0000><size=30>SCP-939</color> [<color=#19FF40>%health%</color>, <color=#19B2FF>%arhealth%</color>] </size>
    Scp3114: <color=#FF0000><size=30>SCP-3114</color> [<color=#19FF40>%health%</color>, <color=#19B2FF>%arhealth%</color>, <color=#FF19FF>%3114disguisetype%</color>]
  # Custom roles integrations. Format: SessionVariable that marks that the player belongs to that role, display string.
  custom_roles_integrations: {}
  # Display string alignment. Possible values: left, center, right.
  text_alignment: right
  # Display text position offset.
  text_position_offset: 30
  # The player seeing the list will be highlighted with the special marker to the left. Leave it empty if disabled.
  players_marker: '<color=#D7E607><size=30>You: </size></color>'
  # Display debug messages in server console?
  debug: false
```
