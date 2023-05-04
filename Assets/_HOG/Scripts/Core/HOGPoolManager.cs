using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.Core
{

    public class HOGPoolManager
    {
        private Dictionary<PoolNames, HOGPool> Pools = new();

        private Transform rootPools;

        public HOGPoolManager()
        {
            rootPools = new GameObject().transform;
            Object.DontDestroyOnLoad(rootPools);
        }

        public void InitPool(string resourceName, int amount, int maxAmount = 100)
        {
            var original = Resources.Load<HOGPoolable>(resourceName);
            InitPool(original, amount, maxAmount);
        }
        public void InitPool(HOGPoolable original, int amount, int maxAmount)
        {
            HOGManager.Instance.FactoryManager.MultiCreateAsync(original, Vector3.zero, amount,
                delegate (List<HOGPoolable> list)
                {
                    foreach (var poolable in list)
                    {
                        poolable.name = original.name;
                        poolable.transform.parent = rootPools;
                        poolable.gameObject.SetActive(false);
                    }

                    var pool = new HOGPool
                    {
                        AllPoolables = new Queue<HOGPoolable>(list),
                        UsedPoolables = new Queue<HOGPoolable>(),
                        AvailablePoolables = new Queue<HOGPoolable>(list),
                        MaxPoolables = maxAmount
                    };

                    Pools.Add(original.PoolName, pool);
                });
        }

        public HOGPoolable GetPoolable(PoolNames poolName)
        {
            if (Pools.TryGetValue(poolName, out HOGPool pool))
            {
                if (pool.AvailablePoolables.TryDequeue(out HOGPoolable poolable))
                {
                    //HOGDebug.Log($"GetPoolable - {poolName}");

                    poolable.OnTakenFromPool();

                    pool.UsedPoolables.Enqueue(poolable);
                    poolable.gameObject.SetActive(true);
                    return poolable;
                }

                //Create more
                HOGDebug.Log($"pool - {poolName} no enough poolables, used poolables {pool.UsedPoolables.Count}");

                return null;
            }

            HOGDebug.Log($"pool - {poolName} wasn't initialized");
            return null;
        }


        public void ReturnPoolable(HOGPoolable poolable)
        {
            if (Pools.TryGetValue(poolable.PoolName, out HOGPool pool))
            {
                pool.AvailablePoolables.Enqueue(poolable);
                poolable.OnReturnedToPool();
                poolable.gameObject.SetActive(false);
            }
        }


        public void DestroyPool(PoolNames name)
        {
            if (Pools.TryGetValue(name, out HOGPool pool))
            {
                foreach (var poolable in pool.AllPoolables)
                {
                    poolable.PreDestroy();
                    ReturnPoolable(poolable);
                }

                foreach (var poolable in pool.AllPoolables)
                {
                    Object.Destroy(poolable);
                }

                pool.AllPoolables.Clear();
                pool.AvailablePoolables.Clear();
                pool.UsedPoolables.Clear();

                Pools.Remove(name);
            }
        }
    }

    public class HOGPool
    {
        public Queue<HOGPoolable> AllPoolables = new();
        public Queue<HOGPoolable> UsedPoolables = new();
        public Queue<HOGPoolable> AvailablePoolables = new();

        public int MaxPoolables = 100;
    }

    public enum PoolNames
    {
        NA = -1,
        ScoreToast = 0
    }

}

