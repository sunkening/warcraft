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
    public Dictionary<int, UnitType> id2UnitType = new Dictionary<int, UnitType>();
    public GameObject characrerDrawerPrefab;
    

    public IEnumerator init()
    {
        PrefabCfg prefabcfg = PrefabCfg.get(1);
        ResourceLoadTask task = new ResourceLoadTask();
        task.path = prefabcfg.resourcePath;
        task.name = prefabcfg.resourceName;
        yield return ResourceLoader.LoadAssetAsync(task);
        if (task.asset==null) {
            Debug.LogError("load charactor drawer prefab erro");
        }

        characrerDrawerPrefab = task.asset as GameObject;
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
            CharacterSprite sprite = new CharacterSprite();
            
            for (int i=0;i<8;i++  )
            {
                int spriteAnimId = spriteCfg.runAnim[i];

                if (spriteAnimId==0)
                {
                    sprite.runAnim[i] = null;
                    continue;
                }
                if (!id2SpriteAnim.ContainsKey(spriteAnimId))
                {
                    SpriteAnimCfg spriteAnimCfg = SpriteAnimCfg.get(spriteAnimId);
                    List<Sprite> anim = new List<Sprite>();
                    ResourceLoadTaskGroup group = new ResourceLoadTaskGroup();
                    for (int j= spriteAnimCfg.nBegin;j<= spriteAnimCfg.nEnd;j++)
                    {
                        ResourceLoadTask task = new ResourceLoadTask();
                        task.path = spriteAnimCfg.resourcePath;
                        string resourceName = j + ".0.png";
                        task.name = resourceName;
                        group.addTask(task);
                    }
                    yield return ResourceLoader.LoadGroupAsync(group);
                    for (int j = 0; j <= spriteAnimCfg.nEnd- spriteAnimCfg.nBegin; j++)
                    {
                        Texture2D texture2d = group.getTaskList()[j].asset as Texture2D;
                        Sprite s = Sprite.Create(texture2d, 
                            new Rect(0,0,texture2d.width,texture2d.height),
                            new Vector2(0.5f, 0),
                            spriteAnimCfg.fixelsPerUnit
                            );

                        anim.Add(s);
                    }
                    id2SpriteAnim[spriteAnimId] = anim;

                }
                
                sprite.runAnim[i] = id2SpriteAnim[spriteAnimId];
                
            }
            id2CharacterSprite[spriteCfgId] = sprite;
        }
        UnitType unitType = new UnitType();
        
        unitType.sprite = id2CharacterSprite[spriteCfgId];
        result.isDone = true;
        result.asset = unitType;
        id2UnitType[unitTypeId] = unitType;
        yield break;
    }
}
