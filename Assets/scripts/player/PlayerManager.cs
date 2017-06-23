using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager   {
    public Player thisPlayer;
    public Player[] players = new Player[Consts.PlayerMax];
    public int NumPlayers;/// How many player slots used
    public int[] PlayerColors=new int[Consts.PlayerMax];
    public string[] PlayerColorNames=new string[Consts.PlayerMax];
    int NoRescueCheck;               /// Disable rescue check
    /**
**  Init players.
*/
    public void InitPlayers( )
    {
        for (int p = 0; p < Consts.PlayerMax; ++p)
        {
            players[p].Index = p;
            if (players[p].Type==0)
            {
                players[p].Type = (int)PlayerType.PlayerNobody;
            }
            //for (int x = 0; x < PlayerColorIndexCount; ++x)
            //{
            //    PlayerColors[p][x] = Video.MapRGB(TheScreen->format,
            //    PlayerColorsRGB[p][x].r,
            //    PlayerColorsRGB[p][x].g, PlayerColorsRGB[p][x].b);
            //}
        }
    }
    /**
**  Clean up players.
*/
    public void CleanPlayers( )
    {
        thisPlayer = null;
        for (int i = 0; i < Consts.PlayerMax; i++)
        {
            players[i].Clear();
        }
        NumPlayers = 0;

        NoRescueCheck = 0;
    }

    /**
    **  Create a new player.
    **
    **  @param type  Player type (Computer,Human,...).
*/
    public void CreatePlayer(int type)
    {
        int team;
        int i;
        Player  player;

        if (NumPlayers == Consts.PlayerMax)
        { // already done for bigmaps!
            return;
        }
        player = players[NumPlayers];
        player.Index = NumPlayers;

        //  Allocate memory for the "list" of this player's units.
        //  FIXME: brutal way, as we won't need UnitMax for this player...
        //  FIXME: ARI: is this needed for 'PlayerNobody' ??
        //  FIXME: A: Johns: currently we need no init for the nobody player.
        //memset(player->Units, 0, sizeof(player->Units));
        player.units = new Unit[Consts.UnitMax];

        //
        //  Take first slot for person on this computer,
        //  fill other with computer players.
        //
        if (type == (int)PlayerType.PlayerPerson && Main.netManager.NetPlayers==0)
        {
            if (thisPlayer==null)
            {
                thisPlayer = player;
            }
            else
            {
                type =(int) PlayerType.PlayerComputer;
            }
        }
        //if (Main.netManager.NetPlayers>0 && NumPlayers == NetLocalPlayerNumber)
        //{
        //    ThisPlayer = &Players[NetLocalPlayerNumber];
        //}

        if (NumPlayers == Consts.PlayerMax)
        {
            Debug.LogError("Too many players");
            return;
        }
        //
        //  Make simple teams:
        //  All person players are enemies.
        //
        switch ((PlayerType)type)
        {
            case PlayerType.PlayerNeutral:
            case PlayerType.PlayerNobody:
            default:
                team = 0;
                player.Name=  "Neutral";
                break;
            case PlayerType.PlayerComputer:
                team = 1;
                player.Name = "Computer";
                break;
            case PlayerType.PlayerPerson:
                team = 2 + NumPlayers;
                player.Name = "Person";
                break;
            case PlayerType.PlayerRescuePassive:
            case PlayerType.PlayerRescueActive:
                // FIXME: correct for multiplayer games?
                player.Name = "Computer";
                team = 2 + NumPlayers;
                break;
        }
        //DebugPrint("CreatePlayer name %s\n" _C_ player->Name.c_str());

        player.Type = type;
        player.Race = 0;
        player.Team = team;
        //player.Enemy = 0;
       // player.Allied = 0;
       // player.AiName = "ai-passive";

 

        //
        //  Initial default incomes.
        //
        //for (i = 0; i < MaxCosts; ++i)
        //{
        //    player->Incomes[i] = DefaultIncomes[i];
        //}

        //memset(player->UnitTypesCount, 0, sizeof(player->UnitTypesCount));

        player.Supply = 0;
        player.unitSpaceUsed = 0;
        player.NumBuildings = 0;
        player.NumUnits = 0;
        player.Score = 0;

       // player.color = PlayerColors[NumPlayers][0];

        if (player.Type == (int)PlayerType.PlayerComputer ||
                player.Type == (int)PlayerType.PlayerRescueActive)
        {
            player.AiEnabled = true;
        }
        else
        {
            player.AiEnabled = false;
        }

        ++NumPlayers;
    }
}
