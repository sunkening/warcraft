using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceInfo {
    //std::string FileWhenLoaded;     /// Change the graphic when the unit is loaded.
	//std::string FileWhenEmpty;      /// Change the graphic when the unit is empty.
	public bool HarvestFromOutside;    /// Unit harvests without entering the building.
	public bool WaitAtResource;        /// Cycles the unit waits while mining.
	public bool ResourceStep;          /// Resources the unit gains per mining cycle.
	public int ResourceCapacity;      /// Max amount of resources to carry.
	public bool WaitAtDepot;           /// Cycles the unit waits while returning.
	public int  ResourceId;            /// Id of the resource harvested. Redundant.
	public bool FinalResource;         /// Convert resource when delivered.
	public bool TerrainHarvester;      /// Unit will harvest terrain(wood only for now).
	public bool LoseResources;         /// The unit will lose it's resource when distracted.
    //  Runtime info:
    public PlayerColorSprite spriteWhenLoaded; /// The graphic corresponding to FileWhenLoaded.
	public PlayerColorSprite spriteWhenEmpty;  /// The graphic corresponding to FileWhenEmpty

}
