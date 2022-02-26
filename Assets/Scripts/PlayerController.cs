using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float playerSpeed;
    [SerializeField] GameObject scanRadius;
    [SerializeField] GameObject floatingTextPrefab;
    [SerializeField] Sprite regularSprite;
    [SerializeField] Sprite noArmSprite;
    [SerializeField] Sprite poisonSprite;
    SpriteRenderer gunSpriteRenderer;

    GameObject scanRadiusInstance;
    SpriteRenderer spriteRenderer;

    int numberOfSpys;
    bool hasGunOut;
    bool hasPoisonOut;
    bool usedRadar;
    bool usedGun;
    bool usedPoison;

    // Start is called before the first frame update
    void Start()
    {
        hasGunOut = false;
        spriteRenderer = this.transform.Find("PlayerSprite").GetComponentInChildren<SpriteRenderer>();
        gunSpriteRenderer = this.transform.Find("GunArms").GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.Instance.IsPaused()) {
            MovePlayer();
            FlipSprite();

            if (Input.GetKeyDown("space") && !usedRadar) {
                ActivateSpyPhoneLocator();
            }

            if (Input.GetKeyDown("e") && hasGunOut == false && !usedGun) {
                if (hasPoisonOut) {
                    PutAwayPoison();
                }

                ActivateGun();
            }
            else if (Input.GetKeyDown("e") && hasGunOut == true) {
                PutAwayGun();
            }

            if (hasGunOut && Input.GetMouseButtonDown(0) && !usedGun) {
                ShootGun();
            }

            if (hasPoisonOut && Input.GetMouseButtonDown(0)) {
                PoisonSpy();
            }

            if (Input.GetKeyDown("q") && !hasPoisonOut && !usedPoison) {
                if (hasGunOut) {
                    PutAwayGun();
                }

                PullOutPoison();
            }
            else if (Input.GetKeyDown("q") && hasPoisonOut) {
                PutAwayPoison();
            }

            if (Input.GetMouseButtonDown(1)) {
                ChooseNonSpy();
            }
        }
    }

    void MovePlayer() {
        transform.position += Vector3.up * Input.GetAxis("Vertical") * (0.1f * playerSpeed) * Time.deltaTime;
        transform.position += Vector3.right * Input.GetAxis("Horizontal") * (0.1f * playerSpeed) * Time.deltaTime;
    }


    void FlipSprite() {
        if (Input.GetKey(KeyCode.A) && spriteRenderer.flipX == false && !hasGunOut) {
            spriteRenderer.flipX = true;
        }

        if (Input.GetKey(KeyCode.D) && spriteRenderer.flipX == true && !hasGunOut) {
            spriteRenderer.flipX = false;
        }

        if (hasGunOut) {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10f);
            var heading = transform.position - (Camera.main.ScreenToWorldPoint(mousePosition) * -1f);
            var distance = heading.magnitude;
            var direction = heading / distance;

            if(direction.x > 0) {
                spriteRenderer.flipX = true;
            }else if (direction.x < 0) {
                spriteRenderer.flipX = false;
            }
        }
    }

    void PullOutPoison() {
        hasPoisonOut = true;
        spriteRenderer.sprite = poisonSprite;
    }

    void PutAwayPoison() {
        hasPoisonOut = false;
        spriteRenderer.sprite = regularSprite;
    }

    void PoisonSpy() {
        RaycastHit2D hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f)),
            Vector2.zero);

        if (hit.collider != null) {
            float distance = Vector2.Distance(hit.collider.transform.position, transform.position);

            if(distance <= 2f) {
                SpyController spy = hit.transform.GetComponent<SpyController>();
                spy.BePoisoned();
                PutAwayPoison();
                AudioManager.Instance.PlayPoisonSound();
                usedPoison = true;
            }   
        }
    }

    void ActivateGun() {
        hasGunOut = true;
        hasGunOut = true;
        spriteRenderer.sprite = noArmSprite;
        gunSpriteRenderer.enabled = true;
    }

    void PutAwayGun() {
        hasGunOut = false;
        spriteRenderer.sprite = regularSprite;
        gunSpriteRenderer.enabled = false;
    }

    void ShootGun() {
        RaycastHit2D hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f)), 
            Vector2.zero);

        if (hit.collider != null) {
            SpyController spy = hit.transform.GetComponent<SpyController>();

            AudioManager.Instance.PlayGunShot();

            spy.BeShotAt();
            PutAwayGun();
            usedGun = true;
        }
    }

    void ChooseNonSpy() {
        RaycastHit2D hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f)),
            Vector2.zero);

        if (hit.collider != null) {
            SpyController spy = hit.transform.GetComponent<SpyController>();

            if (!spy.isSpy) {
                GameController.Instance.GuessGameOver(true);
            }
            else {
                GameController.Instance.GuessGameOver(false);
            }
        }
    }

    void ActivateSpyPhoneLocator() {

        AudioManager.Instance.PlayRadar();

        scanRadiusInstance = Instantiate(scanRadius, transform, false);
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 3.3f);

        foreach(Collider2D hit in hits) {
            SpyController spy = hit.GetComponentInParent<SpyController>();

            if(spy != null) {
                spy.HighlightSpy();
                if (spy.IsSpy()) {
                    numberOfSpys++;
                }
            }    
        }

        var text = Instantiate(floatingTextPrefab, 
            new Vector2(transform.position.x, transform.position.y + 1.7f), Quaternion.identity, transform);
        text.GetComponent<TextMeshPro>().text = "Hmm, I heard " + numberOfSpys.ToString() + " spy phones";

        numberOfSpys = 0;
        Destroy(scanRadiusInstance, 1f);
        usedRadar = true;
    }
}
