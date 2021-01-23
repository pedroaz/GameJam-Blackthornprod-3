using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An enumeration of pooled object names
/// </summary>
public enum PooledObjectName
{
    PlayerBullet,
    EnemyBullet,
    Enemy
}

/// <summary>
/// Provides object pooling for bullets and enemies
/// </summary>
public class ObjectPool : MonoBehaviour
{
    static GameObject prefabEnemyBullet;
    static GameObject prefabPlayerBullet;
    static GameObject prefabEnemy;
    static Dictionary<PooledObjectName, List<GameObject>> pools;

    /// <summary>
    /// Initializes the pools
    /// </summary>
    public static void Initialize()
    {
        // Load prefabs
        prefabEnemyBullet = Resources.Load<GameObject>("Ships/EnemyBullet");
        prefabPlayerBullet = Resources.Load<GameObject>("Ships/PlayerBullet");
        prefabEnemy = Resources.Load<GameObject>("Ships/EnemyShip");

        // initialize dictionary
        pools = new Dictionary<PooledObjectName, List<GameObject>>();
        pools.Add(PooledObjectName.PlayerBullet, new List<GameObject>(GameConstants.InitialBulletPoolCapacity));
        pools.Add(PooledObjectName.EnemyBullet, new List<GameObject>(GameConstants.InitialBulletPoolCapacity));
        pools.Add(PooledObjectName.Enemy, new List<GameObject>(GameConstants.InitialEnemyPoolCapacity));

        // fill player bullet pool
        pools[PooledObjectName.PlayerBullet] = new List<GameObject>(GameConstants.InitialBulletPoolCapacity);
        for (int i = 0; i < pools[PooledObjectName.PlayerBullet].Capacity; i++)
        {
            pools[PooledObjectName.PlayerBullet].Add(GetNewObject(PooledObjectName.PlayerBullet));
        }

        // fill enemy bullet pool
        pools[PooledObjectName.EnemyBullet] = new List<GameObject>(GameConstants.InitialBulletPoolCapacity);
        for (int i = 0; i < pools[PooledObjectName.EnemyBullet].Capacity; i++)
        {
            pools[PooledObjectName.EnemyBullet].Add(GetNewObject(PooledObjectName.EnemyBullet));
        }

        // fill enemy pool
        pools[PooledObjectName.Enemy] = new List<GameObject>(GameConstants.InitialEnemyPoolCapacity);
        for (int i = 0; i < pools[PooledObjectName.Enemy].Capacity; i++)
        {
            pools[PooledObjectName.Enemy].Add(GetNewObject(PooledObjectName.Enemy));
        }
    }

    /// <summary>
    /// Gets a number of bullet objects from the player bullets pool
    /// </summary>
    /// <param name="number">Number of bullets desired</param>
    /// <returns>Bullets</returns>
    public static GameObject[] GetPlayerBullets(int number)
    {
        GameObject[] bulletReturn = new GameObject[number];
        for (int i = 0; i < number; i++)
        {
            bulletReturn[i] = GetPooledObject(PooledObjectName.PlayerBullet);
        }
        return bulletReturn;
    }

    /// <summary>
    /// Gets a number of bullet object from the enemy bullets pool
    /// </summary>
    /// <param name="number">Number of bullets desired</param>
    /// <returns>Bullets</returns>
    public static GameObject[] GetEnemyBullets(int number)
    {
        GameObject[] bulletReturn = new GameObject[number];
        for (int i = 0; i < number; i++)
        {
            bulletReturn[i] = GetPooledObject(PooledObjectName.EnemyBullet);
        }
        return bulletReturn;
    }

    /// <summary>
    /// Gets an enemy object from the pool
    /// </summary>
    /// <returns>enemy</returns>
    public static GameObject GetEnemy()
    {
        return GetPooledObject(PooledObjectName.Enemy);
    }

