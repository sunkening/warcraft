using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player   {
    public int Index;        /// player as number
	string Name;   /// name of non computer

    int Type;         /// type of player (human,computer,...)
	int Race;         /// race of player (orc,human,...)
	string AiName; /// AI for computer

    // friend enemy detection
    int Team;          /// team of player
	//unsigned Enemy;         /// enemy bit field for this player
	//unsigned Allied;        /// allied bit field for this player
	//unsigned SharedVision;  /// shared vision bit field

    int StartX;  /// map tile start X position
	int StartY;  /// map tile start Y position

 //   int Resources[(int)ResourceType.MaxCosts];      /// resources in store
//	int LastResources[ResourceType.MaxCosts];  /// last values for revenue
	//int Incomes[MaxCosts];        /// income of the resources
	//int Revenue[MaxCosts];        /// income rate of the resources

    // FIXME: shouldn't use the constant
   // int UnitTypesCount[UnitTypeMax];  /// total units of unit-type

    int AiEnabled;       /// handle AI on local computer
	//PlayerAi* Ai;          /// Ai structure pointer

    public  Unit[] units=new Unit[Consts.UnitMax]; /// units of this player
	public int NumUnits;  /// total # units for units' list
	public int NumBuildings;   /// # buildings
	public int Supply;         /// supply available/produced
	public int Demand;         /// demand of player

    int UnitLimit;       /// # food units allowed
	int BuildingLimit;   /// # buildings allowed
	int TotalUnitLimit;  /// # total unit number allowed

    int Score;           /// Points for killing ...
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
}
