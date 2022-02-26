using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpyController : MonoBehaviour
{

    [SerializeField] float spySpeed;
    [SerializeField] float talkingPriority;
    [SerializeField] GameObject floatingTextPrefab;

    [SerializeField] bool isAWoman;
    [SerializeField] public bool isTheQuietOne;
    [SerializeField] public bool isTheSelfishOne;
    [SerializeField] public bool isTheInjuredOne;
    [SerializeField] public bool isTheOldOne;
    [SerializeField] public bool isTheMysogynistOne;
    [SerializeField] public bool isTheClassyOne;
    [SerializeField] public bool isTheBoringOne;

    Vector2 pointToMoveTo;
    SpriteRenderer spriteRenderer;
    Animator animator;
    SpyController closestSpyToTalkTo;

    public Material spyHighlightMaterial;

    public bool isSpy;
    bool stopMovement;
    bool isHighlighted;
    bool isPoisoned;
    bool isDying;
    bool isShot;
    bool isTalking;
    

    float fadeHighlightTimer;
    float timeToTalk;
    float timeToTalkElapsed;
    float timeToStandStill;
    float timeStandingStillElapsed;
    float timeToStandUp;
    float timeToStandUpElapsed;
    float timePoisoned;
    float timeSinceLastConversation;
    float timeStopped;
    float timeBetweenTextPopUpsElapsed;
    float timeBetweenTextPopUps;
    float deathTimer;

    // Start is called before the first frame update
    void Start() {
        pointToMoveTo = transform.position;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        fadeHighlightTimer = 0;
        timeToStandUp = 3f;
    }

    // Update is called once per frame
    void Update() {
        if (!stopMovement && !closestSpyToTalkTo) {
            MoveSpy();
            AcquireNewLocationToMoveTo();
        }

        if (!isTalking) {
            timeSinceLastConversation += Time.deltaTime;
        }

        if (isTalking) {
            DoConversation();
        }

        if (stopMovement) {
            timeStopped += Time.deltaTime;
        }

        if(timeStopped > 20f) {
            stopMovement = false;
            timeStopped = 0;
        }

        if (closestSpyToTalkTo && timeSinceLastConversation > 10f) {
            timeToTalkElapsed += Time.deltaTime;

            if (timeToTalkElapsed < timeToTalk) {
                TalkToSpy();
            }
            else if(timeToTalkElapsed >= timeToTalk) {
                closestSpyToTalkTo.StopTalking();
                StopTalking();
                timeToTalkElapsed = 0;
            }
        }

        FlipSprite();

        if (isHighlighted) {
            fadeHighlightTimer += Time.deltaTime;
            
            if(fadeHighlightTimer > 2f) {
                FadeHighlight();
            }
        }

        if (animator.GetBool("isJumping")) {
            GetBackUp();
        }

        if (isPoisoned) {
            timePoisoned += Time.deltaTime;

            if (timePoisoned >= 4f) {
                ResolvePoisonState();
            }
        }

        if (isDying) {
            if (isShot) {
                Die(0);
            }
            else {
                Die(2);
            }   
        }

        if(timeStandingStillElapsed > 0) {
            if (DetermineAppetiteForConversation()) {
                timeStandingStillElapsed = 0f;
                FindSpyToTalkTo();
            }
        }

        if (closestSpyToTalkTo) {
            if(Vector2.Distance(transform.position, closestSpyToTalkTo.transform.position) > 3f) {
                closestSpyToTalkTo = null;
            }
        }
        
    }

    void StopTalking() {
        closestSpyToTalkTo = null;
        stopMovement = false;
        isTalking = false;
        timeToTalk = 0f;
        timeSinceLastConversation = 0f;
    }

    bool DetermineAppetiteForConversation() {
        bool wantsToTalk;
        float roll;

        roll = Random.Range(0f, 100f);

        if (isTheQuietOne) {
            if (roll <= 5) {
                wantsToTalk = true;
            }
            else {
                wantsToTalk = false;
            }
        }
        else {
            if (roll <= 30) {
                wantsToTalk = true;
            }
            else {
                wantsToTalk = false;
            }
        }

        return wantsToTalk;
    }

    void MoveSpy() {
        transform.position = Vector2.MoveTowards(transform.position, pointToMoveTo, (0.1f * spySpeed * Time.deltaTime));
        animator.SetBool("isMoving", true);
    }

    void AcquireNewLocationToMoveTo() {

        if (Vector2.Distance(transform.position, pointToMoveTo) < 0.001f) {

            if (timeStandingStillElapsed <= 0) {
                timeToStandStill = Random.Range(0f, 5f); 
            }

            timeStandingStillElapsed += Time.deltaTime;

            animator.SetBool("isMoving", false);

            if (timeStandingStillElapsed >= timeToStandStill) {
                
                float randomX = Random.Range(0, Camera.main.pixelWidth);
                float randomY = Random.Range(0, Camera.main.pixelHeight);
                Vector2 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(randomX, randomY, Camera.main.transform.position.z));

                if (isTheInjuredOne) {
                    if(Vector2.Distance(transform.position, screenPosition) > 3f) {
                        AcquireNewLocationToMoveTo();
                    }
                    else {
                        pointToMoveTo = screenPosition;
                    }
                }
                else {
                    pointToMoveTo = screenPosition;
                }
                
                timeStandingStillElapsed = 0;
            }
            
        }
    }

    void FindSpyToTalkTo() {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f);
        SpyController closestSpy = null;
        float closestDistance = 100f;

        foreach (Collider2D hit in hits) {
            if (hit.transform != transform) {
                SpyController spy = hit.GetComponentInParent<SpyController>();

                if (spy != null) {
                    if (spy.IsSpy() && !spy.stopMovement && !isTalking) {
                        if (isTheMysogynistOne && spy.isAWoman) {
                            continue;
                        }
                        if(isTheClassyOne && spy.isTheMysogynistOne) {
                            continue;
                        }
                        else {
                            float distance = Vector2.Distance(transform.position, spy.transform.position);
                            if (distance < closestDistance) {
                                closestSpy = spy;
                            }
                        }
                    }
                }
            }
        }

        if (closestSpy) {
            if (closestSpy.isTheBoringOne) {
                timeToTalk = Random.Range(5f, 10f);
            }
            else {
                timeToTalk = Random.Range(10f, 20f);
            }
            

            if (closestSpy.timeSinceLastConversation < 10f) {
                closestSpyToTalkTo = null;
                return;
            }

            if (closestSpy.talkingPriority > talkingPriority) { 
                return;
            }
            else {
                closestSpyToTalkTo = closestSpy;
            }
        }

    }

    void TalkToSpy() {
        isTalking = true;
        closestSpyToTalkTo.BeTalkedTo();

        Vector2 standingPosition = new Vector2(closestSpyToTalkTo.transform.position.x + 1.9f, closestSpyToTalkTo.transform.position.y);

        pointToMoveTo = standingPosition;
        transform.position = Vector2.MoveTowards(
            transform.position,
            standingPosition,
            (0.1f * spySpeed * Time.deltaTime));

        animator.SetBool("isMoving", true);

        if (Vector2.Distance(transform.position, standingPosition) < 0.001f) {
            float directionToLook = transform.position.x - closestSpyToTalkTo.transform.position.x;

            if(directionToLook < 0) {
                spriteRenderer.flipX = false;
            }else if(directionToLook > 0) {
                spriteRenderer.flipX = true;
            }

            stopMovement = true;
            isTalking = true;
            animator.SetBool("isMoving", false);
        }
    }

    void BeTalkedTo() {
        spriteRenderer.flipX = false;
        
        isTalking = true;
        stopMovement = true; 
        animator.SetBool("isMoving", false);
    }

    void DoConversation() {
        if(timeBetweenTextPopUpsElapsed <= 0) {
            timeBetweenTextPopUps = Random.Range(2f, 4f);
        }

        timeBetweenTextPopUpsElapsed += Time.deltaTime;

        if (!GetComponentInChildren<TextMeshPro>() && timeBetweenTextPopUpsElapsed >= timeBetweenTextPopUps) {
            var text = Instantiate(floatingTextPrefab,
            new Vector2(transform.position.x, transform.position.y + 1.7f), Quaternion.identity, transform);
            text.GetComponent<TextMeshPro>().text = "*Whisper*";
            if (closestSpyToTalkTo) {
                if (closestSpyToTalkTo.isTheOldOne) {
                    text.GetComponent<TextMeshPro>().fontSize += 1;
                }
            }
            
        }
    }

    public void BeShotAt() {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.7f);
        SpyController closestSpy = null;
        float closestDistance = 100f;

        foreach (Collider2D hit in hits) {
            if(hit.transform != transform) {
                SpyController spy = hit.GetComponentInParent<SpyController>();

                if (spy != null) {
                    if (spy.IsSpy()) {
                        float distance = Vector2.Distance(transform.position, spy.transform.position);
                        if(distance < closestDistance) {
                            closestDistance = distance;
                            closestSpy = spy;
                        }
                    }
                }
            }
        }

        if (closestSpy) {
            if (!closestSpy.isTheSelfishOne) {
                JumpInFrontOfBullet(closestSpy);
            }
        }
        else {
            stopMovement = true;
            isDying = true;
            isShot = true;
        }
    }

    void JumpInFrontOfBullet(SpyController otherSpy) {
        otherSpy.StopTalking();
        StopTalking();
        otherSpy.transform.position = new Vector2(transform.position.x, transform.position.y - 0.5f);
        otherSpy.animator.SetBool("isJumping", true);
        otherSpy.stopMovement = true;

    }

    public void BePoisoned() {
        isPoisoned = true;
    }

    void ResolvePoisonState() {
        if (isSpy) {
            var text = Instantiate(floatingTextPrefab,
            new Vector2(transform.position.x, transform.position.y + 1.7f), Quaternion.identity, transform);
            text.GetComponent<TextMeshPro>().text = "I recognise this taste from  back\nin the 70's...";
            isPoisoned = false;
        }
        else {
            var text = Instantiate(floatingTextPrefab,
            new Vector2(transform.position.x, transform.position.y + 1.7f), Quaternion.identity, transform);
            text.GetComponent<TextMeshPro>().text = "I don't feel so good...";
            isPoisoned = false;
            stopMovement = true;
            isDying = true;
        }
    }



    void GetBackUp() {
        timeToStandUpElapsed += Time.deltaTime;

        if (timeToStandUpElapsed >= timeToStandUp) {
            stopMovement = false;
            animator.SetBool("isJumping", false);

            var text = Instantiate(floatingTextPrefab,
            new Vector2(transform.position.x, transform.position.y + 1.7f), Quaternion.identity, transform);
            text.GetComponent<TextMeshPro>().text = "Good thing I was wearing\nbullet proof fancy clothing!";

            timeToStandUpElapsed = 0;
        }
    }

    public void Die(float timeToDie) {
        deathTimer += Time.deltaTime;

        if (deathTimer >= timeToDie) {
            animator.SetBool("isDead", true);
            GameController.Instance.SetSomeoneDied();
        }
    }

    void FlipSprite() {
        if ((transform.position.x - pointToMoveTo.x) < 0) {
            spriteRenderer.flipX = false;
        }

        if ((transform.position.x - pointToMoveTo.x) > 0) {
            spriteRenderer.flipX = true;
        }
    }

    void FadeHighlight() {
        spriteRenderer.material = References.litMaterial;
        isHighlighted = false;
        fadeHighlightTimer = 0;
    }

    public bool IsSpy() {
        return isSpy;
    }

    public void HighlightSpy() {
        if (!animator.GetBool("isJumping")) {
            isHighlighted = true;
            spriteRenderer.material = References.spyHighlightMaterial;
        }
        
    }

    private void OnMouseOver() {
        HighlightSpy();
    }

    private void OnMouseExit() {
        spriteRenderer.material = References.litMaterial;
    }
}
