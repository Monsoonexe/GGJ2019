using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backandforthmovement : MonoBehaviour
{
    public float speed = 3f;
    public Vector2 offset;
    // Start is called before the first frame update
    void Start()
    {
        //offset = transform.position.x + 2;
        offset.x = transform.position.x;
        offset.y = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3 (Mathf.PingPong(Time.time, 3)+offset.x, offset.y, transform.position.z);
        //transform.position = new Vector3(Mathf.PingPong(((Time.time - offset) * speed) + 5, 10), transform.position.y, transform.position.z);

    }
}