using System.Collections;
using System.Collections.Generic;
using cfg;
using UnityEngine;

public class Map
{
    public static int TileLength = 64;
    //                              // NE  SE  SW  NW
    public static int[] TileAroundX = { 0, 0, -1, -1 };
    public static int[] TileAroundY = { 0, -1, -1, 0 };
    public static int[] Tile2MapFieldX = { 1, 1, 0, 0 };
    public static int[] Tile2MapFieldY = { 1, 0, 0, 1 };
    public static int[] TileFlags = { TilesetFlagTopRight, TilesetFlagBottomRight, TilesetFlagBottomLeft, TilesetFlagTopLeft };
    public const int TilesetFlagTopLeft = 8; ///  
    public const int TilesetFlagTopRight = 4; ///  
    public const int TilesetFlagBottomLeft = 2; ///  
    public const int TilesetFlagBottomRight = 1; /// 
    public const int MapTileRow = 4;
    public const int MaxMapWidth = 256;  /// max map width supported
    public const int MaxMapHeight = 256;  /// max map height supported
    public const int TileSizeX = 1;              /// Size of a tile in X
	public const int TileSizeY = 1;              /// Size of a tile in Y
          // Not used until now:
    public const int MapFieldSpeedMask = 0x0007; /// Move faster on this tile

    public const int MapFieldHuman = 0x0008; /// Human is owner of the field (walls)

    public const int MapFieldLandAllowed = 0x0010;  /// Land units allowed
    public const int MapFieldCoastAllowed = 0x0020; /// Coast (transporter) units allowed
    public const int MapFieldWaterAllowed = 0x0040; /// Water units allowed
    public const int MapFieldNoBuilding = 0x0080; /// No buildings allowed

    public const int MapFieldUnpassable = 0x0100; /// Field is movement blocked
    public const int MapFieldWall = 0x0200; /// Field contains wall
    public const int MapFieldRocks = 0x0400;  /// Field contains rocks
    public const int MapFieldForest = 0x0800;  /// Field contains forest

    public const int MapFieldLandUnit = 0x1000; /// Land unit on field
    public const int MapFieldAirUnit = 0x2000; /// Air unit on field
    public const int MapFieldSeaUnit = 0x4000; /// Water unit on field
    public const int MapFieldBuilding = 0x8000; /// Building on field

     
    public static Map Instance = new Map();
    public MapField[] fields; /// fields on map
    public List<MapTile> mapTiles = new List<MapTile>();
    public MapInfo Info;             /// descriptive information
    public bool NoFogOfWar;

    public GameObject mapTilePrefab;
    /// fog of war disabled
    //unsigned* Visible[PlayerMax];  /// visible bit-field

    //           _______________
    //           | mapinfo |         |
    //           |         |         |
    //           |         |         |
    //           |_______________
    // 地图宽度，以mapinfo为单位，寻路的单位
    public int width;
    public int height;
    // 地图宽度，以tile为单位， 是mapinfo的2倍
    public int widthInTiles;
    public int heightInTiles;
    //List< Tileset> tilesets= new List<Tileset>();
    Dictionary<int, Tileset> tilesets = new Dictionary<int, Tileset>();
    List<Texture2D> textures = new List<Texture2D>();

    /// tileset data
    //static CGraphic* FogGraphic;      /// graphic for fog of war
    public void release()
    {

    }

    public IEnumerator load()
    {
        ResourceLoadTaskGroup group = new ResourceLoadTaskGroup();

        foreach (TileCfg tileCfg in TileCfg.dataList)
        {
            ResourceLoadTask task = new ResourceLoadTask();
            task.path = tileCfg.resourcePath;
            task.name = tileCfg.resourceName;
            group.addTask(task);
        }
        yield return ResourceLoader.LoadGroupAsync(group);
        for (int i = 0; i < TileCfg.dataList.Count; i++)
        {
            ResourceLoadTask t = group.getTaskList()[i];
            textures.Add(t.asset as Texture2D);
            Tileset tileset = new Tileset();
            tileset.init(t.asset as Texture2D, TileLength);
            tilesets.Add(TileCfg.dataList[i].id, tileset);
        }
        ResourceLoadTask taskMapTilePrefab = new ResourceLoadTask();
        taskMapTilePrefab.path = PrefabCfg.get(2).resourcePath;
        taskMapTilePrefab.name = PrefabCfg.get(2).resourceName;
        yield return ResourceLoader.LoadAssetAsync(taskMapTilePrefab);
        mapTilePrefab = taskMapTilePrefab.asset as GameObject;
    }
    public void createMap(int width_in_tile, int height_in_tile, int tileCfgId)
    {
        width = width_in_tile*2;
        height = height_in_tile*2;
        widthInTiles = width_in_tile;
        heightInTiles = height_in_tile;
        fields = new MapField[width_in_tile * height_in_tile * 4];
        for (int i = 0; i < fields.Length; i++)
        {
            fields[i]=new MapField();

            fields[i].tileCfgId = tileCfgId;
        }
        mapTiles.Clear();
        for (int i = 0; i < width_in_tile; i++)
        {
            for (int j = 0; j < height_in_tile; j++)
            {
                GameObject tileGameObj = GameObject.Instantiate(mapTilePrefab);
                tileGameObj.SetActive(true);
                MapTile maptile = tileGameObj.AddComponent<MapTile>();
                tileGameObj.transform.position = new Vector3(j, 0, i);

                maptile.spriteRenderers = new List<SpriteRenderer>();
                maptile.spriteRenderers.Add(tileGameObj.transform.FindChild("Sprite0").GetComponent<SpriteRenderer>());
                maptile.spriteRenderers.Add(tileGameObj.transform.FindChild("Sprite1").GetComponent<SpriteRenderer>());
                maptile.spriteRenderers.Add(tileGameObj.transform.FindChild("Sprite2").GetComponent<SpriteRenderer>());
                maptile.spriteRenderers.Add(tileGameObj.transform.FindChild("Sprite3").GetComponent<SpriteRenderer>());
                maptile.spriteRenderers[0].sprite = tilesets[tileCfgId].getSprit(15);
                mapTiles.Add(maptile);
            }
        }
    }

