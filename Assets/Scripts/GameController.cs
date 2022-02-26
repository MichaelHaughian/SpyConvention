using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;

public class GameController : MonoBehaviour
{
    private static GameController _instance;

    public static GameController Instance { get { return _instance; } }

    [SerializeField] float timePerRound;
    [SerializeField] Material litMaterial;
    [SerializeField] Material highlightMaterial;
    [SerializeField] GameObject gameUI;
    [SerializeField] Button quitGameButton;
    [SerializeField] TextMeshProUGUI gameTimer;
    [SerializeField] Camera UICamera;
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] Light2D mainLight;
    [SerializeField] Light2D deathLight;

    bool someoneDied;
    bool isPaused;
    float timeCountdown;
    float countDownToGameOver;

    GameObject[] spies;
    SpyController nonSpy;
    Image nonSpySprite;
    Button retryButton;
    TextMeshProUGUI gameOverText;

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
        }

        timeCountdown = timePerRound;

        // set references on references script
        References.litMaterial = litMaterial;
        References.spyHighlightMaterial = highlightMaterial;

        //start game paused for tutorial
        isPaused = true;

        quitGameButton.onClick.AddListener(QuitGame);
    }

    void Start() {
        spies = GameObject.FindGameObjectsWithTag("Spy");
        retryButton = gameOverCanvas.transform.Find("RetryButton").GetComponentInChildren<Button>();
        gameOverText = gameOverCanvas.transform.Find("GameOverText").GetComponentInChildren<TextMeshProUGUI>();

        foreach (GameObject spy in spies) {
            SpyController spyController = spy.GetComponent<SpyController>();
            if (!spyController.isSpy) {
                nonSpy = spyController;
            }
        }

        retryButton.onClick.AddListener(ReloadGame);

        nonSpySprite = gameOverCanvas.transform.Find("NonSpySprite").GetComponentInChildren<Image>();
        nonSpySprite.sprite = nonSpy.transform.GetComponentInChildren<SpriteRenderer>().sprite;
    }

    private void Update() {
        if ((Input.GetKeyDown("tab") || Input.GetKeyDown("j")) && !isPaused) {
            
            isPaused = true;
        }else if ((Input.GetKeyDown("tab") || Input.GetKeyDown("j")) && isPaused) {
            isPaused = false;
        }

        if (isPaused) {
            Time.timeScale = 0;
            gameUI.SetActive(true);
            UICamera.enabled = true;
        }
        else {
            Time.timeScale = 1;
            gameUI.SetActive(false);
            timeCountdown -= Time.deltaTime;
            UICamera.enabled = false;
        }

        UpdateTime();

        if (someoneDied) {
            countDownToGameOver += Time.deltaTime;

            if(countDownToGameOver >= 2f) {
                isPaused = true;

                gameOverCanvas.SetActive(true);
                gameUI.SetActive(false);
            }
            
        }

        if(timeCountdown <= 0) {
            
            EndGameWithTimer();
            
        }
    }

    void EndGameWithTimer() {
        gameUI.SetActive(false);

        mainLight.enabled = false;
        deathLight.enabled = true;
        nonSpy.transform.position = new Vector3(0f, 1.5f);
        nonSpy.Die(0);
    }

    public void GuessGameOver(bool isCorrect) {
        gameUI.SetActive(false);

        isPaused = true;
        gameOverCanvas.SetActive(true);

        if (isCorrect) {
            gameOverText.text = "You guessed correctly!";
        }
        else {
            gameOverText.text = "You guessed incorrectly!";
        }

    }

    void UpdateTime() {
        System.TimeSpan timeInSeconds = System.TimeSpan.FromSeconds(timeCountdown);
        gameTimer.text = timeInSeconds.ToString("mm':'ss");
    }

    void QuitGame() {
        Application.Quit();
        ReloadGame(); // Reload HTML version of game, desktop version will quit before getting here
    }

    void ReloadGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool IsPaused() {
        return isPaused;
    }

    public void SetSomeoneDied() {
        someoneDied = true;
    }
}
