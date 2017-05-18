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

  //  CUnit* Units[UnitMax]; /// units of this player
	int TotalNumUnits;  /// total # units for units' list
	int NumBuildings;   /// # buildings
	int Supply;         /// supply available/produced
	int Demand;         /// demand of player

    int UnitLimit;       /// # food units allowed
	int BuildingLimit;   /// # buildings allowed
	int TotalUnitLimit;  /// # total unit number allowed

    int Score;           /// Points for killing ...
	int TotalUnits;
    int TotalBuildings;
    //int TotalResources[ResourceType.MaxCosts];
    int TotalRazings;
    int TotalKills;      /// How many unit killed

 //   Uint32 Color;           /// color of units on minimap

 //   CUnitColors UnitColors; /// Unit colors for new units

 //   // Upgrades/Allows:
 //   CAllow Allow;                 /// Allowed for player
	//CUpgradeTimers UpgradeTimers; /// Timer for the upgrades
}
