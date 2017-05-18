using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class GameObjectResource
{
    public GameObjectResource(int id,GameObject g, GameObjectPool pool)
    {
        this.id = id;
        gameObject = g;
        this.pool = pool;
    }
   int id;
   GameObject gameObject;
    GameObjectPool pool;
    public void release()
    {
        pool.release(this);
    }

    public GameObject getGameObject()
    {
        return gameObject;
    }

    public int getId()
    {
        return id;
    }

    public GameObjectPool getPool()
    {
        return pool;
    }
}

public abstract class GameObjectFactory
{
    public virtual GameObject CreateGameObject()
    {
        return null;
    }

    public virtual IEnumerator CreateGameObjectAsync(LoaderResult r)
    {
        yield return   null;
    }
}
public class GameObjectPool
{
    public Transform fatherTransform;
    public GameObjectPool(GameObjectFactory f)
    {
        gameObjectFactory = f;
    }
    public GameObjectPool(GameObjectFactory f,Transform t)
    {
        gameObjectFactory = f;
        fatherTransform = t;
    }
    GameObjectFactory gameObjectFactory;
    private int id;
    private Dictionary<int, GameObjectResource> used = new Dictionary<int, GameObjectResource>();
    private Dictionary<int, GameObjectResource> idle = new Dictionary<int, GameObjectResource>();
    private int idCounter=0 ;
    private void add(GameObject gameObject)
    {
        if (fatherTransform)
        {
            gameObject.transform.parent = fatherTransform;
        }
        idCounter += 1;
        GameObjectResource r=new GameObjectResource(idCounter,gameObject,this);
        idle.Add(r.getId(), r);
    }

    public void release(GameObjectResource r)
    {
        if (r.getPool()!=this)
        {
            Debug.LogError("gameObjectPool must release gameObjectResource belong to it");
            return;
        }
        if (!used.ContainsKey(r.getId()))
        {
            
            return;
        }
        
        r.getGameObject().SetActive(false);
 
        used.Remove(r.getId());
        idle.Add(r.getId(),r);
    }

    public GameObjectResource get()
    {
        if (idle.Count<=0)
        {
           GameObject g= gameObjectFactory.CreateGameObject();
           add(g);
        }
        GameObjectResource r= idle.First().Value;
        r.getGameObject().SetActive(true);
        used.Add(r.getId(),r);
        idle.Remove(r.getId());
        return r;
    }
    public IEnumerator getAsync(LoaderResult result)
    {
        result.isDone = false;
        result.asset = null;
        if (idle.Count <= 0)
        {
            LoaderResult r=new LoaderResult();
            yield return gameObjectFactory.CreateGameObjectAsync(r);
            add(r.asset as GameObject);
        }
        GameObjectResource res = idle.First().Value;
        res.getGameObject().SetActive(true);
        used.Add(res.getId(), res);
        idle.Remove(res.getId());
        result.asset = res;
        result.isDone = true;
    }
}
