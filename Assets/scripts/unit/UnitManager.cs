using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cfg;
public class UnitManager   {
    

    //Unit* UnitSlots[MAX_UNIT_SLOTS];         /// All possible units
    // unsigned int UnitSlotFree;                /// First free unit slot
    // CUnit* ReleasedHead;                      /// List of released units.
    // CUnit* ReleasedTail;                      /// List tail of released units.
      
    public   Unit[] AllUnits=new Unit[Consts.MAX_UNIT_NUM];             /// Array of used slots
    public   int NumUnits;                             /// Number of slots used
    public   UnitTypeVar unitTypeVar;
    int XpDamage;                             /// Hit point regeneration for all units
    bool EnableTrainingQueue;                 /// Config: training queues enabled
    bool EnableBuildingCapture;               /// Config: capture buildings enabled
    bool RevealAttacker;                      /// Config: reveal attacker enabled

    long HelpMeLastCycle;     /// Last cycle HelpMe sound played
    int HelpMeLastX;                   /// Last X coordinate HelpMe sound played
    int HelpMeLastY;                   /// Last Y coordinate HelpMe sound played
    public   void addUnit(Unit u)
    {
        u.id = NumUnits;
        AllUnits[NumUnits] = u;
        NumUnits++;
    }
    Dictionary<int, List<Sprite>> id2SpriteAnim = new Dictionary<int, List<Sprite>>();
    Dictionary<int, CharacterSprite> id2CharacterSprite = new Dictionary<int, CharacterSprite>();
    Dictionary<int, UnitType> id2UnitType = new Dictionary<int, UnitType>();
    public IEnumerator init()
    {
       
        
        foreach (UnitTypeCfg unitTypeCfg in UnitTypeCfg.dataList)
        {

        }
        yield return 0;
    } 
    public IEnumerator loadUnitType(int unitTypeId,LoaderResult result)
    {
        if (id2UnitType.ContainsKey(unitTypeId))
        {
            result.isDone = true;
            result.asset = id2UnitType[unitTypeId];
            yield break;
        }
        UnitTypeCfg unitTypeCfg = UnitTypeCfg.get(unitTypeId);
        int spriteCfgId = unitTypeCfg.spritId;
        if (!id2CharacterSprite.ContainsKey(spriteCfgId))
        {
            SpriteCfg spriteCfg = SpriteCfg.get(spriteCfgId);
        }
        UnitType unitType = new UnitType();
        
        unitType.sprite = id2CharacterSprite[spriteCfgId];
        result.isDone = true;
        result.asset = unitType;
        id2UnitType[unitTypeId] = unitType;
        yield break;
    }
}
