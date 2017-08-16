using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ActionManager  {
    public int SyncHash; /// Hash calculated to find sync failures
    /**
**  Rotate a unit
**
**  @param unit    Unit to rotate
**  @param rotate  Number of frames to rotate (>0 clockwise, <0 counterclockwise)
*/
    static void UnitRotate(Unit unit, int rotate)
    {
      //  unit->Direction += rotate * 256 / unit->Type->NumDirections;
      //  UnitUpdateHeading(unit);
    }

    /**
    **  Show unit animation.
    **
    **  @param unit  Unit of the animation.
    **  @param anim  Animation script to handle.
    **
    **  @return      The flags of the current script step.
*/
    public  int UnitShowAnimation(Unit  unit,   Animation  anim) 
    {
	    return UnitShowAnimationScaled(unit, anim, 8);
    }

/**
**  Show unit animation.
**
**  @param unit  Unit of the animation.
**  @param anim  Animation script to handle.
**  @param scale scaling factor of the wait times in animation (8 means no scaling).
**
**  @return      The flags of the current script step.
*/
public int UnitShowAnimationScaled(Unit unit, Animation anim, int scale)
{
    int move;

    // Changing animations
    if (unit->Anim.CurrAnim != anim)
    {
        // Assert fails when transforming unit (upgrade-to).
        Assert(!unit->Anim.Unbreakable);
        unit->Anim.Anim = unit->Anim.CurrAnim = anim;
        unit->Anim.Wait = 0;
    }

    // Currently waiting
    if (unit->Anim.Wait)
    {
        --unit->Anim.Wait;
        if (!unit->Anim.Wait)
        {
            // Advance to next frame
            unit->Anim.Anim = unit->Anim.Anim->Next;
            if (!unit->Anim.Anim)
            {
                unit->Anim.Anim = unit->Anim.CurrAnim;
            }
        }
        return 0;
    }

    move = 0;
    while (!unit->Anim.Wait)
    {
        switch (unit->Anim.Anim->Type)
        {
            case AnimationFrame:
                unit->Frame = unit->Anim.Anim->D.Frame.Frame;
                UnitUpdateHeading(unit);
                break;
            case AnimationExactFrame:
                unit->Frame = unit->Anim.Anim->D.Frame.Frame;
                break;

            case AnimationWait:
                unit->Anim.Wait = unit->Anim.Anim->D.Wait.Wait << scale >> 8;
                if (unit->Variable[SLOW_INDEX].Value)
                { // unit is slowed down
                    unit->Anim.Wait <<= 1;
                }
                if (unit->Variable[HASTE_INDEX].Value && unit->Anim.Wait > 1)
                { // unit is accelerated
                    unit->Anim.Wait >>= 1;
                }
                if (unit->Anim.Wait <= 0)
                    unit->Anim.Wait = 1;
                break;
            case AnimationRandomWait:
                unit->Anim.Wait = unit->Anim.Anim->D.RandomWait.MinWait +
                    SyncRand() % (unit->Anim.Anim->D.RandomWait.MaxWait - unit->Anim.Anim->D.RandomWait.MinWait + 1);
                break;

            case AnimationSound:
                if (unit->IsVisible(ThisPlayer) || ReplayRevealMap)
                {
                    PlayUnitSound(unit, unit->Anim.Anim->D.Sound.Sound);
                }
                break;
            case AnimationRandomSound:
                if (unit->IsVisible(ThisPlayer) || ReplayRevealMap)
                {
                    int sound;
                    sound = SyncRand() % unit->Anim.Anim->D.RandomSound.NumSounds;
                    PlayUnitSound(unit, unit->Anim.Anim->D.RandomSound.Sound[sound]);
                }
                break;

            case AnimationAttack:
                if (unit->Orders[0]->Action == UnitActionSpellCast)
                {
                    if (unit->Orders[0]->Goal &&
                            !unit->Orders[0]->Goal->IsVisibleAsGoal(unit->Player))
                    {
                        unit->ReCast = 0;
                    }
                    else
                    {
                        unit->ReCast = SpellCast(unit, unit->Orders[0]->Arg1.Spell,
                            unit->Orders[0]->Goal, unit->Orders[0]->X, unit->Orders[0]->Y);
                    }
                }
                else
                {
                    FireMissile(unit);
                }
                unit->Variable[INVISIBLE_INDEX].Value = 0; // unit is invisible until attacks
                break;

            case AnimationRotate:
                UnitRotate(unit, unit->Anim.Anim->D.Rotate.Rotate);
                break;
            case AnimationRandomRotate:
                if ((SyncRand() >> 8) & 1)
                {
                    UnitRotate(unit, -unit->Anim.Anim->D.Rotate.Rotate);
                }
                else
                {
                    UnitRotate(unit, unit->Anim.Anim->D.Rotate.Rotate);
                }
                break;

            case AnimationMove:
                Assert(!move);
                move = unit->Anim.Anim->D.Move.Move;
                break;

            case AnimationUnbreakable:
                Assert(unit->Anim.Unbreakable ^ unit->Anim.Anim->D.Unbreakable.Begin);
                unit->Anim.Unbreakable = unit->Anim.Anim->D.Unbreakable.Begin;
                break;

            case AnimationNone:
            case AnimationLabel:
                break;

            case AnimationGoto:
                unit->Anim.Anim = unit->Anim.Anim->D.Goto.Goto;
                break;
            case AnimationRandomGoto:
                if (SyncRand() % 100 < unit->Anim.Anim->D.RandomGoto.Random)
                {
                    unit->Anim.Anim = unit->Anim.Anim->D.RandomGoto.Goto;
                }
                break;
        }

        if (!unit->Anim.Wait)
        {
            // Advance to next frame
            unit->Anim.Anim = unit->Anim.Anim->Next;
            if (!unit->Anim.Anim)
            {
                unit->Anim.Anim = unit->Anim.CurrAnim;
            }
        }
    }

    --unit->Anim.Wait;
    if (!unit->Anim.Wait)
    {
        // Advance to next frame
        unit->Anim.Anim = unit->Anim.Anim->Next;
        if (!unit->Anim.Anim)
        {
            unit->Anim.Anim = unit->Anim.CurrAnim;
        }
    }
    return move;
}

/**
**  Update the actions of all units each game cycle.
**
**  @todo  To improve the preformance use slots for waiting.
*/
public void UnitActions( )
    {
        
        Unit[]  table=new Unit[Consts.UnitMax];
        Unit unit;
        int blinkthiscycle;
        int buffsthiscycle;
        int regenthiscycle;
        int i;
        int tabsize;

        buffsthiscycle = regenthiscycle = blinkthiscycle =
           GameManager.GameCycle % Consts.CYCLES_PER_SECOND;
        Array.Copy(Main.unitManager.AllUnits,table, Main.unitManager.NumUnits);
        tabsize = Main.unitManager.NumUnits;

        //
        // Check for things that only happen every few cycles
        // (faster in their own loops.)
        //

        // 1) Blink flag.
        /*if (blinkthiscycle)
        {
            for (i = 0; i < tabsize; ++i)
            {
                if (table[i]->Destroyed)
                {
                    table[i--] = table[--tabsize];
                    continue;
                }
                if (table[i]->Blink)
                {
                    --table[i]->Blink;
                }
            }
        }
*/

        // 2) Buffs...
        if (buffsthiscycle==0)
        {
            for (i = 0; i < tabsize; ++i)
            {
                if (table[i].Destroyed)
                {
                    table[i--] = table[--tabsize];
                    continue;
                }
                HandleBuffs(table[i], Consts.CYCLES_PER_SECOND);
            }
        }

        // 3) Increase health mana, burn and stuff
        if (regenthiscycle==0)
        {
            for (i = 0; i < tabsize; ++i)
            {
                if (table[i].Destroyed)
                {
                    table[i--] = table[--tabsize];
                    continue;
                }
                HandleRegenerations(table[i]);
            }
        }

        //
        // Do all actions
        //
        for (i = 0; i < tabsize; ++i)
        {
            while (table[i].Destroyed)
            {
                table[i] = table[--tabsize];
            }
            unit = table[i];

            HandleUnitAction(unit);
/*

# ifdef DEBUG_LOG
            //
            // Dump the unit to find the network sync bugs.
            //
            {
                static FILE* logf;

                if (!logf)
                {
                    time_t now;
                    char buf[256];

                    sprintf(buf, "log_of_stratagus_%d.log", ThisPlayer->Index);
                    logf = fopen(buf, "wb");
                    if (!logf)
                    {
                        return;
                    }
                    fprintf(logf, ";;; Log file generated by Stratagus Version "
                            VERSION "\n");
                    time(&now);
                    fprintf(logf, ";;;\tDate: %s", ctime(&now));
                    fprintf(logf, ";;;\tMap: %s\n\n", Map.Info.Description.c_str());
                }

                fprintf(logf, "%lu: ", GameCycle);
                fprintf(logf, "%d %s S%d/%d-%d P%d Refs %d: %X %d,%d %d,%d\n",
                    UnitNumber(unit), unit->Type ? unit->Type->Ident.c_str() : "unit-killed",
                    unit->State, unit->SubAction,
                    !unit->Orders.empty() ? unit->Orders[0]->Action : -1,
                    unit->Player ? unit->Player->Index : -1, unit->Refs, SyncRandSeed,
                    unit->X, unit->Y, unit->IX, unit->IY);

#if 0
		SaveUnit(unit,logf);
#endif
                fflush(NULL);
            }
#endif*/
            //
            // Calculate some hash.
            //
            //SyncHash = (SyncHash << 5) | (SyncHash >> 27);
            //SyncHash ^= !unit->Orders.empty() ? unit->Orders[0]->Action << 18 : 0;
            //SyncHash ^= unit->State << 12;
            //SyncHash ^= unit->SubAction << 6;
            //SyncHash ^= unit->Refs << 3;
        }
    }

    /**
    **  Handle the action of an unit.
    **
    **  @param unit  Pointer to handled unit.
*/
    public   void HandleUnitAction(Unit unit)
    {
        int z;

        //
        // If current action is breakable proceed with next one.
        //
        if (!unit.anim.Unbreakable)
        {
            if (unit.criticalOrder.action != UnitAction.UnitActionStill)
            {
                HandleActionTable(unit.criticalOrder.action,unit);
                unit.criticalOrder.action = UnitAction.UnitActionStill;
            }

            //
            // o Look if we have a new order and old finished.
            // o Or the order queue should be flushed.
            //
            if (unit.orderCount > 1 && (unit.orders[0].action == UnitAction.UnitActionStill || unit.orderFlush>0))
            {
                if (unit.Removed)
                { // FIXME: johns I see this as an error
                    //DebugPrint("Flushing removed unit\n");
                    // This happens, if building with ALT+SHIFT.
                    return;
                }

                //
                // Release pending references.
                //
                if (unit.orders[0].goal!=null)
                {
                    // If mining decrease the active count on the resource.
                    if (unit.orders[0].action == UnitAction.UnitActionResource &&
                            unit.subAction == 60)
                    {
                        // FIXME: SUB_GATHER_RESOURCE ?
                    //    unit.orders[0].goal.data.Resource.Active--;
                        //Assert(unit->Orders[0]->Goal->Data.Resource.Active >= 0);
                    }
                    // Still shouldn't have a reference unless attacking
                  //  Assert(!(unit->Orders[0]->Action == UnitAction.UnitActionStill && !unit->SubAction));
                  //  unit.orders[0].goal.RefsDecrease();
                }
                /*if (unit->CurrentResource)
                {
                    if (unit->Type->ResInfo[unit->CurrentResource]->LoseResources &&
                        unit->ResourcesHeld < unit->Type->ResInfo[unit->CurrentResource]->ResourceCapacity)
                    {
                        unit->ResourcesHeld = 0;
                    }
                }*/

                //
                // Shift queue with structure assignment.
                //
                unit.orderCount--;
                unit.orderFlush = 0;
                unit.orders.RemoveAt(0);
                //
                // Note subaction 0 should reset.
                //
                unit.subAction = unit.actionState = 0;

                if (IsOnlySelected(unit))
                { // update display for new action
                    SelectedUnitChanged();
                }
            }
        }

        //
        // Select action. FIXME: should us function pointers in unit structure.
        //
        HandleActionTable[unit->Orders[0]->Action](unit);
    }
    public static void HandleActionTable(UnitAction action,Unit unit)
    {

    }
}
