using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using cfg;
public class GameManager : MonoBehaviour
{
    public bool inited;

    public static int GameCycle;             /// Game simulation cycle counter
    public static int FastForwardCycle;      /// Cycle to fastforward to in a replay
    GameSettings gameSettings = new GameSettings();
    public string CurrentMapPath;
    public float progress;
  
    public static bool GameRunning;                    /// Current running state
    public static bool GamePaused;                     /// Current pause state
    public static bool GameObserve;                    /// Observe mode
    public static int SkipGameCycle = 0;
GameResult gameResult;
    // Use this for initialization


    /**
    **  CreateGame.
    **
    **  Load map, graphics, sounds, etc
    **
    **  @param filename  map filename
    **  @param map       map loaded
    **
    **  @todo FIXME: use in this function InitModules / LoadModules!!!
*/
    public void CreateGame(string filename, Map map)
    {


        //if (SaveGameLoading)
        //{
        //    SaveGameLoading = 0;
        //    Load game, already created game with Init/ LoadModules
        //    CommandLog(NULL, NoUnitP, FlushCommands, -1, -1, NoUnitP, NULL, -1);
        //    return;
        //}


        // InitVisionTable(); // build vision table for fog of war

        Main.playerManager.InitPlayers();

        //if (Map.Info.Filename.empty() && filename) {
        //	char path[PATH_MAX];


        //       Assert(filename);

        //       LibraryFileName(filename, path, sizeof(path));
        //	if(strcasestr(filename, ".smp")) {

        //           LuaLoadFile(path);
        //	}
        //}
        Map curmap = Main.mapManager.curMap;
        for (int i = 0; i < Consts.PlayerMax; ++i)
        {
            int playertype = curmap.Info.PlayerType[i];
            // Network games only:
            if (gameSettings.Presets[i].Type != GameSettings.SettingsPresetMapDefault)
            {
                playertype = gameSettings.Presets[i].Type;
            }

            Main.playerManager.CreatePlayer(playertype);
        }

        if (filename != null)
        {
            if (CurrentMapPath != filename)
            {
                CurrentMapPath = filename;
            }

            //
            // Load the map.
            //
            //InitUnitTypes(1);
            Main.mapManager.loadMap(filename);

        }

        GameCycle = 0;
        FastForwardCycle = 0;
        Main.actionManager.SyncHash = 0;
        Util.InitSyncRand();

/*

        if (IsNetworkGame())
        { // Prepare network play

            DebugPrint("Client setup: Calling InitNetwork2\n");

            InitNetwork2();
        }
        else
        {
            if (LocalPlayerName && strcmp(LocalPlayerName, "Anonymous"))
            {
                ThisPlayer->SetName(LocalPlayerName);
            }
        }


        CallbackMusicOn();

        if (FlagRevealMap)
        {
            Map.Reveal();
        }

        //
        // Setup game types
        //
        // FIXME: implement more game types
        if (GameSettings.GameType != SettingsGameTypeMapDefault)
        {
            switch (GameSettings.GameType)
            {
                case SettingsGameTypeMelee:
                    break;
                case SettingsGameTypeFreeForAll:

                    GameTypeFreeForAll();
                    break;
                case SettingsGameTypeTopVsBottom:

                    GameTypeTopVsBottom();
                    break;
                case SettingsGameTypeLeftVsRight:

                    GameTypeLeftVsRight();
                    break;
                case SettingsGameTypeManVsMachine:

                    GameTypeManVsMachine();
                    break;
                case SettingsGameTypeManTeamVsMachine:

                    GameTypeManTeamVsMachine();

                    // Future game type ideas

            }
        }
*/

        //
        // Graphic part
        //

//        LoadCursors(PlayerRaces.Name[ThisPlayer->Race]);
        Main.mouse.unitUnderCursor = null;


       // InitMissileTypes();
//# ifndef DYNAMIC_LOAD
 //       LoadMissileSprites();
//#endif
 //       InitConstructions();

 //       LoadConstructions();

       // LoadUnitTypes();

   //     LoadDecorations();


    //    InitSelections();


   //     InitUserInterface();
      //  UI.Load();

     //   UI.Minimap.Create();
     //   Map.Init();

      //  PreprocessMap();

        //
        // Sound part
        //
        //     LoadUnitSounds();

        //     MapUnitSounds();
        //      if (SoundEnabled())
        //     {

        //         InitSoundClient();
        //     }

        //
        // Spells
        //
        //     InitSpells();

        //
        // Init units' groups
        //
        //    InitGroups();

        //
        // Init players?
        //
        //     DebugPlayers();

        //      PlayersInitAi();

        //
        // Upgrades
        //
        //     InitUpgrades();

        //
        // Dependencies
        //
        //     InitDependencies();

        //
        // Buttons (botpanel)
        //
        //      InitButtons();

        //
        // Triggers
        //
        //    InitTriggers();


    //    UI.SelectedViewport->Center(
    //        ThisPlayer->StartX, ThisPlayer->StartY, TileSizeX / 2, TileSizeY / 2);

        //
        // Various hacks wich must be done after the map is loaded.
        //
        // FIXME: must be done after map is loaded
    //    InitAStar();
        //
        // FIXME: The palette is loaded after the units are created.
        // FIXME: This loops fixes the colors of the units.
        //
       /* for (i = 0; i < NumUnits; ++i)
        {
            // I don't really think that there can be any rescued
            // units at this point.
            if (Units[i]->RescuedFrom)
            {
                Units[i]->Colors = &Units[i]->RescuedFrom->UnitColors;
            }
            else
            {
                Units[i]->Colors = &Units[i]->Player->UnitColors;
            }
        }*/

       gameResult = GameResult.GameNoResult;


       // CommandLog(NULL, NoUnitP, FlushCommands, -1, -1, NoUnitP, NULL, -1);
       // Video.ClearScreen();
    }
}
