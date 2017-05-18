using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public enum UnitState
{
    None,
    Built,
    Upgrade,
}
public enum UnitAction
{
    UnitActionNone,         /// No valid action

    UnitActionStill,        /// unit stand still, does nothing
	UnitActionStandGround,  /// unit stands ground
	UnitActionFollow,       /// unit follows units
	UnitActionMove,         /// unit moves to position/unit
	UnitActionAttack,       /// unit attacks position/unit
	UnitActionAttackGround, /// unit attacks ground
	UnitActionDie,          /// unit dies

    UnitActionSpellCast,    /// unit casts spell

    UnitActionTrain,        /// building is training
	UnitActionUpgradeTo,    /// building is upgrading itself
	UnitActionResearch,     /// building is researching spell
	UnitActionBuilt,      /// building is under construction

    // Compound actions
    UnitActionBoard,        /// unit entering transporter
	UnitActionUnload,       /// unit leaving transporter
	UnitActionPatrol,       /// unit paroling area
	UnitActionBuild,        /// unit builds building

    UnitActionRepair,       /// unit repairing
	UnitActionResource,     /// unit harvesting resources
	UnitActionReturnGoods,  /// unit returning any resource
	UnitActionTransformInto /// unit transform into type.
}

public enum UnitVoice
{
    VoiceSelected,          /// If selected
	VoiceAcknowledging,     /// Acknowledge command
	VoiceReady,             /// Command completed
	VoiceHelpMe,            /// If attacked
	VoiceDying,             /// If killed
	VoiceWorkCompleted,     /// only worker, work completed
	VoiceBuilding,          /// only for building under construction
	VoiceDocking,           /// only for transport reaching coast
	VoiceRepairing,         /// repairing
	VoiceHarvesting,        /// harvesting
}

public enum UnitDirection
{
    LookingN  ,      /// Unit looking north 
	LookingNE  ,     ///  Unit looking north east  
	LookingE  ,      ///  Unit looking east
	LookingSE ,      /// Unit looking south east
	LookingS  ,      /// Unit looking south
	LookingSW  ,      /// Unit looking south west
	LookingW  ,      /// Unit looking west
	LookingNW  ,      /// Unit looking north west
    Total,
}
public class Unit
{

    public const int NextDirection = 45; /// Next direction N->NE->E...
    public const int UnitNotSeen = 0x7fffffff;

    /// Unit not seen, used by CUnit::SeenFrame
    public int id;
    public int X; /// Map position X
	public int Y; /// Map position Y
    public float worldx;
    public float worldy;
    UnitColor Colors;    /// Player colors
	//public int IX;         /// X image displacement to map position
	//public int IY;         /// Y image displacement to map position

    public int insideUnitCount;   /// Number of units inside.
	public int BoardCount;    /// Number of units transported inside.
	public Unit UnitInside;    /// Pointer to one of the units inside.
	public Unit Container;     /// Pointer to the unit containing it (or 0)
	public Unit NextContained; /// Next unit in the container.
	public Unit PrevContained; /// Previous unit in the container.


    public UnitType unitType;              /// Pointer to unit-type (peon,...)
	public Player  Player;            /// Owner of this unit
    public Player rescuedFrom;        /// The original owner of a rescued unit.
	UnitStats Stats;             /// Current unit stats
	int CurrentSightRange; /// Unit's Current Sight Range

   
	//int Frame;      /// Image frame: <0 is mirrored

    int Direction  ; /// angle  unit looking

    float lastAttackedTime; /// gamecycle unit was last attacked
 
    public bool Burning  ;   /// unit is burning
	public bool Destroyed  ; /// unit is destroyed pending reference
	public bool Removed  ;   /// unit is removed (not on map)
	public bool Selected  ;  /// unit is selected
	public bool TeamSelected;  /// unit is selected by a team member.

    public bool constructed  ;    /// Unit is in construction 用于建筑，正在建造
	public bool Active  ;         /// Unit is active for AI
	public bool Boarded ;        /// Unit is on board a transporter.
	
                                /// NULL if the unit was not rescued.
    /* Seen stuff. */
    //int VisCount[PlayerMax];     /// Unit visibility counts
	public SeenStuff seen;

    //unsigned SubAction : 8; /// sub-action of unit
	public float waitTime;          /// action counter
	int actionState  ;     /// action state
	int Blink ;     /// Let selection rectangle blink
	public bool Moving  ;

    /// The unit is moving
    //unsigned ReCast : 1;    /// Recast again next cycle
    public UnitAnim anim;

    public int resourcesHeld;      /// Resources Held by a unit
	public ResourceType currentResourceType;

