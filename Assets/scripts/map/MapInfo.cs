using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo  {

    public string Description;     /// Map description
	public string Filename;        /// Map filename
	public int MapWidth;          /// Map width
	public int MapHeight;         /// Map height
	public int[] PlayerType=new int[Consts.PlayerMax];   /// Same player->Type
	public int[] PlayerSide=new int[Consts.PlayerMax];  /// Same player->Side
	//unsigned int MapUID;   /// Unique Map ID (hash)
}
