using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HOG.Core;

public class Move : HOGMonoBehaviour
{
  
    private void OnEnable()
    {
        AddListener("pressSpace", MoveCar);
    }
    private void OnDisable()
    {
        RemoveListener("pressSpace", MoveCar);
    }

    void MoveCar(object obj)
    {
        transform.Translate(Vector3.up);
    }
}
