using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManager;
    private static int sceneNo = 0;

    [SerializeField] private bool TESTING = false;
    [SerializeField] private Transform startPoint;
    [SerializeField] private int winCelebrateSeconds = 5;
    [SerializeField] private GameObject victoryWindow;

    [SerializeField] private GameObject playerPrefab;

    private Vector3 nextSpawnPoint;

    private void Awake()
    {
        SingletonPattern();
    }

    // Start is called before the first frame update
    void Start()
    {

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
    }

    private void OnLevelWasLoaded(int sceneNo)
    {
        GetStartPosition();
        nextSpawnPoint = startPoint.position;
        victoryWindow.SetActive(false);
        Instantiate(playerPrefab, nextSpawnPoint, Quaternion.identity);

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
        victoryWindow.SetActive(true);

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
