using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class   SettingsPresets
{
    public int Race;  /// Race of the player
	public int Team;  /// Team of player -- NOT SELECTABLE YET
	public int Type;  /// Type of player (for network games)
};
public class GameSettings   {
    public const int SettingsPresetMapDefault = -1; /// Special: Use map supplied
    /**
    **  Single or multiplayer settings
    */
    public const int SettingsSinglePlayerGame = 1;
    public const int SettingsMultiPlayerGame = 2;
    public int NetGameType;   /// Multiplayer or single player

    //  Individual presets:
    //  For single-player game only Presets[0] will be used..
    public SettingsPresets[]  Presets=new SettingsPresets[Consts.PlayerMax];

    //  Common settings:
    public int Resources;   /// Preset resource factor
    public int NumUnits;    /// Preset # of units
    public int Opponents;   /// Preset # of ai-opponents
    public int Difficulty;  /// Terrain type (summer,winter,...)
    public int GameType;    /// Game type (melee, free for all,...)
    public bool NoFogOfWar; /// No fog of war
    public int RevealMap;   /// Reveal map
    public int MapRichness; /// Map richness
    
}
