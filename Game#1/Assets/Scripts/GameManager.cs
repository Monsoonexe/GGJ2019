using System.Collections;
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
    private AudioSource audioSource;

    private Transform startPoint;
    private Vector3 nextSpawnPoint;

    private void Awake()
    {
        SingletonPattern();
    }

    private void LoadAudio()
    {
        if(audioSource && levelMusic.Length > sceneNo)
        {
            audioSource.clip = levelMusic[sceneNo];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetStartPosition();
        nextSpawnPoint = startPoint.position;

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
    private void OnLevelWasLoaded(int sceneNo)
    {
        KillExistingPlayerIfAny();

        //where does the player start the level
        GetStartPosition();

        //init next spawn point
        nextSpawnPoint = startPoint.position;

        //create a new player object
        Instantiate(playerPrefab, nextSpawnPoint, Quaternion.identity);




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
        //say congrats!
        Instantiate(victoryWindow);

        //do win animations
        StartCoroutine(LoadLevelAfterDelay(winCelebrateSeconds));

    }

    /// <summary>
    /// Wait for some seconds for animations, then load next level
    /// </summary>
    /// <returns></returns>
    private static IEnumerator LoadLevelAfterDelay(int delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        //load next level
        SceneManager.LoadScene(++sceneNo);
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
