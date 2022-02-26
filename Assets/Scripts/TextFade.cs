using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFade : MonoBehaviour
{
    float timeToFade;
    TextMeshPro textMesh;

    // Start is called before the first frame update
    void Start()
    {
        timeToFade = 0f;
        textMesh = GetComponent<TextMeshPro>();
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        timeToFade += Time.deltaTime;

        if(timeToFade >= 3f) {
            Color tempColor = textMesh.color;
            float tempAlphaValue = tempColor.a - (Time.deltaTime * 2f);
            tempColor.a = tempAlphaValue;

            textMesh.color = tempColor;
        }
    }
}
