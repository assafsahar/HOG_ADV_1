using HOG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.Test
{
    
    public class HOGTester : HOGMonoBehaviour
    {
        [SerializeField] float speed = 30f;
        private Queue<HOGPoolable> poolables = new();

        private void OnEnable()
        {
            //AddListener(HOGEventNames.ReturnBullet, ReturnPoolable);
        }

        private void OnDisable()
        {
            RemoveListener(HOGEventNames.ReturnBullet, ReturnPoolable);
        }
        private void ClonesCreated(List<GameObject> obj)
        {
            Debug.Log("clones created! " + obj.Count);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                /*var bullet = Manager.PoolManager.GetPoolable("BulletsPool");
                poolables.Enqueue(bullet);*/
            }
            /*if (Input.GetKeyDown(KeyCode.A))
            {
                var bullet = poolsables.Dequeue();
                Manager.PoolManager.ReturnPoolable(bullet);
            }*/
        }
        

        void ReturnPoolable(object obj)
        {
            /*var bullet = poolables.Dequeue();
            Manager.PoolManager.ReturnPoolable(bullet);*/
        }


    }
}