    public void refreshTileSprite(int x, int y)
    {
        //Dictionary<int ,int > tileId2num=new Dictionary<int, int>();
        Dictionary<int, int> tileId2flag = new Dictionary<int, int>();
        for (int i = 0; i < 4; i++)
        {
            int mapFieldX = x * 2 + Tile2MapFieldX[i];
            int mapFieldY = y * 2 + Tile2MapFieldY[i];
            int mapFieldIndex = mapFieldY * width + mapFieldX;
            MapField field = fields[mapFieldIndex];
            //if (!tileId2num.ContainsKey(field.tileCfgId))
            //{
            //    tileId2num[field.tileCfgId] = 1;
            //}
            //else
            //{
            //    tileId2num[field.tileCfgId] += 1;
            //}
            if (!tileId2flag.ContainsKey(field.tileCfgId))
            {
                tileId2flag[field.tileCfgId] = TileFlags[i];
            }
            else
            {
                tileId2flag[field.tileCfgId] = tileId2flag[field.tileCfgId] + TileFlags[i];
            }
        }
        List<int> tileIdList = new List<int>();
        foreach (int key in tileId2flag.Keys)
        {
            TileCfg tileCfg=TileCfg.get(key);
            int insertIndex = 0;
            for (int i = 0; i < tileIdList.Count; i++)
            {
                float height   = TileCfg.get(tileIdList[i]).height;
                insertIndex = i;
                if (tileCfg.height<=height)
                {
                    break;
                }
                if (i == tileIdList.Count - 1)
                {
                    insertIndex++;
                }
            }
            tileIdList.Insert(insertIndex,key);
        }
        
        int tileIndex = y * widthInTiles + x;
        mapTiles[tileIndex].removeSprit();
        for (int i =0; i < tileIdList.Count; i++)
        {
            int tileCfgId = tileIdList[i];
            if (i==0)/// 最下层的贴图使用全图
            {
                mapTiles[tileIndex].spriteRenderers[i].sprite = tilesets[tileCfgId].getSprit(15);
            }
            else
            {
                int flag = tileId2flag[tileCfgId];
                mapTiles[tileIndex].spriteRenderers[i].sprite = tilesets[tileCfgId].getSprit(flag);
            }
        }
    }
    public void setTile(int x, int y, int tileCfgId)
    {
        //if (tilesetIndex<0||tilesetIndex>= tilesets.Count)
        //{
        //    Debug.LogError("Map.setTile tilesetIndex out of range ");
        //    return;
        //}
        if (x < 0 || y < 0 || x > widthInTiles || y > heightInTiles)
        {
            Debug.LogError("Map.setTile x y out of range ");
            return;
        }
        Tileset tileset = tilesets[tileCfgId];
        for (int i = 0; i < 4; i++)
        {

            int mapFieldX = x * 2 + TileAroundX[i];
            int mapFieldY = y * 2 + TileAroundY[i];
            if (mapFieldX < 0 || mapFieldY < 0 || mapFieldX > width || mapFieldY > height)
            {
                continue;
            }
            int mapFieldIndex = mapFieldY * width + mapFieldX;
            if (fields[mapFieldIndex].tileCfgId == tileCfgId)
            {
                continue;
            }
            fields[mapFieldIndex].tileCfgId = tileCfgId;
            refreshTileSprite(mapFieldX/2, mapFieldY/2) ;
        }
        //for (int i = 0; i < 4; i++)
        //{
        //    int tileX = x + TileAroundX[i];
        //    int tileY = y + TileAroundY[i];
        //    if (tileX < 0 || tileY < 0 || tileX > widthInTiles || tileY > heightInTiles)
        //    {
        //        continue;
        //    }

        //    int tileIndex = tileY * widthInTiles + tileX;
        //    if (mapTiles[tileIndex].)
        //    {

        //    }
        //}
    }
}
