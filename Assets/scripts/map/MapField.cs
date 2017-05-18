using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapField
{
  

    public int tileCfgId = 0;      /// graphic tile number
	public int seenTile = 0;  /// last seen tile (FOW)
	public int Flags = 0;     /// field flags
	public int cost = 0;       /// unit cost to move in this tile
    // FIXME: Value can be removed, walls and regeneration can be handled
    //        different.
    public int Value = 0;                  /// HP for walls/ Wood Regeneration
	public int[] Visible;    /// Seen counter 0 unexplored
	public int[] VisCloak;    /// Visiblity for cloaking.
	public int[] Radar;       /// Visiblity for radar.
	public int[] RadarJammer; /// Jamming capabilities.
	List<Unit> UnitCache;

    /// A unit on the map field.
    public MapField()
    {
        Visible = new int[Consts.PlayerMax];
        VisCloak = new int[Consts.PlayerMax];
        Radar = new int[Consts.PlayerMax];
        RadarJammer = new int[Consts.PlayerMax];
    }
}
