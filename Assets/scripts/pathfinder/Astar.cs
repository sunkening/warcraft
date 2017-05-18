using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
class Node
{
    public char Direction;     /// Direction for trace back
	public char InGoal;        /// is this point in the goal
	public int CostFromStart;  /// Real costs to reach this point
};
public class Open
{
    public int mapX;     /// X coordinate
    public int mapY;     /// Y coordinate
	public int mapIndex;     /// Offset into matrix
	public int costsToGoal; /// complete costs to goal
    public float costsFromSrc;
    public Open pre;
};
public class OpenIndexCompare : IComparer<Open>
{
    public int Compare(Open x, Open y)
    {
        return x.mapIndex-y.mapIndex;
    }
}
public class Astar
{
    static int OpenSetMaxSize=1000;
    static int[ ] map = new int[10000] ;
    static int mapWidth=100;
    static int mapHeight=100;
    int openListCount;
    List<Open> openList = new List<Open>();
    //List<Open> closeSet = new List<Open>();
    Dictionary<int,Open> closeMap=new Dictionary<int, Open>();
    List<int> pathList = new List<int>();
    //  Convert heading into direction.
    //                 //  N NE  E   SE  S  SW   W  NW
    int[] Heading2X = { 0, +1, +1, +1, 0, -1, -1, -1, 0 };
     int[] Heading2Y  = { -1, -1, 0, +1, +1, +1, 0, -1, 0 };
      //int[][] XY2Heading = { {7,6,5},{0,0,4},{1,2,3}};
    /// heuristic cost fonction for a star
    // Other heuristic functions
    // #define AStarCosts(sx,sy,ex,ey) 0
    // #define AStarCosts(sx,sy,ex,ey) isqrt((abs(sx-ex)*abs(sx-ex))+(abs(sy-ey)*abs(sy-ey)))
    // #define AStarCosts(sx,sy,ex,ey) max(abs(sx-ex),abs(sy-ey))
    public void init()
    {
        for (int i = 0; i < mapWidth* mapHeight; i++)
        {
            map[i] = 0;
        }
        for (int i = 0; i < mapHeight - 1; i++)
        {
            map[i* mapWidth+ mapWidth-2] =8;
        }
        for (int i = 1; i < mapWidth - 1; i++)
        {
            map[(mapHeight - 2) * mapWidth + i] = 8;
        }
        // openList.Clear();
        //openList.Capacity = 100;
        closeMap.Clear();
        openListCount = 0;
    }

    
    public int costs(int sx, int sy, int ex, int ey)
    {
        return (Math.Abs(sx - ex) + Math.Abs(sy - ey));
    }
    /**
    ** Add a new node to the open set (and update the heap structure)
    ** Returns Pathfinder failed
    */
    private MoveReturn addNode(Open newNode)
    {
        int big_i, small_i;
        int midcost;
        int midi;

        if (openListCount + 1 >= OpenSetMaxSize)
        {
            Debug.LogError( "A* internal error: raise Open Set Max Size ,current value  "+ OpenSetMaxSize);
            return  MoveReturn.PF_FAILED;
        }
        // find where we should insert this node.
        big_i = 0;
        small_i = openListCount;
        // binary search where to insert the new node
        while (big_i < small_i)
        {
            midi = (small_i + big_i) >> 1;
            midcost = openList[midi].costsToGoal;
            if (newNode.costsToGoal > midcost)
            {
                small_i = midi;
            }
            else if (newNode.costsToGoal < midcost)
            {
                if (big_i == midi)
                {
                    big_i++;
                }
                else
                {
                    big_i = midi;
                }
            }
            else
            {
                big_i = midi;
                small_i = midi;
            }
        }
        //if (openList.Count > big_i)
        //{
        //    //memmove(&OpenSet[bigi + 1], &OpenSet[bigi], (OpenSetSize - bigi) * sizeof(Open));
        //}
        openList.Insert(big_i, newNode);
        openListCount++;
        return MoveReturn.PF_WAIT;
    }
    public void removeNode(Open node)
    {
        int index = openList.BinarySearch(node, new OpenIndexCompare());
        if (index>=0)
        {
            openList.RemoveAt(index);
        }
    }
    public int findMinimum()
    {
        return openListCount - 1;
    }

