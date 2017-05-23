using UnityEngine;
using System.Collections;
using cfg;

public class CfgLoader  {
    public static void LoadForEditor()
    {
        //TerrainBrickCfg.load((Resources.Load("cfg/terrainBrick") as TextAsset).bytes);
        //StaticDoodadCfg.load((Resources.Load("cfg/staticDoodad") as TextAsset).bytes  );
        //PrefabCfg.load((Resources.Load("cfg/prefab") as TextAsset).bytes );
        //PrefabTypeCfg.load((Resources.Load("cfg/prefabType") as TextAsset).bytes );
        //AudioCfg.load((Resources.Load("cfg/audio") as TextAsset).bytes  );
    }
    public static IEnumerator Load ()
    {
        
        ResourceLoadTask task=new ResourceLoadTask();
        task.path = "cfg";
        task.name = "tile.csv";
        yield return ResourceLoader.LoadAssetAsync(task);
        TileCfg.load((task.asset as TextAsset).bytes);

        task.name = "resources.csv";
        yield return ResourceLoader.LoadAssetAsync(task);
        ResourcesCfg.load((task.asset as TextAsset).bytes);

        task.name = "prefab.csv";
        yield return ResourceLoader.LoadAssetAsync(task);
        PrefabCfg.load((task.asset as TextAsset).bytes);

        task.name = "prefabType.csv";
        yield return ResourceLoader.LoadAssetAsync(task);
        PrefabTypeCfg.load((task.asset as TextAsset).bytes);

        task.name = "audio.csv";
        yield return ResourceLoader.LoadAssetAsync(task);
        AudioCfg.load((task.asset as TextAsset).bytes);

        task.name = "frameAnimation.csv";
        yield return ResourceLoader.LoadAssetAsync(task);
        FrameAnimationCfg.load((task.asset as TextAsset).bytes);
        task.name = "characterSprite.csv";
        yield return ResourceLoader.LoadAssetAsync(task);
        CharacterSpriteCfg.load((task.asset as TextAsset).bytes);
        task.name = "unitType.csv";
        yield return ResourceLoader.LoadAssetAsync(task);
        UnitTypeCfg.load((task.asset as TextAsset).bytes);

    }
}
