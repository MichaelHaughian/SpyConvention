using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpySelectorController : MonoBehaviour
{
    int currentSpySelection;

    [SerializeField] Sprite Spy1;
    [SerializeField] Sprite Spy2;
    [SerializeField] Sprite Spy3;
    [SerializeField] Sprite Spy4;
    [SerializeField] Sprite Spy5;
    [SerializeField] Sprite Spy6;
    [SerializeField] Sprite Spy7;
    [SerializeField] Sprite Spy8;

    Sprite[] spies;

    Button leftButton;
    Button rightButton;
    Image currentSprite;
    // Start is called before the first frame update
    void Start()
    {
        currentSpySelection = 0;

        //spriteRenderer = this.transform.Find("PlayerSprite").GetComponentInChildren<SpriteRenderer>();

        currentSprite = GetComponent<Image>();
        leftButton = this.transform.Find("LeftSelector").GetComponentInChildren<Button>();
        rightButton = this.transform.Find("RightSelector").GetComponentInChildren<Button>();

        leftButton.onClick.AddListener(DecreaseSpySelector);
        rightButton.onClick.AddListener(IncreaseSpySelector);

        spies = new Sprite[] { Spy1, Spy2, Spy3, Spy4, Spy5, Spy6, Spy7, Spy8 };
    }

    void ChangeSprite() {
        currentSprite.sprite = spies[currentSpySelection];
    }

    void DecreaseSpySelector() {
        if(currentSpySelection > 0) {
            currentSpySelection--;
        }else if(currentSpySelection <= 0) {
            currentSpySelection = 7;
        }

        ChangeSprite();
    }

    void IncreaseSpySelector() {
        if (currentSpySelection < 7) {
            currentSpySelection ++;
        }
        else if (currentSpySelection >= 7) {
            currentSpySelection = 0;
        }

        ChangeSprite();
    }
}