    public int findNode(int mapIndex)
    {
        //在100*100的地图里，openlistCount值不会太大，极端情况下200左右
        for (int i = 0; i < openListCount; i++)
        {
            if (openList[i].mapIndex==mapIndex)
            {
                return i;
            }
        }
       return -1;
    }
    //public int AStarFindPath(Unit unit, int gx, int gy, int gw, int gh, int minrange, int maxrange, char path)
    //{

    //}
   

    public bool isInClose(int mapIndex)
    {
        return closeMap.ContainsKey(mapIndex);
    }

    public float total = 0;
    public void findPath(int sx,int sy,int dx,int dy)
    {
        int dIndex = dy*mapWidth + dx;
        Open start=new Open();
        start.mapX = sx;
        start.mapY = sy;
        start.mapIndex = sy*mapWidth + sx;
        start.costsToGoal = costs(sx, sy, dx, dy);
        openList.Add(start);
        openListCount++;
        Open end = null;
        int max = openListCount;
        while (openListCount > 0)
        {
            if (openListCount>max)
            {
                max = openListCount;

            }
            int min = findMinimum();
            Open curOpen= openList[min];
            openList.RemoveAt(min);
            openListCount--;
            //Debug.Log(" curopen " + curOpen.mapX + "  " + curOpen.mapY+" " +curOpen.costsToGoal);
            if (curOpen.mapIndex==dIndex)
            {
                end = curOpen;
                break;
            }
            closeMap.Add(curOpen.mapIndex,curOpen );
            
            for (int i = 0; i < 8; i++)
            {
                int around_x = curOpen.mapX + Heading2X[i];
                int around_y = curOpen.mapY + Heading2Y[i];
                if (around_x<0||around_x>=mapWidth||around_y<0||around_y>=mapHeight)
                {
                    continue;
                }
                int around_index = around_y * mapWidth + around_x;
                if (map[around_index]==8)
                {
                    continue;
                }
                map[around_index] = 2;
                
                if (isInClose(around_index))
                {
                    continue;
                }
                
                float t1 = Time.realtimeSinceStartup;
                int openIndex= findNode(around_index);
                float t2 = Time.realtimeSinceStartup;
                total += t2 - t1;
                if (openIndex < 0)
                {
                    Open o=new Open();
                    o.mapX = around_x;
                    o.mapY = around_y;
                    o.mapIndex = around_index;
                    o.costsToGoal = costs(o.mapX,o.mapY,dx,dy);
                    if (i%2 == 0)
                    {
                        o.costsFromSrc = curOpen.costsFromSrc + 1;
                    }
                    else
                    {
                        o.costsFromSrc = curOpen.costsFromSrc + 1.414f;
                    }
                    o.pre = curOpen;
                   
                    addNode(o);
                   
                }
                else
                {
                    if (curOpen.costsFromSrc+1<openList[openIndex].costsFromSrc)
                    {
                        openList[openIndex].costsFromSrc = curOpen.costsFromSrc + 1;
                        openList[openIndex].pre = curOpen;
                    }
                }
            }
          
        }
        if (end!=null)
        {
            Open c = end;
            while (c!=null)
            {
                map[c.mapIndex] = 1;
                c = c.pre;
            }
        }
        else
        {
            Debug.Log("path find fail");
        }
       Debug.Log(max);
    }

    public void printMap()
    {
        string s = "";
        for (int i = 0; i < mapHeight; i++)
        {
            int startIndex = i*mapWidth;
            
            for (int j = startIndex; j < startIndex+ mapWidth; j++)
            {
                if (map[j]==0)
                {
                    s +=  "_";
                }
                else
                {
                    s += map[j]+"";
                }
                
            }
            s += "\n";
           
        }
        Debug.Log(s);
    }
}
