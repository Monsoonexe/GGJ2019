using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backandforthmovement : MonoBehaviour
{
    public float speed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + Mathf.PingPong(Time.time * speed, 2), transform.position.y, transform.position.z);
 
            
    }
}
