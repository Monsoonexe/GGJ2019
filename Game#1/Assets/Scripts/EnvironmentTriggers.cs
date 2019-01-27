using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class EnvironmentTriggers : MonoBehaviour
{
    public enum TriggerType { checkpoint, finish, death }
    public TriggerType _triggerType;
    private GameManager _gameManager;
    private Fox_Move _player;

    private void Awake()
    {
        BoxCollider2D bc = GetComponent<BoxCollider2D>();
        bc.isTrigger = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Fox_Move>() as Fox_Move;
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>() as GameManager;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (_triggerType)
            {
                case TriggerType.checkpoint:
                    Debug.Log("Checkpoint Trigger Activated");
                    _gameManager.UpdateCheckPoint(transform);
                    break;
                case TriggerType.death:
                    Debug.Log("Death Trigger Activated");
                    _player.SendMessage("Dead");
                    break;
                case TriggerType.finish:
                    Debug.Log("Finish Trigger Activated");
                    _gameManager.WinLevel();
                    break;
            }
        }
    }
}
