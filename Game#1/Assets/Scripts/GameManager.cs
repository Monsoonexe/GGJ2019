﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManager;
    private static int sceneNo = 0;

    [SerializeField] private bool TESTING = false;
    [SerializeField] private int winCelebrateSeconds = 5;
    [SerializeField] private GameObject victoryWindow;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private AudioClip[] levelMusic;
    [SerializeField] private AudioClip victoryFanfare;

    private AudioSource audioSource;

    private Transform startPoint;
    private Vector3 nextSpawnPoint;
    private bool levelIsWon = false;
    private GameObject playerInstance;
    
    private void Awake()
    {
        SingletonPattern();
        audioSource = GetComponent<AudioSource>() as AudioSource;
        //Debug.Log("AWAKE");
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("START!");
        GetStartPosition();
        nextSpawnPoint = startPoint.position;
        LoadAudio();
        levelIsWon = false;
        playerInstance = GameObject.FindGameObjectWithTag("Player");
        playerInstance.transform.position = nextSpawnPoint;

        SceneManager.sceneLoaded += OnLevelLoaded;
        //Debug.Log("Scenes in list: " + SceneManager.sceneCountInBuildSettings);

    }

    // Update is called once per frame
    void Update()
    {
        if (TESTING)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

    }

    private void LoadAudio()
    {
        if (audioSource && levelMusic.Length > sceneNo)
        {
            audioSource.clip = levelMusic[sceneNo >= levelMusic.Length ? levelMusic.Length - 1 : sceneNo];

            audioSource.Play();
        }
    }

    private void GetStartPosition()
    {
        startPoint = GameObject.FindGameObjectWithTag("PlayerStart").transform;

        if (!startPoint)
        {
            Debug.LogError("ERROR! No start point in scene!!!");
        }
    }

    /// <summary>
    /// Whateva! I do what I want!
    /// </summary>
    /// <param name="sceneNo"></param>
    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        //KillExistingPlayerIfAny();

        //where does the player start the level
        GetStartPosition();

        //init next spawn point
        nextSpawnPoint = startPoint.position;

        GameObject.FindGameObjectWithTag("Player").transform.position = nextSpawnPoint;

        LoadAudio();

        levelIsWon = false;

        //create a new player object
        //Instantiate(playerPrefab, nextSpawnPoint, Quaternion.identity);
        

    }

    private void KillExistingPlayerIfAny()
    {
        GameObject[] extraPlayer = GameObject.FindGameObjectsWithTag("Player");
        foreach (var p in extraPlayer)
        {
            Destroy(p);
        }
    }

    private void SingletonPattern()
    {
        if (!gameManager)
        {
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>() as GameManager;
            DontDestroyOnLoad(this.gameObject);//IMMORTALITY!!!!
        }
        else
        {
            Destroy(this.gameObject);//THERE CAN ONLY BE ONE!!!!
        }
    }

    public void WinLevel()
    {
        //ensure level can only be "won" once per level
        if (!levelIsWon)
        {
            //flag
            levelIsWon = true;

            //say congrats!
            Instantiate(victoryWindow);
            audioSource.clip = victoryFanfare;
            audioSource.Play();

            //do win animations

            //load next scene
            if (++sceneNo >= SceneManager.sceneCountInBuildSettings) sceneNo = 0; // repeat when run out of scenes
            Debug.Log("this scene no: " + sceneNo + " / " + SceneManager.sceneCountInBuildSettings);
            StartCoroutine(LoadLevelAfterDelay(winCelebrateSeconds, sceneNo));

        }

    }

    /// <summary>
    /// Wait for some seconds for animations, then load next level
    /// </summary>
    /// <returns></returns>
    private static IEnumerator LoadLevelAfterDelay(int delay, int sceneToLoad)
    {
        yield return new WaitForSecondsRealtime(delay);

        //load next level
        SceneManager.LoadScene(sceneToLoad);

        //reload same level
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateCheckPoint(Transform newCheckpoint)
    {
        UpdateCheckPoint(newCheckpoint.position);
    }

    public void UpdateCheckPoint(Vector3 newCheckPoint)
    {
        nextSpawnPoint = newCheckPoint;
    }

    public Vector3 GetSpawnPoint()
    {
        return nextSpawnPoint;
    }
}
