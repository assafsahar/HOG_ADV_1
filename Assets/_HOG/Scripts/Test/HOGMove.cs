using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HOG.Core;

public class HOGMove : HOGMonoBehaviour
{
    [SerializeField] float speed = 10f;

    private void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * speed;
        if(transform.position.y > 5)
        {
            InvokeEvent(HOGEventNames.ReturnBullet);
        }
    }

}
