using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;

public class UIManager : MonoBehaviour
{
    [SerializeField] private bool gameIsPaused;

    [Header("Options")]
    [Space]
    [SerializeField] private bool inSettings;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject settingsButton;

    [Header("Credits")]
    [Space]
    [SerializeField] private bool InCredits;
    [SerializeField] private GameObject creditMenuCanvas;
    [SerializeField] private GameObject creditMenuButton;
    [Space]
    [SerializeField] private GameObject quitButton;

    [Header("Credits")]
    [Space]
    [SerializeField] private GameObject restartButton;

    public UnityEngine.UI.Image speedOMeter;
    public UnityEngine.UI.Image map;
    public TextMeshProUGUI countdownText;
    private Color transparent;


    private void Start()
    {
        transparent= new Color(255,255,255,0);
        settingsButton.SetActive(false);
        creditMenuButton.SetActive(false);
        quitButton.SetActive(false);
        restartButton.SetActive(false);
        speedOMeter.color = transparent;
        map.color = transparent;
    }

    private void Update()
    {
        PauseManagement();
    }

    private void PauseManagement()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }

        }

        BackToMainMenu(settings, inSettings, true);
        BackToMainMenu(creditMenuCanvas, InCredits, true);
    }

    private void Resume()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        MainMenuButtonStates(settingsButton, creditMenuButton, quitButton, restartButton, false);
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        gameIsPaused = true;
        MainMenuButtonStates(settingsButton, creditMenuButton, quitButton, restartButton, true);
    }
    public void UIQuit()
    {
        GameManager.Instance.Quit();
    }
    
    #region Options & Credits functionality
    public void Options()//wordt alleen gebruikt zodra je op de button drukt
    {
        if (inSettings == false)
        {
            settings.SetActive(true);
            inSettings = true;
            settingsButton.SetActive(false);
            creditMenuButton.SetActive(false);
            restartButton.SetActive(false);
            quitButton.SetActive(false);
        }
        else
        {
            settings.SetActive(false);
            inSettings = false;
            settingsButton.SetActive(true);
        }
    }

    public void Credits()//wordt alleen gebruikt zodra je op de button drukt (nog met UIManager implementen)
    {
        if (InCredits == false)
        {
            creditMenuCanvas.SetActive(true);
            InCredits = true;
            creditMenuButton.SetActive(false);
            settingsButton.SetActive(false);
            restartButton.SetActive(false);
            quitButton.SetActive(false);
        }
        else
        {
            creditMenuCanvas.SetActive(false);
            InCredits = false;
            creditMenuButton.SetActive(true);
        }
    }
    #endregion

    public IEnumerator FadeInUI(bool fadeIn, UnityEngine.UI.Image image)
    {
        if (fadeIn)
        {
            for (float i = 0; i <= 1; i += 0.33f * Time.deltaTime)
            {
                image.color = new Color(255, 255, 255, i);
                yield return null;
            }
        }
    }

    /// <summary>
    /// This function counts down at the start of the game.
    /// </summary>
    /// <returns></returns>
    public IEnumerator countDown()
    {
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        countdownText.text = "GO!";
        //needs slower fade out
        GameManager.Instance.playerMovementScript.enabled = true;
        yield return new WaitForSeconds(1);
        countdownText.text = "";
    }

    /// <summary>
    /// This function call the restartlevel function from the gamemanager
    /// </summary>
    public void ResetButton()
    {
        GameManager.Instance.RestartLevel();
    }

    #region Code cleanup functions
    /// <summary>
    /// This func serves to clean up code
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="state"></param>
    /// <param name="gameObject1"></param>
    /// <param name="state1"></param>
    /// <param name="gameObject2"></param>
    /// <param name="state2"></param>
    /// <param name="gameObject3"></param>
    /// <param name="state3"></param>
    private void MainMenuButtonStates(GameObject gameObject, GameObject gameObject1, GameObject gameObject2, GameObject gameObject3, bool state)
    {
        gameObject.SetActive(state);
        gameObject1.SetActive(state);
        gameObject2.SetActive(state);
        gameObject3.SetActive(state);
    }

    private void BackToMainMenu(GameObject currentMenu, bool InXMenu, bool Enable)
    {
        if (InXMenu && Input.GetKeyDown(KeyCode.Escape))//go back to pausemenu
        {
            currentMenu.SetActive(false);
            InXMenu = false;
            creditMenuButton.SetActive(Enable);
            settingsButton.SetActive(Enable);
            quitButton.SetActive(Enable);
            restartButton.SetActive(Enable);
            Time.timeScale = 0;
            gameIsPaused = Enable;
        }
    }
    #endregion
}
