using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//This Script is intended for demoing and testing animations only.


public class bearController : MonoBehaviour {

    public Transform Waypoint1;
    public Transform Waypoint2;

	private bool facingRight = true;
    
    //Used for flipping Character Direction
	public static Vector3 theScale;

	//Jumping Stuff
	private bool grounded = false;
	private float groundRadius = 0.15f;

	private Animator anim;
    private Rigidbody2D _rigbod;

	// Use this for initialization
	void Awake ()
	{
//		startTime = Time.time;
		anim = GetComponent<Animator> ();
        _rigbod = GetComponent<Rigidbody2D>() as Rigidbody2D;
	}

	void FixedUpdate ()
	{
        if (_rigbod.velocity.y == 0)
            grounded = true;
        else
            grounded = false;

        anim.SetBool("ground", grounded);
	}

	void Update()
	{

        moveXInput = Input.GetAxis("Horizontal");

        if ((grounded) && Input.GetButtonDown("Jump"))
        {
            anim.SetBool("ground", false);

            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.y, jumpForce);
        }


        anim.SetFloat("HSpeed", Mathf.Abs(moveXInput));
        anim.SetFloat("vSpeed", GetComponent<Rigidbody2D>().velocity.y);


        GetComponent<Rigidbody2D>().velocity = new Vector2((moveXInput * HSpeed), GetComponent<Rigidbody2D>().velocity.y);

        if (Input.GetButtonDown("Fire1") && (grounded)) { anim.SetTrigger("Punch"); }

        if (Input.GetButton("Fire2"))
        {
            anim.SetBool("Sprint", true);
            HSpeed = 14f;
}
        else
        {
            anim.SetBool("Sprint", false);
            HSpeed = 10f;
        }

        //Flipping direction character is facing based on players Input
        if (moveXInput > 0 && !facingRight)
            Flip();
        else if (moveXInput < 0 && facingRight)
            Flip();
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
