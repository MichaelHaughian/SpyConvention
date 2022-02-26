using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    bool facingRight;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() { 

        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);

        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if(angle < -90 || angle > 90) {
            if(transform.eulerAngles.y == 0) {
                transform.localRotation = Quaternion.Euler(180, 0, -angle);
            }
            else if (transform.eulerAngles.y == 180) {
                transform.localRotation = Quaternion.Euler(180, 180, -angle);
            }
        }       

    }
}
