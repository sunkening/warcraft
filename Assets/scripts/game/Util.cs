using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util  {

    /*----------------------------------------------------------------------------
--  Random
----------------------------------------------------------------------------*/

    public static uint SyncRandSeed;               /// sync random seed value.

    /**
    **  Inititalize sync rand seed.
*/
    public static void InitSyncRand( )
    {
        SyncRandSeed = 0x87654321;
    }

    /**
    **  Synchronized random number.
    **
    **  @note This random value must be same on all machines in network game.
    **  Very simple random generations, enough for us.
*/
    public static int SyncRand( )
    {
        uint val;
        val = SyncRandSeed >> 16;
        SyncRandSeed = SyncRandSeed * (0x12345678 * 4 + 1) + 1;
        return (int)val;
    }

}
