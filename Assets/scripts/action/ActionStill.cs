﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ActionStill  {
    /**
  **  Unit stands still!
  **
  **  @param unit  Unit pointer for still action.
*/
    public static void HandleActionStill(Unit unit)
    {
        ActionStillGeneric(unit, false);
    }

    /**
    **  Unit stands still or stand ground.
    **
    **  @param unit          Unit pointer for action.
    **  @param stand_ground  true if unit is standing ground.
*/
    public static void ActionStillGeneric(Unit unit, bool stand_ground)
    {
        // If unit is not bunkered and removed, wait
        if (unit.Removed && (unit.Container==null))
            //||
              //  !unit.Container.unitType.canTransport ||
              //  !unit.Container.unitType.AttackFromTransporter ||
              //  unit->Type->Missile.Missile->Class == MissileClassNone))
        {
            // If unit is in building or transporter it is removed.
            return;
        }

        // Animations
        if (unit.subAction>0)
        { // attacking unit in attack range.
           // AnimateActionAttack(unit);
        }
        else
        {
            Main.actionManager.UnitShowAnimation(unit, unit.unitType.Animations.Still);
        }

       /* if (unit->Anim.Unbreakable)
        { // animation can't be aborted here
            return;
        }*/

    /*    if (MoveRandomly(unit) || AutoCast(unit) || AutoRepair(unit))
        {
            return;
        }

        AutoAttack(unit, stand_ground);*/
    }

}
