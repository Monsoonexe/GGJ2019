﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//This Script is intended for demoing and testing animations only.


public class EnemyController : MonoBehaviour {

    [SerializeField] private int flingForce = 10;
    [SerializeField] private float wanderDistance;
    [SerializeField] private float wanderSpeed;

    [SerializeField] private int wanderDirection = 1;//left or right
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
        grounded = _rigbod.velocity.y == 0;

        anim?.SetBool("ground", grounded);
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

            float moveAmount = Time.deltaTime * wanderSpeed * wanderDirection;
            float distanceFromPivot = Vector3.Distance(startingPosition, transform.position);
            transform.Translate(new Vector3(Time.deltaTime * wanderSpeed * wanderDirection, 0, 0));

            if (distanceFromPivot + moveAmount > wanderDistance)
            {
                Flip();

                transform.Translate(new Vector3(Time.deltaTime * wanderSpeed * 4 * wanderDirection, 0, 0)); // course correction
            }
            else
            {

                //moveAmount = Time.deltaTime * wanderSpeed * wanderDirection;

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

    public void EnemyAttacked(Vector3 attackerPosition)
    {
        Vector3 pushDirection = attackerPosition - transform.position;
        pushDirection = new Vector3(pushDirection.x, pushDirection.y + 5, pushDirection.z);

        _rigbod.isKinematic = false;
        Collider2D[] colliders = GetComponents<Collider2D>();
        //Debug.Log(colliders.Length);
        foreach(var col in colliders)
        {
            col.enabled = false;
        }
        
        _rigbod.AddForce((pushDirection) * flingForce, ForceMode2D.Impulse);
        Destroy(this.gameObject, 3);
    }

    private IEnumerator RemoveEnemy(int delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Destroy(this.gameObject);
    }
    ////Flipping direction of character
    void Flip()
    {
        transform.Rotate(new Vector3(0, 180, 0));
    }

}
