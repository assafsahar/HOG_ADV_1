using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HOGBulletComponent : HOGPoolable
{

    private GameObject startLocation;

    private void Awake()
    {
        
    }
    override public void OnTakenFromPool()
    {
        //Manager.EventsManager.AddListener(HOG.Core.HOGEventNames.PlayerTaken, OnPlayerTaken);
        base.OnTakenFromPool();

        Manager.EventsManager.InvokeEvent(HOG.Core.HOGEventNames.PlayerTaken, this);


    }
    public override void OnReturnedToPool()
    {
        //Manager.EventsManager.RemoveListener(HOG.Core.HOGEventNames.PlayerTaken, OnPlayerTaken);
        //transform.position = Vector3.zero;
        base.OnReturnedToPool();

    }

    private void OnPlayerTaken(object obj)
    {
        
        
    }
}