    int OrderCount;            /// how many orders in queue
	char OrderFlush;            /// cancel current order, take next
	public List<Order> orders; /// orders to process
        /**  This order is executed, if the current order is finished.
** This is used for attacking units, to return to the old
** place or for patrolling units to return to patrol after
** killing some enemies.Any new order given to the unit,
**  clears this saved order.*/
    Order savedOrder;           /// order to continue after current
	Order newOrder;             /// order for new trained units
	Order criticalOrder;        /// order to do as possible in breakable animation.
    //char* AutoCastSpell;        /// spells to auto cast
    //unsigned AutoRepair : 1;    /// True if unit tries to repair on still action.
    public int frameNum;
    public UnitDirection direction;
    public SpriteDrawer spriteDrawer;
    public void init(UnitType type)
    {
 
       GameManager. unitManager.addUnit(this);
        //
        //  Initialise unit structure (must be zero filled!)
        //
        unitType = type;

        seen.Frame = UnitNotSeen; // Unit isn't yet seen

       // Frame = type->StillFrame;

        //if (UnitManager.unitTypeVar.NumberVariable)
        //{
        //    Assert(!Variable);
        //    Variable = new CVariable[UnitTypeVar.NumberVariable];
        //    memcpy(Variable, Type->Variable,
        //        UnitTypeVar.NumberVariable * sizeof(*Variable));
        //}

        // Set a heading for the unit if it Handles Directions
        // Don't set a building heading, as only 1 construction direction
        //   is allowed.
        if (type.numDirections > 1 && type.sprite!=null && !type.isBuilding)
        {
            Direction = MathUtil.GetIntRandomNumber(0, 359);//(MyRand() >> 8) & 0xFF; // 0-225 random heading
            updateHeading( );
        }

        if (type.canCastSpell)
        {
            //AutoCastSpell = new char[SpellTypeTable.size()];
            //if (Type->AutoCastActive)
            //{
            //    memcpy(AutoCastSpell, Type->AutoCastActive, SpellTypeTable.size());
            //}
            //else
            //{
            //    memset(AutoCastSpell, 0, SpellTypeTable.size());
            //}
        }
        Active = true;

        Removed = true;

        //Assert(Orders.empty());

        orders.Add(new Order());

        OrderCount = 1; // No orders
        orders[0].action = UnitAction.UnitActionStill;
        orders[0].x = orders[0].y= -1;
      //  Assert(!Orders[0]->Goal);
        newOrder.action = UnitAction.UnitActionStill;
        newOrder.x = newOrder.y = -1;
        // Assert(!NewOrder.Goal);
        savedOrder.action = UnitAction.UnitActionStill;
        savedOrder.x = savedOrder.y = -1;
        // Assert(!SavedOrder.Goal);
        criticalOrder.action = UnitAction.UnitActionStill;
        criticalOrder.x = criticalOrder.y = -1;
       // Assert(!CriticalOrder.Goal);
    }
    public void updateHeading()
    {

    }
    /**
    **  Remove unit from a container. It only updates linked list stuff.
    **
    **  @param unit    Pointer to unit.
*/
    public  void removeFromContainer(Unit unit)
    {
        //CUnit* host;  // transporter which contain unit.

        //host = unit->Container;
        //Assert(unit->Container);
        //Assert(unit->Container->InsideCount > 0);
        //host->InsideCount--;
        //unit->NextContained->PrevContained = unit->PrevContained;
        //unit->PrevContained->NextContained = unit->NextContained;
        //if (host->InsideCount == 0)
        //{
        //    host->UnitInside = NoUnitP;
        //}
        //else
        //{
        //    if (host->UnitInside == unit)
        //    {
        //        host->UnitInside = unit->NextContained;
        //    }
        //}
        //unit->Container = NoUnitP;
    }
    /**
**  Add unit to a container. It only updates linked list stuff.
**
**  @param host  Pointer to container.
*/
    void addInContainer(Unit host)
    {
        //Assert(host && Container == 0);
        Container = host;
        if (host.insideUnitCount == 0)
        {
            NextContained = PrevContained = this;
        }
        else
        {
            NextContained = host.UnitInside;
            PrevContained = host.UnitInside.PrevContained;
            host.UnitInside.PrevContained.NextContained = this;
            host.UnitInside.PrevContained = this;
        }
        host.UnitInside = this;
        host.insideUnitCount++;
    }
    public bool isVisible(Player player)
    {
        return true;
    }
    public void draw()
    {
        float x;
        float y;
        int frame;
        UnitState state;
        bool constructed;
        PlayerColorSprite sprite;
        ResourceInfo resinfo;
       // CConstructionFrame* cframe;
        UnitType type;

        if (unitType.isRevealer)
        { // Revealers are not drawn
            return;
        }

        // Those should have been filtered. Check doesn't make sense with ReplayRevealMap
        //Assert(ReplayRevealMap || this->Type->VisibleUnderFog || this->IsVisible(ThisPlayer));

        if (MapManager.isReveal || isVisible(PlayerManager.thisPlayer))
        {
            type = unitType;
            frame = this.frameNum;
            y = worldy;
            x = worldx;
            //x += CurrentViewport->Map2ViewportX(this->X);
            //y += CurrentViewport->Map2ViewportY(this->Y);
            //state = (this->Orders[0]->Action == UnitActionBuilt) |
            //    ((this->Orders[0]->Action == UnitActionUpgradeTo) << 1);
            constructed = this.constructed;
            // Reset Type to the type being upgraded to
            if (this.orders[0].action==UnitAction.UnitActionUpgradeTo)
            {
                state = UnitState.Upgrade;
                type = this.orders[0].unitType;
            }else 
            if(this.orders[0].action == UnitAction.UnitActionBuilt)
            {
                state = UnitState.Built;
            }
            else
            {
                state = UnitState.None;
            }
            // This is trash unless the unit is being built, and that's when we use it.
          //  cframe = this->Data.Built.Frame;
        }
        else
        {
            x = this.seen.worldx;
            y = this.seen.worldy;

            frame = this.seen.Frame;
            type = this.seen.unitType;
            constructed = this.seen.constructed;
            state = this.seen.state;
           // cframe = this.seen.CFrame;
        }

//# ifdef DYNAMIC_LOAD
//        if (!type->Sprite)
//        {
//            LoadUnitTypeSprite(type);
//        }
//#endif

        if (!isVisible(PlayerManager.thisPlayer) && frame == UnitNotSeen)
        {
            //DebugPrint("FIXME: Something is wrong, unit %d not seen but drawn time %lu?.\n" _C_
            //    this->Slot _C_ GameCycle);
            Debug.Log("FIXME: Something is wrong, unit id "+ id+"not seen but drawn ");
            return;
        }


        //if (state ==UnitState.Built && constructed)
        //{
        //    DrawConstructionShadow(this, cframe, frame, x, y);
        //}
        //else
        //{
        //    DrawShadow(this, NULL, frame, x, y);
        //}

        ////
        //// Show that the unit is selected
        ////
        //DrawUnitSelection(this);

        //
        // Adjust sprite for Harvesters.
        //
        sprite = type.sprite;
        if (type.isHarvester && this.currentResourceType > 0)
        {
            resinfo = type.ResInfo[(int)this.currentResourceType];
            if (this.resourcesHeld>0)
            {
                if (resinfo.spriteWhenLoaded!=null)
                {
                    sprite = resinfo.spriteWhenLoaded;
                }
            }
            else
            {
                if (resinfo.spriteWhenEmpty!=null)
                {
                    sprite = resinfo.spriteWhenEmpty;
                }
            }
        }

        //
        // Now draw!
        // Buildings under construction/upgrade/ready.
        //
        if (state ==UnitState.Built)
        {
            if (constructed)
            {
                //DrawConstruction(this, cframe, type, frame,
                //    x + (type->TileWidth * TileSizeX) / 2,
                //    y + (type->TileHeight * TileSizeY) / 2);
            }
            //
            // Draw the future unit type, if upgrading to it.
            //
        }
        else if (state == UnitState.Upgrade)
        {
            //// FIXME: this frame is hardcoded!!!
            //DrawUnitType(type, sprite,
            //    this->RescuedFrom ? this->RescuedFrom->Index : this->Player->Index,
            //    frame < 0 ? -1 - 1 : 1, x, y);
        }
        else
        {
            this.spriteDrawer.drawUnitType(sprite, this.Player.Index, frame);
            //DrawUnitType(type, sprite,
            //    this.RescuedFrom ? this->RescuedFrom->Index : this->Player->Index,
            //    frame, x, y);
        }

        // Unit's extras not fully supported.. need to be decorations themselves.
        //DrawInformations(this, type, x, y);
    }
}

public class SeenStuff
{
    public int ByPlayer;    /// Track unit seen by player
    public int Frame;                   /// last seen frame/stage of buildings
    public  UnitType unitType;                    /// Pointer to last seen unit-type
    public int x;                       /// Last unit->X Seen
    public int y;                       /// Last unit->Y Seen
    public float worldx;
    public float worldy;
   // public int IX;                      /// Seen X image displacement to map position
   // public int IY;                      /// seen Y image displacement to map position
    public bool constructed;         /// Unit seen construction
    public UnitState state;               /// Unit seen build/upgrade state
    //unsigned Destroyed : PlayerMax;   /// Unit seen destroyed or not
    //CConstructionFrame* CFrame;                  /// Seen construction frame
}