    /// <summary>
    /// Gets a pooled object from the pool
    /// </summary>
    /// <returns>pooled object</returns>
    /// <param name="name">name of the pooled object to get</param>
    static GameObject GetPooledObject(PooledObjectName name)
    {
        List<GameObject> pool = pools[name];

        // Check for available object in pool
        if (pool.Count > 0 && !pools[name][pools[name].Count - 1].activeInHierarchy)
        {
            //Return the last enemy in the list if any is available
            GameObject obj = pools[name][pools[name].Count - 1];
            pools[name].RemoveAt(pools[name].Count - 1);
            return obj;
        }
        else
        {
            // Pool empty, so expand pool and return new object (replace code below)

            // Expand the pool and return a new object if none available, the object will be
            // eadded to the pool when not in use anymroe
            pools[name].Capacity++;
            return GetNewObject(name);
        }
    }

    /// <summary>
    /// Returns a bullet object to the bullet pool
    /// </summary>
    /// <param name="bullet">bullet</param>
    public static void ReturnBullet(GameObject bullet)
    {
        if (bullet.tag == "PlayerBullet")
        {
            ReturnPooledObject(PooledObjectName.PlayerBullet, bullet);
        }
        else if (bullet.tag == "EnemyBullet")
        {
            ReturnPooledObject(PooledObjectName.EnemyBullet, bullet);
        }
    }

    /// <summary>
    /// Returns an enemy object to the pool
    /// </summary>
    /// <param name="enemy">enemy</param>
    public static void ReturnEnemy(GameObject enemy)
    {
        ReturnPooledObject(PooledObjectName.Enemy, enemy);
    }

    /// <summary>
    /// Returns a pooled object to the pool
    /// </summary>
    /// <param name="name">name of pooled object</param>
    /// <param name="obj">object to return to pool</param>
    public static void ReturnPooledObject(PooledObjectName name, GameObject obj)
    {
        switch (name)
        {
            case PooledObjectName.PlayerBullet:
            case PooledObjectName.EnemyBullet:
                obj.GetComponent<Bullet>().StopMoving();
                break;
            case PooledObjectName.Enemy:
                obj.GetComponent<Enemy>().Deactivate();
                break;
            default: break;
        }

        obj.SetActive(false);
        pools[name].Add(obj);
    }

    /// <summary>
    /// Gets a new object
    /// </summary>
    /// <returns>new object</returns>
    static GameObject GetNewObject(PooledObjectName name)
    {
        GameObject obj = null;
        switch (name)
        {
            case PooledObjectName.PlayerBullet:
                obj = GameObject.Instantiate(prefabPlayerBullet);
                obj.GetComponent<Bullet>().Initialize();
                break;

            case PooledObjectName.EnemyBullet:
                obj = GameObject.Instantiate(prefabEnemyBullet);
                obj.GetComponent<Bullet>().Initialize();
                break;

            case PooledObjectName.Enemy:
                obj = GameObject.Instantiate(prefabEnemy);
                obj.GetComponent<Enemy>().Initialize();
                break;
        }

        obj.SetActive(false);
        GameObject.DontDestroyOnLoad(obj);
        return obj;
    }

    /// <summary>
    /// Removes all the pooled objects from the object pools
    /// </summary>
    public static void EmptyPools()
    {
        for (int i = 0; i < pools[PooledObjectName.PlayerBullet].Count - 1; i++)
        {
            Destroy(pools[PooledObjectName.PlayerBullet][i]);
        }
        pools[PooledObjectName.PlayerBullet].Clear();

        for (int i = 0; i < pools[PooledObjectName.EnemyBullet].Count - 1; i++)
        {
            Destroy(pools[PooledObjectName.EnemyBullet][i]);
        }
        pools[PooledObjectName.EnemyBullet].Clear();

        for (int i = 0; i < pools[PooledObjectName.Enemy].Count - 1; i++)
        {
            Destroy(pools[PooledObjectName.Enemy][i]);
        }
        pools[PooledObjectName.Enemy].Clear();
    }
}
