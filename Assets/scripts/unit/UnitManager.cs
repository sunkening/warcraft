using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cfg;
public class UnitManager   {
    

    //Unit* UnitSlots[MAX_UNIT_SLOTS];         /// All possible units
    // unsigned int UnitSlotFree;                /// First free unit slot
    // CUnit* ReleasedHead;                      /// List of released units.
    // CUnit* ReleasedTail;                      /// List tail of released units.
    public 
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
    public IEnumerator init()
    {
        Dictionary<int, List<Sprite>> id2SpriteAnim = new Dictionary<int, List<Sprite>>();
        foreach (SpriteCfg spriteCfg in SpriteCfg.dataList)
        {

            spriteCfg.
        }
        foreach (UnitTypeCfg unitTypeCfg in UnitTypeCfg.dataList)
        {

        }
        yield return 0;
    } 
}
