using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitType
{
    public int id;
    public string name;
    public string file;
    public string shadowFile;
    public int drawLevel;
    public float width;
    public float height;
    public int numDirections;/// Number of directions the unit can face
    public bool isBuilding;
    public CharacterSprite sprite;
    public bool canCastSpell;
    public bool needFlip ;              /// Flip image when facing left
    public bool isRevealer;//什么类型的单位？
    public bool isHarvester;//是否农民
    int[] CanStore=new int[(int)ResourceType.MaxCosts];             /// Resources that we can store here.
	int GivesResource;                  /// The resource this unit gives.
	public ResourceInfo[] ResInfo=new ResourceInfo[(int)ResourceType.MaxCosts];    /// Resource information.
	//std::vector<CBuildRestriction*> BuildingRules;   /// Rules list for building a building.
	//SDL_Color NeutralMinimapColorRGB;   /// Minimap Color for Neutral Units.


    public int icon;
    public int Animations;
 

}
