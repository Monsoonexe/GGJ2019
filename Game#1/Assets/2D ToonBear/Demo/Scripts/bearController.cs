using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//This Script is intended for demoing and testing animations only.


public class bearController : MonoBehaviour {

    public bool waypoints;

    public Transform Waypoint1;
    public Transform Waypoint2;
    public float walkSpeed;

    private bool facingRight = true;
    private bool _attacking = false;

    //Used for flipping Character Direction
    public static Vector3 theScale;

    //Jumping Stuff
    private bool grounded = false;
    private float groundRadius = 0.15f;

    private Animator anim;
    private Rigidbody2D _rigbod;

    // Use this for initialization
    void Awake()
    {
        //		startTime = Time.time;
        anim = GetComponent<Animator>();
        _rigbod = GetComponent<Rigidbody2D>() as Rigidbody2D;
        _attacking = false;
    }

    void FixedUpdate()
    {
        if (_rigbod.velocity.y == 0)
            grounded = true;
        else
            grounded = false;

        anim.SetBool("ground", grounded);
    }

    void Update() {

        if (!grounded)
        {
            anim.SetBool("ground", false);
        }

        if (waypoints && !_attacking)
        {
            anim.SetFloat("HSpeed", 0.055f);
            if (facingRight)
                transform.position = Vector3.Lerp(transform.position, Waypoint2.position, 0.5f * Time.deltaTime);
            else
                transform.position = Vector3.Lerp(transform.position, Waypoint1.position, 0.5f * Time.deltaTime);

            //Flipping direction character is facing based on players Input
            if (Vector3.Distance(transform.position, Waypoint1.position) < 1)
                Flip();
            else if (Vector3.Distance(transform.position, Waypoint2.position) < 1)
                Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _attacking = true;
            anim.SetFloat("HSpeed", 0f);
            if (collision.gameObject.GetComponent<Fox_Move>().attacking)
            {
                GetComponent<Rigidbody2D>().AddForce(collision.GetComponent<Rigidbody2D>().velocity, ForceMode2D.Impulse);
                StartCoroutine("RemoveBear", 2);
                Destroy(this.gameObject);
            }
            else
                anim.SetTrigger("Punch");
        }
    }

    private static IEnumerator RemoveBear(int delay)
    {
        yield return new WaitForSecondsRealtime(delay);
    }
    ////Flipping direction of character
    void Flip()
    {
        facingRight = !facingRight;
        theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
