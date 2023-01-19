using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HOGTester : HOGMonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(Manager.GetNumber());
        }
    }
}
