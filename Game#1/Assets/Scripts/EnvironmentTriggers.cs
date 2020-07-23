using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class EnvironmentTriggers : MonoBehaviour
{
    public enum TriggerType { checkpoint, finish, death }
    public TriggerType _triggerType;
    public Sprite sprite1;
    public Sprite sprite2;
    private GameManager _gameManager;

    private void Awake()
    {
        BoxCollider2D bc = GetComponent<BoxCollider2D>();
        bc.isTrigger = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>() as GameManager;
        if (_triggerType == TriggerType.checkpoint)
        {
            var spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = this.gameObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sortingOrder = 1;
                spriteRenderer.sprite = sprite1;
            }
        }
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
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite2;
                    break;
                case TriggerType.death:
                    Debug.Log("Death Trigger Activated");
                    collision.gameObject.SendMessage("Dead");
                    break;
                case TriggerType.finish:
                    Debug.Log("Finish Trigger Activated");
                    _gameManager.WinLevel();
                    break;
            }
        }
    }
}
