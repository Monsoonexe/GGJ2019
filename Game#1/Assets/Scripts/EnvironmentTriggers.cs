using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class EnvironmentTriggers : MonoBehaviour
{
    enum TriggerType { checkpoint, finish, death }
    public string _triggerType;
    private TriggerType _trigType;

    private void Awake()
    {
        BoxCollider2D bc = GetComponent<BoxCollider2D>();
        bc.isTrigger = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        switch(_triggerType)
        {
            case "checkpoint":
                _trigType = TriggerType.checkpoint;
                break;
            case "finsh":
                _trigType = TriggerType.finish;
                break;
            case "death":
                _trigType = TriggerType.death;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (_trigType)
        {
            case TriggerType.checkpoint:
                break;
            case TriggerType.death:
                break;
            case TriggerType.finish:
                break;
        }
    }
}
