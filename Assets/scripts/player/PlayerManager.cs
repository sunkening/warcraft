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
    /**
**  Init players.
*/
    public void InitPlayers( )
    {
        for (int p = 0; p < Consts.PlayerMax; ++p)
        {
            players[p].Index = p;
            if (!players[p].Type)
            {
                players[p].Type = PlayerNobody;
            }
            for (int x = 0; x < PlayerColorIndexCount; ++x)
            {
                PlayerColors[p][x] = Video.MapRGB(TheScreen->format,
                PlayerColorsRGB[p][x].r,
                PlayerColorsRGB[p][x].g, PlayerColorsRGB[p][x].b);
            }
        }
    }
}
