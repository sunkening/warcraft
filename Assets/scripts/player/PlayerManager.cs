using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerType
{
    PlayerNeutral = 2,        /// neutral
	PlayerNobody = 3,        /// unused slot
	PlayerComputer = 4,       /// computer player
	PlayerPerson = 5,         /// human player
	PlayerRescuePassive = 6,  /// rescued passive
	PlayerRescueActive = 7,   /// rescued  active
};
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
            if (!ThisPlayer)
            {
                ThisPlayer = player;
            }
            else
            {
                type = PlayerComputer;
            }
        }
        if (NetPlayers && NumPlayers == NetLocalPlayerNumber)
        {
            ThisPlayer = &Players[NetLocalPlayerNumber];
        }

        if (NumPlayers == PlayerMax)
        {
            static int already_warned;

            if (!already_warned)
            {
                DebugPrint("Too many players\n");
                already_warned = 1;
            }
            return;
        }

        //
        //  Make simple teams:
        //  All person players are enemies.
        //
        switch (type)
        {
            case PlayerNeutral:
            case PlayerNobody:
            default:
                team = 0;
                player->SetName("Neutral");
                break;
            case PlayerComputer:
                team = 1;
                player->SetName("Computer");
                break;
            case PlayerPerson:
                team = 2 + NumPlayers;
                player->SetName("Person");
                break;
            case PlayerRescuePassive:
            case PlayerRescueActive:
                // FIXME: correct for multiplayer games?
                player->SetName("Computer");
                team = 2 + NumPlayers;
                break;
        }
        DebugPrint("CreatePlayer name %s\n" _C_ player->Name.c_str());

        player->Type = type;
        player->Race = 0;
        player->Team = team;
        player->Enemy = 0;
        player->Allied = 0;
        player->AiName = "ai-passive";

        //
        //  Calculate enemy/allied mask.
        //
        for (i = 0; i < NumPlayers; ++i)
        {
            switch (type)
            {
                case PlayerNeutral:
                case PlayerNobody:
                default:
                    break;
                case PlayerComputer:
                    // Computer allied with computer and enemy of all persons.
                    if (Players[i].Type == PlayerComputer)
                    {
                        player->Allied |= (1 << i);
                        Players[i].Allied |= (1 << NumPlayers);
                    }
                    else if (Players[i].Type == PlayerPerson ||
                          Players[i].Type == PlayerRescueActive)
                    {
                        player->Enemy |= (1 << i);
                        Players[i].Enemy |= (1 << NumPlayers);
                    }
                    break;
                case PlayerPerson:
                    // Humans are enemy of all?
                    if (Players[i].Type == PlayerComputer ||
                            Players[i].Type == PlayerPerson)
                    {
                        player->Enemy |= (1 << i);
                        Players[i].Enemy |= (1 << NumPlayers);
                    }
                    else if (Players[i].Type == PlayerRescueActive ||
                          Players[i].Type == PlayerRescuePassive)
                    {
                        player->Allied |= (1 << i);
                        Players[i].Allied |= (1 << NumPlayers);
                    }
                    break;
                case PlayerRescuePassive:
                    // Rescue passive are allied with persons
                    if (Players[i].Type == PlayerPerson)
                    {
                        player->Allied |= (1 << i);
                        Players[i].Allied |= (1 << NumPlayers);
                    }
                    break;
                case PlayerRescueActive:
                    // Rescue active are allied with persons and enemies of computer
                    if (Players[i].Type == PlayerComputer)
                    {
                        player->Enemy |= (1 << i);
                        Players[i].Enemy |= (1 << NumPlayers);
                    }
                    else if (Players[i].Type == PlayerPerson)
                    {
                        player->Allied |= (1 << i);
                        Players[i].Allied |= (1 << NumPlayers);
                    }
                    break;
            }
        }

        //
        //  Initial default incomes.
        //
        for (i = 0; i < MaxCosts; ++i)
        {
            player->Incomes[i] = DefaultIncomes[i];
        }

        memset(player->UnitTypesCount, 0, sizeof(player->UnitTypesCount));

        player->Supply = 0;
        player->Demand = 0;
        player->NumBuildings = 0;
        player->TotalNumUnits = 0;
        player->Score = 0;

        player->Color = PlayerColors[NumPlayers][0];

        if (Players[NumPlayers].Type == PlayerComputer ||
                Players[NumPlayers].Type == PlayerRescueActive)
        {
            player->AiEnabled = 1;
        }
        else
        {
            player->AiEnabled = 0;
        }

        ++NumPlayers;
    }
}
