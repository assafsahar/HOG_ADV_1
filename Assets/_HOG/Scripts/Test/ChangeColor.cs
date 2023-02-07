using HOG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : HOGMonoBehaviour
{
    public Color color = Color.red;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        AddListener("pressSpace", ChangeMyColor);
    }
    private void OnDisable()
    {
        RemoveListener("pressSpace", ChangeMyColor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeMyColor(object obj)
    {
        if(spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
        
    }
}
