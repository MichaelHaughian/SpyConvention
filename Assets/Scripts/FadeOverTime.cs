using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOverTime : MonoBehaviour
{


    SpriteRenderer scanRadiusSpriteRenderer;
    
    // Start is called before the first frame update
    void Awake()
    {
        scanRadiusSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Color tempColor = scanRadiusSpriteRenderer.color;
        float tempAlphaValue = tempColor.a - Time.deltaTime;
        tempColor.a = tempAlphaValue;

        scanRadiusSpriteRenderer.color = tempColor;
    }
}
