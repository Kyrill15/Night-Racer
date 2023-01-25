using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<GameObject> Checkpoints;
    private UIManager UIManager;
    
    [HideInInspector] public Vector3 StartCheckpointPos;
    public Vector3 FinishCheckpointPos;

    private CheckPointScript[] checkPointScript;

    [HideInInspector] public PlayerMovement playerMovementScript;

    [Header("Respawning")]
    [Space]
    [SerializeField] private float RespawnTimer;

    [SerializeField] private Transform Player;

    private void Awake()
    {
        if (Instance != null && Instance != this){
            Destroy(this);
        }
        else{
            Instance = this;
        }

        playerMovementScript = FindObjectOfType<PlayerMovement>();
        UIManager = FindObjectOfType<UIManager>();
        checkPointScript = FindObjectsOfType<CheckPointScript>();//multiple objects (works as normal array with indeces and such)
    }
    void Start() {
        StartCoroutine(StartGame());
    }

    void Update()
    {
        Respawn();
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ResetAllCheckpoints();
        }
    }

    private void Respawn()
    {
        if (Input.GetKey(KeyCode.R))
        {
            for (int i = 0; i < checkPointScript.Length; i++)
            {
                if (checkPointScript[i].isStart)
                    print(gameObject.name + "cords are: " + StartCheckpointPos);
            }

            RespawnTimer += Time.deltaTime;
            //screen dimming
            if (RespawnTimer >= 3)
            {
                playerMovementScript.transform.position = Checkpoints[Checkpoints.Count-1].transform.position;//count -1 bc of indexing
                playerMovementScript.transform.rotation = Checkpoints[Checkpoints.Count-1].transform.rotation;
                RespawnTimer = 0;
            }
        }
        else
        {
            RespawnTimer = 0;
        }
    }

    private IEnumerator StartGame()
    {
        playerMovementScript.enabled = false;
        StartCoroutine(UIManager.FadeInUI(true, UIManager.speedOMeter));//true asks if you wanna fade in or not
        StartCoroutine(UIManager.FadeInUI(true, UIManager.map));
        StartCoroutine(UIManager.countDown());
        yield return null;
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3);
        //set all vars to default states/values
        //deactivate UI Elements lke speed, timers etc.
        //activate End/or/Death screen
    }

    private void ResetAllCheckpoints()
    {
        foreach (GameObject checkpoint in Checkpoints)
        {
            checkpoint.GetComponent<BoxCollider>().enabled = true;
        }
        Checkpoints.Clear();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void RestartLevel()//to call from UIManager
    {
        Player.transform.position = Checkpoints[0].transform.position;
        Player.transform.rotation = Checkpoints[0].transform.rotation;
        ResetAllCheckpoints();
        //fade out & in (maybe???)
    }
}
