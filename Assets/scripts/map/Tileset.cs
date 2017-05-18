using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TileType
{
    TileTypeUnknown,    /// Unknown tile type
	TileTypeWood,       /// Any wood tile
	TileTypeRock,       /// Any rock tile
	TileTypeCoast,      /// Any coast tile
	TileTypeHumanWall,  /// Any human wall tile
	TileTypeOrcWall,    /// Any orc wall tile
	TileTypeWater,      /// Any water tile
};

public class TileInfo
{
    TileType tileType=TileType.TileTypeUnknown;
    public int resourceId;
}
public class Tileset
{
    public string name;
    public int flag;
    public TileInfo[] tileInfoes;
    List<Sprite>  sprites;
    public void init(Texture2D texture2D,int tileLength)
    {
        int row = texture2D.height/tileLength;
        int col = texture2D.width/tileLength;
        if (row!=Map.MapTileRow||(col!=row&&col!=row*2))
        {
            Debug.LogError("map tile init error");
            return;
        }
        sprites=new List<Sprite>();
        //地图tile有两种， 4行8列，4行4列
        //先生成4行4列
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < row; j++)
            {
                //if (i == 0 && j == 0)
                //{
                //    continue;
                //}
                Sprite s= Sprite.Create(
                    texture2D,
                    new Rect(j * tileLength, (row-1-i) * tileLength, tileLength, tileLength),
                    new Vector2(0, 0),
                    tileLength);
                sprites.Add(s);
            }
        }
        //Sprite s0 = Sprite.Create(
        //           texture2D,
        //           new Rect(0, 0, tileLength, tileLength),
        //           new Vector2(0, 0),
        //           tileLength);
        //sprites.Add(s0);
        if (col == row) return;
        for (int i = 0; i < row; i++)
        {
            for (int j = row; j < col; j++)
            {
                Sprite s = Sprite.Create(
                    texture2D, 
                    new Rect(j * tileLength, (row - 1 - i) * tileLength, tileLength, tileLength), 
                    new Vector2(0, 0), 
                    tileLength);
                sprites.Add(s);
            }
        }
    }

    public Sprite getSprit(int flag)
    {
        if (flag == 0|| flag>15)
        {
            Debug.LogError("Tileset.getSprit() flag==0");
            return null;
        }
        if (flag>0&&flag<15)
        {
            return sprites[flag];
        }
        if (sprites.Count> 16)
        {
            int index=MathUtil.GetIntRandomNumber(15, 31);
            return sprites[index];
        }
        else
        {
            int index = MathUtil.GetIntRandomNumber(0, 1);
            if (index==1)
            {
                index = 15;
            }
            return sprites[index];
        }
    }
}
