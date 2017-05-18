using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveReturn
{
    PF_FAILED = -3,       /// This Pathfinder failed, try another
	PF_UNREACHABLE = -2,  /// Unreachable stop
	PF_REACHED = -1,      /// Reached goal stop
	PF_WAIT = 0,          /// Wait, no time or blocked
	PF_MOVE = 1,          /// On the way moving
};
public class PathFinder   {
     
}
