using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cfg;
public class UnitManager   {

    public Unit[] UnitSlots = new Unit[Consts.MAX_UNIT_SLOTS];        /// All possible units
    int UnitSlotFree;                /// First free unit slot
    public Unit ReleasedHead;//存放被释放的unit
    public Unit ReleasedTail;//

    public   Unit[] AllUnits=new Unit[Consts.MAX_UNIT_SLOTS];             /// Array of used slots
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
    //只保存所有unit用到的，不会加载frameAnimationCfg所有数据
    Dictionary<int, FrameAnimation> id2FrameAnimation = new Dictionary<int, FrameAnimation>();
    Dictionary<int, CharacterSprite> id2CharacterSprite = new Dictionary<int, CharacterSprite>();

    //加载所有的unitTypeCfg数据
    public Dictionary<int, UnitType> id2UnitType = new Dictionary<int, UnitType>();
    public GameObject characrerDrawerPrefab;

    public GameObjectPool unitDrawer;  
    public IEnumerator init()
    {
        InitUnitsMemory();
        unitDrawer = new GameObjectPool(new UnitDrawerFactory());
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
            CharacterSpriteCfg spriteCfg = CharacterSpriteCfg.get(spriteCfgId);
            CharacterSprite sprite = new CharacterSprite();
            sprite.animations[(int)UnitAnimation.Run] = new CharacterAnimation();
            for (int i=0;i<8;i++  )
            {
                int spriteAnimId = spriteCfg.runAnim[i];

                if (spriteAnimId==0)
                {
                    sprite.animations[(int)UnitAnimation.Run].anim[i] = null;
                    continue;
                }
                if (!id2FrameAnimation.ContainsKey(spriteAnimId))
                {
                    FrameAnimationCfg spriteAnimCfg = FrameAnimationCfg.get(spriteAnimId);
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
                    FrameAnimation frameanimation = new FrameAnimation();
                    frameanimation.frames = anim;
                    frameanimation.frameRate = spriteAnimCfg.frameRate;
                    id2FrameAnimation[spriteAnimId] = frameanimation;
                }

                sprite.animations[(int)UnitAnimation.Run].anim[i] = id2FrameAnimation[spriteAnimId].frames;
                
            }
            sprite.frameRate = spriteCfg.frameRate;
            id2CharacterSprite[spriteCfgId] = sprite;
        }
        UnitType unitType = new UnitType();
        
        unitType.sprite = id2CharacterSprite[spriteCfgId];
        result.isDone = true;
        result.asset = unitType;
        id2UnitType[unitTypeId] = unitType;
        yield break;
    }
    /**
**  Create a new unit.
**
**  @param type      Pointer to unit-type.
**  @param player    Pointer to owning player.
**
**  @return          Pointer to created unit.
*/
    public Unit createUnit(UnitType type, Player player)
    {
        Unit unit;

        unit = AllocUnit();
        if (unit == null)
        {
            return null;
        }
        GameObjectResource g = unitDrawer.get();
 
        unit.init(type, g.getGameObject().GetComponent<SpriteDrawer>());

        // Only Assign if a Player was specified
        if (player!=null)
        {
            unit.assignToPlayer(player);
        }

        return unit;
    }
    void InitUnitsMemory( )
    {
        // Initialize the "list" of free unit slots
        // memset(UnitSlots, 0, MAX_UNIT_SLOTS * sizeof(*UnitSlots));
        // UnitSlotFree = 0;
        AllUnits = new Unit[Consts.MAX_UNIT_SLOTS];
        ReleasedTail = ReleasedHead = null; // list of unfreed units.
        NumUnits = 0;
    }
    /**
**  Allocate Unit
**
**  Allocates memory for a new unit, It will recycle free slots
**
**  @return  Pointer to memory allocated for new unit, memory is zero'd
*/
    public Unit AllocUnit()
    {
        Unit unit;
        int slot;
        //
        // Game unit limit reached.
        //
        if (NumUnits >= Consts.UnitMax)
        {
            Debug.LogError("Over all unit limit reached " + Consts.UnitMax);
            return null;
        }

        //
        // Can use released unit?
        //
        if (ReleasedHead!=null &&  ReleasedHead.Refs <GameManager. GameCycle)
        {
            unit = ReleasedHead;
            ReleasedHead = unit.Next;
            if (ReleasedHead == null)
            { // last element
              //  DebugPrint("Released unit queue emptied\n");
                ReleasedTail = ReleasedHead = null ;
            }
           // DebugPrint("%lu:Release %p %d\n" _C_ GameCycle _C_ unit _C_ unit->Slot);
            slot = unit.slotIndex;
            //unit.init();
            unit = new Unit();
        }
        else
        {
            //
            // Allocate structure
            //
            if (Consts.MAX_UNIT_SLOTS <= UnitSlotFree)
            { // should not happen!
                Debug.LogError("Maximum of units reached");
                return null;
            }
            slot = UnitSlotFree;
            UnitSlotFree++;
            unit = new Unit();
        }
        unit.slotIndex = slot  ; // back index
        return unit;
    }

    //魔兽争霸源码中，此函数在CUnit::Release()
    public void releaseUnit(Unit unit)
    {
        Unit temp;

        // Assert(Type); // already free.
        // Assert(OrderCount == 1);
        //  Assert(!Orders[0]->Goal);
        // Must be removed before here
        // Assert(Removed);

        //
        // First release, remove from lists/tables.
        //
        if (!unit.Destroyed)
        {
            //  DebugPrint("First release %d\n" _C_ Slot);

            //
            // Are more references remaining?
            //
            unit.Destroyed = true; // mark as destroyed

            if (unit.Container != null)
            {
                //MapUnmarkUnitSight(this);
                unit.removeFromContainer();
            }

            //if (--Refs > 0)
            //{
            //    return;
            //}
        }

        // RefsAssert(!Refs);

        //
        // No more references remaining, but the network could have an order
        // on the way. We must wait a little time before we could free the
        // memory.
        //
        //
        // Remove the unit from the global units table.
        //
        // Assert(*UnitSlot == this);


        temp = AllUnits[-- NumUnits];
        temp.unitIndex = unit.unitIndex;
        AllUnits[unit.unitIndex] = temp;
        AllUnits[ NumUnits] = null;

        //temp = Units[--NumUnits];
        //temp->UnitSlot = UnitSlot;
        //*UnitSlot = temp;
        //Units[NumUnits] = NULL;

        if (ReleasedHead!=null)
        {
            ReleasedTail.Next = unit;
            ReleasedTail = unit;
        }
        else
        {
            ReleasedHead = ReleasedTail = unit;
        }
        unit.Next = null;

        unit.Refs = GameManager.GameCycle + (Consts. NetworkMaxLag << 1); // could be reuse after this time
        unit.unitType = null;  // for debugging.

        //for (std::vector<COrder*>::iterator order = Orders.begin(); order != Orders.end(); ++order)
        //{
        //    delete* order;
        //}
        unit.orders.Clear();
    }
}

public class UnitDrawerFactory : GameObjectFactory
{


    public override GameObject CreateGameObject()
    {
        GameObject d = GameObject.Instantiate(GameManager. unitManager.characrerDrawerPrefab);
        SpriteDrawer drawer = d.AddComponent<SpriteDrawer>();

        return d;
    }

    public override IEnumerator CreateGameObjectAsync(LoaderResult r)
    {
        GameObject d = GameObject.Instantiate(GameManager.unitManager.characrerDrawerPrefab);
        SpriteDrawer drawer = d.AddComponent<SpriteDrawer>();
        r.isDone = true;
        r.asset = d;
        yield return 0;
    }
}