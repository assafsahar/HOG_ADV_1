using HOG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.GameLogic
{
    public class HOGGameLogicLoader : HOGGameLoaderBase
    {
        //[SerializeField] HOGBulletComponent bulletComponent;
        public override void StartLoad(Action onComplete)
        {
            HOGGameLogicManager hogGameLogicManager = new HOGGameLogicManager();
            hogGameLogicManager.LoadManager( () =>
            {
                base.StartLoad(onComplete);
            });
            //Manager.PoolManager.InitPool(bulletComponent, 20, 100);
            

        }
    }

}
