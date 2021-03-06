﻿using System.Collections;
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
public enum PlayerColor
{
    PlayerNeutral = 2,        /// neutral
	PlayerNobody = 3,        /// unused slot
	PlayerComputer = 4,       /// computer player
	PlayerPerson = 5,         /// human player
	PlayerRescuePassive = 6,  /// rescued passive
	PlayerRescueActive = 7,   /// rescued  active
};
public class Player   {
    public int Index;        /// player as number
	public string Name;   /// name of non computer
    public int color;
    public int Type;         /// type of player (human,computer,...)
	public int Race;         /// race of player (orc,human,...)
	string AiName; /// AI for computer

    // friend enemy detection
    public int Team;          /// team of player
	//unsigned Enemy;         /// enemy bit field for this player
	//unsigned Allied;        /// allied bit field for this player
	//unsigned SharedVision;  /// shared vision bit field

    int StartX;  /// map tile start X position
	int StartY;  /// map tile start Y position

    public int[] Resources=new int[(int)ResourceType.MaxCosts];      /// resources in store
	public int[] LastResources = new int[(int)ResourceType.MaxCosts];  /// last values for revenue
	public int[] Incomes = new int[(int)ResourceType.MaxCosts];        /// income of the resources
	public int[] Revenue = new int[(int)ResourceType.MaxCosts];        /// income rate of the resources

    // FIXME: shouldn't use the constant
    // int UnitTypesCount[UnitTypeMax];  /// total units of unit-type

    public bool AiEnabled;       /// handle AI on local computer
	//PlayerAi* Ai;          /// Ai structure pointer

    public  Unit[] units=new Unit[Consts.UnitMax]; /// units of this player
	public int NumUnits;  /// total # units for units' list
	public int NumBuildings;   /// # buildings
	public int Supply;         /// supply available/produced
	public int unitSpaceUsed;         /// demand of player当前已经使用的人口空间

    int UnitLimit;       /// # food units allowed
	int BuildingLimit;   /// # buildings allowed
	int TotalUnitLimit;  /// # total unit number allowed

    public int Score;           /// Points for killing ...
	public int TotalUnitsMade;//生产过的总数
    public int TotalBuildingsMade;
    //int TotalResources[ResourceType.MaxCosts];
    int TotalRazings;
    int TotalKills;      /// How many unit killed

    //   Uint32 Color;           /// color of units on minimap

    //   CUnitColors UnitColors; /// Unit colors for new units

    //   // Upgrades/Allows:
    //   CAllow Allow;                 /// Allowed for player
    //CUpgradeTimers UpgradeTimers; /// Timer for the upgrades
    /**
**  Clear all player data excepts members which don't change.
**
**  The fields that are not cleared are 
**  UnitLimit, BuildingLimit, TotalUnitLimit and Allow.
*/
    public void  Clear()
    {
        Index = 0;
        Name="";
        Type = 0;
        Race = 0;
        AiName="";
        Team = 0;
       // Enemy = 0;
      //  Allied = 0;
      //  SharedVision = 0;
        StartX = 0;
        StartY = 0;
        Resources = new int[(int)ResourceType.MaxCosts];      /// resources in store

       // memset(LastResources, 0, sizeof(LastResources));
       // memset(Incomes, 0, sizeof(Incomes));
       // memset(Revenue, 0, sizeof(Revenue));
      //  memset(UnitTypesCount, 0, sizeof(UnitTypesCount));
        AiEnabled = false;
        //  Ai = 0;
        units = new Unit[Consts.UnitMax];
        NumUnits = 0;
        NumBuildings = 0;
        Supply = 0;
        unitSpaceUsed = 0;
 
        Score = 0;
        TotalUnitsMade = 0;
        TotalBuildingsMade = 0;
        //  memset(TotalResources, 0, sizeof(TotalResources));
        TotalRazings = 0;
        TotalKills = 0;
      //  Color = 0;
      //  UpgradeTimers.Clear();
    }
}
