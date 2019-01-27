using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//This Script is intended for demoing and testing animations only.


public class EnemyController : MonoBehaviour {


    [SerializeField] private float wanderDistance;
    [SerializeField] private float wanderSpeed;

    private int wanderDirection = 1;
    private Vector3 startingPosition;
    private bool _attacking = false;

    //Used for flipping Character Direction
    public static Vector3 theScale;

    //Jumping Stuff
    private bool grounded = false;

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

    private void Start()
    {
        startingPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (_rigbod.velocity.y == 0)
            grounded = true;
        else
            grounded = false;
        if (anim != null)
            anim.SetBool("ground", grounded);
    }

    void Update() {

        if (!grounded)
        {
            if (anim != null)
                anim.SetBool("ground", false);
        }
        else
        {
            if (anim != null)
            {
                anim.SetFloat("HSpeed", 0.055f);
            }
            transform.Translate(new Vector3(Time.deltaTime * wanderSpeed * wanderDirection, 0, 0));
            float distanceFromPivot = Vector3.Distance(startingPosition, transform.position);
            if (distanceFromPivot >= wanderDistance)
            {
                wanderDirection *= -1;
                Flip();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            if (anim != null)
            {
                anim.SetFloat("HSpeed", 0f);
                if (!collision.GetComponent<Fox_Move>().attacking)
                    anim.SetTrigger("Punch");
            }
        }
    }

    public void EnemyAttacked(Vector2 otherVel)
    {
        if (anim != null)
        {
            anim.SetFloat("HSpeed", 0f);
        }
        float defaultX = 3f;
        float defaultY = 3f;
        grounded = false;
        if (otherVel.x != 0 && otherVel.y != 0)
            GetComponent<Rigidbody2D>().AddForce(otherVel * 20, ForceMode2D.Impulse);
        else if (otherVel.x == 0 && otherVel.y == 0)
            GetComponent<Rigidbody2D>().AddForce(new Vector2(defaultX, defaultY) * 20, ForceMode2D.Impulse);
        else if (otherVel.x == 0)
            GetComponent<Rigidbody2D>().AddForce(new Vector2(defaultX, otherVel.y) * 20, ForceMode2D.Impulse);
        else
            GetComponent<Rigidbody2D>().AddForce(new Vector2(otherVel.x, defaultY) * 20, ForceMode2D.Impulse);
        StartCoroutine("RemoveEnemy",1);
    }

    private IEnumerator RemoveEnemy(int delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Destroy(this.gameObject);
    }
    ////Flipping direction of character
    void Flip()
    {
        theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
