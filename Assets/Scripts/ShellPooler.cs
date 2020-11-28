using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellPooler : MonoSingletonGeneric<ShellPooler>
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject shellPrefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Start()
    {
        
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach(Pool pool in pools)
        {
            Queue<GameObject> objPool = new Queue<GameObject>();
            for(int i = 0; i < pool.size;i++)
            {
                GameObject shell = Instantiate(pool.shellPrefab);
                shell.SetActive(false);
                objPool.Enqueue(shell);
            }
            poolDictionary.Add(pool.tag, objPool);
        }
       
        
    }
    public void SpawnFromPool(string tag, Vector3 position, Quaternion rotation , Vector3 projectileVelocity)
    {
        GameObject objToSpawn = poolDictionary[tag].Dequeue();
        objToSpawn.SetActive(true);
        //objToSpawn.transform.position = position;
        //objToSpawn.transform.rotation = rotation;
        if (tag == "Shell")
        {
            Rigidbody rb = objToSpawn.GetComponent<Rigidbody>();
            rb.transform.position = position;
            rb.transform.rotation = rotation;
            rb.velocity = projectileVelocity;
        }
        else if(tag == "Buggy") 
        {
            objToSpawn.transform.position = position;
            objToSpawn.transform.rotation = rotation;
            objToSpawn.GetComponent<Rigidbody>().velocity = projectileVelocity;
        }
        IpooledObject pooledObj = objToSpawn.GetComponent<IpooledObject>();
        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }

        poolDictionary[tag].Enqueue(objToSpawn);
    }

}
