using HOG.Character;
using HOG.Core;
using HOG.GameLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.Test
{
    
    public class HOGTester : HOGMonoBehaviour
    {
        //[SerializeField] float speed = 30f;
        //private Queue<HOGPoolable> poolables = new();

        private void Start()
        {
            //HOGGameLogicManager.Instance.ScoreManager.ChangeScoreByTagByAmount(GameLogic.ScoreTags.MainScore, 50);
        }
        private void OnEnable()
        {
            Debug.Log("Tester enabled");
            //AddListener(HOGEventNames.ReturnBullet, ReturnPoolable);
        }

        private void OnDisable()
        {
            //RemoveListener(HOGEventNames.ReturnBullet, ReturnPoolable);
        }


        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                InvokeEvent(HOGEventNames.OnTest, HOGCharacterState.CharacterStates.Defense);
                /*var bullet = Manager.PoolManager.GetPoolable("BulletsPool");
                poolables.Enqueue(bullet);*/
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                InvokeEvent(HOGEventNames.OnTest, HOGCharacterState.CharacterStates.Attack);

            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                InvokeEvent(HOGEventNames.OnTest, HOGCharacterState.CharacterStates.Move);

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

