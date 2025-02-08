using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static List<PooledObjectsInfo> ObjectPools = new List<PooledObjectsInfo>();

    public static GameObject SpawnObject(GameObject objToSpawn, Vector3 spawnPos, Quaternion spawnRot)
    {

        PooledObjectsInfo pool = ObjectPools.Find(p => p.LookupString == objToSpawn.name);

        if (pool == null)
        {
            pool = new PooledObjectsInfo() { LookupString = objToSpawn.name };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

        if (spawnableObj == null)
            spawnableObj = Instantiate(objToSpawn, spawnPos, spawnRot);
        else
        {
            // Reuse the object from the pool
            spawnableObj.transform.position = spawnPos;
            spawnableObj.transform.rotation = spawnRot;
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }

        return spawnableObj;
    }

    public static void ReturnToPool(GameObject obj)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7);
        PooledObjectsInfo pool = ObjectPools.Find(p => p.LookupString == goName);

        if (pool == null)
            Debug.LogWarning($"Trying to release an object that is not pooled: " + obj.name);
        else
        {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }

    public static IEnumerator ReenableAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        PooledObjectsInfo pool = ObjectPools.Find(p => p.LookupString == obj.name);

        // Ensure the object is active before adding it to the pool
        obj.SetActive(true);
        pool.InactiveObjects.Remove(obj);
    }
}